using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class SquadAddUpd : Window
{
    private List<Squad> squads;
    private List<Specialization> spec;
    private Squad CurrentSquads;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public SquadAddUpd(Squad currentSquads, List<Squad> squadList)
    {
        InitializeComponent();
        CurrentSquads = currentSquads;
        this.DataContext = CurrentSquads;
        squads = squadList;
        FillSpecialization();
        this.Closing += SquadAU_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void SquadAU_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }
    private void FillSpecialization()
    {
        spec = new List<Specialization>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM specialization", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentSpec = new Specialization()
            {
                SpecName  = reader.GetString("SpecName")
            };
            spec.Add(currentSpec);
        }
        conn.Close();
        Специализация.ItemsSource = spec;
    }
    
    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = squads.FirstOrDefault(x => x.ID == CurrentSquads.ID);
        if (code == null)
        {
            try
            {
                int rowCount = squads.Count + 1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO squad VALUES (" + Convert.ToInt32(ID.Text) + ", '" +
                             Convert.ToString(Название_отряда.Text) + "', " +
                             Convert.ToInt32(Специализация.SelectedIndex + 1) + "," + Convert.ToInt32(Ответственный.Text) +
                             " );";
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
                string upd = "UPDATE squad SET NameSquad = '" + Convert.ToString(Название_отряда.Text) +
                             "', SpecializationName = " + Convert.ToInt32(Специализация.SelectedIndex + 1) +
                             ",  StaffID = " +
                             Convert.ToInt32(Ответственный.Text) + " WHERE ID =  " + Convert.ToInt32(ID.Text) +
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
        SquadForm sf = new SquadForm();
        Hide();
        sf.Show();
    }
}