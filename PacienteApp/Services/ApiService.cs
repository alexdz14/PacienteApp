using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PacienteApp.Helpers;
using PacienteApp.Models;
using System.Collections.Generic;

namespace PacienteApp.Services
{
    public static class ApiService
    {
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new System.Uri("https://localhost:7068/")
        };

        public static async Task<(bool success, Usuario usuario)> LoginAsync(string email, string password)
        {
            var loginData = new LoginRequest { Email = email, Password = password };
            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/usuarios/login", content);
            if (!response.IsSuccessStatusCode)
                return (false, null);

            var resultJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(resultJson);

            var token = doc.RootElement.GetProperty("token").GetString();
            var userElement = doc.RootElement.GetProperty("usuario");

            SessionHelper.Token = token;
            SessionHelper.UsuarioId = userElement.GetProperty("id").GetString();

            return (true, new Usuario
            {
                Id = SessionHelper.UsuarioId,
                Nombre = userElement.GetProperty("nombre").GetString(),
                Email = userElement.GetProperty("email").GetString(),
                Rol = userElement.GetProperty("rol").GetString()
            });
        }

        public static void SetAuthHeader()
        {
            if (!string.IsNullOrEmpty(SessionHelper.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SessionHelper.Token);
        }

        public static async Task<List<Consulta>> ObtenerHistorialAsync()
        {
            SetAuthHeader();

            var response = await client.GetAsync("api/consultas/paciente");
            if (!response.IsSuccessStatusCode)
                return new List<Consulta>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Consulta>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
