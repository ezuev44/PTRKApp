using PTRKApp.AppData;
using PTRKApp.STR;
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

namespace PTRKApp
{
    /// <summary>
    /// Логика взаимодействия для login.xaml
    /// </summary>
    public partial class login : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public login()
        {
            InitializeComponent();
        }      

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var user = context.Users.FirstOrDefault(x => x.Login == Login.Text && x.Password == Password.Password);
            if(user != null)
            {
                UserSession.CurrentUser = user;
                NavigationService.Navigate(new MeinMenu());
            }
            else
            {
                MessageBox.Show("Пароль не верный, уходите!");
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegisterUser());
        }
    }
}
