using PTRKApp.AppData;
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

namespace PTRKApp.STR
{
    /// <summary>
    /// Interaction logic for ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        private readonly AppDbContext context = new AppDbContext();

        public ServicesPage()
        {
            InitializeComponent();
            SborTab.ItemsSource = context.Services.ToList();
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeinMenu());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddService());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var srvs = SborTab.SelectedItems.Cast<Services>().ToList();
            foreach (var srv in srvs)
            {
                if (context.Orders.Any(x => x.Services.Id == srv.Id))
                {
                    MessageBox.Show("Данные используются в таблице Заказы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (MessageBox.Show($"Удалить запись {srv.ServicesName}", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var srvDel = context.Services.FirstOrDefault(x => x.Id == srv.Id);
                    context.Services.Remove(srvDel);
                    context.SaveChanges();
                    SborTab.ItemsSource = context.Services.ToList();
                }
            }
        }
    }
}
