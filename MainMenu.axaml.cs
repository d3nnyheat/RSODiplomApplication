using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RSODiplomApplication;

public partial class MainMenu : Window
{
    public MainMenu()
    {
        InitializeComponent();
        this.Closing += MainMenu_Closing; // завершает работу приложения в случае закрытии программы на крестик
    }
    private void MainMenu_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Environment.Exit(0);
    }

    private void MembersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        MembersForm mf = new MembersForm();
        Hide();
        mf.Show();
    }

    private void Logon_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindow lw = new LoginWindow();
        Hide();
        lw.Show();
    }

    private void CatalogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        CatalogForm cf = new CatalogForm();
        Hide();
        cf.Show();
    }

    private void OrdersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        OrdersForm of = new OrdersForm();
        Hide();
        of.Show();
    }

    private void StaffButton_OnClick(object? sender, RoutedEventArgs e)
    {
        StaffForm sf = new StaffForm();
        Hide();
        sf.Show();
    }

    private void ProceduresButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ProcedureForm pf = new ProcedureForm();
        Hide();
        pf.Show();
    }

    private void ParametersButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ParametrsForm param = new ParametrsForm();
        Hide();
        param.Show();
    }
}