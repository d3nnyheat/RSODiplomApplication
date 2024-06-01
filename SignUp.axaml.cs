using System;
using System.Security.Cryptography;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class SignUp : Window
{
    public SignUp()
    {
        InitializeComponent();
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
    private void SignUpAttempt(object? sender, RoutedEventArgs e)
    {
        try
        {
            conn = new MySqlConnection(connString);
            conn.Open();
            string hashedPassword = HashPassword(PasswordTextBox.Text);
            string add = "INSERT INTO us(login, password, role_id) VALUES ('"+ LoginTextBox.Text +"', '"+ hashedPassword +"',  '"+Convert.ToInt32(RoleTextBox.Text)+"');";
            MySqlCommand cmd = new MySqlCommand(add, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        LoginWindow lw = new LoginWindow();
        Close();
        lw.Show();
    }
}