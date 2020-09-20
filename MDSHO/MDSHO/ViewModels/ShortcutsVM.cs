using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using MDSHO.Helpers;
using MDSHO.Data;
using System;

namespace MDSHO.ViewModels
{
    public class ShortcutsVM : BaseVM
    {

        #region PRIVATE PROPERTIES

        private ItemVM copiedItemVM;
        private bool cutCopiedItemVM;

        #endregion

        #region PUBLIC PROPERTIES

        public string Version { get; set; }
        public bool CanPaste => copiedItemVM != null;
        public ObservableCollection<WindowItemVM> WindowItemVMs { get; set; }
        public ObservableCollection<BackupVM> BackupVMs { get; set; }
        // We use this property to count the number of save requests
        public int SaveRequestCounter { get; set; }

        #endregion

        #region CONSTRUCTOR

        public ShortcutsVM(string version)
        {
            Version = version;
            // Set default values
            WindowItemVMs = new ObservableCollection<WindowItemVM>();
            BackupVMs = new ObservableCollection<BackupVM>();
            SaveRequestCounter = 0;
        }

        #endregion

        #region FUNCTIONS

        public void NewGroup(ObservableCollection<ItemVM> itemVMs)
        {
            // Create a new group 
            ItemVM itemVM = ContextHelper.NewGroup();
            itemVMs.Add(itemVM);
            SaveData.SaveShortcuts(false);
        }
        public void DeleteItem(ItemVM itemVMToDelete)
        {
            bool stop = false;
            foreach (WindowItemVM windowItemVM in WindowItemVMs)
            {
                if (windowItemVM.RootItemVMs.Contains(itemVMToDelete))
                {
                    // Delete physical file/s first
                    ShortcutUtils.DeleteShortcuts(itemVMToDelete);
                    // Delete the item
                    windowItemVM.RootItemVMs.Remove(itemVMToDelete);
                    stop = true;
                    break;
                }

                foreach (ItemVM rootItemVM in windowItemVM.RootItemVMs)
                {
                    IEnumerable<ItemVM> itemVMs = rootItemVM.FlattenItemVMs();
                    foreach (ItemVM itemVM in itemVMs)
                    {
                        if(itemVM.IsGroup)
                        {
                            if (itemVM.ItemVMs.Contains(itemVMToDelete))
                            {
                                // Delete physical file/s first
                                ShortcutUtils.DeleteShortcuts(itemVMToDelete);
                                // Delete the item
                                itemVM.ItemVMs.Remove(itemVMToDelete);
                                // 
                                stop = true;
                                break;
                            }
                        }

                        if (stop) { break; }
                    }

                    if (stop) { break; }
                }

                if (stop) { break; }
            }
            SaveData.SaveShortcuts(false);
        }
        public void DeleteAllItems(WindowItemVM windowItemVM)
        {
            // Delete physical files first
            foreach(ItemVM subItemVMToDelete in windowItemVM.RootItemVMs)
            {
                ShortcutUtils.DeleteShortcuts(subItemVMToDelete);
            }
            // Delete all items
            windowItemVM.RootItemVMs.Clear();
            SaveData.SaveShortcuts(false);
        }
        public void CutItem(ItemVM copiedItemVM)
        {
            this.copiedItemVM = copiedItemVM;
            cutCopiedItemVM = true;
            // Save to settings
            // No need to save here as there is no change to the data contex
        }
        public void CopyItem(ItemVM copiedItemVM)
        {
            this.copiedItemVM = copiedItemVM;
            cutCopiedItemVM = false;
            // Save to settings
            // No need to save here as there is no change to the data contex
        }
        public void PasteItem(ObservableCollection<ItemVM> itemVMs)
        {
            if (copiedItemVM != null)
            {
                // Patste the copiedItemVM
                ItemVM clonedItemVM = ContextHelper.DeepCloneItemVM(copiedItemVM);
                itemVMs.Add(clonedItemVM);
                if (cutCopiedItemVM)
                {
                    // Delete copiedItemVM
                    DeleteItem(copiedItemVM);
                    // Save to settings
                    // No need to save here as DeleteItem has its own save statement
                }
                else
                {
                    SaveData.SaveShortcuts(false);
                }
            }
        }
        public void NewWindow(System.Windows.Forms.Screen screen)
        {
            // Create a new shortcut window
            WindowItemVM windowItemVM = ContextHelper.NewWindow(screen);
            ShortcutsWindow shortcutsWindow = new ShortcutsWindow();
            WindowItemVMs.Add(windowItemVM);
            shortcutsWindow.DataContext = windowItemVM;
            shortcutsWindow.Show();
            SaveData.SaveShortcuts(false);
        }
        public void CloneWindow(WindowItemVM parent)
        {
            // Clone parent window
            WindowItemVM windowItemVMClone = ContextHelper.CloneWindow(parent);
            ShortcutsWindow shortcutsWindowCloned = new ShortcutsWindow();
            shortcutsWindowCloned.DataContext = windowItemVMClone;
            shortcutsWindowCloned.Show();
            SaveData.SaveShortcuts(false);
        }
        public void CloneWindowToAllScreens(WindowItemVM parent)
        {
            // Clone parent window
            List<WindowItemVM> windowItemVMClones = ContextHelper.CloneWindowToAllScreens(parent);
            foreach(WindowItemVM windowItemVMClone in windowItemVMClones)
            {
                ShortcutsWindow shortcutsWindowCloned = new ShortcutsWindow();
                shortcutsWindowCloned.DataContext = windowItemVMClone;
                shortcutsWindowCloned.Show();
            }
            SaveData.SaveShortcuts(false);
        }
        public void CloseWindow(WindowItemVM windowItemVMToClose)
        {
            if (WindowItemVMs == null)
            {
                // Nothing to close
                return;
            }

            if(windowItemVMToClose == null)
            {
                // Nothing to close
                return;
            }

            if (windowItemVMToClose.IsClone)
            {
                // This is a clone window, remove it from windowItemVMClones.
                ObservableCollection<WindowItemVM> windowItemVMClones = Helper.GetWindowItemVMClones(windowItemVMToClose);
                windowItemVMClones.Remove(windowItemVMToClose);
            }

            //  Close the window asociated with windowItemVMToClose data contect
            Window window = Helper.GetWindowFromWindowItemVM(windowItemVMToClose);
            window.Close();
            SaveData.SaveShortcuts(false);
        }
        public void DeleteWindow(WindowItemVM windowItemVMToDelete)
        {
            if (WindowItemVMs == null)
            {
                // Nothing to delete
                return;
            }

            if (windowItemVMToDelete == null)
            {
                // Nothing to delete
                return;
            }

            if (!windowItemVMToDelete.IsClone)
            {
                // TODO Create the backup

                // This is not a clone window, just close it along with all its cloned window
                // First close all window clones
                foreach (WindowItemVM windowItemVMClone in windowItemVMToDelete.WindowItemVMClones)
                {
                    Window windowClone = Helper.GetWindowFromWindowItemVM(windowItemVMClone);
                    windowClone.Close();
                }

                // Firts we need to delete all the shortcuts thaat are inside this window
                DeleteAllItems(windowItemVMToDelete);
                // Delete windowItemVMToDelete from the WindowItemVMs
                WindowItemVMs.Remove(windowItemVMToDelete);
            }

            //  Close the window asociated with windowItemVMToDelete data contect
            Window window = Helper.GetWindowFromWindowItemVM(windowItemVMToDelete);
            window.Close();
            SaveData.SaveShortcuts(false);
        }
        public void ShowRestoreWindow()
        {
            // No need to save settings here.
            ((App)Application.Current).ShowRestoreWindow();
        }
        public void ShowAboutWindow()
        {
            // No need to save settings here.
            ((App)Application.Current).ShowAboutWindow();
        }
        public void ExitApplication(Window parent)
        {
            SaveData.SaveShortcuts(true);

            // Exit the application
            ((App)Application.Current).ExitApplication(false);
        }
        public void SortAscending(ObservableCollection<ItemVM> itemVMs)
        {
            itemVMs.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            SaveData.SaveShortcuts(false);
        }
        public void SortDescending(ObservableCollection<ItemVM> itemVMs)
        {
            itemVMs.Sort((a, b) => { return b.Name.CompareTo(a.Name); });
            SaveData.SaveShortcuts(false);
        }

        #endregion

    }
}
