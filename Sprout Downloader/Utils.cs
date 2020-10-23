using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace Sprout_Downloader
{
    internal class Utils
    {
        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            var arr = new byte[hex.Length >> 1];

            for (var i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte) ((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            var val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static void Decrypt(FileStream mainFile, FileStream segment, Key key)
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.Zeros;

                aes.Key = key.Bytes;
                aes.IV = key.Iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var cryptoStream = new CryptoStream(segment, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(mainFile);
                    }
                }
            }
        }

        public static void ConvertToMp4(string mainFile, Action callback)
        {
            var filename = mainFile + ".ts";
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments =
                    $"/C ffmpeg -y -i \"{Path.GetFullPath(filename)}\" -map 0 -c copy \"{Path.GetFullPath(mainFile + ".mp4")}\""
            };
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                if (process.ExitCode == 0)
                    File.Delete(filename);

                callback();
            };
            process.Start();
        }
    }
}