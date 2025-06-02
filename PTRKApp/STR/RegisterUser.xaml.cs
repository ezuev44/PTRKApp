using PTRKApp.AppData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace PTRKApp.STR
{
    /// <summary>
    /// Interaction logic for RegisterUser.xaml
    /// </summary>
    public partial class RegisterUser : Page
    {
        public RegisterUser()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                if (string.IsNullOrWhiteSpace(Login.Text) || string.IsNullOrWhiteSpace(Password.Password))
                {
                    MessageBox.Show("Поля логин и пароль обязательны для заполнения");
                    return;
                }

                var user = context.Users.FirstOrDefault(x => x.Login == Login.Text);
                if (user != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует");
                    return;
                }
                var newUser = new Users
                {
                    FIO = FIO.Text,
                    Login = Login.Text,
                    Password = Password.Password,
                    BirdDate = BirdDate.Text,
                    Email = Email.Text,
                    Speciality = Speciality.Text,
                    RoleId = 2
                };
                try
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    NavigationService.Navigate(new login());
                }
                catch(Exception ex)
                {

                }
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new login());
        }
    }
}
