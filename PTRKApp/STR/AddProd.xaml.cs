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
    /// Interaction logic for AddProd.xaml
    /// </summary>
    public partial class AddProd : Page
    {
        private readonly AppDbContext context = new AppDbContext();

        public AddProd()
        {
            InitializeComponent();
            AddGrCmb.ItemsSource = context.group.ToList();
            AddSrvCmb.ItemsSource = context.Services.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var gr = (group) AddGrCmb.SelectedItem;
            var grDb = context.group.FirstOrDefault(x => x.id_group == gr.id_group);
            var srv = (Services) AddSrvCmb.SelectedItem;
            var srvDb = context.Services.FirstOrDefault(x => x.Id == srv.Id);
            var order = new Orders
            {
                Deadline = DeadLine.Text,
                StartWorkDate = StartWorkDate.Text,
                Name = ServiceName.Text,
                Customer = CustomerName.Text,
                Services = srvDb,
                ServiceId = srvDb.Id,
                group = grDb,
                groupId = grDb.id_group
            };
            context.Orders.Add(order);
            context.SaveChanges();
            NavigationService.Navigate(new Product());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Product());
        }
    }
}
