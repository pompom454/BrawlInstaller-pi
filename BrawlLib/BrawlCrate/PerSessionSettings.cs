using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BrawlLib.BrawlCrate
{
    public static class PerSessionSettings
    {
        public static readonly bool IsBrawlCrateGold = BrawlCrateGoldVerification();

        private static bool BrawlCrateGoldVerification()
        {
            byte[] brawlCrateGoldMd5 =
            {0x14, 0x36, 0x5B, 0xA2, 0xCC, 0x18, 0x9A, 0x44, 0x29, 0x84, 0x88, 0xA1, 0x79, 0xBC, 0x55, 0x6D};

            string curProcessPath;
            try
            {
                curProcessPath = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
            }
            catch
            {
                curProcessPath = string.Empty;
            }

            if (string.IsNullOrEmpty(curProcessPath))
            {
                curProcessPath = AppContext.BaseDirectory;
            }

            string certificatePath = Path.Combine(Path.GetDirectoryName(curProcessPath) ?? string.Empty, "BrawlCrateGoldCertificate");

            if (File.Exists(certificatePath))
            {
                using (MD5 md5 = MD5.Create())
                using (FileStream stream = File.OpenRead(certificatePath))
                {
                    byte[] hash = md5.ComputeHash(stream);
                    return hash.SequenceEqual(brawlCrateGoldMd5);
                }
            }

            return false;
        }

        public static readonly bool ProgramBirthday = DateTime.Now.Day == 8 && DateTime.Now.Month == 4;

        public static readonly bool BrawlCrateBirthday = DateTime.Now.Day == 30 && DateTime.Now.Month == 8;
    }
}