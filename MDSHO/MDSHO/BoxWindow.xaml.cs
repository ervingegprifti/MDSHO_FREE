using MDSHO.ViewModels;
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

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for BoxWindow.xaml
    /// </summary>
    public partial class BoxWindow : Window
    {
        public BoxWindow(BoxViewModel boxViewModel)
        {
            InitializeComponent();

            DataContext = boxViewModel;
        }



        private void SetBoxBackgroundOpacity(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Rectangle rectangle = (Rectangle)sender;
                if (((BoxViewModel)DataContext) != null)
                {
                   ((BoxViewModel)DataContext).InfoViewModel.WindowBackgroundOpacity = rectangle.Fill.Opacity;
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }



    }
}
