using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 手绘闭合场地 → Prism 拉伸 → Mode3D 渲染。
/// </summary>
public static class RaylexPrismMeshSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-prism-mesh ===");
        Console.WriteLine("左键加点，右键封闭；中键重置；进入 3D 后右键拖动相机。");

        RWindow rw = new(800, 800, "raylex-prism-mesh");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400), 5);
        rw.SetOrthoRenderView(new(0, 0, 0), new(-10, -10, -10), 500, (float)2);

        bool[] starts = [true, false, false];
        bool[] ends = [false, false, false];
        Polyline sitePoly = new();
        Prism? prism = null;

        while (rw.Running)
        {
            using var f = new Frame();

            rw.GetPolylineProcess(ref starts[0], ref ends[0], ref sitePoly);

            if (starts[1] && !ends[1])
            {
                sitePoly.AntiClockwise();
                prism = new Prism(Space3d.WorldXY, sitePoly, 50);
                ends[1] = true;
            }

            if (starts[0] && !ends[0])
            {
                rw.SetBackgroundColor(RColor.White);
                rw.DrawAxis(100);
            }

            if (starts[2] && !ends[2] && prism is not null)
            {
                if (rw.MouseRightDown)
                    rw.UpdateCamera(4);
                rw.SetBackgroundColor(RColor.White);

                using (var rm = new RenderMode())
                {
                    rw.Render(prism, RColor.LightGray, RColor.Blue);
                    rw.BuildAxis(100);
                }
            }

            if (ends[0])
                starts[1] = true;
            if (ends[1])
                starts[2] = true;

            if (rw.MouseMiddlePressed)
            {
                starts = [true, false, false];
                ends = [false, false, false];
                sitePoly = new();
                prism = null;
            }
        }
    }
}
