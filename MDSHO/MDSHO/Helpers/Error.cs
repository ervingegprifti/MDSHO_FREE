using System;

namespace MDSHO.Helpers
{
    public static class Error
    {
        public static void Show(Exception ex)
        {
            ErrorWindow errorWindow = new ErrorWindow(ex);
            errorWindow.ShowDialog();
        }
    }
}
