using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 封闭折线 OffsetSimp：红=输入，蓝=偏移结果。
/// </summary>
public static class RaylexOffsetSimpSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-offset-simp ===");
        Console.WriteLine("左键加点，右键封闭结束；中键重置。");

        RWindow rw = new(800, 800, "raylex-offset-simp");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 2);
        Relation.Tolerance = 1;

        bool[] starts = [true, false];
        bool[] ends = [false, false];
        Polyline poly1 = new();
        Polyline poly2 = new();
        double offset = 40;
        string status = "绘制封闭多边形…";

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);

            rw.GetPolylineProcess(ref starts[0], ref ends[0], ref poly1);

            if (starts[1] && !ends[1])
            {
                try
                {
                    if (poly1.Count < 3)
                        throw new InvalidOperationException("至少 3 个点。");
                    if (!poly1.IsClosed)
                        poly1.Close();
                    poly2 = poly1.OffsetSimp(offset);
                    status = $"offset={offset}, resultPts={poly2.Count}";
                    Console.WriteLine(status);
                }
                catch (Exception ex)
                {
                    poly2 = new();
                    status = "失败: " + ex.Message;
                    Console.WriteLine(status);
                }
                ends[1] = true;
            }

            rw.InsertText($"红=输入  蓝=OffsetSimp({offset})", 20, 20, 16);
            rw.InsertText("左键加点 / 右键封闭 / 中键重置", 20, 42, 16);
            rw.InsertText(status, 20, 64, 16, RColor.DarkBlue);

            if (ends[0])
                rw.Draw(poly1, RColor.Red);
            if (ends[1] && poly2.Count >= 2)
                rw.Draw(poly2, RColor.Blue);

            if (ends[0])
                starts[1] = true;

            if (rw.MouseMiddlePressed)
            {
                starts[0] = true;
                ends[0] = false;
                starts[1] = false;
                ends[1] = false;
                poly1 = new();
                poly2 = new();
                status = "绘制封闭多边形…";
            }
        }
    }
}
