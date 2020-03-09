using MDSHO.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MDSHO
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            LabelVersion.Content = Helper.GetApplicationVersion();
        }

        private void HyperlinkWebsite_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Navigate(e);
        }

        private void HyperlinkLicense_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Navigate(e);
        }


        private void Navigate(RequestNavigateEventArgs e)
        {
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo
                {
                    FileName = e.Uri.AbsoluteUri,
                    UseShellExecute = true
                };
                Process.Start(processStartInfo);
            }
            catch (Exception ex)
            {
                Error.Show(ex);
            }
            finally
            {
                e.Handled = true;
            }
        }

    }
}
