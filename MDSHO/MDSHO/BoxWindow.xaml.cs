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
        private BoxViewModel BoxContext
        {
            get
            {
                return (BoxViewModel)DataContext;
            }
        }

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
                if (DataContext != null)
                {
                    BoxContext.InfoViewModel.WindowBackgroundOpacity = rectangle.Fill.Opacity;
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }



    }
}
