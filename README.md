# ArchiGeoSharp.Samples

ArchiGeoSharp 官方配套**教程案例**仓库。克隆后即可上手，**不依赖文档站**也能按 sample 学习 Basic 几何内核与 Raylex 可视化。

文档站（仍在完善）：[ArchiGeoSharp-site](https://froggycroak.github.io/ArchiGeoSharp-site/)

## 前置条件

- [.NET SDK 7.0+](https://dotnet.microsoft.com/download)（本工程目标框架为 `net7.0`；SDK 8/9 也可构建）
- **Basic** 控制台示例：任意平台
- **Raylex** 窗口示例：需要桌面环境（依赖 Raylib 原生窗口；推荐 Windows）

通过 NuGet 引用已发布的包（**无需**克隆主库源码）：

- [ArchiGeoSharp.Basic](https://www.nuget.org/packages/ArchiGeoSharp.Basic)
- [ArchiGeoSharp.Raylex](https://www.nuget.org/packages/ArchiGeoSharp.Raylex)

## 快速开始

```bash
git clone https://github.com/froggycroak/ArchiGeoSharp.Samples.git
cd ArchiGeoSharp.Samples

dotnet restore
dotnet run                  # 打印菜单
dotnet run -- basic-plane   # 运行指定 sample
```

无参数或 `list` / `help` 会列出全部 sample id。

## Sample 索引

### Basic（控制台）

| id | 主题 | 说明 |
|----|------|------|
| `basic-plane` | Plane / Space3d | 局部↔世界坐标与平移（入门最小示例） |
| `basic-angle-tripple` | AngleTripple | 欧拉 / 四元数、逆变换、复合，与 Space3d 对齐 |
| `basic-linear-system` | MatrixSimple | 线性方程组求解若干用例 |
| `basic-random-distribution` | RandomDistribution | 整数 / 浮点波形采样 |

### Raylex（RWindow 交互）

| id | 主题 | 说明 |
|----|------|------|
| `raylex-polyline-obb` | Polyline OBB | 封闭折线定向包围盒 |
| `raylex-arc` | Arc | 圆弧与圆心 / 端点 |
| `raylex-hemesh` | HEMesh | 半边网格顶点与轮廓 |
| `raylex-box-3d` | Box + Mode3D | 正交 3D 渲染；右键拖相机 |
| `raylex-prism-mesh` | Prism | 手绘场地 → 拉伸棱柱 → 3D |
| `raylex-buffer-open` | Polyline.Buffer | 开放折线缓冲；Tab / S |
| `raylex-offset-simp` | OffsetSimp | 封闭折线简单偏移 |
| `raylex-bsp-subdivision` | AngleGrid + BSP | 包围盒角度场 + BSP 剖分；R / Tab / `[` `]` |

## 在自己的项目里使用 NuGet

```bash
dotnet new console -n MyApp -f net7.0
cd MyApp
dotnet add package ArchiGeoSharp.Basic
dotnet add package ArchiGeoSharp.Raylex   # 可选，可视化
```

最小几何示例见 `basic-plane` 源码，或文档站 [快速上手](https://froggycroak.github.io/ArchiGeoSharp-site/guide/getting-started)。

## 仓库结构

```
ArchiGeoSharp.Samples.csproj / .sln
Program.cs                 # 菜单与入口
Samples/
  SampleCatalog.cs         # id → 入口注册
  Common/                  # 轻量断言等
  Basic/                   # 控制台案例
  Raylex/                  # 窗口案例
LICENSE                    # MIT（仅本仓库示例源码）
```

## 许可

- **本仓库示例源码**：MIT（见 [LICENSE](LICENSE)）
- **ArchiGeoSharp.\* NuGet 包**：专有许可（二进制经混淆；公共 API 保留）。摘要见 [许可说明](https://froggycroak.github.io/ArchiGeoSharp-site/license)。使用包时请遵守其条款；示例代码的 MIT **不**改变包的许可。

## 相关链接

| 资源 | URL |
|------|-----|
| Basic NuGet | https://www.nuget.org/packages/ArchiGeoSharp.Basic |
| Raylex NuGet | https://www.nuget.org/packages/ArchiGeoSharp.Raylex |
| 文档站 | https://froggycroak.github.io/ArchiGeoSharp-site/ |
| 主库（内核源码私有） | https://github.com/froggycroak/ArchiGeoSharp |
