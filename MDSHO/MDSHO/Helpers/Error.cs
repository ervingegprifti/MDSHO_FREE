using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MDSHO.Helpers
{
    public static class Error
    {
        public static void ShowDialog(Exception ex)
        {
            ErrorWindow errorWindow = new ErrorWindow(ex);
            errorWindow.ShowDialog();
        }
    }
}
