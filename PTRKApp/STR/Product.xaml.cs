using Microsoft.Win32;
using PTRKApp.AppData;
using PTRKApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

namespace PTRKApp.STR
{
    /// <summary>
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class Product : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public Product()
        {
            InitializeComponent();
            SborTab.ItemsSource = context.Orders.Select(x => new OrdersDataPage
            {
                Customer = x.Customer,
                Name = x.Name,
                Id = x.Id,
                Deadline = x.Deadline,
                StartWorkDate = x.StartWorkDate,
                Services = x.Services.ServicesName,
                Group = x.group.Name_group
            }).ToList();
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeinMenu());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddProd());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var orders = SborTab.SelectedItems.Cast<OrdersDataPage>().ToList();
            foreach (var order in orders)
            {
                if (MessageBox.Show($"Удалить запись {order.Name}", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    var orderDel = context.Orders.FirstOrDefault(x => x.Id == order.Id);
                    context.Orders.Remove(orderDel);
                    context.SaveChanges();
                    SborTab.ItemsSource = context.Orders.Select(x => new OrdersDataPage
                    {
                        Customer = x.Customer,
                        Name = x.Name,
                        Id = x.Id,
                        Deadline = x.Deadline,
                        StartWorkDate = x.StartWorkDate,
                        Services = x.Services.ServicesName,
                        Group = x.group.Name_group
                    }).ToList();
                }
            }
        }

        private void Excel_Click(object sender, RoutedEventArgs e)
        {
            var data = context.Orders.Select(x => new OrdersDataPage
            {
                Customer = x.Customer,
                Name = x.Name,
                Id = x.Id,
                Deadline = x.Deadline,
                StartWorkDate = x.StartWorkDate,
                Services = x.Services.ServicesName,
                Group = x.group.Name_group
            }).ToList();

            var saveDialog = new SaveFileDialog();

            saveDialog.Filter = "Excel Files|*.xlsx";
            saveDialog.Title = "Сохранить отчет";
            saveDialog.FileName = "ProductReport.xlsx";

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

                string[] headers = { "ID", "Заказ", "Заказчик", "Дата начала работ", "Сроки", "Услуга", "Команда" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1] = headers[i];
                    ((Excel.Range)worksheet.Cells[1, i + 1]).Font.Bold = true;
                }


                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1] = item.Id;
                    worksheet.Cells[row, 2] = item.Name;
                    worksheet.Cells[row, 3] = item.Customer;
                    worksheet.Cells[row, 4] = item.StartWorkDate;
                    worksheet.Cells[row, 5] = item.Deadline;
                    worksheet.Cells[row, 6] = item.Services;
                    worksheet.Cells[row, 7] = item.Group;
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
