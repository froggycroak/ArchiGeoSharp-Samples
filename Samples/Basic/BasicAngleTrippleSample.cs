using ArchiGeo.Basic.Geometry;
using ArchiGeo.Basic.Numeric;
using ArchiGeo.Basic.Transform;

namespace ArchiGeoSharp.Samples.Basic;

/// <summary>
/// AngleTripple：四元数、欧拉往返、逆变换、复合，以及与 Space3d 的一致性。
/// </summary>
public static class BasicAngleTrippleSample
{
    public static void Run()
    {
        Console.WriteLine("=== basic-angle-tripple: AngleTripple ===");
        const double tolVec = 1e-9;

        static void Fail(string msg) => throw new InvalidOperationException(msg);

        static bool VecNear(Vector a, Vector b, double tol)
        {
            a = a.DimensionalShift(3);
            b = b.DimensionalShift(3);
            return DoubleMath.TildeEqual(a[0], b[0], tol)
                   && DoubleMath.TildeEqual(a[1], b[1], tol)
                   && DoubleMath.TildeEqual(a[2], b[2], tol);
        }

        static bool UnitVecNear(Vector a, double x, double y, double z, double tol) =>
            VecNear(a, new Vector(x, y, z), tol);

        AngleTripple id = AngleTripple.Zero;
        var (qw, qx, qy, qz) = id.Quaternion;
        double qnorm = Math.Sqrt(qw * qw + qx * qx + qy * qy + qz * qz);
        if (!DoubleMath.TildeEqual(qnorm, 1.0, 1e-12))
            Fail("quaternion 应为单位长度");

        if (!UnitVecNear(id.XDir, 1, 0, 0, tolVec) || !UnitVecNear(id.YDir, 0, 1, 0, tolVec)
            || !UnitVecNear(id.ZDir, 0, 0, 1, tolVec))
            Fail("Zero 应对应世界 XYZ 轴");

        AngleTripple at1 = new(new Angle(30), new Angle(-45), new Angle(60));
        if (!VecNear(Vector.Cross(at1.XDir, at1.YDir), at1.ZDir, tolVec))
            Fail("X×Y 应等于 Z");

        Space3d s1 = at1.ToSpace3d;
        if (!VecNear(at1.XDir, s1.XDir.Normalize, tolVec) || !VecNear(at1.YDir, s1.YDir.Normalize, tolVec))
            Fail("ToSpace3d 的 X/Y 方向应与 AngleTripple 一致");

        var (roll0, pitch0, yaw0) = at1.ToEulerZYX();
        AngleTripple at2 = new(roll0, pitch0, yaw0);
        Vector v0 = new(0.37, -0.21, 0.93);
        if (!VecNear(v0 * at1, v0 * at2, tolVec))
            Fail("欧拉往返后，对同向量的旋转结果应一致");
        if (!VecNear(at1.XDir, at2.XDir, tolVec) || !VecNear(at1.YDir, at2.YDir, tolVec))
            Fail("ToEulerZYX 后再构造，X/Y 轴应与原旋转一致");

        Vector wv = new(2, -5, 7);
        if (!VecNear(wv, wv * at1 / at1, tolVec))
            Fail("vec * at / at 应回到原向量");
        Point wp = new(11, -3, 0.5);
        if (!wp.TildeEqual(wp * at1 / at1, tolVec))
            Fail("pt * at / at 应回到原点");

        AngleTripple a = new(new Angle(12), new Angle(5), new Angle(-33));
        AngleTripple b = new(new Angle(-7), new Angle(18), new Angle(40));
        AngleTripple ab = a * b;
        if (!VecNear(v0 * ab, (v0 * b) * a, tolVec))
            Fail("四元数复合应与分步旋转一致");

        AngleTripple fromXy = new(at1.XDir, at1.YDir);
        if (!VecNear(v0 * at1, v0 * fromXy, tolVec))
            Fail("FromXY(X,Y) 应与由欧拉构造的旋转一致");

        // 与 Space3d AbsoluteDescribe 对齐（绕 +Z 60°）
        AngleTripple atZ = new(Angle.Zero, Angle.Zero, new Angle(60));
        Space3d spZ = atZ.ToSpace3d;
        Point probe = new(2.3, -1.4, 7.0);
        if (!atZ.AbsoluteDescribe(probe).TildeEqual(spZ.AbsoluteDescribe(probe), tolVec))
            Fail("AngleTripple 与 Space3d 的 AbsoluteDescribe(点) 应一致");

        Random rnd = new(20260417);
        for (int k = 0; k < 50; k++)
        {
            double dr = rnd.NextDouble() * 360 - 180;
            double dp = rnd.NextDouble() * 160 - 80;
            double dy = rnd.NextDouble() * 360 - 180;
            AngleTripple g = new(new Angle(dr), new Angle(dp), new Angle(dy));
            var (wq, qix, qiy, qiz) = g.Quaternion;
            double nq = Math.Sqrt(wq * wq + qix * qix + qiy * qiy + qiz * qiz);
            if (!DoubleMath.TildeEqual(nq, 1.0, 1e-11))
                Fail($"样本 {k}: 四元数模长应为 1");

            Vector vv = new(rnd.NextDouble() * 2 - 1, rnd.NextDouble() * 2 - 1, rnd.NextDouble() * 2 - 1);
            if (!VecNear(vv, vv * g / g, tolVec))
                Fail($"样本 {k}: 逆变换 round-trip 失败");

            double crossLen = Vector.Cross(g.XDir, g.YDir).Magnitude;
            if (!DoubleMath.TildeEqual(crossLen, 1.0, tolVec))
                Fail($"样本 {k}: X、Y 应为单位正交");
        }

        Console.WriteLine("全部检查通过。");
    }
}
