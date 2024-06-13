using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class ClientEvents : Window
{
    public ClientEvents()
    {
        InitializeComponent();
        this.Closing += EventsForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "select events.ID, events.Name, eventype.EventTypeName, events.Description from events\n" +
                               "join eventype on events.EventType=eventype.ID;";
        ShowTable(fullTableShow);
        FiltrTable();
    }
    private void EventsForm_Closing(object? sender, WindowClosingEventArgs e)
    {
        Environment.Exit(0);
    }
    private List<Events> eventsList;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void ShowTable(string sql)
    {
        eventsList = new List<Events>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentEvents = new Events()
            {
                ID = reader.GetInt32("ID"),
                Name = reader.GetString("Name"),
                EventTypeName = reader.GetString("EventTypeName"),
                Description = reader.GetString("Description")
            };
            eventsList.Add(currentEvents);
        }
        conn.Close();
        EventsGrid.ItemsSource = eventsList;
    }
    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        var events = eventsList;
        events = events.Where(x => x.Name.Contains(SearchTextBox.Text)).ToList();
        EventsGrid.ItemsSource = events;
    }
    private void BackToMenu(object? sender, RoutedEventArgs e)
    {
        ClientMainMenu menu = new ClientMainMenu();
        Hide();
        menu.Show();
    }
     private void FiltrTable()
    {
        try
        {
            eventsList = new List<Events>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("select events.ID, events.Name, eventype.EventTypeName, events.Description from events\n" +
                                                    "join eventype on events.EventType=eventype.ID;", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentEvents = new Events()
                {
                    ID = reader.GetInt32("ID"),
                    Name = reader.GetString("Name"),
                    EventTypeName = reader.GetString("EventTypeName"),
                    Description = reader.GetString("Description")
                };
                eventsList.Add(currentEvents);
            }
            conn.Close();
            var typecmb = this.Find<ComboBox>(name:"FiltrComboBox");
            typecmb.ItemsSource = eventsList;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void FiltrTable_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        try
        {
            var TypeCmB = (ComboBox)sender;
            var currentEvent = TypeCmB.SelectedItem as Events;
            var fltrmember = eventsList
                .Where(x => x.ID == currentEvent.ID)
                .ToList();
            EventsGrid.ItemsSource = fltrmember;
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
            string fullTableShow = "select events.ID, events.Name, eventype.EventTypeName, events.Description from events\n" +
                                   "join eventype on events.EventType=eventype.ID;";
            ShowTable(fullTableShow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}