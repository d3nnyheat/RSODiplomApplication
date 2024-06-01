using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class StaffForm : Window
{
    public StaffForm()
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
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var stf = staff;
        stf = stf.Where(x => x.Position.Contains(SearchTextBox.Text)).ToList();
        StaffGrid.ItemsSource = stf;
    }

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Staff newStaff = new Staff();
        StaffAddUpd addWindow = new StaffAddUpd(newStaff, staff);
        addWindow.Show();
        this.Hide();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Staff fullstaff = StaffGrid.SelectedItem as Staff;
        if (fullstaff == null)
        {
            return;
        }
        StaffAddUpd updateWindow = new StaffAddUpd(fullstaff, staff);
        updateWindow.Show();
        this.Hide();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Staff fullstaff = StaffGrid.SelectedItem as Staff;
            if (fullstaff == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM staff WHERE ID = " + fullstaff.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            staff.Remove(fullstaff);
            ShowTable("SELECT * FROM catalog;");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void BackToMenu(object? sender, RoutedEventArgs e)
    {
        MainMenu menu = new MainMenu();
        Hide();
        menu.Show();
    }
}