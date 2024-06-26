using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class ClientSquad : Window
{
    public ClientSquad()
    {
        InitializeComponent();
        this.Closing += SquadForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "select squad.id, squad.NameSquad, specialization.SpecName, squad.StaffID from squad\n" +
                               "join specialization on squad.SpecializationName=specialization.ID;";
        ShowTable(fullTableShow);
        FiltrTable();
    }
    private void SquadForm_Closing(object? sender, WindowClosingEventArgs e)
    {
        Environment.Exit(0);
    }
    private List<Squad> squadList;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void ShowTable(string sql)
    {
        squadList = new List<Squad>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentSquads = new Squad()
            {
                ID = reader.GetInt32("ID"),
                NameSquad = reader.GetString("NameSquad"),
                SpecName = reader.GetString("SpecName"),
                StaffID = reader.GetInt32("StaffID")
            };
            squadList.Add(currentSquads);
        }
        conn.Close();
        SquadGrid.ItemsSource = squadList;
    }
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var squads = squadList;
        squads = squads.Where(x => x.NameSquad.Contains(SearchTextBox.Text)).ToList();
        SquadGrid.ItemsSource = squads;
    }
     private void FiltrTable()
    {
        try
        {
            squadList = new List<Squad>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("select squad.id, squad.NameSquad, specialization.SpecName, squad.StaffID from squad\n" +
                                                    "join specialization on squad.SpecializationName=specialization.ID;", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentSquad = new Squad()
                {
                    ID = reader.GetInt32("ID"),
                    NameSquad = reader.GetString("NameSquad"),
                    SpecName = reader.GetString("SpecName"),
                    StaffID = reader.GetInt32("StaffID")
                };
                squadList.Add(currentSquad);
            }
            conn.Close();
            var typecmb = this.Find<ComboBox>(name:"FiltrComboBox");
            typecmb.ItemsSource = squadList;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
            var currentSquad = TypeCmB.SelectedItem as Squad;
            var fltrmember = squadList
                .Where(x => x.ID == currentSquad.ID)
                .ToList();
            SquadGrid.ItemsSource = fltrmember;
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
            string fullTableShow = "select squad.id, squad.NameSquad, specialization.SpecName, squad.StaffID from squad\n" +
                                   "join specialization on squad.SpecializationName=specialization.ID;";
            ShowTable(fullTableShow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}