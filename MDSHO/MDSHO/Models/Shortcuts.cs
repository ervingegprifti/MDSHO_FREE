using System;
using System.Collections.Generic;

namespace MDSHO.Models
{
    [Serializable]
    public class Shortcuts
    {
        public string Version { get; set; }
        public List<WindowItem> WindowItems { get; set; }

        public Shortcuts(string version)
        {
            Version = version;
            // Set the default values
            WindowItems = new List<WindowItem>();
        }
    }
}
