using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class EventsForm : Window
{
    public EventsForm()
    {
        InitializeComponent();
        this.Closing += EventsForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
        string fullTableShow = "select events.ID, events.Name, eventype.EventTypeName, events.Description from events\n" +
                               "join eventype on events.EventType=eventype.ID;";
        ShowTable(fullTableShow);
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

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Events newEvent = new Events();
        EventsAddUpd addWindow = new EventsAddUpd(newEvent, eventsList);
        addWindow.Show();
        this.Hide();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Events fulleventlist = EventsGrid.SelectedItem as Events;
        if (fulleventlist == null)
        {
            return;
        }
        EventsAddUpd updateWindow = new EventsAddUpd(fulleventlist, eventsList);
        updateWindow.Show();
        this.Hide();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Events fulleventlist = EventsGrid.SelectedItem as Events;
            if (fulleventlist == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM events WHERE ID = " + fulleventlist.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            eventsList.Remove(fulleventlist);
            ShowTable("select events.ID, events.Name, eventype.EventTypeName, events.Description from events\n" +
                      "join eventype on events.EventType=eventype.ID;");
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