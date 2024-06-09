using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RSODiplomApplication;

public partial class ClientMainMenu : Window
{
    public ClientMainMenu()
    {
        InitializeComponent();
    }

    private void CatalogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientCatalog cc = new ClientCatalog();
        Hide();
        cc.Show();
    }

    private void StaffButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientStaff cs = new ClientStaff();
        Hide();
        cs.Show();
    }

    private void SquadButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientSquad csquad = new ClientSquad();
        Hide();
        csquad.Show();
    }

    private void EventsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        ClientEvents ce = new ClientEvents();
        Hide();
        ce.Show();
    }

    private void Logon_OnClick(object? sender, RoutedEventArgs e)
    {
        LoginWindow lw = new LoginWindow();
        Hide();
        lw.Show();
    }
}