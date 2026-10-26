using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Transform;
using ArchiGeo.Raylex;
using Raylib_cs;

namespace ArchiGeoSharp.Samples.Raylex;

/// <summary>
/// AngleGrid + BSP：在场地包围盒上构造角度场，再做编码/自适应剖分。
/// （不依赖 GRBSolver.PolySmoothGrid，仅用 Basic 已发布 API。）
/// </summary>
public static class RaylexBspSubdivisionSample
{
    public static void Run()
    {
        Console.WriteLine("=== raylex-bsp-subdivision ===");
        Console.WriteLine("Tab=编码/自适应  R=随机  [/]=编码或深度  中键=手绘");

        string[] encodings =
        [
            "0101222",
            "0010222",
            "0110222",
            "01010122222",
            "00101122222",
            "01100122222",
            "01001122222",
            "010101022222222",
            "001100122222222",
            "011010022222222",
            "010011022222222",
            "001011022222222",
            "000101122222222",
            "011101022222222",
            "010110122222222",
        ];
        int encIndex = 7;
        bool adaptiveMode = true;
        int adaptiveMaxDepth = 3;
        double adaptiveMinElongation = 0.28;
        double adaptiveMinAreaFrac = 0.06;

        RWindow rw = new(1000, 800, "raylex-bsp-subdivision");
        rw.SetViewByType(ViewType.TOP, new Point(400, 400, 0), 1);

        Random rng = new();
        bool[] starts = [false, false];
        bool[] ends = [false, false];
        Polyline poly = new();
        AngleGrid? field = null;
        LineSeg[] crossRep = [];
        Polyline[] blocks = [];
        LineSeg[] streets = [];
        string status = "";

        RColor[] blockColors =
        [
            RColor.Blue, RColor.Green, RColor.Orange, RColor.Purple,
            RColor.DarkBlue, RColor.Lime, RColor.Brown, RColor.SkyBlue
        ];

        void ResetDrawState()
        {
            starts = [true, false];
            ends = [false, false];
            poly = new();
            field = null;
            crossRep = [];
            blocks = [];
            streets = [];
            status = "手绘闭合多边形…";
        }

        void ApplyRandomSite()
        {
            poly = MakeRandomClosedPolygon(rng, new Point(400, 400), radius: 220, vertexCount: rng.Next(5, 9));
            starts = [false, true];
            ends = [true, false];
            field = null;
            crossRep = [];
            blocks = [];
            streets = [];
            status = $"随机场地 n={poly.Count}";
            Console.WriteLine(status);
        }

        void Recompute()
        {
            if (!ends[0] || poly.Count < 3)
                return;
            try
            {
                if (!poly.IsClosed)
                    poly.Close();
                poly.AntiClockwise();

                field = BuildUniformField(poly, unit: 40, baseAngleDeg: 15);
                crossRep = field.GenerateCrossRep(
                    repUnit: Math.Max(20, field.Unit),
                    crossLength: Math.Max(4, field.Unit * 0.25));

                BspNode root;
                if (adaptiveMode)
                {
                    double siteArea = Math.Abs(poly.Area);
                    var adapt = new BspAdaptiveOptions
                    {
                        MaxDepth = adaptiveMaxDepth,
                        MinLeafArea = siteArea * adaptiveMinAreaFrac,
                        MinElongation = adaptiveMinElongation,
                        Ratio = 0.5,
                    };
                    root = poly.SubdivideBspAdaptive(field, adapt);
                }
                else
                {
                    var encoding = new BspEncoding(encodings[encIndex]);
                    root = poly.SubdivideBsp(encoding, field);
                }

                blocks = root.Blocks();
                streets = root.Streets();

                double areaSum = blocks.Sum(b => Math.Abs(b.Area));
                double minElong = blocks.Length == 0 ? 0 : blocks.Min(b =>
                {
                    try { return b.Elongation(); }
                    catch { return 0; }
                });
                status = adaptiveMode
                    ? $"自适应 depth≤{adaptiveMaxDepth}  minArea={adaptiveMinAreaFrac:P0}  minElong={adaptiveMinElongation:F2}  " +
                      $"blocks={blocks.Length}  streets={streets.Length}  worstElong={minElong:F2}  area={areaSum:F0}/{Math.Abs(poly.Area):F0}"
                    : $"enc[{encIndex}]={encodings[encIndex]}  blocks={blocks.Length}  " +
                      $"streets={streets.Length}  areaSum={areaSum:F0}/{Math.Abs(poly.Area):F0}";
                Console.WriteLine(status);
                ends[1] = true;
            }
            catch (Exception ex)
            {
                field = null;
                crossRep = [];
                blocks = [];
                streets = [];
                ends[1] = false;
                status = "失败: " + ex.Message;
                Console.WriteLine(status);
            }
        }

        ApplyRandomSite();
        Recompute();

        while (rw.Running)
        {
            using var f = new Frame();
            rw.SetBackgroundColor(RColor.White);
            rw.DrawAxis(100);

            if (Raylib.IsKeyPressed(KeyboardKey.R))
            {
                ApplyRandomSite();
                Recompute();
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Tab))
            {
                adaptiveMode = !adaptiveMode;
                Recompute();
            }

            if (ends[1] || ends[0])
            {
                if (Raylib.IsKeyPressed(KeyboardKey.LeftBracket))
                {
                    if (adaptiveMode)
                        adaptiveMaxDepth = Math.Max(0, adaptiveMaxDepth - 1);
                    else
                        encIndex = (encIndex - 1 + encodings.Length) % encodings.Length;
                    Recompute();
                }
                if (Raylib.IsKeyPressed(KeyboardKey.RightBracket))
                {
                    if (adaptiveMode)
                        adaptiveMaxDepth = Math.Min(6, adaptiveMaxDepth + 1);
                    else
                        encIndex = (encIndex + 1) % encodings.Length;
                    Recompute();
                }
            }

            if (starts[0])
                rw.GetPolylineProcess(ref starts[0], ref ends[0], ref poly);

            if (ends[0] && starts[1] && !ends[1])
                Recompute();

            if (ends[0] && !starts[1])
                starts[1] = true;

            rw.InsertText("粉=场地  灰=场交叉  彩=地块  红=街道割缝", 20, 20, 16);
            rw.InsertText("Tab=模式  R=随机  [/]=编码或深度  中键=手绘", 20, 42, 16);
            rw.InsertText(status, 20, 64, 16, RColor.DarkBlue);

            if (ends[0] && poly.Count >= 3)
                rw.Draw(poly, RColor.Pink);

            if (ends[1])
            {
                if (crossRep.Length > 0)
                    rw.Draw(crossRep, RColor.LightGray);

                for (int i = 0; i < blocks.Length; i++)
                    rw.Draw(blocks[i], blockColors[i % blockColors.Length]);

                if (streets.Length > 0)
                    rw.Draw(streets, RColor.Red);
            }

            if (rw.MouseMiddlePressed)
                ResetDrawState();
        }
    }

    /// <summary>
    /// 在场地包围盒上构造近似均匀的 AngleGrid（Basic 包内 API，不依赖 PolySmoothGrid）。
    /// </summary>
    private static AngleGrid BuildUniformField(Polyline site, double unit, double baseAngleDeg)
    {
        double minX = site.Pts.Min(p => p[0]);
        double maxX = site.Pts.Max(p => p[0]);
        double minY = site.Pts.Min(p => p[1]);
        double maxY = site.Pts.Max(p => p[1]);
        double pad = unit * 2;
        Point basePt = new(minX - pad, minY - pad);
        int xCount = Math.Max(2, (int)Math.Ceiling((maxX - minX + 2 * pad) / unit));
        int yCount = Math.Max(2, (int)Math.Ceiling((maxY - minY + 2 * pad) / unit));

        AngleGrid grid = new(xCount, yCount, unit, basePt);
        for (int i = 0; i <= xCount; i++)
        {
            for (int j = 0; j <= yCount; j++)
            {
                // 轻微空间变化，避免完全平行割缝过于单调
                double wobble = 8 * Math.Sin(i * 0.35) * Math.Cos(j * 0.28);
                grid[i, j] = new Angle(baseAngleDeg + wobble);
            }
        }
        return grid;
    }

    private static Polyline MakeRandomClosedPolygon(Random rng, Point center, double radius, int vertexCount)
    {
        var pts = new List<Point>(vertexCount);
        for (int i = 0; i < vertexCount; i++)
        {
            double ang = rng.NextDouble() * Math.PI * 2;
            double r = radius * (0.55 + 0.45 * rng.NextDouble());
            pts.Add(new Point(center[0] + r * Math.Cos(ang), center[1] + r * Math.Sin(ang)));
        }

        Polyline hull = pts.ConvexHull();
        if (!hull.IsClosed)
            hull.Close();
        hull.AntiClockwise();
        return hull;
    }
}
