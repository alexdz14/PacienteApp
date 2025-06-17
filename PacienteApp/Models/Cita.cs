using System;

namespace PacienteApp.Models
{
    public class Cita
    {
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
    }
}
