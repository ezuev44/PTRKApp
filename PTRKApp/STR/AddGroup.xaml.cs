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
using PTRKApp.STR;

namespace PTRKApp.STR
{
    /// <summary>
    /// Логика взаимодействия для AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        group gro;
        bool checkNew;
        public AddGroup(group c)
        {
            InitializeComponent();
            if (c == null)
            {
                c = new group();
                checkNew = true;
            }
            else checkNew = false;
            DataContext = gro = c;
            Update();
        }

        void Update()
        {
            SborDG.ItemsSource = gro.Users.ToList();
            AddSborCmb.ItemsSource = context.Users.ToList().Except(gro.Users.ToList());
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new gr());
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkNew)
            {
                context.group.Add(gro);
            }
            if (!checkNew)
            {
                var current = context.group.FirstOrDefault(x => x.id_group == gro.id_group);
                current.Users = gro.Users;
                current.Name_group = gro.Name_group;
            }
            context.SaveChanges();
            NavigationService.Navigate(new gr());
        }

        private void AddSborBtn_Click(object sender, RoutedEventArgs e)
        {
            gro.Users.Add(AddSborCmb.SelectedItem as Users);
            Update();
        }

        private void DelSborBtn_Click(object sender, RoutedEventArgs e)
        {
            var DelSbor = SborDG.SelectedItems.Cast<Users>().ToList();
            foreach(var d in DelSbor)
                gro.Users.Remove(d);
            Update();
        }
    }
}
