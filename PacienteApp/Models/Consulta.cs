using System;

namespace PacienteApp.Models
{
    public class Consulta
    {
        public string Id { get; set; }
        public string PacienteId { get; set; }
        public string MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
    }
}
