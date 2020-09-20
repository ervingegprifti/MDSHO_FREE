using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Reflection;
using MDSHO.Helpers;

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

            try
            {
                string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                LabelVersion.Content = version;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void HyperlinkWebsite_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                // https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Error.ShowDialog(ex);
            }
        }

        private void HyperlinHome_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
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
