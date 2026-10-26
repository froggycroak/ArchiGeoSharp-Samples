namespace ArchiGeoSharp.Samples.Common;

/// <summary>
/// 控制台 sample 用的轻量断言（非 xUnit）。
/// </summary>
public static class SampleAssert
{
    public static void RunCase(string name, Action test)
    {
        Console.WriteLine($"\n=== {name} ===");
        try
        {
            test.Invoke();
        }
        catch (Exception ex)
        {
            // 教程 sample：打印失败信息后继续后续用例，不中断整个程序
            Console.WriteLine($"失败: {ex.Message}");
        }
    }

    public static void Equal(double[] actual, double[] expected, double tolerance = 1e-10)
    {
        if (actual.Length != expected.Length)
            throw new Exception($"长度不匹配：预期 {expected.Length}，实际 {actual.Length}");

        for (int i = 0; i < actual.Length; i++)
        {
            if (Math.Abs(actual[i] - expected[i]) > tolerance)
                throw new Exception($"元素 {i} 不匹配：预期 {expected[i]}，实际 {actual[i]}");
        }
        Console.WriteLine($"通过: [{string.Join(", ", actual)}]");
    }

    public static void ApproximateEqual(double[] actual, double[] expected, double threshold)
    {
        if (actual.Length != expected.Length)
            throw new Exception($"长度不匹配：预期 {expected.Length}，实际 {actual.Length}");

        for (int i = 0; i < actual.Length; i++)
        {
            double error = Math.Abs(actual[i] - expected[i]);
            double relativeError = expected[i] == 0 ? error : Math.Abs(error / expected[i]);
            if (error > threshold && relativeError > threshold)
                throw new Exception(
                    $"元素 {i} 超出阈值：预期 {expected[i]}，实际 {actual[i]}，绝对误差={error}");
        }
        Console.WriteLine($"通过: [{string.Join(", ", actual)}]（容差 ±{threshold}）");
    }
}
