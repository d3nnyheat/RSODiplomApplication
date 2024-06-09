using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class CatalogAddUpd : Window
{
    private List<Catalog> catalogs;
    private Catalog fullcatalog;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public CatalogAddUpd(Catalog currentCatalog, List<Catalog> catalog)
    {
        InitializeComponent();
        fullcatalog = currentCatalog;
        this.DataContext = fullcatalog;
        catalogs = catalog;
        this.Closing += CatalogAU_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void CatalogAU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = catalogs.FirstOrDefault(x => x.ID == fullcatalog.ID);
        if (code == null)
        {
            try
            {
                int rowCount = catalogs.Count+1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO catalog VALUES (" + Convert.ToInt32(ID.Text)+ ", '" + Наименование_каталог.Text + "', '" + Цена.Text +"' );";
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
                string upd = "UPDATE catalog SET CatalogName = '" + Наименование_каталог.Text + "', Price = '" + Цена.Text + "' WHERE ID =  " + Convert.ToInt32(ID.Text) + ";";
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
        CatalogForm cf = new CatalogForm();
        Hide();
        cf.Show();
    }
}