using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;

namespace ArchiGeoSharp.Samples.Basic;

/// <summary>
/// 入门：Plane + Space3d 局部/世界坐标与平移。
/// </summary>
public static class BasicPlaneSample
{
    public static void Run()
    {
        Console.WriteLine("=== basic-plane: Plane + Space3d ===");

        Point basePt = new(0, 0, 0);
        Vector xDir = new(1, 0, 0);
        Vector yDir = new(0, 1, 0);

        Plane plane = new(basePt, xDir, yDir);

        Point local = new(2, 3, 0);
        Point world = plane.AbsoluteDescribe(local);

        Console.WriteLine($"局部点 local = {local}");
        Console.WriteLine($"世界点 world = plane.AbsoluteDescribe(local) = {world}");

        Space3d move = Space3d.Translation(new Vector(10, 0, 0));
        Plane moved = plane * move;

        Console.WriteLine($"平移后平面基点 = {moved.BasePt}");
        Console.WriteLine($"平移后平面 X 轴 = {moved.XDir}");
        Console.WriteLine("完成。");
    }
}
