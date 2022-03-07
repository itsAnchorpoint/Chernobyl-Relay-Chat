using System;
using System.Threading;
using System.Windows.Forms;

namespace Chernobyl_Relay_Chat
{
    static class Program
    {
        // GUID from AssemblyInfo.cs
        private static readonly Mutex mutex = new Mutex(false, "64eb5dda-2131-47fc-a32c-fbc64f440d8a");

        private static Thread displayThread, clientThread;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if !DEBUG
            // Prevent multiple instances
            if (!mutex.WaitOne(TimeSpan.FromSeconds(0), false))
                return;
#endif

            CRCStrings.Load();
            bool optionsLoaded = CRCOptions.Load();
            if (!optionsLoaded)
            {
                // No point localizing this since localization may not even be loaded
                MessageBox.Show("CRC was unable to access the registry, which is needed to preserve settings.\r\n"
                    + "Please try running the application As Administrator.\r\n\r\n"
                    + "CRC не смог получить доступ к реестру, который необходим для сохранения настроек.\r\n"
                    + "Попробуйте запустить приложение с правами администратора.",
                    CRCStrings.Localize("crc_name"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CRCUpdate.CheckFirstUpdate())
                return;

            displayThread = new Thread(CRCDisplay.Start);
            displayThread.Start();
            clientThread = new Thread(CRCClient.Start);
            clientThread.Start();

            clientThread.Join();
            displayThread.Join();
            CRCOptions.Save();
        }
    }
}
