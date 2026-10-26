# ArchiGeoSharp.Samples

ArchiGeoSharp 官方配套**教程案例**。以 **Raylex 窗口交互** 为主：手绘折线、调参、看结果。另附 **1 个** Basic 控制台入门，不依赖文档站也能上手。

文档站（仍在完善）：[ArchiGeoSharp-site](https://froggycroak.github.io/ArchiGeoSharp-site/)

## 前置条件

- [.NET SDK 7.0+](https://dotnet.microsoft.com/download)（工程目标框架 `net7.0`）
- Raylex 案例需要**桌面环境**（Raylib 窗口；推荐 Windows）

NuGet（无需克隆主库源码）：

- [ArchiGeoSharp.Basic](https://www.nuget.org/packages/ArchiGeoSharp.Basic)
- [ArchiGeoSharp.Raylex](https://www.nuget.org/packages/ArchiGeoSharp.Raylex)

## 快速开始

```bash
git clone https://github.com/froggycroak/ArchiGeoSharp.Samples.git
cd ArchiGeoSharp.Samples

dotnet restore
dotnet run                       # 打印菜单
dotnet run -- basic-plane        # 控制台入门
dotnet run -- raylex-buffer-open # 交互 Buffer
```

## 操作约定（Raylex）

| 操作 | 含义 |
|------|------|
| 左键 | 折线加点 |
| 右键 | 结束当前折线（开放或封闭视案例而定） |
| 中键 | 重置当前案例状态 |
| Tab / `[` `]` / `-` `=` / R / S | 见各案例屏幕提示 |

关闭窗口即退出该 sample。

## Sample 索引

### Basic（控制台，仅入门）

| id | 说明 |
|----|------|
| `basic-plane` | Plane + Space3d：局部↔世界坐标与平移 |

### Raylex（全部可交互）

| id | 说明 |
|----|------|
| `raylex-polyline-obb` | 手绘封闭折线 → 定向包围盒 OBB |
| `raylex-buffer-open` | 开放折线 `Buffer`；Tab / S 切换参数 |
| `raylex-trim-ends` | `TrimEnds`；`[` `]` 调裁剪距离 |
| `raylex-simplify-min-edge` | `SimplifyMinEdge`；Tab / `[` `]` / `-` `=` |
| `raylex-relation-poly` | 两条折线的 `RelateSimp` |
| `raylex-prism-mesh` | 手绘场地 → Prism → Mode3D（右键拖相机） |
| `raylex-bsp-subdivision` | AngleGrid + BSP；R 随机、Tab 模式、`[` `]` |

## 在自己的项目里用 NuGet

```bash
dotnet new console -n MyApp -f net7.0
cd MyApp
dotnet add package ArchiGeoSharp.Basic
dotnet add package ArchiGeoSharp.Raylex
```

几何最小代码见 `basic-plane`，或文档站 [快速上手](https://froggycroak.github.io/ArchiGeoSharp-site/guide/getting-started)。

## 许可

- **本仓库示例源码**：MIT（见 [LICENSE](LICENSE)）
- **ArchiGeoSharp.\* 包**：专有许可 — [许可说明](https://froggycroak.github.io/ArchiGeoSharp-site/license)

## 相关链接

| 资源 | URL |
|------|-----|
| Basic | https://www.nuget.org/packages/ArchiGeoSharp.Basic |
| Raylex | https://www.nuget.org/packages/ArchiGeoSharp.Raylex |
| 文档站 | https://froggycroak.github.io/ArchiGeoSharp-site/ |
