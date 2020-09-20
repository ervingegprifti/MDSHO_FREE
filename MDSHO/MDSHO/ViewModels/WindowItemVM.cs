 using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Windows;
using System;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Linq;
using System.Windows.Controls;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using MDSHO.Helpers;
using System.IO;
using MDSHO.Data;
using MDSHO.ViewModels.Commands;



// https://github.com/punker76/gong-wpf-dragdrop
// https://github.com/punker76/gong-wpf-dragdrop/issues/141

namespace MDSHO.ViewModels
{
    public class WindowItemVM : BaseVM, IDropTarget
    {

        #region PRIVATE PROPERTIES

        private int left;
        private int top;
        private int width;
        private int height;
        private ObservableCollection<ItemVM> rootItemVMs;

        #endregion

        #region PUBLIC PROPERTIES

        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
                OnPropertyChanged(nameof(Left));
            }
        }
        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
                OnPropertyChanged(nameof(Top));
            }
        }
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public bool IsClone { get; set; }
        public string TitleDisplay
        {
            get
            {
                if(WindowInfoVM != null)
                {
                    if(IsClone)
                    {
                        return $"{WindowInfoVM.Title} (Clone)";
                    }
                    else
                    {
                        return WindowInfoVM.Title;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        public WindowInfoVM WindowInfoVM { get; set; }
        public Visibility CloneWindowVisibility
        {
            get
            {
                if(IsClone)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public Visibility CloseWindowVisibility
        {
            get
            {
                if (IsClone)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility DeleteWindowVisibility
        {
            get
            {
                if (IsClone)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        public Visibility DragItemsHereInfoVisibility
        {
            get
            {
                if (RootItemVMs.Count > 0)
                {
                    // There are shortcuts in this window. Hide DragItemsHereInfo.
                    return Visibility.Collapsed;
                }
                else
                {
                    // This window is empty. Display DragItemsHereInfo.
                    return Visibility.Visible;
                }
            }
        }
        public ObservableCollection<ItemVM> RootItemVMs
        {
            get
            {
                return rootItemVMs;
            }
            set
            {
                rootItemVMs = value;
                rootItemVMs.CollectionChanged += RootItemVMs_CollectionChanged;
            }
        }
        public ObservableCollection<WindowItemVM> WindowItemVMClones { get; set; }
        public RelayCommand NewGroupCommand { get; }
        public RelayCommand PasteCommand { get; }
        public RelayCommand NewWindowCommand { get; }
        public RelayCommand CloneWindowCommand { get; }
        public RelayCommand CloneWindowToAllScreensCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand DeleteWindowCommand { get; }
        public RelayCommand DeleteAllItemsCommand { get; }
        public RelayCommand ExitCommand { get; }
        public RelayCommand ShowRestoreWindowCommand { get; }
        public RelayCommand ShowAboutWindowCommand { get; }
        public RelayCommand SortAscendingCommand { get; }
        public RelayCommand SortDescendingCommand { get; }
        public RelayCommand BackupShortcutsCommand { get; }
        public RelayCommand RestoreLastBackupCommand { get; }

        #endregion

        #region CONSTRUCTOR

        public WindowItemVM(bool isClone, int left, int top, int width, int height, WindowInfoVM windowInfoVM)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                IsClone = isClone;
                Left = left;
                Top = top;
                Width = width;
                Height = height;
                WindowInfoVM = windowInfoVM;
                // Set the default values
                RootItemVMs = new ObservableCollection<ItemVM>();
                WindowItemVMClones = new ObservableCollection<WindowItemVM>();
                NewGroupCommand = new RelayCommand(NewGroup);
                PasteCommand = new RelayCommand(Paste, CanExecutePaste);
                NewWindowCommand = new RelayCommand(NewWindow);
                CloneWindowCommand = new RelayCommand(CloneWindow);
                CloneWindowToAllScreensCommand = new RelayCommand(CloneWindowToAllScreens);
                CloseWindowCommand = new RelayCommand(CloseWindow);
                DeleteWindowCommand = new RelayCommand(DeleteWindow);
                DeleteAllItemsCommand = new RelayCommand(DeleteAllItems, CanDeleteAllItems);
                ExitCommand = new RelayCommand(ExitApplication);
                ShowRestoreWindowCommand = new RelayCommand(ShowRestoreWindow);
                ShowAboutWindowCommand = new RelayCommand(ShowAboutWindow);
                SortAscendingCommand = new RelayCommand(SortAscending, CanSort);
                SortDescendingCommand = new RelayCommand(SortDescending, CanSort);
                BackupShortcutsCommand = new RelayCommand(BackupShortcuts);
                RestoreLastBackupCommand = new RelayCommand(RestoreLastBackup);
                // Events
                WindowInfoVM.PropertyChanged += WindowInfoVM_PropertyChanged;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        #endregion

        #region EVENTS

        private void WindowInfoVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(WindowInfoVM.Title))
            {
                OnPropertyChanged(nameof(TitleDisplay));
                SaveShortcutSettings();
            }
        }
        private void RootItemVMs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(DragItemsHereInfoVisibility));
        }

        #endregion

        #region METHODS

        // Can methods
        private bool CanExecutePaste(object parameter)
        {
            ShortcutsVM shortcutsVM = ((App)Application.Current).DataContext;
            if (shortcutsVM.CanPaste)
            {
                return true;
            }
            return false;
        }
        private bool CanDeleteAllItems(object parameter)
        {
            if(RootItemVMs != null)
            {
                if(RootItemVMs.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        private bool CanSort(object parameter)
        {
            if (RootItemVMs != null)
            {
                if (RootItemVMs.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }



        // Methods
        private void NewGroup(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.NewGroup(RootItemVMs);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void Paste(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.PasteItem(RootItemVMs);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void NewWindow(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                System.Windows.Forms.Screen screen = Helper.GetScreenFromWindow(parent);
                ((App)Application.Current).DataContext.NewWindow(screen);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void CloneWindow(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.CloneWindow(this);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void CloneWindowToAllScreens(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.CloneWindowToAllScreens(this);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void CloseWindow(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.CloseWindow(this);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void DeleteWindow(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(parent, "Are you sure you want to remove this window? \nYou will lose all the shortcuts that are inside it. All the target files will be intact.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    ((App)Application.Current).DataContext.DeleteWindow(this);
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void DeleteAllItems(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(parent, "Are you sure you want to delete all shortcuts in this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(messageBoxResult == MessageBoxResult.Yes)
                {
                    ((App)Application.Current).DataContext.DeleteAllItems(this);
                }                
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void ExitApplication(object parameter)
        {
            Window parent = Helper.GetWindowFromWindowItemVM(this);
            try
            {
                ((App)Application.Current).DataContext.ExitApplication(parent);                
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void ShowRestoreWindow(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.ShowRestoreWindow();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void ShowAboutWindow(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.ShowAboutWindow();
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
                ((App)Application.Current).DataContext.SortAscending(RootItemVMs);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SortDescending(object parameter)
        {
            try
            {
                ((App)Application.Current).DataContext.SortDescending(RootItemVMs);
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void SaveShortcutSettings()
        {
            try
            {
                if (!IsClone)
                {
                    SaveData.SaveShortcuts(false);
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void BackupShortcuts(object parameter)
        {
            try
            {
                string backupFileName = BackupIO.BackupShortcuts();
                string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
                string backupFilePath = Path.Combine(shortcutsBackupPath, backupFileName);
                // Displaying the result to the user
                if(File.Exists(backupFilePath))
                {
                    string resultMessage = "";
                    resultMessage += "Backup " + backupFileName + "\n";
                    resultMessage += "created successfully.\n\n";
                    resultMessage += "To see backups, open \"Restore shortcuts...\" from MDSHO window title context menu.";
                    MessageBox.Show(resultMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
        private void RestoreLastBackup(object parameter)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to restore the last backup?\nYou will loose current windows & sortcuts.", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    string backupFileName = BackupIO.RestoreLastBackup();
                    if(!string.IsNullOrEmpty(backupFileName))
                    {
                        // Displaying the result to the user
                        string shortcutsBackupPath = Helper.GetShortcutsBackupPath();
                        string resultMessage = "";
                        resultMessage += "Backup " + backupFileName + "\n";
                        resultMessage += "restored successfully.\n\n";
                        resultMessage += "To see backups, open \"Restore shortcuts...\" from MDSHO window title context menu.";
                        MessageBox.Show(resultMessage, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // If the returnValue from the BackupIO.RestoreLastBackup() was null or empty then there is no backup created yet.
                        string resultMessage = "";
                        resultMessage += "There are no backups created yet.\n";
                        resultMessage += "There is nothing to restore from.\n\n";
                        resultMessage += "To see backups, open \"Restore shortcuts...\" from MDSHO window title context menu.";
                        MessageBox.Show(resultMessage, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }



        #endregion

        #region For IDropTarget

        public void DragOver(IDropInfo dropInfo)
        {
            // To allow a drop at the current drag position, DropInfo.Effects property on dropInfo should be set to a value other than DragDropEffects.None DropInfo.Data should be set to a non-null value.

            // Your target has to set a DragDropEffect in the DragOver handler to allow the drop:
            // https://stackoverflow.com/questions/29744251/no-drop-with-basic-gongsolution-sample
            // To alow drop from control panel items set Effects to DragDropEffects.Link

            if (CanAcceptData(dropInfo))
            {
                dropInfo.Effects = GetDragDropEffects(dropInfo);
                var isTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter) && dropInfo.VisualTargetItem is TreeViewItem;
                if (isTreeViewItem)
                {
                    // Highlight -> Is the rectangular border soraunding the item
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                }
                else
                {
                    // Insert -> Is the line indicator
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                }
            }
        }
        public static bool CanAcceptData(IDropInfo dropInfo)
        {
            // There is nothing we can do here.
            if(dropInfo == null) { return false; }
            // Data is populated from external and internal drag & drop. If it is null there is nothing we can do here.
            if (dropInfo.Data == null) { return false; }
            // Check if target is not group, then there is nothing we can do here.
            bool targetIsTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter) && dropInfo.VisualTargetItem is TreeViewItem;
            if (targetIsTreeViewItem)
            {
                TreeViewItem treeViewItem = (TreeViewItem)dropInfo.VisualTargetItem;
                ItemVM itemVM = (ItemVM)treeViewItem.DataContext;
                if (!itemVM.IsGroup)
                {
                    return false;
                }
            }

            // Target is group proced with the other checks...
         
            // ------------------------------------------------------------------
            // Check if Drag & Drop is from external source
            // ------------------------------------------------------------------
            if(dropInfo.Data is DataObject)
            {
                return true;
            }

            // ------------------------------------------------------------------
            // Drag & Drop is from internal source
            // ------------------------------------------------------------------
            // Prevent target is in the same context as the source
            if (!dropInfo.IsSameDragDropContextAsSource)
            {
                return false;
            }

            // do not drop on itself
            if (targetIsTreeViewItem && dropInfo.VisualTargetItem == dropInfo.DragInfo.VisualSourceItem)
            {
                return false;
            }

            //
            if (dropInfo.DragInfo.SourceCollection == dropInfo.TargetCollection)
            {
                var targetList = dropInfo.TargetCollection.TryGetList();
                return targetList != null;
            }
            //else if (dropInfo.DragInfo.SourceCollection is ItemCollection)
            //{
            //    return false;
            //}
            else if (dropInfo.TargetCollection == null)
            {
                return false;
            }
            else
            {
                if (TestCompatibleTypes(dropInfo.TargetCollection, dropInfo.Data))
                {
                    var isChildOf = IsChildOf(dropInfo.VisualTargetItem, dropInfo.DragInfo.VisualSourceItem);
                    return !isChildOf;
                }
                else
                {
                    return false;
                }
            }

        }
        public static DragDropEffects GetDragDropEffects(IDropInfo dropInfo)
        {
            // dropInfo is never null here
            // dropInfo.Data is never null here

            // ------------------------------------------------------------------
            // Check if the  Drag & Drop is from external source
            // ------------------------------------------------------------------
            if (dropInfo.Data is DataObject)
            {
                // We use link if the  Drag & Drop is from external source
                return DragDropEffects.Link;
            }

            // ------------------------------------------------------------------
            // Drag & Drop is from internal source
            // ------------------------------------------------------------------
            // If it is Drag & Drop from the internal source then use the default library behaviour
            var copyData = (
                                (dropInfo.DragInfo.DragDropCopyKeyState != default(DragDropKeyStates))
                                &&
                                dropInfo.KeyStates.HasFlag(dropInfo.DragInfo.DragDropCopyKeyState)
                           )
                           ||
                           dropInfo.DragInfo.DragDropCopyKeyState.HasFlag(DragDropKeyStates.LeftMouseButton);
            copyData = copyData
                       //&& (dropInfo.DragInfo.VisualSource != dropInfo.VisualTarget)
                       && !(dropInfo.DragInfo.SourceItem is HeaderedContentControl)
                       && !(dropInfo.DragInfo.SourceItem is HeaderedItemsControl)
                       && !(dropInfo.DragInfo.SourceItem is ListBoxItem);

            if(copyData)
            {
                return DragDropEffects.Copy;
            }
            else
            {
                return DragDropEffects.Move;
            }
        }
        public static IEnumerable ExtractData(object data)
        {
            if (data is IEnumerable && !(data is string))
            {
                return (IEnumerable)data;
            }
            else
            {
                return Enumerable.Repeat(data, 1);
            }
        }
        protected static bool IsChildOf(UIElement targetItem, UIElement sourceItem)
        {
            var parent = ItemsControl.ItemsControlFromItemContainer(targetItem);

            while (parent != null)
            {
                if (parent == sourceItem)
                {
                    return true;
                }

                parent = ItemsControl.ItemsControlFromItemContainer(parent);
            }

            return false;
        }
        protected static bool TestCompatibleTypes(IEnumerable target, object data)
        {
            TypeFilter filter = (t, o) =>
            {
                return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            };

            var enumerableInterfaces = target.GetType().FindInterfaces(filter, null);
            var enumerableTypes = from i in enumerableInterfaces select i.GetGenericArguments().Single();

            if (enumerableTypes.Count() > 0)
            {
                var dataType = TypeUtilities.GetCommonBaseClass(ExtractData(data));
                return enumerableTypes.Any(t => t.IsAssignableFrom(dataType));
            }
            else
            {
                return target is IList;
            }
        }
        public void Drop(IDropInfo dropInfo)
        {
            // There is nothing we can do here.
            if (dropInfo == null) { return; }
            // Data is populated from external and internal drag & drop. If it is null there is nothing we can do here.
            if (dropInfo.Data == null) { return; }
            // Check if target is not group, if yes, then there is nothing we can do here.
            bool targetIsTreeViewItem = dropInfo.InsertPosition.HasFlag(RelativeInsertPosition.TargetItemCenter) && dropInfo.VisualTargetItem is TreeViewItem;
            if (targetIsTreeViewItem)
            {
                TreeViewItem treeViewItem = (TreeViewItem)dropInfo.VisualTargetItem;
                ItemVM itemVM = (ItemVM)treeViewItem.DataContext;
                if (!itemVM.IsGroup)
                {
                    return;
                }
            }

            // Target is group proced with the other checks...

            var insertIndex = dropInfo.InsertIndex != dropInfo.UnfilteredInsertIndex ? dropInfo.UnfilteredInsertIndex : dropInfo.InsertIndex;
            var itemsControl = dropInfo.VisualTarget as ItemsControl;
            if (itemsControl != null)
            {
                var editableItems = itemsControl.Items as IEditableCollectionView;
                if (editableItems != null)
                {
                    var newItemPlaceholderPosition = editableItems.NewItemPlaceholderPosition;
                    if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtBeginning && insertIndex == 0)
                    {
                        ++insertIndex;
                    }
                    else if (newItemPlaceholderPosition == NewItemPlaceholderPosition.AtEnd && insertIndex == itemsControl.Items.Count)
                    {
                        --insertIndex;
                    }
                }
            }

            var destinationList = dropInfo.TargetCollection.TryGetList();
            var data = ExtractData(dropInfo.Data).OfType<object>().ToList();

            // If this is a move for sure it is from the internal Drag & Drop, so delete the source item.
            DragDropEffects dragDropEffects = GetDragDropEffects(dropInfo);
            if (dragDropEffects == DragDropEffects.Move)
            {
                if(dropInfo.DragInfo != null)
                {
                    // This is a drag & drop item from treeview its self
                    var sourceList = dropInfo.DragInfo.SourceCollection.TryGetList();
                    if (sourceList != null)
                    {
                        foreach (var o in data)
                        {
                            var index = sourceList.IndexOf(o);
                            if (index != -1)
                            {
                                sourceList.RemoveAt(index);
                                // so, is the source list the destination list too ?
                                if (destinationList != null && Equals(sourceList, destinationList) && index < insertIndex)
                                {
                                    --insertIndex;
                                }
                            }
                        }
                    }
                }
            }

            // Add item
            if (destinationList != null)
            {
                // check for cloning
                var cloneData = dropInfo.Effects.HasFlag(DragDropEffects.Copy) || dropInfo.Effects.HasFlag(DragDropEffects.Link);
                foreach (var o in data)
                {
                    // ----------------------------------------------
                    // This is a Drag & Drop from the external source
                    // ----------------------------------------------
                    if(dropInfo.Data is DataObject)
                    {
                        DataObject dataObject = o as DataObject;
                        if(dataObject != null)
                        {
                            // ******************************************************************************************************************************************
                            // FileDrop. Queries a data object for the presence of data in the System.Windows.DataFormats.FileDrop data format. 
                            // ******************************************************************************************************************************************
                            if (dataObject.ContainsFileDropList() && dataObject.GetDataPresent(DataFormats.FileDrop, true))
                            {
                                // We can have more than one file dropped.
                                var droppedFilePaths = dataObject.GetFileDropList();
                                foreach (string droppedFilePath in droppedFilePaths)
                                {
                                    string shortcutsDirectoryPath = Helper.GetShortcutsPath();
                                    string guid = NewData.NewGuid();
                                    string name = Path.GetFileName(droppedFilePath);
                                    DirectoryInfo directoryInfo = new DirectoryInfo(droppedFilePath);
                                    if(directoryInfo.Parent == null)
                                    {
                                        // This is the root. In that case name is empty so we need to set the name differently.
                                        DriveInfo driveInfo = new DriveInfo(droppedFilePath);
                                        // Something like this: Local Disk (C:)
                                        string volumeLabel = driveInfo.VolumeLabel;
                                        if (string.IsNullOrEmpty(volumeLabel))
                                        {
                                            volumeLabel = "Drive";
                                        }
                                        name = $"{volumeLabel} ({driveInfo.Name.Replace(@"\", "")})"; 
                                    }

                                    ItemVM obj2Insert = null;

                                    // Check if the dropped file is a .lnk file. We just copy the .lnk files to the sortcuts folder and use them from there.
                                    if (Path.GetExtension(droppedFilePath).Equals(Constants.EXT_LNK, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        // This is a .lnk file. In this case we copy this file to our shortcuts
                                        string shortcutLnkFilePath = Path.Combine(shortcutsDirectoryPath, guid + Constants.EXT_LNK);
                                        File.Copy(droppedFilePath, shortcutLnkFilePath);
                                        obj2Insert = new ItemVM(guid, false, false, name, shortcutLnkFilePath, Constants.EXT_LNK);
                                    }
                                    // Check if the dropped file is a .url file. We just copy the .url files to the sortcuts folder and use them from there.
                                    else if (Path.GetExtension(droppedFilePath).Equals(Constants.EXT_URL, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        // This is a .url file. In this case we copy this file to our shortcuts
                                        string shortcutUrlFilePath = Path.Combine(shortcutsDirectoryPath, guid + Constants.EXT_URL);
                                        File.Copy(droppedFilePath, shortcutUrlFilePath);
                                        obj2Insert = new ItemVM(guid, false, false, name, shortcutUrlFilePath, Constants.EXT_URL);
                                    }
                                    else
                                    {
                                        obj2Insert = new ItemVM(guid, false, false, name, droppedFilePath, Constants.EXT_LNK);                                      
                                    }

                                    destinationList.Insert(insertIndex++, obj2Insert);
                                    SaveShortcutSettings();

                                }
                            }
                            // ******************************************************************************************************************************************
                            // AbsoluteUri. Queries a data object for the presence of data in the System.Windows.DataFormats.UnicodeText format. 
                            // ******************************************************************************************************************************************
                            else if (dataObject.ContainsText() && dataObject.GetDataPresent(DataFormats.Text, true))
                            {
                                string dataText = (string)dataObject.GetData(DataFormats.Text, true);
                                Uri uri;
                                bool isAbsoluteUri = Uri.TryCreate(dataText, UriKind.Absolute, out uri);
                                if (isAbsoluteUri)
                                {
                                    ItemVM obj2Insert = new ItemVM(NewData.NewGuid(), false, false, uri.AbsoluteUri, uri.AbsoluteUri, Constants.EXT_URL);
                                    destinationList.Insert(insertIndex++, obj2Insert);
                                    // Save shortcut settings.
                                    SaveShortcutSettings();
                                }
                            }
                            else
                            {
                                // ******************************************************************************************************************************************
                                // ShellIDListArray. Perhaps data contain virtual files
                                // ******************************************************************************************************************************************
                                // Returns a list of formats in which the data in this data object is stored, or can be converted to.
                                string[] formats = dataObject.GetFormats();
                                foreach (string format in formats)
                                {
                                    if (format == "Shell IDList Array")
                                    {
                                        // Virtual file/s been draged.
                                        string message = "To create this kind of shortcut please do:\n";
                                        message += "1. Drag to create a desktop shortcut first,\n";
                                        message += "2. Drag the shortcut from desktop to MDSHO,\n";
                                        message += "3. Delete the shortcut from desktop if you want.";
                                        Window parent = Helper.GetWindowFromWindowItemVM(this);
                                        MessageBox.Show(parent, message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                    }
                    // ---------------------------------------------------------------------------
                    // This is a Drag & Drop from the internal source. Item from treeview its self
                    // ---------------------------------------------------------------------------
                    else
                    {
                        var obj2Insert = o;
                        if (cloneData)
                        {
                            var cloneable = o as ICloneable;
                            if (cloneable != null)
                            {
                                obj2Insert = cloneable.Clone();
                            }
                        }
                        destinationList.Insert(insertIndex++, obj2Insert);
                        // Save shortcut settings.
                        SaveShortcutSettings();
                    }
                }
            }
        }

        #endregion

    }
}







