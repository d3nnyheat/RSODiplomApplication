using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class ClientStaff : Window
{
    public ClientStaff()
    {
        InitializeComponent();
        this.Closing += StaffForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "SELECT * FROM staff;";
        ShowTable(fullTableShow);
    }
    private List<Staff> staff;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void StaffForm_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private void ShowTable(string sql)
    {
        staff = new List<Staff>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var fullstaff = new Staff()
            {
                ID = reader.GetInt32("ID"),
                Surname = reader.GetString("Surname"),
                Name = reader.GetString("Name"),
                FatherName = reader.GetString("FatherName"),
                Position = reader.GetString("Position")
            };
            staff.Add(fullstaff);
        }
        conn.Close();
        StaffGrid.ItemsSource = staff;
    }
    private void FiltrTable()
    {
        try
        {
            staff = new List<Staff>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM staff;", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentStaff = new Staff()
                {
                    ID = reader.GetInt32("ID"),
                    Surname = reader.GetString("Surname"),
                    Name = reader.GetString("Name"),
                    FatherName = reader.GetString("FatherName"),
                    Position = reader.GetString("Position")
                };
                staff.Add(currentStaff);
            }
            conn.Close();
            var typecmb = this.Find<ComboBox>(name:"FiltrComboBox");
            typecmb.ItemsSource = staff;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var stf = staff;
        stf = stf.Where(x => x.Position.Contains(SearchTextBox.Text)).ToList();
        StaffGrid.ItemsSource = stf;
    }
    private void BackToMenu(object? sender, RoutedEventArgs e)
    {
        ClientMainMenu menu = new ClientMainMenu();
        Hide();
        menu.Show();
    }

    private void FiltrTable_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            var TypeCmB = (ComboBox)sender;
            var currentStaff = TypeCmB.SelectedItem as Staff;
            var fltrmember = staff
                .Where(x => x.ID == currentStaff.ID)
                .ToList();
            StaffGrid.ItemsSource = fltrmember;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void ResetTable_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            SearchTextBox.Text = string.Empty;
            string fullTableShow = "SELECT * FROM staff;";
            ShowTable(fullTableShow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}