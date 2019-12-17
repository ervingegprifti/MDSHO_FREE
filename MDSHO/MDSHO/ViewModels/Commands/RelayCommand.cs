using System;
using System.Windows.Input;

// https://www.youtube.com/watch?v=HDSRG7GvPbo
// https://www.youtube.com/watch?v=8WfD2cFRymM&t=730s
// https://www.c-sharpcorner.com/UploadFile/20c06b/icommand-and-relaycommand-in-wpf/
// https://www.codeproject.com/Tips/1199563/%2FTips%2F1199563%2FBinding-WPF-TreeView-TreeViewNode-to-Parent-ViewMo
// https://github.com/punker76/gong-wpf-dragdrop/blob/dev/src/Showcase/ViewModels/SimpleCommand.cs

namespace MDSHO.ViewModels.Commands
{
    public class RelayCommand : ICommand
    {

       public RelayCommand(Action<object> execute = null, Predicate<object> canExecute = null)
        {
            CanExecuteDelegate = canExecute;
            ExecuteDelegate = execute;
        }

        public Predicate<object> CanExecuteDelegate { get; set; }
        public Action<object> ExecuteDelegate { get; set; }

        public bool CanExecute(object parameter)
        {
            var canExecute = CanExecuteDelegate;
            return canExecute == null || canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            ExecuteDelegate?.Invoke(parameter);
        }
    }
}


/*
Use it like this:

in cs:
public RelayCommand EditNameCommand{ get; }
EditNameCommand = new RelayCommand(EditName);
private void EditName(object parameter)
{
    // If you want to use parameters the do it like this
    string param = parameter as string;
    MessageBox.Show(param);

    // Do what ever logic here
}

In xaml:
<MenuItem Header="Rename" Command="{Binding EditNameCommand}" CommandParameter="AAA">
    <MenuItem.Icon>
        <TextBlock Style="{StaticResource StyleMenuItemFontIcon}" Text="{StaticResource rename-o}"/>
    </MenuItem.Icon>
</MenuItem>

*/

