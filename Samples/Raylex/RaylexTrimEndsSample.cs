using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;
using Raylib_cs;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 开放折线 TrimEnds；[ / ] 调整裁剪距离。
/// </summary>
public static class RaylexTrimEndsSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-trim-ends ===");
        Console.WriteLine("左键加点，右键结束（不封闭）；[ / ] 调 trim；中键重置。");

        RWindow rw = new(1000, 800, "raylex-trim-ends");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 1);
        Relation.Tolerance = 1;

        bool[] starts = [true, false];
        bool[] ends = [false, false];
        Polyline poly = new();
        Polyline trimmed = new(new List<Point>(), isClosed: false);
        double trim = 30;
        string status = "绘制开放折线…";

        void Recompute()
        {
            if (!ends[0] || poly.Count < 2)
                return;
            try
            {
                trimmed = poly.TrimEnds(trim);
                status = $"trim={trim:F1}, len={poly.Perimeter:F1}, resultPts={trimmed.Count}";
                Console.WriteLine(status);
            }
            catch (Exception ex)
            {
                trimmed = new(new List<Point>(), isClosed: false);
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
                if (Raylib.IsKeyPressed(KeyboardKey.LeftBracket))
                {
                    trim = Math.Max(0, trim - 5);
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.RightBracket))
                {
                    trim += 5;
                    Recompute();
                }
            }

            rw.GetPolylineProcess(ref starts[0], ref ends[0], ref poly, close: false);

            if (starts[1] && !ends[1])
            {
                ends[1] = true;
                Recompute();
            }

            rw.InsertText($"红=原折线  橙=TrimEnds  trim={trim:F1}  ([/]调节)", 20, 20, 16);
            rw.InsertText("左键加点 / 右键结束(开放) / 中键重置", 20, 42, 16);
            rw.InsertText(status, 20, 64, 16, RColor.DarkBlue);

            if (ends[0])
                rw.Draw(poly, RColor.Red);
            if (ends[1])
            {
                if (trimmed.Count >= 2)
                    rw.Draw(trimmed, RColor.Orange);
                else if (trimmed.Count == 1)
                    rw.Draw(trimmed[0], 6, RColor.Orange);
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
                trimmed = new(new List<Point>(), isClosed: false);
                status = "绘制开放折线…";
            }
        }
    }
}
