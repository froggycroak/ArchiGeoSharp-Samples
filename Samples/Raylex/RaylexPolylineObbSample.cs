using ArchiGeo.Basic.Geometry;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 封闭折线定向包围盒（OBB）可视化。
/// </summary>
public static class RaylexPolylineObbSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-polyline-obb ===");
        Console.WriteLine("关闭窗口结束。");

        Point pt1 = new(10, 10);
        Point pt2 = new(10, -10);
        Point pt3 = new(5, 5);
        Point pt4 = new(-10, 10);
        Polyline poly = new([pt1, pt2, pt3, pt4], true);
        Polyline obb = poly.OrientedBoundingBox().ToPoly;

        Console.WriteLine("OBB 顶点:");
        foreach (Point pt in obb.Pts)
            Console.WriteLine($"  {pt}");

        RWindow rw = new(800, 800, "raylex-polyline-obb");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 10);
        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.RayWhite);
            rw.Draw(poly);
            rw.Draw(obb);
            rw.Draw([pt1, pt2, pt3, pt4], 4);
        }
    }
}
