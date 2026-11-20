using System;
using System.Windows.Forms;

namespace examen_marlon_de_la_cruz
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmAlquiler());
        }
    }
}
