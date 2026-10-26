using ArchiGeo.Basic.Geometry;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 圆弧：圆心 / 起点 / 终点着色显示。
/// </summary>
public static class RaylexArcSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-arc ===");
        Console.WriteLine("红=弧  黑=圆心  蓝=起点  绿=终点；关闭窗口结束。");

        RWindow rw = new(800, 800, "raylex-arc");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 2);
        Arc arc1 = new(new Point(10, 100), new Point(10, 0), 0);

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);
            rw.Draw(arc1, RColor.Red);
            rw.Draw(arc1.Center, 3, RColor.Black);
            rw.Draw(arc1.StartPt, 3, RColor.Blue);
            rw.Draw(arc1.EndPt, 3, RColor.Green);
        }
    }
}
