using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class StaffAddUpd : Window
{
    private List<Staff> staffs;
    private Staff fullstaff;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public StaffAddUpd(Staff currentStaff, List<Staff> staff)
    {
        InitializeComponent();
        fullstaff = currentStaff;
        this.DataContext = fullstaff;
        staffs = staff;
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = staffs.FirstOrDefault(x => x.ID == fullstaff.ID);
        if (code == null)
        {
            try
            {
                int rowCount = staffs.Count+1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO staff VALUES (" + Convert.ToInt32(ID.Text)+ ", '" + Фамилия.Text + "', '" + Имя.Text +"', '" + Отчество.Text +"', '" + Должность.Text +"' );";
                MySqlCommand cmd = new MySqlCommand(add, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                Good.IsVisible = true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error" + exception);
                Error.IsVisible = true;
            }
        }
        else
        {
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                string upd = "UPDATE staff SET Surname = '" + Фамилия.Text + "', Name = '" + Имя.Text + "', FatherName = '" + Отчество.Text + "', Position = '" + Должность.Text + "' WHERE ID =  " + Convert.ToInt32(ID.Text) + ";";
                MySqlCommand cmd = new MySqlCommand(upd, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                Good.IsVisible = true;
            }
            catch (Exception exception)
            {
                Console.Write("Error" + exception);
                Error.IsVisible = true;
            }
        }
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        StaffForm sf = new StaffForm();
        Hide();
        sf.Show();
    }
}