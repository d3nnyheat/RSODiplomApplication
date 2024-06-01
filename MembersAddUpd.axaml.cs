using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Microsoft.CodeAnalysis;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class MembersAddUpd : Window
{
    private List<Members> members;
    private List<City> city;
    private List<Street> streets;
    private List<Squad> squads;
    private List<Position> positions;
    private Members CurrentMember;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public MembersAddUpd(Members currentMember, List<Members> member)
    {
        InitializeComponent();
        CurrentMember = currentMember;
        this.DataContext = CurrentMember;
        members = member;
        FillCity();
        FillStreet();
        FillSquad();
        FillPositions();
        this.Closing += MemberAddUpd_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }

    private void MemberAddUpd_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }

    private void FillPositions()
    {
        positions = new List<Position>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM position", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentPosition = new Position()
            {
                PositionName  = reader.GetString("PositionName")
                
            };
            positions.Add(currentPosition);
        }
        conn.Close();
        Должность.ItemsSource = positions;
    }

    private void FillSquad()
    {
        squads = new List<Squad>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM squad", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentSquad = new Squad()
            {
                NameSquad  = reader.GetString("NameSquad")
                
            };
            squads.Add(currentSquad);
        }
        conn.Close();
        Отряд.ItemsSource = squads;
    }

    private void FillCity()
    {
        city = new List<City>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM city", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentCity = new City()
            {
                CityName  = reader.GetString("CityName")
                
            };
            city.Add(currentCity);
        }
        conn.Close();
        Город.ItemsSource = city;
    }

    private void FillStreet()
    {
        streets = new List<Street>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand("SELECT * FROM street", conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentStreet = new Street()
            {
                StreetName  = reader.GetString("StreetName")
                
            };
            streets.Add(currentStreet);
        }
        conn.Close();
        Улица.ItemsSource = streets;
    }

    private void Save_OnClick(object? sender, RoutedEventArgs e)
    {
        var code = members.FirstOrDefault(x => x.ID == CurrentMember.ID);
        if (code == null)
        {
            try
            {
                int rowCount = members.Count+1;
                ID.Text = rowCount.ToString();
                conn = new MySqlConnection(connString);
                conn.Open();
                string add = "INSERT INTO members VALUES (" + Convert.ToInt32(ID.Text)+ ", '" + Фамилия.Text + "', '" + Имя.Text +"','" + Отчество.Text +"', " + Convert.ToInt32(Возраст.Text)+ ",'" + Номер_телефона.Text +"', " + Convert.ToInt32(Город.SelectedIndex+1 ) + ", " + Convert.ToInt32(Улица.SelectedIndex+1 ) + ", " + Convert.ToInt32(Номер_дома.Text)+ ", " + Convert.ToInt32(Номер_квартиры.Text)+ ", " + Convert.ToInt32(ID_паспорта.Text)+ "," + Convert.ToInt32(Отряд.SelectedIndex+1 ) + "," + Convert.ToInt32(Должность.SelectedIndex+1 ) + " );";
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
                string upd = "UPDATE members SET Surname = '" + Фамилия.Text + "', Name = '" + Имя.Text + "', FatherName = '" + Отчество.Text + "', Age = " + Convert.ToInt32(Возраст.Text) + ", PhoneNumber = '" + Номер_телефона.Text + "', City = "+ Convert.ToInt32(Город.SelectedIndex+1) + ", Street = "+ Convert.ToInt32(Улица.SelectedIndex+1) + ", HouseNumber = " + Convert.ToInt32(Номер_дома.Text) + ", ApartmentNumber = " + Convert.ToInt32(Номер_квартиры.Text) + ", PassportID = " + Convert.ToInt32(ID_паспорта.Text) + ", SquadName = "+ Convert.ToInt32(Отряд.SelectedIndex+1) + ", Position = "+ Convert.ToInt32(Должность.SelectedIndex+1) + " WHERE ID =  " + Convert.ToInt32(ID.Text) + ";";
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
        MembersForm mf = new MembersForm();
        Hide();
        mf.Show();
    }
}