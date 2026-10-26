using ArchiGeo.Basic.Geometry;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// Mode3D：正交相机下渲染 Box，右键拖动更新相机。
/// </summary>
public static class RaylexBox3dSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-box-3d ===");
        Console.WriteLine("右键拖动旋转相机；关闭窗口结束。");

        Plane s1 = new(new Point(0, 0, 0), new Vector(1, 0, 0), new Vector(0, 1, 0));
        Box b1 = new(s1, 5, 10, 15);

        RWindow rw = new(800, 800, "raylex-box-3d");
        rw.SetOrthoRenderView(new(5, 5, 0), new(-1, -1, -1), 200);

        while (rw.Running)
        {
            using var f = new Frame();

            rw.SetBackgroundColor(RColor.White);
            if (rw.MouseRightDown)
                rw.UpdateCamera(4);

            using (var rm = new RenderMode())
            {
                rw.Render(b1, RColor.LightGray);
                rw.BuildAxis(20);
            }

            rw.Draw(new LineSeg(new(0, 0, 0), new(100, 100, 100)));
        }
    }
}
