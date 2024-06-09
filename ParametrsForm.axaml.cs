using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class ParametrsForm : Window
{
    public ParametrsForm()
    {
        InitializeComponent();
        this.Closing += Parametrs_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void Parametrs_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    public class MySQLRestore
    {
        private string connectionString;
        private string backupFilePath;

        public MySQLRestore(string connectionString, string backupFilePath)
        {
            this.connectionString = connectionString;
            this.backupFilePath = backupFilePath;
        }

        public void Restore_OnClick()
        {
            try
            {
                // Создаем подключение к MySQL
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Открываем соединение
                    connection.Open();
                    // Читаем содержимое файла резервной копии
                    string script = File.ReadAllText(backupFilePath);

                    // Создаем команду SQL для восстановления базы данных из файла
                    using (MySqlCommand cmd = new MySqlCommand(script, connection))
                    {
                        // Выполняем команду
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("База данных успешно восстановлена из файла резервной копии.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при восстановлении базы данных: {ex.Message}");
            }
        }
    }
    public class MySQLBackup
    {
        private string connectionString;
        private string backupPath;

        public MySQLBackup(string connectionString, string backupPath)
        {
            this.connectionString = connectionString;
            this.backupPath = backupPath;
        }

        public void Backup_OnClick()
        {
            try
            {
                // Создаем подключение к MySQL
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Открываем соединение
                    connection.Open();

                    // Создаем команду SQL для выполнения резервного копирования базы данных
                    string dbName = connection.Database;
                    string backupFile = Path.Combine(backupPath, $"{dbName}_{Convert.ToString("Backup")}.sql");
                    Process.Start($"C:/Program Files/MySQL/MySQL Server 8.3/bin/mysqldump.exe",
                        $"-v -h localhost -u root -p {dbName} --ignore-error=TRUE --default-character-set=utf8 --single-transaction=TRUE --skip-triggers --protocol=tcp --result-file={backupFile}");
                    Console.WriteLine("Резервное копирование завершено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при резервном копировании: {ex.Message}");
            }
        }
    }

    private void RestoreMethod(object? sender, RoutedEventArgs e)
    {
        string connectionString = "server=localhost;user=root;password=landoNorris4;port=3306;database=rsoimport;";
        string backupFilePath = @"C:\Backup\rsodatabase_Backup.sql";
        MySQLRestore restore = new MySQLRestore(connectionString, backupFilePath);
        restore.Restore_OnClick();
    }

    private void BackupMethod(object? sender, RoutedEventArgs e)
    {
        string connectionString = "server=localhost;user=root;password=landoNorris4;port=3306;database=rsodatabase;";
        string backupPath = @"C:\Backup";
        MySQLBackup backup = new MySQLBackup(connectionString, backupPath);
        backup.Backup_OnClick();
    }

    private void BackToMenu(object? sender, RoutedEventArgs e)
    {
        MainMenu menu = new MainMenu();
        Hide();
        menu.Show();
    }
}

