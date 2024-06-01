using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class OrdersAddUpd : Window
{
    private List<Orders> orders;
    private List<Catalog> catalog;
    private List<Status> status;
    private Orders CurrentOrders;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public OrdersAddUpd(Orders currentCatalog, List<Orders> ordersList)
    {
        InitializeComponent();
        CurrentOrders = currentCatalog;
        this.DataContext = CurrentOrders;
        orders = ordersList;
        FillCatalog();
        FillStatus();
    }

    private void FillStatus()
    {
        status = new List<Status>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM status", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentStatus = new Status()
            {
                StatusName  = reader.GetString("StatusName")
                
            };
            status.Add(currentStatus);
        }
        conn.Close();
        Статус.ItemsSource = status;
    }

    private void FillCatalog()
    {
            catalog = new List<Catalog>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM catalog", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentCatalog = new Catalog()
                {
                    CatalogName  = reader.GetString("CatalogName")
                
                };
                catalog.Add(currentCatalog);
            }
            conn.Close();
            Наименования_каталога.ItemsSource = catalog;
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = orders.FirstOrDefault(x => x.ID == CurrentOrders.ID);
        if (code == null)
        {
            try
            {
                int rowCount = orders.Count+1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO orders VALUES (" + Convert.ToInt32(ID.Text)+ ", '" + Convert.ToInt32(ID_члена_организации.Text)+ "', " + Convert.ToInt32(Наименования_каталога.SelectedIndex+1 ) + ", '" + Convert.ToInt32(Количество.Text)+ "'," + Convert.ToInt32(Статус.SelectedIndex+1 ) + " );";
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
                string upd = "UPDATE orders SET MemberID = '" + Convert.ToInt32(ID_члена_организации.Text) + "', CatalogName = " + Convert.ToInt32(Наименования_каталога.SelectedIndex+1 ) + ", Amount = '"+ Convert.ToInt32(Количество.Text) + "', Status = " + Convert.ToInt32(Статус.SelectedIndex+1 ) + " WHERE ID =  " + Convert.ToInt32(ID.Text) + ";";
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
        OrdersForm of = new OrdersForm();
        Hide();
        of.Show();
    }
}