using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace ControlPad
{
    public partial class App : Application
    {
        private static Mutex _mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            const string mutexName = "ControlPad";
            bool createdNew;

            _mutex = new Mutex(true, mutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Control Pad is already open.");
                Current.Shutdown();
                return;
            }

            base.OnStartup(e);

            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            bool startHidden = e.Args.Any(a => string.Equals(a, "--hidden", StringComparison.OrdinalIgnoreCase));

            var mw = new MainWindow();
            Current.MainWindow = mw;

            var helper = new WindowInteropHelper(mw);
            _ = helper.EnsureHandle();

            mw.Show();

            if (startHidden)
                mw.Hide();
        }
    }

}