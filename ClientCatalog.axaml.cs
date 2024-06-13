using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class ClientCatalog : Window
{
    public ClientCatalog()
    {
        InitializeComponent();
        this.Closing += CatalogForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "SELECT * FROM catalog;";
        ShowTable(fullTableShow);
        FiltrTable();
    }
    private List<Catalog> catalog;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void CatalogForm_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private void ShowTable(string sql)
    {
        catalog = new List<Catalog>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var fullcatalog = new Catalog()
            {
                ID = reader.GetInt32("ID"),
                CatalogName = reader.GetString("CatalogName"),
                Price = reader.GetInt32("Price")
            };
            catalog.Add(fullcatalog);
        }
        conn.Close();
        CatalogGrid.ItemsSource = catalog;
    }
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var ctlg = catalog;
        ctlg = ctlg.Where(x => x.CatalogName.Contains(SearchTextBox.Text)).ToList();
        CatalogGrid.ItemsSource = ctlg;
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
            var currentCatalog = TypeCmB.SelectedItem as Catalog;
            var fltrmember = catalog
                .Where(x => x.ID == currentCatalog.ID)
                .ToList();
            CatalogGrid.ItemsSource = fltrmember;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
     private void FiltrTable()
    {
        try
        {
            catalog = new List<Catalog>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("SELECT * FROM catalog;", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentCatalog = new Catalog()
                {
                    ID = reader.GetInt32("ID"),
                    CatalogName = reader.GetString("CatalogName"),
                    Price = reader.GetInt32("Price")
                };
                catalog.Add(currentCatalog);
            }
            conn.Close();
            var typecmb = this.Find<ComboBox>(name:"FiltrComboBox");
            typecmb.ItemsSource = catalog;
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
            string fullTableShow = "SELECT * FROM catalog;";
            ShowTable(fullTableShow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}