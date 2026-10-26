using ArchiGeo.Basic.Numeric;

namespace ArchiGeoSharp.Samples.Basic;

/// <summary>
/// RandomDistribution 波形采样演示。
/// </summary>
public static class BasicRandomDistributionSample
{
    public static void Run()
    {
        Console.WriteLine("=== basic-random-distribution: RandomDistribution ===");
        Console.WriteLine();

        Console.WriteLine("--- intWave（单位格 4）---");
        int[] arr = RandomDistribution.intWave(10, 16.3 / 4, 12 / 4, 24 / 4);
        foreach (int d in arr)
            Console.WriteLine(d * 4);
        Console.WriteLine($"平均值: {arr.Average() * 4}");
        Console.WriteLine();

        Console.WriteLine("--- unitWave ---");
        double[] arr2 = RandomDistribution.unitWave(10, 16.3, 12, 24, 4);
        foreach (double d in arr2)
            Console.WriteLine(d);
        Console.WriteLine($"平均值: {arr2.Average()}");
        Console.WriteLine();

        Console.WriteLine("--- doubleWave（目标均值约 50）---");
        double[] arr3 = RandomDistribution.doubleWave(10, 50.0, 40, 200.0);
        foreach (double d in arr3)
            Console.WriteLine(d);
        Console.WriteLine($"平均值: {arr3.Average()}");
        Console.WriteLine("完成。");
    }
}
