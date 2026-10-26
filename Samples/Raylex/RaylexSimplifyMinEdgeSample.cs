using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;
using Raylib_cs;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// SimplifyMinEdge：手绘折线；Tab 开/闭，[ / ] 调 t，- / = 调 ε。
/// </summary>
public static class RaylexSimplifyMinEdgeSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-simplify-min-edge ===");
        Console.WriteLine("左键加点，右键结束；Tab=开/闭；[/]=t；-/=ε；中键重置。");

        RWindow rw = new(1000, 800, "raylex-simplify-min-edge");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 1);
        Relation.Tolerance = 1;

        bool[] starts = [true, false];
        bool[] ends = [false, false];
        bool wantClosed = false;
        Polyline poly = new();
        Polyline simplified = new(new List<Point>(), isClosed: false);
        double t = 40;
        double epsilon = 5;
        string status = "绘制开放折线…";

        static double MinSegLen(Polyline p)
        {
            if (p.SegmentCount == 0)
                return double.NaN;
            double min = double.MaxValue;
            for (int i = 0; i < p.SegmentCount; i++)
                min = Math.Min(min, Distance.Dist(p[i], p[i + 1]));
            return min;
        }

        void Recompute()
        {
            if (!ends[0] || poly.Count < 2)
                return;
            try
            {
                if (wantClosed)
                    poly.Close();
                else
                    poly.Open();

                simplified = poly.SimplifyMinEdge(t, epsilon);
                double minIn = MinSegLen(poly);
                double minOut = MinSegLen(simplified);
                status =
                    $"closed={simplified.IsClosed}  in={poly.Count}→out={simplified.Count}  " +
                    $"t={t:F1} ε={epsilon:F1}  minIn={minIn:F1} minOut={minOut:F1}";
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                simplified = new(new List<Point>(), isClosed: wantClosed);
                status = "失败: " + ex.Message;
                Console.WriteLine(status);
            }
        }

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);

            if (ends[1])
            {
                if (Raylib.IsKeyPressed(KeyboardKey.Tab))
                {
                    wantClosed = !wantClosed;
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.LeftBracket))
                {
                    t = Math.Max(1, t - 5);
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.RightBracket))
                {
                    t += 5;
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Minus))
                {
                    epsilon = Math.Max(0, epsilon - 1);
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.Equal))
                {
                    epsilon += 1;
                    Recompute();
                }
            }
            else if (!ends[0] && Raylib.IsKeyPressed(KeyboardKey.Tab))
            {
                wantClosed = !wantClosed;
                status = wantClosed ? "绘制闭合折线…" : "绘制开放折线…";
            }

            rw.GetPolylineProcess(ref starts[0], ref ends[0], ref poly, close: wantClosed);

            if (starts[1] && !ends[1])
            {
                ends[1] = true;
                Recompute();
            }

            string mode = wantClosed ? "闭合" : "开放";
            rw.InsertText($"红=原折线  橙=SimplifyMinEdge  模式={mode}  t={t:F1}  ε={epsilon:F1}", 20, 20, 16);
            rw.InsertText("左键加点 / 右键结束 / Tab开闭 / [/]调t / -/=调ε / 中键重置", 20, 42, 16);
            rw.InsertText(status, 20, 64, 16, RColor.DarkBlue);

            if (ends[0])
                rw.Draw(poly, RColor.Red);
            if (ends[1])
            {
                if (simplified.Count >= 2)
                    rw.Draw(simplified, RColor.Orange);
                else if (simplified.Count == 1)
                    rw.Draw(simplified[0], 6, RColor.Orange);
                for (int i = 0; i < simplified.Count; i++)
                    rw.Draw(simplified[i], 4, RColor.DarkBlue);
            }

            if (ends[0])
                starts[1] = true;

            if (rw.MouseMiddlePressed)
            {
                starts[0] = true;
                ends[0] = false;
                starts[1] = false;
                ends[1] = false;
                poly = new();
                simplified = new(new List<Point>(), isClosed: wantClosed);
                status = wantClosed ? "绘制闭合折线…" : "绘制开放折线…";
            }
        }
    }
}
