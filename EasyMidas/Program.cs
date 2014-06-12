using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace EasyMidas
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Xceed.DockingWindows.Licenser.LicenseKey = "DWN21-98MLR-9812S-JGDA";
            Xceed.Grid.Licenser.LicenseKey = "GRD38-NWMX3-2NF8E-ZKPA";

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
