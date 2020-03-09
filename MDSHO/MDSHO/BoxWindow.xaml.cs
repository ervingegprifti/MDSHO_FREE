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
        public BoxWindow(BoxVM boxVM)
        {
            InitializeComponent();

            DataContext = boxVM;
        }




        #region Functions used to change the background color and opacity from clicking on the bottom buttons of a box.

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                SetBoxBgOpacity(sender);
            }
            if (e.RightButton == MouseButtonState.Pressed)
            {
                SetBoxBgOpacity(sender);
            }
        }
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                SetBoxBgOpacity(sender);
            }
            if(e.RightButton == MouseButtonState.Pressed)
            {
                SetBoxBgOpacity(sender);
            }
        }
        private void SetBoxBgOpacity(object sender)
        {
            BoxVM boxVM = (BoxVM)DataContext;
            if (boxVM != null && sender is Rectangle)
            {
                Rectangle rectangle = (Rectangle)sender;
                boxVM.InfoVM.BoxBgOpacity = rectangle.Fill.Opacity;
            }
        }

        #endregion




    }
}
