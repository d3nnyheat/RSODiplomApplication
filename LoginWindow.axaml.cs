using System;
using System.Security.Cryptography;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        this.Closing += LoginWindow_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void LoginWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    private void LoginAttempt(object? sender, RoutedEventArgs e)
    {
        try
        {
            string hashedPassword = HashPassword(PasswordTextBox.Text);
            string sql1 = "SELECT login, password, role_id FROM users WHERE login = '" + LoginTextBox.Text +
                          "' AND password = '" +
                          hashedPassword + "' AND role_id = '1'";
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql2 = "SELECT login, password, role_id FROM users WHERE login = '" + LoginTextBox.Text +
                          "' AND password = '" +
                          hashedPassword + "' AND role_id = '2'";
            MySqlCommand sqlCommand = new MySqlCommand(sql2, conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql1, conn);

            if (mySqlCommand.ExecuteScalar() != null)
            {
                MainMenu menu = new MainMenu();
                Hide();
                menu.Show();
            }
            if (sqlCommand.ExecuteScalar() != null)
            {
                MainMenu menu = new MainMenu();
                Hide();
                menu.Show();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private void GoToSignUp(object? sender, RoutedEventArgs e)
    {
        SignUp signUp = new SignUp();
        Hide();
        signUp.Show();
    }
}
