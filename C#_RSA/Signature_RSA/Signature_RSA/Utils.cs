using System;
using System.Numerics;
using System.Security.Cryptography;

namespace WpfApp1
{
    class Utils
    {
        public BigInteger P, Q, N, E, D, Phi;

        public void sinhKhoaNhap(BigInteger p, BigInteger q)
        {
            if (p == q || !IsProbablePrime(p) || !IsProbablePrime(q))
            {
                throw new ArgumentException("Số nguyên tố không hợp lệ hoặc p và q bằng nhau.");
            }

            P = p;
            Q = q;
            N = P * Q;
            Phi = (P - 1) * (Q - 1);
            E = GenerateCoprime(Phi);
            D = ModInverse(E, Phi);
        }

        public void sinhKhoa()
        {
            P = GenerateLargePrime(1024);
            do
            {
                Q = GenerateLargePrime(1024);
            } while (P == Q);

            N = P * Q;
            Phi = (P - 1) * (Q - 1);
            E = GenerateCoprime(Phi);
            D = ModInverse(E, Phi);
        }

        public bool IsProbablePrime(BigInteger source, int certainty = 10)
        {
            if (source == 2 || source == 3)
                return true;

            if (source < 2 || source % 2 == 0)
                return false;

            BigInteger d = source - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[source.ToByteArray().LongLength];
                for (int i = 0; i < certainty; i++)
                {
                    BigInteger a;
                    do
                    {
                        rng.GetBytes(bytes);
                        a = new BigInteger(bytes);
                    } while (a < 2 || a >= source - 2);

                    BigInteger x = BigInteger.ModPow(a, d, source);
                    if (x == 1 || x == source - 1)
                        continue;

                    for (int r = 1; r < s; r++)
                    {
                        x = BigInteger.ModPow(x, 2, source);
                        if (x == 1)
                            return false;
                        if (x == source - 1)
                            break;
                    }

                    if (x != source - 1)
                        return false;
                }
            }
            return true;
        }

        public BigInteger GenerateLargePrime(int bits)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[bits / 8];
                BigInteger prime;

                do
                {
                    rng.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= 0x7F;
                    prime = new BigInteger(bytes);
                } while (!IsProbablePrime(prime));

                return prime;
            }
        }

        public BigInteger GenerateCoprime(BigInteger phi)
        {
            BigInteger e;
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] bytes = new byte[phi.ToByteArray().Length];
                do
                {
                    rng.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= 0x7F; // Đảm bảo số không âm
                    e = new BigInteger(bytes) % (phi - 1) + 1;
                } while (BigInteger.GreatestCommonDivisor(e, phi) != 1 || e % 2 == 0);
            }

            return e;
        }

        public BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m, t, q;
            BigInteger x0 = 0, x1 = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;
                m = a % m;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
                x1 += m0;

            return x1;
        }

        public bool isChecked(BigInteger p, BigInteger q)
        {
            if (!IsProbablePrime(p) || !IsProbablePrime(q) || p == q)
            {
                return false;
            }
            return true;
        }
    }
}
