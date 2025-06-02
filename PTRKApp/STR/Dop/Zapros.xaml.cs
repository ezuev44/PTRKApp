using Microsoft.Win32;
using PTRKApp.AppData;
using PTRKApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace PTRKApp.STR.Dop
{
    /// <summary>
    /// Логика взаимодействия для Zapros.xaml
    /// </summary>
    public partial class Zapros : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public Zapros()
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
            }).Where(x => !x.IsAdmin).ToList();
            SborTab.ItemsSource = users;

        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            var searchText = SearchBox.Text;
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
            }).Where(x => !x.IsAdmin && (x.FIO.Contains(searchText) || x.Login.Contains(searchText) || x.Speciality.Contains(searchText) || x.Email.Contains(searchText))).ToList();
            SborTab.ItemsSource = users;
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Sbor());
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            var data = context.Users.ToList().Select(x =>
            {
                var result = new List<UsersExcelReport>();
                var teams = context.group.Where(t => t.Users.Any(u => u.Id == x.Id)).ToList();
                foreach (var team in teams)
                {
                    var rep = new UsersExcelReport
                    {
                        UserName = x.FIO,
                        UserId = x.Id,
                        Speciality = x.Speciality,
                        Team = team.Name_group
                    };
                    var order = context.Orders.FirstOrDefault(o => o.groupId == team.id_group);
                    if (order != null)
                    {
                        rep.Orders = order.Name;
                    }
                    result.Add(rep);
                }
                return result;
            }).SelectMany(x => x).ToList();

            var saveDialog = new SaveFileDialog();

            saveDialog.Filter = "Excel Files|*.xlsx";
            saveDialog.Title = "Сохранить отчет";
            saveDialog.FileName = "UserReport.xlsx"; 

            if (saveDialog.ShowDialog() != true) return;

            string filePath = saveDialog.FileName;

            Excel.Application excelApp = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                excelApp = new Excel.Application();
                workbook = excelApp.Workbooks.Add();
                worksheet = (Excel.Worksheet)workbook.ActiveSheet;

                string[] headers = { "ID", "ФИО", "Специальность", "Команда", "Заказ" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1] = headers[i];
                    ((Excel.Range)worksheet.Cells[1, i + 1]).Font.Bold = true;
                }


                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1] = item.UserId;
                    worksheet.Cells[row, 2] = item.UserName;
                    worksheet.Cells[row, 3] = item.Speciality;
                    worksheet.Cells[row, 4] = item.Team;
                    worksheet.Cells[row, 5] = item.Orders;
                    row++;
                }

                // Автоподбор ширины столбцов
                worksheet.Columns.AutoFit();

                // Сохраняем файл
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                // Показываем подтверждение
                MessageBox.Show($"Отчёт успешно создан!\nПуть: {filePath}",
                                "Успех",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании отчета: {ex.Message}",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            finally
            {
                // Освобождаем COM-объекты
                if (worksheet != null) Marshal.ReleaseComObject(worksheet);
                if (workbook != null) Marshal.ReleaseComObject(workbook);
                if (excelApp != null) Marshal.ReleaseComObject(excelApp);
            }
        }
    }
}
