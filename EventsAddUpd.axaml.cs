using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class EventsAddUpd : Window
{
    private List<Events> events;
    private List<EventType> eventtype;
    private Events CurrentEvents;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public EventsAddUpd(Events currentEvents, List<Events> eventsList)
    {
        InitializeComponent();
        CurrentEvents = currentEvents;
        this.DataContext = CurrentEvents;
        events = eventsList;
        FillEventType();
        this.Closing += EventsAU_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void EventsAU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private void FillEventType()
    {
        eventtype = new List<EventType>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM eventype", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentEventType = new EventType()
            {
                EventTypeName  = reader.GetString("EventTypeName")
            };
            eventtype.Add(currentEventType);
        }
        conn.Close();
        Уровень.ItemsSource = eventtype;
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = events.FirstOrDefault(x => x.ID == CurrentEvents.ID);
        if (code == null)
        {
            try
            {
                int rowCount = events.Count + 1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO events VALUES (" + Convert.ToInt32(ID.Text) + ", '" +
                             Convert.ToString(Название_мероприятия.Text) + "', " +
                             Convert.ToInt32(Уровень.SelectedIndex + 1) + ",'" +
                             Convert.ToString(Описание.Text) +
                             "');";
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
                string upd = "UPDATE events SET Name = '" + Convert.ToString(Название_мероприятия.Text) +
                             "', EventType = " + Convert.ToInt32(Уровень.SelectedIndex + 1) +
                             ",  Description = '" +
                             Convert.ToString(Описание.Text) + "' WHERE ID =  " + Convert.ToInt32(ID.Text) +
                             ";";
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
        EventsForm ef = new EventsForm();
        Hide();
        ef.Show();
    }
}