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
    /// Логика взаимодействия для gr.xaml
    /// </summary>
    public partial class gr : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public gr()
        {
            InitializeComponent();
            SborTab.ItemsSource = context.group.ToList();
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeinMenu());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddGroup(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddGroup((sender as Button).DataContext as group));

        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var DelSbor = SborTab.SelectedItems.Cast<group>().ToList();
            foreach (var dsbor in DelSbor)
            {
                if (context.Orders.Any(x => x.group.id_group == dsbor.id_group))
                {
                    MessageBox.Show("Данные используются в таблице Заказов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (MessageBox.Show($"Удалить запись {dsbor.id_group}", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var grForDel = context.group.FirstOrDefault(x => x.id_group == dsbor.id_group);
                    context.group.Remove(grForDel);
                    context.SaveChanges();
                    SborTab.ItemsSource = context.group.ToList();
                }
            }
        }
    }
}
