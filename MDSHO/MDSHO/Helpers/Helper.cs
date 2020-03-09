using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace MDSHO.Helpers
{
    public static class Helper
    {
        public static string GetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static string GetApplicationName()
        {
            return Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}
