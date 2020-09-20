using MDSHO.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using MDSHO.ViewModels.Commands;
using System.Windows.Controls;

namespace MDSHO.ViewModels
{
    public class ItemVM : BaseVM
    {
        // Private properties
        private bool isExpanded;
        private string name;

        // Public properties
        public string Guid { get; set; }
        public bool IsGroup { get; set; }
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Target { get; set; }
        public string Ext { get; set; }
        public ObservableCollection<ItemVM> ItemVMs { get; set; }
        public Visibility NewGroupVisibility
        {
            get
            {
                if (IsGroup)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility PasteVisibility
        {
            get
            {
                if (IsGroup)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility FontIconVisibility
        {
            get
            {
                if (IsGroup)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility SortVisibility
        {
            get
            {
                if (IsGroup)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility ImageIconVisibility
        {
            get
            {
                if (IsGroup)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public BitmapSource ImageIconBitmapSource { get; set; }
        public RelayCommand NewGroupCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CutCommand { get; }
        public RelayCommand CopyCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand EditNameCommand { get; }
        public RelayCommand SortAscendingCommand { get; }
        public RelayCommand SortDescendingCommand { get; }

        public ItemVM(string guid, bool isGroup, bool isExpanded, string name, string target, string ext)
        {
            try
            {
                Guid = guid;
                IsGroup = isGroup;
                IsExpanded = isExpanded;

                if(string.IsNullOrEmpty(name))
                {
                    // This is called from a drop down event. In this case the name is not specified. This item is a shortcut.
                    if(!string.IsNullOrEmpty(target))
                    {
                        if (File.Exists(target) || Directory.Exists(target))
                        {
                            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(target);
                            string fileNameWithExtension = System.IO.Path.GetFileName(target);
                            Name = fileNameWithExtension;
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(target));
                    }
                }
                else
                {
                    // This is called from an convertion item (items have their names specified) to viewmodel or it is just a group.
                    Name = name;
                }

                Target = target;
                Ext = ext;
                // Set the default values
                ItemVMs = new ObservableCollection<ItemVM>();
                NewGroupCommand = new RelayCommand(NewGroup, CanExecuteNewGroup);
                DeleteCommand = new RelayCommand(Delete);
                CutCommand = new RelayCommand(Cut);
                CopyCommand = new RelayCommand(Copy);
                PasteCommand = new RelayCommand(Paste, CanExecutePaste);
                EditNameCommand = new RelayCommand(EditName);
                SortAscendingCommand = new RelayCommand(SortAscending, CanSort);
                SortDescendingCommand = new RelayCommand(SortDescending, CanSort);
                
                // Get the icon for the shortcut
                if (IsGroup)
                {
                    ImageIconBitmapSource = null;
                }
                else
                {
                    // IMPORTANT INFO
                    // In case of control panel items we first create the link 
                    // then COPY the link in Shortcuts folder and execute from there
                    // In case of normal cases we CREATE the link in Shortcuts folder and execute from there
                    // If the target does not exists then there is nothing to create in the Shortcuts folder 
                    // (we are talking not in the case of a drag androp event, but in just opening the application and the shortcut in Shortcuts folder has been deleted)

                    // Write the shortcut (not groups) to physical user local folder C:\Users\USER_NAME\AppData\Local
                    // LocalApplicationData (applies to current user - local only). No need for Administrator access to write on that folder. 
                    // File.Exists & Directory.Exists are time consuming in case of \\server... For that reason we just let it fail silently
                    try
                    {
                        if (ext.Equals(Constants.EXT_LNK, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ShortcutUtils.GreateShortcutLnk(guid, target);

                        }
                        else if (ext.Equals(Constants.EXT_URL, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ShortcutUtils.GreateShortcutUrl(guid, target);
                        }
                    }
                    catch (Exception)
                    {
                        // Let it fail silently
                    }

                    string shortcutsDirectoryPath = Helper.GetShortcutsPath();
                    string shortcutFilePath = Path.Combine(shortcutsDirectoryPath, Guid + Ext);
                    if(File.Exists(shortcutFilePath))
                    {
                        ImageIconBitmapSource = ShortcutUtils.GetShortcutIconBitmapSource(shortcutFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        // Can functions
        private bool CanExecuteNewGroup(object parameter)
        {
            if (IsGroup)
            {
                return true;
            }
            return false;
        }
        private bool CanExecutePaste(object parameter)
        {
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            if (shortcutsVM.CanPaste)
            {
                if (IsGroup)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CanSort(object parameter)
        {
            if(IsGroup)
            {
                if (ItemVMs != null)
                {
                    if (ItemVMs.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Functions
        private void NewGroup(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.NewGroup(ItemVMs);
            }
            catch (Exception ex)
            {
                // TODO find the right parent
                Error.ShowDialog(ex);
            }
        }
        private void Delete(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.DeleteItem(this);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void Cut(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.CutItem(this);                
            }
            catch (Exception ex)
            {
                // TODO find the right parent
                Error.ShowDialog(ex);
            }
        }
        private void Copy(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.CopyItem(this);
            }
            catch (Exception ex)
            {
                // TODO find the right parent
                Error.ShowDialog(ex);
            }
        }
        private void Paste(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.PasteItem(ItemVMs);
            }
            catch (Exception ex)
            {
                // TODO find the right parent
                Error.ShowDialog(ex);
            }
        }
        private void EditName(object parameter)
        {
            try
            {
                ContextMenu contextMenu = (ContextMenu)parameter;
                if(contextMenu != null)
                {
                    // Get the StackPanelItem from which the context menu Rename was clicked
                    StackPanel stackPanel = (StackPanel)contextMenu.PlacementTarget;
                    if(stackPanel != null)
                    {
                        TextBlock textBlockName = (TextBlock)stackPanel.FindName("TextBlockName");
                        TextBox textBoxName = (TextBox)stackPanel.FindName("TextBoxName");
                        if(textBlockName != null && textBoxName != null)
                        {
                            textBlockName.Visibility = Visibility.Hidden;
                            textBoxName.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SortAscending(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.SortAscending(ItemVMs);
            }
            catch (Exception ex)
            {
                // TODO find the right parent
                Error.ShowDialog(ex);
            }
        }
        private void SortDescending(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.SortDescending(ItemVMs);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
    }
}
