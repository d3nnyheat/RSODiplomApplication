using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class SquadForm : Window
{
    public SquadForm()
    {
        InitializeComponent();
        this.Closing += SquadForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "select squad.id, squad.NameSquad, specialization.SpecName, squad.StaffID from squad\n" +
                               "join specialization on squad.SpecializationName=specialization.ID;";
        ShowTable(fullTableShow);
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

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Squad newSquad = new Squad();
        SquadAddUpd addWindow = new SquadAddUpd(newSquad, squadList);
        addWindow.Show();
        this.Hide();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Squad fullsquadlist = SquadGrid.SelectedItem as Squad;
        if (fullsquadlist == null)
        {
            return;
        }
        SquadAddUpd updateWindow = new SquadAddUpd(fullsquadlist, squadList);
        updateWindow.Show();
        this.Hide();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Squad fullsquadlist = SquadGrid.SelectedItem as Squad;
            if (fullsquadlist == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM squad WHERE ID = " + fullsquadlist.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            squadList.Remove(fullsquadlist);
            ShowTable("select squad.id, squad.NameSquad, specialization.SpecName, squad.StaffID from squad " +
                      "join specialization on squad.SpecializationName=specialization.ID;");
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