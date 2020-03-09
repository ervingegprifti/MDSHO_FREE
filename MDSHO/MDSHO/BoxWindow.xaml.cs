﻿using MDSHO.ViewModels;
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



        private void SetBoxBgOpacity(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Rectangle rectangle = (Rectangle)sender;
                if (((BoxVM)DataContext) != null)
                {
                   ((BoxVM)DataContext).InfoVM.BoxBgOpacity = rectangle.Fill.Opacity;
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }



    }
}
