using System;
using System.Windows.Media;
using MDSHO.Models;
using MDSHO.Helpers;

namespace MDSHO.Data
{
    public static class NewData
    {
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString().ToUpper();
        }
        public static Item NewGroup()
        {
            return new Item(NewGuid(), true, true, "New group", "", "");
        }
        public static WindowItem NewWindow()
        {
            WindowItem windowItem = new WindowItem
            (   
                left: 0,
                top: 0,
                width: 400,
                height: 500,
                title: "MDSHO",
                windowBorderColor: Colors.Black.ToInt(),
                titleBackgroundColor: Colors.Black.ToInt(),
                titleBackgroundOpacity: 1.0,
                titleTextColor: Colors.White.ToInt(),
                windowBackgroundColor: Colors.Black.ToInt(),
                windowBackgroundOpacity: 0.7,
                windowTextColor: Colors.White.ToInt()
            );
            return windowItem;
        }
        public static Shortcuts NewShortcuts()
        {
            WindowItem windowItem = NewWindow();
            Shortcuts shortcuts = new Shortcuts(Constants.SHORTCUTS_VERSION);
            shortcuts.WindowItems.Add(windowItem);
            return shortcuts;
        }
    }
}


