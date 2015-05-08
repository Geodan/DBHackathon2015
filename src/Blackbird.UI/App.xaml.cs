using System;
using System.Windows;
using Blackbird.WPF.Logging;

namespace Blackbird.WPF
{
    public partial class App
    {
        public App()
        {
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += ExceptionHandler;
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            Log4netLogger.FatalFormat("Blackbird crashed - Message: {0} InnerException: {1}", e.Message, e.InnerException);
            MessageBox.Show(e.Message);
        }
    }
}
