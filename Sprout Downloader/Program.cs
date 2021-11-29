namespace Sprout_Downloader
{
    internal static class Program
    {
        private static CancellationTokenSource _cts = new();
        /// <summary>
        ///     Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        public static void Cancel()
        {
            _cts.Cancel();
        }

        public static void RefreshCTS()
        {
            _cts = new();
        }

        public static CancellationToken GetCancellationToken()
        {
            return _cts.Token;
        }

        public static bool IsCancellationRequested()
        {
            return GetCancellationToken().IsCancellationRequested;
        }
    }
}