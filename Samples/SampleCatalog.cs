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
/// Raylex 案例均为窗口交互；Basic 仅保留一个控制台入门。
/// </summary>
public static class SampleCatalog
{
    public static IReadOnlyList<SampleEntry> All { get; } =
    [
        new("basic-plane", "Basic", "Plane + Space3d",
            "【控制台入门】创建平面并做局部/世界坐标与平移。",
            BasicPlaneSample.Run),

        new("raylex-polyline-obb", "Raylex", "Polyline OBB",
            "手绘封闭折线 → OrientedBoundingBox（红输入 / 蓝 OBB）。",
            RaylexPolylineObbSample.Run),

        new("raylex-buffer-open", "Raylex", "Polyline.Buffer (open)",
            "手绘开放折线 Buffer；Tab/S 切换 edgeBuffer / softEdge。",
            RaylexBufferOpenSample.Run),

        new("raylex-trim-ends", "Raylex", "Polyline.TrimEnds",
            "手绘开放折线两端裁剪；[ / ] 调节 trim。",
            RaylexTrimEndsSample.Run),

        new("raylex-simplify-min-edge", "Raylex", "SimplifyMinEdge",
            "手绘折线简化；Tab 开闭，[/] 调 t，-/= 调 ε。",
            RaylexSimplifyMinEdgeSample.Run),

        new("raylex-relation-poly", "Raylex", "Relation (poly-poly)",
            "依次手绘两条折线，显示 RelateSimp 拓扑关系。",
            RaylexRelationPolySample.Run),

        new("raylex-prism-mesh", "Raylex", "Prism Mode3D",
            "手绘场地 → Prism 拉伸 → Mode3D；右键拖相机。",
            RaylexPrismMeshSample.Run),

        new("raylex-bsp-subdivision", "Raylex", "AngleGrid + BSP",
            "随机/手绘场地 → AngleGrid + BSP 剖分；R / Tab / [ ]。",
            RaylexBspSubdivisionSample.Run),
    ];

    public static void PrintMenu()
    {
        Console.WriteLine("ArchiGeoSharp.Samples — 教程配套案例（以交互为主）");
        Console.WriteLine();
        Console.WriteLine("用法:");
        Console.WriteLine("  dotnet run                     # 显示本菜单");
        Console.WriteLine("  dotnet run -- <sample-id>      # 运行指定 sample");
        Console.WriteLine("  dotnet run -- list             # 同菜单");
        Console.WriteLine();
        Console.WriteLine("操作约定（Raylex）:");
        Console.WriteLine("  左键加点 / 右键结束折线 / 中键重置；部分案例有 Tab、[ ] 等快捷键。");
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
