using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSHO.Helpers
{
    public static class Constants
    {
        public const string EXT_LNK = ".lnk";
        public const string EXT_URL = ".url";
        public const string EXT_EXE = ".exe";

        public const string SHORTCUTS_VERSION = "1";

        // This is used in the save shortcuts data in order not to save per each save request, but to save only every 10 save requests for example.
        // (At the moment we have to set it to 1. If we use a number greater then 1 then we have to save at Application_SessionEnding)
        public const int SAVE_REQUEST_FREQUENCY = 1;
        public const int NR_OF_NEW_FILES_TO_KEEP = 30;
    }
}
