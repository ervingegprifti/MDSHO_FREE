using MDSHO.Helpers;
using MDSHO.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MDSHO.Models;
using MDSHO.Data;


namespace MDSHO.Helpers
{
    public static class ContextHelper
    {
        public static ItemVM ItemToVM(Item item)
        {
            ItemVM itemVM = new ItemVM(item.Guid, item.IsGroup, item.IsExpanded, item.Name, item.Target, item.Ext);
            foreach (Item childItem in item.Items)
            {
                ItemVM childItemVM = ItemToVM(childItem);
                itemVM.ItemVMs.Add(childItemVM);
            }
            return itemVM;
        }
        public static Item VMToItem(ItemVM itemVM)
        {
            Item item = new Item(itemVM.Guid, itemVM.IsGroup, itemVM.IsExpanded, itemVM.Name, itemVM.Target, itemVM.Ext);
            foreach (ItemVM childItemVM in itemVM.ItemVMs)
            {
                Item childItem = VMToItem(childItemVM);
                item.Items.Add(childItem);
            }
            return item;
        }
        public static ObservableCollection<ItemVM> ItemsToVMs(List<Item> items)
        {
            ObservableCollection<ItemVM> itemVMs = new ObservableCollection<ItemVM>();
            foreach (Item item in items)
            {
                ItemVM itemVM = ItemToVM(item);
                itemVMs.Add(itemVM);
            }
            return itemVMs;
        }
        public static List<Item> VMsToItems(ObservableCollection<ItemVM> itemVMs)
        {
            List<Item> items = new List<Item>();
            foreach (ItemVM itemVM in itemVMs)
            {
                Item item = VMToItem(itemVM);
                items.Add(item);
            }
            return items;
        }

        public static WindowItemVM WindowItemToVM(WindowItem windowItem)
        {
            WindowItemVM windowItemVM = new WindowItemVM
            (
                isClone: false,
                left: windowItem.Left,
                top: windowItem.Top,
                width: windowItem.Width,
                height: windowItem.Height,
                windowInfoVM: new WindowInfoVM
                (
                    title: windowItem.Title,
                    windowBorderSolidColorBrush: windowItem.WindowBorderColor.ToColor().ToSolidColorBrush(),
                    titleBackgroundSolidColorBrush: windowItem.TitleBackgroundColor.ToColor().ToSolidColorBrush(),
                    titleBackgroundOpacity: windowItem.TitleBackgroundOpacity,
                    titleTextSolidColorBrush: windowItem.TitleTextColor.ToColor().ToSolidColorBrush(),
                    windowBackgroundSolidColorBrush: windowItem.WindowBackgroundColor.ToColor().ToSolidColorBrush(),
                    windowBackgroundOpacity: windowItem.WindowBackgroundOpacity,
                    windowTextSolidColorBrush: windowItem.WindowTextColor.ToColor().ToSolidColorBrush()
                )
            );

            // Convert Items to ItemVMs
            windowItemVM.RootItemVMs = ItemsToVMs(windowItem.Items);
            // Convert WindowClones to WindowItemVMClones
            foreach (WindowClone windowClone in windowItem.WindowClones)
            {
                WindowItemVM windowItemVMClone = new WindowItemVM(
                    true,
                    windowClone.Left,
                    windowClone.Top,
                    windowClone.Width,
                    windowClone.Height,
                    windowItemVM.WindowInfoVM);
                windowItemVMClone.RootItemVMs = windowItemVM.RootItemVMs;
                windowItemVM.WindowItemVMClones.Add(windowItemVMClone);
            }
            return windowItemVM;
        }
        public static WindowItem VMToWindowItem(WindowItemVM windowItemVM)
        {
            WindowItem windowItem = new WindowItem(
                windowItemVM.Left,
                windowItemVM.Top,
                windowItemVM.Width,
                windowItemVM.Height,
                windowItemVM.WindowInfoVM.Title,
                windowItemVM.WindowInfoVM.WindowBorderSolidColorBrush.Color.ToInt(),
                windowItemVM.WindowInfoVM.TitleBackgroundSolidColorBrush.Color.ToInt(),
                windowItemVM.WindowInfoVM.TitleBackgroundOpacity,
                windowItemVM.WindowInfoVM.TitleTextSolidColorBrush.Color.ToInt(),
                windowItemVM.WindowInfoVM.WindowBackgroundSolidColorBrush.Color.ToInt(),
                windowItemVM.WindowInfoVM.WindowBackgroundOpacity,
                windowItemVM.WindowInfoVM.WindowTextSolidColorBrush.Color.ToInt());
            // Convert ItemVMs to Items
            windowItem.Items = VMsToItems(windowItemVM.RootItemVMs);
            // Convert  WindowItemVMClones to WindowClones
            foreach (WindowItemVM windowItemVMClone in windowItemVM.WindowItemVMClones)
            {
                WindowClone windowClone = new WindowClone(windowItemVMClone.Left, windowItemVMClone.Top, windowItemVMClone.Width, windowItemVMClone.Height);
                windowItem.WindowClones.Add(windowClone);
            }
            return windowItem;
        }
        public static ObservableCollection<WindowItemVM> WindowItemsToVMs(List<WindowItem> windowItems)
        {
            ObservableCollection<WindowItemVM> windowItemVMs = new ObservableCollection<WindowItemVM>();
            foreach (WindowItem windowItem in windowItems)
            {
                WindowItemVM windowItemVM = WindowItemToVM(windowItem);
                windowItemVMs.Add(windowItemVM);
            }
            return windowItemVMs;
        }
        public static List<WindowItem> VMsToWindowItems(ObservableCollection<WindowItemVM> windowItemVMs)
        {
            List<WindowItem> windowItems = new List<WindowItem>();
            foreach (WindowItemVM windowItemVM in windowItemVMs)
            {
                WindowItem windowItem = VMToWindowItem(windowItemVM);
                windowItems.Add(windowItem);
            }
            return windowItems;
        }

        public static ShortcutsVM ShortcutsToVM(Shortcuts shortcuts)
        {
            ShortcutsVM shortcutsVM = new ShortcutsVM(shortcuts.Version);
            shortcutsVM.WindowItemVMs = WindowItemsToVMs(shortcuts.WindowItems);
            return shortcutsVM;
        }
        public static Shortcuts VMToShortcuts(ShortcutsVM shortcutsVM)
        {
            Shortcuts shortcuts = new Shortcuts(shortcutsVM.Version);
            shortcuts.WindowItems = VMsToWindowItems(shortcutsVM.WindowItemVMs);
            return shortcuts;
        }

        public static ItemVM DeepCloneItemVM(ItemVM source)
        {
            ItemVM itemVMCloned = new ItemVM(source.Guid, source.IsGroup, source.IsExpanded, source.Name, source.Target, source.Ext);
            foreach (ItemVM child in source.ItemVMs)
            {
                ItemVM childItemVM = DeepCloneItemVM(child);
                itemVMCloned.ItemVMs.Add(childItemVM);
            }
            return itemVMCloned;
        }
        public static ObservableCollection<ItemVM> DeepCloneItemVMs(ObservableCollection<ItemVM> itemVMs)
        {
            ObservableCollection<ItemVM> itemVMsCloned = new ObservableCollection<ItemVM>();
            foreach (ItemVM itemVM in itemVMs)
            {
                ItemVM itemVMCloned = DeepCloneItemVM(itemVM);
                itemVMsCloned.Add(itemVMCloned);
            }
            return itemVMs;
        }


        public static ItemVM NewGroup()
        {
            Item item = NewData.NewGroup();
            ItemVM itemVM = ItemToVM(item);
            return itemVM;
        }
        public static WindowItemVM NewWindow(Screen screen)
        {
            // The default new window model
            WindowItem windowItem = NewData.NewWindow();
            // Position the new window at the center of the screen
            int left = screen.Bounds.Left + (screen.Bounds.Width - windowItem.Width) / 2;
            int top = screen.Bounds.Top + (screen.Bounds.Height - windowItem.Height) / 2;
            WindowInfoVM windowInfoVM = new WindowInfoVM
            (
                title: windowItem.Title,
                windowBorderSolidColorBrush: windowItem.WindowBorderColor.ToColor().ToSolidColorBrush() ,
                titleBackgroundSolidColorBrush: windowItem.TitleBackgroundColor.ToColor().ToSolidColorBrush(),
                titleBackgroundOpacity:  windowItem.TitleBackgroundOpacity,
                titleTextSolidColorBrush: windowItem.TitleTextColor.ToColor().ToSolidColorBrush(),
                windowBackgroundSolidColorBrush: windowItem.WindowBackgroundColor.ToColor().ToSolidColorBrush(),
                windowBackgroundOpacity: windowItem.WindowBackgroundOpacity,
                windowTextSolidColorBrush: windowItem.WindowTextColor.ToColor().ToSolidColorBrush()
            );
            WindowItemVM windowItemVM = new WindowItemVM(false, left, top, windowItem.Width, windowItem.Height, windowInfoVM);
            return windowItemVM;
        }
        public static WindowItemVM CloneWindow(WindowItemVM parent)
        {
            Window window = Helper.GetWindowFromWindowItemVM(parent);
            Screen screen = Helper.GetScreenFromWindow(window);

            // Position the new window at the center of the screen
            int left = screen.Bounds.Left + (screen.Bounds.Width - parent.Width) / 2;
            int top = screen.Bounds.Top + (screen.Bounds.Height - parent.Height) / 2;
            WindowItemVM windowClone = new WindowItemVM(true, left, top, parent.Width, parent.Height, parent.WindowInfoVM);
            windowClone.RootItemVMs = parent.RootItemVMs;
            parent.WindowItemVMClones.Add(windowClone);
            return windowClone;
        }
        public static List<WindowItemVM> CloneWindowToAllScreens(WindowItemVM parent)
        {
            List<WindowItemVM> windowClones = new List<WindowItemVM>();
            // Loop through all available windows
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                // Position the new window at the center of the screen
                int left = screen.Bounds.Left + (screen.Bounds.Width - parent.Width) / 2;
                int top = screen.Bounds.Top + (screen.Bounds.Height - parent.Height) / 2;
                WindowItemVM windowClone = new WindowItemVM(true, left, top, parent.Width, parent.Height, parent.WindowInfoVM);
                windowClone.RootItemVMs = parent.RootItemVMs;
                parent.WindowItemVMClones.Add(windowClone);
                windowClones.Add(windowClone);
            }
            return windowClones;
        }

    }

}

