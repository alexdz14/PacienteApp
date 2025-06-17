using System.Collections.Generic;
using System.Windows;
using PacienteApp.Models;
using PacienteApp.Services;

namespace PacienteApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CargarHistorial();
        }

        private async void CargarHistorial()
        {
            List<Consulta> historial = await ApiService.ObtenerHistorialAsync();
            dgHistorial.ItemsSource = historial;
        }
    }
}
