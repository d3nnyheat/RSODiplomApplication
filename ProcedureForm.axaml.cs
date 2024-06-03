using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MySql.Data.MySqlClient;
using OfficeOpenXml;

namespace RSODiplomApplication;

public partial class ProcedureForm : Window
{
    public ProcedureForm()
    {
        InitializeComponent();
        Call("call rsodatabase.AmountSquadMembers();");
    }
    private List<Procedure> procedures;
    private string connString = "server=localhost;database=rsodatabase;User Id=root;password=landoNorris4";
    private MySqlConnection conn;
    public void Call(string sql)
    {
        procedures = new List<Procedure>();
        conn = new MySqlConnection(connString);
        conn.Open();
        MySqlCommand command = new MySqlCommand(sql, conn);
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var currentProcedure = new Procedure()
            {
                NameSquad = reader.GetString("NameSquad"),
                Amount = reader.GetInt32("Amount")
            };
            procedures.Add(currentProcedure);
        }
        conn.Close();
        DataGrid.ItemsSource = procedures;
    }
    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        MainMenu menu = new MainMenu();
        Hide();
        menu.Show();
    }

    private void DocumentButton_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            string outputFile = @"C:\Otchet\otchet.xlsx";
            string query = "call rsodatabase.AmountSquadMembers();";
            MySqlCommand command = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader dataReader = command.ExecuteReader();
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("КоличествоЧеловек");
                int row = 1;

                for (int i = 1; i <= dataReader.FieldCount; i++)
                {
                    worksheet.Cells[row, i].Value = dataReader.GetName(i - 1);
                }
                while (dataReader.Read())
                {
                    row++;
                    for (int i = 1; i <= dataReader.FieldCount; i++)
                    {

                        worksheet.Cells[row, i].Value = dataReader[i - 1];

                    }
                }
                excelPackage.SaveAs(new FileInfo(outputFile));
            }
            dataReader.Close();
            conn.Close();
            Console.WriteLine("Данные успешно экспортированы в Excel файл.");
        }
        catch (Exception exception)
        {
            Console.Write("Error" + exception);
            Console.WriteLine("Ошибка");
        }
    }
}