using System.Windows;
using PacienteApp.Services;
using PacienteApp.Models;

namespace PacienteApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            var (success, usuario) = await ApiService.LoginAsync(email, password);

            if (!success)
            {
                MessageBox.Show("Credenciales incorrectas.");
                return;
            }

            if (usuario.Rol != "paciente")
            {
                MessageBox.Show("Acceso denegado. Solo los pacientes pueden iniciar sesión en esta aplicación.");
                return;
            }

            // Ir a la ventana principal
            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
