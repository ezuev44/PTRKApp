using PTRKApp.AppData;
using PTRKApp.STR.Dop;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using PTRKApp.Models;
using PTRKApp.STR;

namespace PTRKApp
{
    /// <summary>
    /// Логика взаимодействия для Sbor.xaml
    /// </summary>
    public partial class Sbor : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public Sbor()
        {
            InitializeComponent();
            var users = context.Users.ToList().Select(x =>
            {
                var role = context.Roles.FirstOrDefault(q => q.Id == x.RoleId);
                var user = new UsersWithRole
                {
                    FIO = x.FIO ?? string.Empty,
                    Login = x.Login ?? string.Empty,
                    RoleId = x.RoleId,
                    Speciality = x.Speciality ?? string.Empty,
                    Email = x.Email ?? string.Empty,
                    BirdDate = x.BirdDate,
                    Id = x.Id,
                    RoleName = role.Role,
                    IsAdmin = role.IsAdmin
                };
                return user;
            }).ToList();

            SborTab.ItemsSource = users;
            if (!UserSession.CurrentUser.Roles.IsAdmin)
            {
                Button6.Visibility = Visibility.Collapsed;
                Button3.Visibility = Visibility.Collapsed;
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeinMenu());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUser());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var usersForRemove = SborTab.SelectedItems.Cast<UsersWithRole>().ToList();
            foreach (var user in usersForRemove)
            {
                if (context.group.Any(x => x.Users.Any(q => q.Id == user.Id)))
                {
                    MessageBox.Show("Данные используются в таблице Команды", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (MessageBox.Show($"Удалить {user.FIO}", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var data = context.Users.FirstOrDefault(x => x.Id == user.Id);
                    context.Users.Remove(data);
                    context.SaveChanges();
                }
            }
            try
            {
                context.SaveChanges();
                var users = context.Users.ToList().Select(x =>
                {
                    var role = context.Roles.FirstOrDefault(q => q.Id == x.RoleId);
                    var user = new UsersWithRole
                    {
                        FIO = x.FIO ?? string.Empty,
                        Login = x.Login ?? string.Empty,
                        RoleId = x.RoleId,
                        Speciality = x.Speciality ?? string.Empty,
                        Email = x.Email ?? string.Empty,
                        BirdDate = x.BirdDate,
                        Id = x.Id,
                        RoleName = role.Role,
                        IsAdmin = role.IsAdmin
                    };
                    return user;
                }).ToList();
                SborTab.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Zapros());
        }
    }
}
