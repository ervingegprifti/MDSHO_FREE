using MDSHO.Data;
using MDSHO.Helpers;
using MDSHO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace MDSHO
{
    /// <summary>
    /// Interaction logic for CustomThemeWindow.xaml
    /// </summary>
    public partial class CustomThemeWindow : Window
    {
        // Store old values
        private Color windowBorderColor;
        private Color titleBackgroundColor;
        private double titleBackgroundOpacity;
        private Color titleTextColor;
        private Color windowBackgroundColor;
        private double windowBackgroundOpacity;
        private Color windowTextColor;
        //
        bool isCancel = true;

        public CustomThemeWindow(WindowInfoVM windowInfoVM)
        {
            InitializeComponent();

            try
            {
                // Get the datacontext of the window calling this custom theme window
                DataContext = windowInfoVM;

                // Store old values
                windowBorderColor = windowInfoVM.WindowBorderSolidColorBrush.Color;
                titleBackgroundColor = windowInfoVM.TitleBackgroundSolidColorBrush.Color;
                titleBackgroundOpacity = windowInfoVM.TitleBackgroundOpacity;
                titleTextColor = windowInfoVM.TitleTextSolidColorBrush.Color;
                windowBackgroundColor = windowInfoVM.WindowBackgroundSolidColorBrush.Color;
                windowBackgroundOpacity = windowInfoVM.WindowBackgroundOpacity;
                windowTextColor = windowInfoVM.WindowTextSolidColorBrush.Color;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void buttonApply_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveData.SaveShortcuts(false);
                isCancel = false;
                // Close custom theme window
                Close();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                isCancel = true;
                // Close custom theme window
                Close();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                RestoreOldValues();
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void RestoreOldValues()
        {
            try
            {
                if (isCancel)
                {
                    WindowInfoVM windowInfoVM = (WindowInfoVM)DataContext;
                    // Restore old values
                    windowInfoVM.WindowBorderSolidColorBrush.Color = windowBorderColor;
                    windowInfoVM.TitleBackgroundSolidColorBrush.Color = titleBackgroundColor;
                    windowInfoVM.TitleBackgroundOpacity = titleBackgroundOpacity;
                    windowInfoVM.TitleTextSolidColorBrush.Color = titleTextColor;
                    windowInfoVM.WindowBackgroundSolidColorBrush.Color = windowBackgroundColor;
                    windowInfoVM.WindowBackgroundOpacity = windowBackgroundOpacity;
                    windowInfoVM.WindowTextSolidColorBrush.Color = windowTextColor;
                }
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }
    }

}


/*
try
{

}
catch (Exception ex)
{
    Error.ShowDialog(ex, this);
}
*/
