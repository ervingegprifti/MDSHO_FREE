using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDSHO.Models
{
    [Serializable]
    public class Item
    {
        public string Guid { get; set; }
        public bool IsGroup { get; set; }
        public bool IsExpanded { get; set; }
        public string Name { get; set; }
        public string Target { get; set; }
        public string Ext { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();

        public Item(string guid, bool isGroup, bool isExpanded, string name, string target, string ext)
        {
            Guid = guid;
            IsGroup = isGroup;
            IsExpanded = isExpanded;
            Name = name;
            Target = target;
            Ext = ext;
        }

    }

}
