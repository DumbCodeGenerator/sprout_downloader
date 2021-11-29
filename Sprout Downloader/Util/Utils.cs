using Sprout_Downloader.Json;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Sprout_Downloader.Util
{
    internal class Utils
    {
        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + GetHexVal(hex[(i << 1) + 1]));

            return arr;
        }

        private static int GetHexVal(char hex)
        {
            char val = hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static void Decrypt(FileStream mainFile, FileStream segment, Key key)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.Zeros;

            aes.Key = key.Bytes;
            aes.IV = key.Iv;

            using ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using CryptoStream cryptoStream = new(segment, decryptor, CryptoStreamMode.Read);
            cryptoStream.CopyTo(mainFile);
        }

        public static void ConvertToMp4(string fileName, Action callback)
        {
            string mainFile = fileName + ".ts";
            Process process = new();
            ProcessStartInfo startInfo = new()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "cmd.exe",
                Arguments =
                    $"/C ffmpeg -y -i \"{Path.GetFullPath(mainFile)}\" -map 0 -c copy \"{Path.GetFullPath(Path.GetFileNameWithoutExtension(fileName) + ".mp4")}\""
            };
            process.StartInfo = startInfo;
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) =>
            {
                if (process.ExitCode == 0)
                    File.Delete(mainFile);

                callback();
            };
            process.Start();
        }
    }
}