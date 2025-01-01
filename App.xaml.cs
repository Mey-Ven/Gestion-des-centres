using Microsoft.Maui.Controls;

namespace MauiApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent(); // Charge les ressources définies dans App.xaml
            MainPage = new MainPage(); // Définit la page principale
        }
    }
}
