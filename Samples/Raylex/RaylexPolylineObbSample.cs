using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 手绘封闭折线 → OrientedBoundingBox。
/// </summary>
public static class RaylexPolylineObbSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-polyline-obb ===");
        Console.WriteLine("左键加点，右键封闭；中键重置。红=折线，蓝=OBB。");

        RWindow rw = new(800, 800, "raylex-polyline-obb");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 1);
        Relation.Tolerance = 1;

        bool[] starts = [true, false];
        bool[] ends = [false, false];
        Polyline poly = new();
        Polyline obb = new(new List<Point>(), isClosed: true);
        string status = "绘制封闭多边形…";

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);

            rw.GetPolylineProcess(ref starts[0], ref ends[0], ref poly, close: true);

            if (starts[1] && !ends[1])
            {
                try
                {
                    if (poly.Count < 3)
                        throw new InvalidOperationException("至少 3 个点。");
                    if (!poly.IsClosed)
                        poly.Close();
                    obb = poly.OrientedBoundingBox().ToPoly;
                    status = $"pts={poly.Count}, obbPts={obb.Count}";
                    Console.WriteLine(status);
                    foreach (Point pt in obb.Pts)
                        Console.WriteLine($"  OBB {pt}");
                }
                catch (Exception ex)
                {
                    obb = new(new List<Point>(), isClosed: true);
                    status = "失败: " + ex.Message;
                    Console.WriteLine(status);
                }
                ends[1] = true;
            }

            rw.InsertText("红=输入折线  蓝=OrientedBoundingBox", 20, 20, 16);
            rw.InsertText("左键加点 / 右键封闭 / 中键重置", 20, 42, 16);
            rw.InsertText(status, 20, 64, 16, RColor.DarkBlue);

            if (ends[0])
                rw.Draw(poly, RColor.Red);
            if (ends[1] && obb.Count >= 2)
                rw.Draw(obb, RColor.Blue);

            if (ends[0])
                starts[1] = true;

            if (rw.MouseMiddlePressed)
            {
                starts[0] = true;
                ends[0] = false;
                starts[1] = false;
                ends[1] = false;
                poly = new();
                obb = new(new List<Point>(), isClosed: true);
                status = "绘制封闭多边形…";
            }
        }
    }
}
