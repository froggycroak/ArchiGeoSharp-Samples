using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// 依次手绘两条折线，显示 Relation.RelateSimp 结果。
/// </summary>
public static class RaylexRelationPolySample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-relation-poly ===");
        Console.WriteLine("先画折线 A（红），再画折线 B（蓝）；中键重置。");

        RWindow rw = new(800, 800, "raylex-relation-poly");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 2);
        Relation.Tolerance = 1;

        bool startGetPoly1 = true;
        bool endGetPoly1 = false;
        bool startGetPoly2 = false;
        bool endGetPoly2 = false;
        bool startRelation = false;
        bool endRelation = false;
        Polyline poly1 = new();
        Polyline poly2 = new();
        RelationType t = default;

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);

            rw.GetPolylineProcess(ref startGetPoly1, ref endGetPoly1, ref poly1);
            rw.GetPolylineProcess(ref startGetPoly2, ref endGetPoly2, ref poly2);

            if (startRelation && !endRelation)
            {
                t = Relation.RelateSimp(poly1, poly2);
                endRelation = true;
                Console.WriteLine($"RelateSimp = {t}");
            }

            rw.InsertText("红=折线A  蓝=折线B", 20, 20, 16);
            rw.InsertText("依次完成两条折线；中键重置", 20, 42, 16);
            if (endRelation)
                rw.InsertText($"Relation: {t}", 20, 64, 20, RColor.DarkBlue);

            if (endGetPoly1)
                rw.Draw(poly1, RColor.Red);
            if (endGetPoly2)
                rw.Draw(poly2, RColor.Blue);

            if (endGetPoly1 && !endGetPoly2)
                startGetPoly2 = true;
            if (endGetPoly1 && endGetPoly2 && !endRelation)
                startRelation = true;

            if (rw.MouseMiddlePressed)
            {
                startGetPoly1 = true;
                endGetPoly1 = false;
                startGetPoly2 = false;
                endGetPoly2 = false;
                startRelation = false;
                endRelation = false;
                poly1 = new();
                poly2 = new();
                t = default;
            }
        }
    }
}
