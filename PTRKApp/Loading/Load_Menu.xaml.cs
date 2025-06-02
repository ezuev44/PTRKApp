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
using PTRKApp.AppData;

namespace PTRKApp.Loading
{
    /// <summary>
    /// Логика взаимодействия для Load_Menu.xaml
    /// </summary>
    public partial class Load_Menu : Window
    {
      
        public Load_Menu()
        {
            InitializeComponent();
            Frame1.Navigate(new login());           
        }
    }
}
