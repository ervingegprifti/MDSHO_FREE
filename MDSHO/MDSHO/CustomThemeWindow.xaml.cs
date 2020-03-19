using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MDSHO.ViewModels;

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for CustomThemeWindow.xaml
    /// </summary>
    public partial class CustomThemeWindow : Window
    {
        // Store old values
        //private Color windowBorderColor;
        //private Color titleBackgroundColor;
        //private double titleBackgroundOpacity;
        //private Color titleTextColor;
        private Color boxBg;
        private double boxBgOpacity;
        //private Color windowTextColor;
        ////
        bool isCancel = true;


        public CustomThemeWindow(InfoVM infoVM)
        {
            InitializeComponent();

            // Get the datacontext of the window calling this custom theme window
            DataContext = infoVM;

            // Store old values
            //windowBorderColor = infoVM.WindowBorderSolidColorBrush.Color;
            //titleBackgroundColor = infoVM.TitleBackgroundSolidColorBrush.Color;
            //titleBackgroundOpacity = infoVM.TitleBackgroundOpacity;
            //titleTextColor = infoVM.TitleTextSolidColorBrush.Color;
            boxBg = infoVM.BoxBg.Color;
            boxBgOpacity = infoVM.BoxBgOpacity;
            //windowTextColor = infoVM.WindowTextSolidColorBrush.Color;
        }

        private void ButtonApply_Click(object sender, RoutedEventArgs e)
        {
            // TODO SaveData.SaveShortcuts(false);
            isCancel = false;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            isCancel = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RestoreOldValues();
        }

        private void RestoreOldValues()
        {
            if (isCancel)
            {
                InfoVM infoVM = (InfoVM)DataContext;
                // Restore old values
                //infoVM.WindowBorderSolidColorBrush.Color = windowBorderColor;
                //infoVM.TitleBackgroundSolidColorBrush.Color = titleBackgroundColor;
                //infoVM.TitleBackgroundOpacity = titleBackgroundOpacity;
                //infoVM.TitleTextSolidColorBrush.Color = titleTextColor;
                infoVM.BoxBg.Color = boxBg;
                infoVM.BoxBgOpacity = boxBgOpacity;
                //infoVM.WindowTextSolidColorBrush.Color = windowTextColor;
            }
        }


    }
}
