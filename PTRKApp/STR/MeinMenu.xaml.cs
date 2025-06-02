using PTRKApp.STR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace PTRKApp
{
    /// <summary>
    /// Логика взаимодействия для MeinMenu.xaml
    /// </summary>
    public partial class MeinMenu : Page
    {
        public MeinMenu()
        {
            InitializeComponent();

            if (!UserSession.CurrentUser.Roles.IsAdmin)
            {
                Button7.Visibility = Visibility.Collapsed;
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new login());
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.fsb.ru/");
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Sbor());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new gr());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Product());

        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServicesPage());
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {    
            NavigationService.Navigate(new DopMenu());
        }
    }
}
