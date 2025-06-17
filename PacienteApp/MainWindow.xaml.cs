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
            CargarCitas();
            CargarNotificaciones();
        }

        private async void CargarCitas()
        {
            var citas = await ApiService.ObtenerCitasAsync();
            var ahora = DateTime.Now;

            var futuras = citas.Where(c => c.FechaHora > ahora).OrderBy(c => c.FechaHora).ToList();
            var pasadas = citas.Where(c => c.FechaHora <= ahora).OrderByDescending(c => c.FechaHora).ToList();

            dgCitasFuturas.ItemsSource = futuras;
            dgCitasPasadas.ItemsSource = pasadas;

            var citaProxima = futuras.FirstOrDefault(c => (c.FechaHora - ahora).TotalHours <= 24);
            if (citaProxima != null)
            {
                MessageBox.Show(
                    $"🔔 Recuerda tu cita próximamente:\n\n🗓 Fecha: {citaProxima.FechaHora:g}\n📄 Motivo: {citaProxima.Motivo}",
                    "Cita Próxima",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }

        private async void CargarHistorial()
        {
            List<Consulta> historial = await ApiService.ObtenerHistorialAsync();
            dgHistorial.ItemsSource = historial;
        }

        private async void CargarNotificaciones()
{
    var notificaciones = new List<string>();

    var citas = await ApiService.ObtenerCitasAsync();
    var ahora = DateTime.Now;
    var proxima = citas.FirstOrDefault(c => (c.FechaHora - ahora).TotalHours <= 24 && c.FechaHora > ahora);

    if (proxima != null)
    {
        notificaciones.Add($"📅 Tienes una cita programada para mañana a las {proxima.FechaHora:t}.");
    }

    var historial = await ApiService.ObtenerHistorialAsync();
    var ultimaConsulta = historial.OrderByDescending(c => c.FechaHora).FirstOrDefault();

    if (ultimaConsulta != null)
    {
        notificaciones.Add($"✅ Tu última consulta fue el {ultimaConsulta.FechaHora:dd/MM/yyyy}.\n📄 Diagnóstico: {ultimaConsulta.Diagnostico}");
    }

    if (notificaciones.Count == 0)
    {
        notificaciones.Add("🕊️ No tienes notificaciones por ahora.");
    }

    lstNotificaciones.ItemsSource = notificaciones;
}

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            Helpers.SessionHelper.Token = null;
            Helpers.SessionHelper.UsuarioId = null;

            var login = new Views.LoginWindow();
            login.Show();

            this.Close();
        }

    }
}
