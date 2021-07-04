using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;


namespace RgbController
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool NewWindow;
            Mutex Mut = new Mutex(true, "RgbController", out NewWindow);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!NewWindow)
                return;

            Application.Run(new RgbController());
            GC.KeepAlive(Mut);
        }
    }
}
