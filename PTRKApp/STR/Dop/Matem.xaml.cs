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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PTRKApp.STR.Dop
{
    /// <summary>
    /// Логика взаимодействия для Matem.xaml
    /// </summary>
    public partial class Matem : Page
    {
        public Matem()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            int kg = Convert.ToInt32(TextKG.Text);
            int d = Convert.ToInt32(TextD.Text);
            int OTV = kg / d;
            TextOTV.Text = $"{OTV}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DopMenu());
        }

        
    }
}
