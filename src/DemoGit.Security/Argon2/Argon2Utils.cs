using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;

namespace DemoGit.Security.Argon2
{
    public static class Argon2Utils
    {
        private static readonly byte[] GeneratedSalt = CreateSalt();

        private static byte[] CreateSalt()
        {
            var buffer = new byte[128];

            if (!SaltExists())
            {
                using (var rg = RandomNumberGenerator.Create())
                {
                    rg.GetBytes(buffer);
                }

                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "encrypt.txt", buffer);

                return buffer;
            }

            buffer = File.ReadAllBytes(AppDomain.CurrentDomain.BaseDirectory + "encrypt.txt");

            return buffer;
        }

        private static bool SaltExists()
        {
            return File.Exists(AppDomain.CurrentDomain.BaseDirectory + "encrypt.txt");
        }

        public static byte[] HashPassword(string password)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            argon2.Salt = GeneratedSalt;
            argon2.DegreeOfParallelism = 16;
            argon2.Iterations = 40;
            argon2.MemorySize = 8192;

            return argon2.GetBytes(128);
        }

        public static bool VerifyHash(string password, byte[] hash)
        {
            var newHash = HashPassword(password);
            return hash.SequenceEqual(newHash);
        }
    }
}