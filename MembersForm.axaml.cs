using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;

namespace RSODiplomApplication;

public partial class MembersForm : Window
{
    public MembersForm()
    {
        InitializeComponent();
        string fullTableShow = "select members.ID, members.Surname, members.Name, members.FatherName, members.Age, members.PhoneNumber, city.CityName, street.StreetName, members.HouseNumber, members.ApartmentNumber, members.PassportID, squad.NameSquad, position.PositionName\n" +
                               "from members\n" +
                               "join city \non members.City=city.ID\n" +
                               "join street\non members.Street=street.ID\n" +
                               "join squad\non members.SquadName = squad.ID\n" +
                               "join position\non members.Position=position.ID;";
        ShowTable(fullTableShow);
        FiltrTable();
        this.Closing += MembersForm_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }

    private void MembersForm_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }

    private List<Members> memberlist;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    private void ShowTable(string sql)
    {
        try
        {
            memberlist = new List<Members>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand(sql, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentMembers = new Members()
                {
                    ID = reader.GetInt32("ID"),
                    Surname  = reader.GetString("Surname"),
                    Name = reader.GetString("Name"),
                    FatherName = reader.GetString("FatherName"),
                    Age = reader.GetInt32("Age"),
                    PhoneNumber = reader.GetString("PhoneNumber"),
                    CityName = reader.GetString("CityName"),
                    StreetName = reader.GetString("StreetName"),
                    HouseNumber = reader.GetInt32("HouseNumber"),
                    ApartmentNumber = reader.GetInt32("ApartmentNumber"),
                    PassportID = reader.GetInt32("PassportID"),
                    SquadName = reader.GetString("NameSquad"),
                    PositionName = reader.GetString("PositionName")
                };
                memberlist.Add(currentMembers);
            }
            conn.Close();
            MemberGrid.ItemsSource = memberlist;
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
            memberlist = new List<Members>();
            conn = new MySqlConnection(connString);
            conn.Open();
            MySqlCommand command = new MySqlCommand("select members.ID, members.Surname, members.Name, members.FatherName, members.Age, members.PhoneNumber, city.CityName, street.StreetName, members.HouseNumber, members.ApartmentNumber, members.PassportID, squad.NameSquad, position.PositionName\n" +
                                                    "from members\n" +
                                                    "join city \non members.City=city.ID\n" +
                                                    "join street\non members.Street=street.ID\n" +
                                                    "join squad\non members.SquadName = squad.ID\n" +
                                                    "join position\non members.Position=position.ID;", conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read() && reader.HasRows)
            {
                var currentMembers = new Members()
                {
                    ID = reader.GetInt32("ID"),
                    Surname  = reader.GetString("Surname"),
                    Name = reader.GetString("Name"),
                    FatherName = reader.GetString("FatherName"),
                    Age = reader.GetInt32("Age"),
                    PhoneNumber = reader.GetString("PhoneNumber"),
                    CityName = reader.GetString("CityName"),
                    StreetName = reader.GetString("StreetName"),
                    HouseNumber = reader.GetInt32("HouseNumber"),
                    ApartmentNumber = reader.GetInt32("ApartmentNumber"),
                    PassportID = reader.GetInt32("PassportID"),
                    SquadName = reader.GetString("NameSquad"),
                    PositionName = reader.GetString("PositionName")
                };
                memberlist.Add(currentMembers);
            }
            conn.Close();
            var typecmb = this.Find<ComboBox>(name:"FiltrComboBox");
            typecmb.ItemsSource = memberlist;
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
            var currentMember = TypeCmB.SelectedItem as Members;
            var fltrmember = memberlist
                .Where(x => x.ID == currentMember.ID)
                .ToList();
            MemberGrid.ItemsSource = fltrmember;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void SearchTextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        try
        {
            var mbr = memberlist;
            mbr = mbr.Where(x => x.Surname.Contains(SearchTextBox.Text)).ToList();
            MemberGrid.ItemsSource = mbr;
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
            string fullTableShow = "select members.ID, members.Surname, members.Name, members.FatherName, members.Age, members.PhoneNumber, city.CityName, street.StreetName, members.HouseNumber, members.ApartmentNumber, members.PassportID, squad.NameSquad, position.PositionName\n" +
                                   "from members\n" +
                                   "join city \non members.City=city.ID\n" +
                                   "join street\non members.Street=street.ID\n" +
                                   "join squad\non members.SquadName = squad.ID\n" +
                                   "join position\non members.Position=position.ID;";
            ShowTable(fullTableShow);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void AddData(object? sender, RoutedEventArgs e)
    {
        Members newMember = new Members();
        MembersAddUpd addWindow = new MembersAddUpd(newMember, memberlist);
        addWindow.Show();
        this.Hide();
    }

    private void EditData(object? sender, RoutedEventArgs e)
    {
        Members currentMember = MemberGrid.SelectedItem as Members;
        if (currentMember == null)
        {
            return;
        }
        MembersAddUpd updateWindow = new MembersAddUpd(currentMember, memberlist);
        updateWindow.Show();
        this.Hide();
    }

    private void DeleteData(object? sender, RoutedEventArgs e)
    {
        try
        {
            Members currentMember = MemberGrid.SelectedItem as Members;
            if (currentMember == null)
            {
                return;
            }
            conn = new MySqlConnection(connString);
            conn.Open();
            string sql = "DELETE FROM Members WHERE ID = " + currentMember.ID;
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            memberlist.Remove(currentMember);
            ShowTable("select members.ID, members.Surname, members.Name, members.FatherName, members.Age, members.PhoneNumber, city.CityName, street.StreetName, members.HouseNumber, members.ApartmentNumber, members.PassportID, squad.NameSquad, position.PositionName\n" +
                      "from members\n" +
                      "join city \non members.City=city.ID\n" +
                      "join street\non members.Street=street.ID\n" +
                      "join squad\non members.SquadName = squad.ID\n" +
                      "join position\non members.Position=position.ID;");
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