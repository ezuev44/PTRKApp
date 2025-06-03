using PTRKApp.AppData;
using PTRKApp.Models;
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
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Page
    {
        private readonly AppDbContext context = new AppDbContext();

        public AddUser()
        {
            InitializeComponent();
            if (UserSession.CurrentUser.Roles.Id != 1)
            {
                cbAdm.Visibility = Visibility.Hidden;
                cbMod.Visibility = Visibility.Hidden;
            }
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
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
                RoleId = cbMod.IsChecked.GetValueOrDefault() ? 3 : cbAdm.IsChecked.GetValueOrDefault() ? 1 : 2
            };
            context.Users.Add(newUser);
            context.SaveChanges();
            NavigationService.Navigate(new Sbor());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox current = (CheckBox)sender;
            if (current == cbAdm)
            {
                cbMod.IsChecked = false;
                cbMod.IsEnabled = false; // Блокируем второй
            }
            else if (current == cbMod)
            {
                cbAdm.IsChecked = false;
                cbAdm.IsEnabled = false; // Блокируем первый
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Разблокируем оба CheckBox при снятии выбора
            cbAdm.IsEnabled = true;
            cbMod.IsEnabled = true;
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Sbor());
        }
    }
}
