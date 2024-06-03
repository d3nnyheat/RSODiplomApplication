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
                    string backupFile = Path.Combine(backupPath, $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}.sql");
                    Process.Start($"C:/Program Files/MySQL/MySQL Server 8.3/bin/mysqldump.exe",
                        $"-v -h localhost -u root -p landoNorris4 {dbName} --result-file={backupFile}");
                    Console.WriteLine("Резервное копирование завершено.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при резервном копировании: {ex.Message}");
            }
        }
    }

    private void Restore_OnClick(object? sender, RoutedEventArgs e)
    {
        MainMenu menu = new MainMenu();
        Hide();
        menu.Show();
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

