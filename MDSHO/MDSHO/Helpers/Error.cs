using System;

namespace MDSHO.Helpers
{
    public static class Error
    {
        public static void Show(Exception ex, string title = null)
        {
            ErrorWindow errorWindow = new ErrorWindow(ex, title);
            errorWindow.ShowDialog();
        }
    }
}
