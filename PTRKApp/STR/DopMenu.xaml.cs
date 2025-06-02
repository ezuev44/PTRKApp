using Microsoft.Win32;
using PTRKApp.AppData;
using PTRKApp.STR.Dop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
    /// Логика взаимодействия для DopMenu.xaml
    /// </summary>
    public partial class DopMenu : Page
    {

        private readonly AppDbContext context = new AppDbContext();

        public DopMenu()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Matem());
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new uchet());
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Sbor());
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MeinMenu());
        }

        private void Button5_Click_1(object sender, RoutedEventArgs e)
        {
            context.Database.Connection.Open();
            Console.WriteLine(context.Database.Connection.ConnectionString);
            var dbName = context.Database.SqlQuery<string>("SELECT DB_NAME() AS DatabaseName").FirstOrDefault();
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Database backups (*.bak)|*.bak";
            saveFile.AddExtension = true;
            saveFile.ShowDialog();
            string _ = saveFile.FileName;
            Console.WriteLine(_);
            if (_ != string.Empty)
                context.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, $"BACKUP DATABASE [{dbName}] to disk=" + "'" + _ + "'");
            context.Database.Connection.Close();
            MessageBox.Show($"Бэкап успешно создан!\nПуть: {_}",
                  "Успех",
                  MessageBoxButton.OK,
                  MessageBoxImage.Information);
        }

        private void Restore_Click(object sender, RoutedEventArgs e)
        {
            RestoreDatabase();
        }

        public void RestoreDatabase()
        {
            try
            {
                string backupPath = SelectBackupFile();
                if (string.IsNullOrEmpty(backupPath)) return;

                var backupInfo = GetBackupInfo(backupPath);
                if (!backupInfo.HasValue) return;

                RestoreDatabase(
                    backupPath,
                    backupInfo.Value.DatabaseName,
                    backupInfo.Value.LogicalDataFile,
                    backupInfo.Value.LogicalLogFile);

                MessageBox.Show("База данных успешно восстановлена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка восстановления: {ex.Message}");
            }
        }

        private string SelectBackupFile()
        {
            var dialog = new OpenFileDialog();
            
            dialog.Filter = "Backup files (*.bak)|*.bak|All files (*.*)|*.*";
            dialog.Title = "Выберите файл резервной копии";
            return dialog.ShowDialog() == true? dialog.FileName : null;
            
        }

        private (string DatabaseName, string LogicalDataFile, string LogicalLogFile)? GetBackupInfo(string backupPath)
        {
            using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();

                string databaseName = null;
                using (var cmd = new SqlCommand("RESTORE HEADERONLY FROM DISK = @backupPath", connection))
                {
                    cmd.Parameters.AddWithValue("@backupPath", backupPath);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            databaseName = reader["DatabaseName"].ToString();
                        }
                    }
                }

                if (string.IsNullOrEmpty(databaseName))
                {
                    MessageBox.Show("Не удалось определить имя базы данных из резервной копии");
                    return null;
                }

                string logicalDataFile = null;
                string logicalLogFile = null;
                using (var cmd = new SqlCommand("RESTORE FILELISTONLY FROM DISK = @backupPath", connection))
                {
                    cmd.Parameters.AddWithValue("@backupPath", backupPath);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string type = reader["Type"].ToString();
                            if (type == "D") logicalDataFile = reader["LogicalName"].ToString();
                            if (type == "L") logicalLogFile = reader["LogicalName"].ToString();
                        }
                    }
                }

                if (string.IsNullOrEmpty(logicalDataFile) || string.IsNullOrEmpty(logicalLogFile))
                {
                    MessageBox.Show("Не удалось получить информацию о файлах базы данных");
                    return null;
                }

                return (databaseName, logicalDataFile, logicalLogFile);
            }
        }

        private void RestoreDatabase(string backupPath, string databaseName, string logicalDataFile, string logicalLogFile)
        {
            using (var connection = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                connection.Open();

                string dataPath = GetSqlDataPath(connection);
                string mdfPath = $"{databaseName}_mdf.mdf";
                string ldfPath = $"{databaseName}_log.ldf";

                string sql = $@"
                IF EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}')
                BEGIN
                    ALTER DATABASE [{databaseName}] 
                    SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                END

                RESTORE DATABASE [{databaseName}] 
                FROM DISK = @backupPath
                WITH 
                    REPLACE,
                    MOVE '{logicalDataFile}' TO '{mdfPath}',
                    MOVE '{logicalLogFile}' TO '{ldfPath}';

                ALTER DATABASE [{databaseName}] 
                SET MULTI_USER;";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@backupPath", backupPath);
                    cmd.CommandTimeout = 600;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetSqlDataPath(SqlConnection connection)
        {
            using (var cmd = new SqlCommand("SELECT SERVERPROPERTY('InstanceDefaultDataPath')", connection))
            {
                var result = cmd.ExecuteScalar() as string;
                return result;
            }
        }
    }
}
