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
using System.Windows.Threading;
using PTRKApp;
using PTRKApp.Loading;

namespace PTRKApp
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private int progressValue = 0;
        public int P3 = 0;

        public MainWindow()
        {
            InitializeComponent();
            StartProgress();
        }


        private void StartProgress()
        {
            Progress_load1.Value = 0;
            progressValue = 0;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(5);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (progressValue < 100)
            {
                progressValue++;
                Progress_load1.Value = progressValue;
            }
            else
            {
                timer.Stop();
                OpenNextWindow();
            }
        }
        private void OpenNextWindow()
        {
            Load_Menu loading = new Load_Menu();
            loading.Show();
            this.Close();
        }
    }
}
