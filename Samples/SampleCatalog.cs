using ArchiGeoSharp.Samples.Basic;
using ArchiGeoSharp.Samples.Raylex;

namespace ArchiGeoSharp.Samples;

public sealed record SampleEntry(
    string Id,
    string Category,
    string Title,
    string Description,
    Action Run);

/// <summary>
/// 教程 sample 目录：id → 可运行入口。
/// </summary>
public static class SampleCatalog
{
    public static IReadOnlyList<SampleEntry> All { get; } =
    [
        new("basic-plane", "Basic", "Plane + Space3d",
            "创建平面并做局部/世界坐标与平移变换（入门最小示例）。",
            BasicPlaneSample.Run),

        new("basic-angle-tripple", "Basic", "AngleTripple",
            "欧拉角 / 四元数、逆变换、复合，以及与 Space3d 的一致性检查。",
            BasicAngleTrippleSample.Run),

        new("basic-linear-system", "Basic", "MatrixSimple",
            "用 MatrixSimple.SolveLinearEquation 解若干线性方程组用例。",
            BasicLinearSystemSample.Run),

        new("basic-random-distribution", "Basic", "RandomDistribution",
            "RandomDistribution 整数/浮点波形采样与均值观察。",
            BasicRandomDistributionSample.Run),

        new("raylex-polyline-obb", "Raylex", "Polyline OBB",
            "封闭折线的定向包围盒（OBB），在 RWindow 中绘制。",
            RaylexPolylineObbSample.Run),

        new("raylex-arc", "Raylex", "Arc",
            "圆弧几何：圆心、起终点在顶视图中可视化。",
            RaylexArcSample.Run),

        new("raylex-hemesh", "Raylex", "HEMesh",
            "半边网格顶点构造，并在窗口中绘制四边形轮廓。",
            RaylexHemeshSample.Run),

        new("raylex-box-3d", "Raylex", "Box Mode3D",
            "正交 3D 相机下渲染 Box，右键拖动更新相机。",
            RaylexBox3dSample.Run),

        new("raylex-prism-mesh", "Raylex", "Prism",
            "手绘场地多边形 → Prism 拉伸 → Mode3D 渲染。",
            RaylexPrismMeshSample.Run),

        new("raylex-buffer-open", "Raylex", "Polyline.Buffer (open)",
            "开放折线 Buffer；Tab/S 切换 edgeBuffer / softEdge。",
            RaylexBufferOpenSample.Run),

        new("raylex-offset-simp", "Raylex", "Polyline.OffsetSimp",
            "封闭折线简单偏移（OffsetSimp）；红=输入，蓝=结果。",
            RaylexOffsetSimpSample.Run),

        new("raylex-bsp-subdivision", "Raylex", "AngleGrid + BSP",
            "场地包围盒上构造 AngleGrid → BSP 编码/自适应剖分可视化。",
            RaylexBspSubdivisionSample.Run),
    ];

    public static void PrintMenu()
    {
        Console.WriteLine("ArchiGeoSharp.Samples — 教程配套案例");
        Console.WriteLine();
        Console.WriteLine("用法:");
        Console.WriteLine("  dotnet run                     # 显示本菜单");
        Console.WriteLine("  dotnet run -- <sample-id>      # 运行指定 sample");
        Console.WriteLine("  dotnet run -- list             # 同菜单");
        Console.WriteLine();
        Console.WriteLine("Sample 列表:");
        Console.WriteLine();

        string? lastCat = null;
        foreach (SampleEntry s in All)
        {
            if (s.Category != lastCat)
            {
                Console.WriteLine($"  [{s.Category}]");
                lastCat = s.Category;
            }
            Console.WriteLine($"    {s.Id,-28} {s.Title}");
            Console.WriteLine($"    {"".PadLeft(28)} {s.Description}");
            Console.WriteLine();
        }
    }

    public static bool TryRun(string id, out string? error)
    {
        SampleEntry? match = All.FirstOrDefault(
            s => string.Equals(s.Id, id, StringComparison.OrdinalIgnoreCase));

        if (match is null)
        {
            error = $"未知 sample id: \"{id}\"。请使用 list 查看可用 id。";
            return false;
        }

        Console.WriteLine($">>> 运行: {match.Id} — {match.Title}");
        Console.WriteLine($"    {match.Description}");
        Console.WriteLine();
        match.Run();
        error = null;
        return true;
    }
}
