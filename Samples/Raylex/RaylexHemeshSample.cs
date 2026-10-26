using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Topology;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 半边网格：构造单位正方形顶点并绘制轮廓。
/// </summary>
public static class RaylexHemeshSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-hemesh ===");
        Console.WriteLine("构造 HEMesh 四顶点，绘制折线轮廓；关闭窗口结束。");

        HEMesh mesh = new();
        mesh.Vertices.Add(new HEVertex(new Point(0, 0)));
        mesh.Vertices.Add(new HEVertex(new Point(100, 0)));
        mesh.Vertices.Add(new HEVertex(new Point(100, 100)));
        mesh.Vertices.Add(new HEVertex(new Point(0, 100)));

        Console.WriteLine($"顶点数量: {mesh.Vertices.Count}");
        foreach (HEVertex v in mesh.Vertices)
            Console.WriteLine($"  {v.Pt}");

        // 用折线表达轮廓，便于在 Raylex 中观察
        Polyline outline = new(
            mesh.Vertices.Select(v => v.Pt).ToList(),
            isClosed: true);

        RWindow rw = new(800, 800, "raylex-hemesh");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 2);
        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);
            rw.Draw(outline, RColor.Blue);
            rw.Draw(mesh.Vertices.Select(v => v.Pt).ToList(), 5, RColor.Red);
            rw.InsertText($"HEMesh vertices={mesh.Vertices.Count}", 20, 20, 16);
        }
    }
}
