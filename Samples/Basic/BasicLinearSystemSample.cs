using ArchiGeoSharp.Samples.Common;
using static ArchiGeo.Basic.Numeric.MatrixSimple;

namespace ArchiGeoSharp.Samples.Basic;

/// <summary>
/// MatrixSimple.SolveLinearEquation 若干用例。
/// </summary>
public static class BasicLinearSystemSample
{
    public static void Run()
    {
        Console.WriteLine("=== basic-linear-system: MatrixSimple ===");

        SampleAssert.RunCase("基础 2x2 方程组", () =>
        {
            double[,] matrix = { { 2, 1 }, { 1, -1 } };
            double[] vector = { 5, 1 };
            double[] expected = { 2, 1 };
            SampleAssert.Equal(SolveLinearEquation(matrix, vector), expected);
        });

        SampleAssert.RunCase("主元为 0 需行交换", () =>
        {
            double[,] matrix = { { 0, 1 }, { 1, 0 } };
            double[] vector = { 1, 2 };
            double[] expected = { 2, 1 };
            SampleAssert.Equal(SolveLinearEquation(matrix, vector), expected);
        });

        SampleAssert.RunCase("3x3 方程组唯一解", () =>
        {
            double[][] matrix2 =
            [
                [1, 1, 1],
                [2, 1, 3],
                [1, 2, 1]
            ];
            double[] vector = [6.0, 13, 8];
            double[] expected = [1, 2, 3];
            double[] solution = SolveLinearEquation(matrix2, vector);
            Console.WriteLine($"解 = ({solution[0]}, {solution[1]}, {solution[2]})");
            SampleAssert.Equal(solution, expected);
        });

        SampleAssert.RunCase("浮点数精度（弱主元）", () =>
        {
            // 0.0001 x + y = 1;  x + y = 2  →  x ≈ 1.0001, y ≈ 0.9999
            double[,] matrix = { { 0.0001, 1 }, { 1, 1 } };
            double[] vector = { 1, 2 };
            double[] expected = { 1.00010001, 0.99989999 };
            SampleAssert.ApproximateEqual(SolveLinearEquation(matrix, vector), expected, 1e-4);
        });

        SampleAssert.RunCase("大数主元稳定性", () =>
        {
            double[,] matrix = { { 1e15, 1 }, { 1, 1 } };
            double[] vector = { 1e15 + 1, 2 };
            double[] expected = { 1, 1 };
            SampleAssert.ApproximateEqual(SolveLinearEquation(matrix, vector), expected, 1e-10);
        });

        SampleAssert.RunCase("4x4 方程组", () =>
        {
            double[,] matrix =
            {
                { 2, 1, 1, 1 },
                { 1, 2, 1, 1 },
                { 1, 1, 2, 1 },
                { 1, 1, 1, 2 }
            };
            double[] vector = { 6, 7, 8, 9 };
            double[] expected = { 0, 1, 2, 3 };
            SampleAssert.Equal(SolveLinearEquation(matrix, vector), expected);
        });

        SampleAssert.RunCase("奇异矩阵应抛异常", () =>
        {
            double[,] matrix = { { 1, 2 }, { 2, 4 } };
            double[] vector = { 3, 6 };
            try
            {
                SolveLinearEquation(matrix, vector);
                throw new Exception("未抛出预期异常");
            }
            catch (ArgumentException ex) when (ex.Message.Contains("singular", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("通过：检测到奇异矩阵");
            }
        });

        SampleAssert.RunCase("非方阵应抛异常", () =>
        {
            double[,] matrix = { { 1, 2, 3 }, { 4, 5, 6 } };
            double[] vector = { 7, 8 };
            try
            {
                SolveLinearEquation(matrix, vector);
                throw new Exception("未抛出预期异常");
            }
            catch (ArgumentException ex) when (ex.Message.Contains("square", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("通过：检测到非方阵");
            }
        });

        Console.WriteLine("\n全部用例完成。");
    }
}
