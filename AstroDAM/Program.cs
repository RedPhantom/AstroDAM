using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AstroDAM
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // https://stackoverflow.com/questions/17953298/c-sharp-application-exits-when-main-form-closes
            var StartupForm = new frmSplash();
            StartupForm.Show();

            Application.Run();
        }
    }
}
