using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class OrdersForm : Window
{
    public OrdersForm()
    {
        InitializeComponent();
        this.Closing += OrdersForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "select orders.ID, orders.MemberID, catalog.CatalogName, orders.Amount, status.StatusName\n" +
                               "from orders\n" +
                               "join catalog on orders.CatalogName=catalog.ID\n" +
                               "join status on orders.Status=status.ID;";
        ShowTable(fullTableShow);
    }
    private List<Orders> ordersList;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void OrdersForm_Closing(object? sender, WindowClosingEventArgs e)
    {
        Environment.Exit(0);
    }
    private void ShowTable(string sql)
    {
        ordersList = new List<Orders>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentOrders = new Orders()
            {
                ID = reader.GetInt32("ID"),
                MemberID = reader.GetInt32("MemberID"),
                CatalogName = reader.GetString("CatalogName"),
                Amount = reader.GetInt32("Amount"),
                StatusName = reader.GetString("StatusName")
            };
            ordersList.Add(currentOrders);
        }
        conn.Close();
        OrdersGrid.ItemsSource = ordersList;
    }

    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var ords = ordersList;
        ords = ords.Where(x => x.CatalogName.Contains(SearchTextBox.Text)).ToList();
        OrdersGrid.ItemsSource = ords;
    }

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Orders newOrder = new Orders();
        OrdersAddUpd addWindow = new OrdersAddUpd(newOrder, ordersList);
        addWindow.Show();
        this.Hide();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Orders fullorders = OrdersGrid.SelectedItem as Orders;
        if (fullorders == null)
        {
            return;
        }
        OrdersAddUpd updateWindow = new OrdersAddUpd(fullorders, ordersList);
        updateWindow.Show();
        this.Hide();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Orders fullorders = OrdersGrid.SelectedItem as Orders;
            if (fullorders == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM orders WHERE ID = " + fullorders.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            ordersList.Remove(fullorders);
            ShowTable("select orders.ID, orders.MemberID, catalog.CatalogName, orders.Amount, status.StatusName\n" +
                      "from orders\n" +
                      "join catalog on orders.CatalogName=catalog.ID\n" +
                      "join status on orders.Status=status.ID;");
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