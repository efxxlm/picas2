
using System;

namespace asivamosffie.model.APIModels
{
    public class ComiteGrilla
    {
        public int Id { get; set; }
        public DateTime FechaComite { get; set; }
        public string NumeroComite { get; set; }
        public string EstadoComite { get; set; } 
        public string EstadoComiteCodigo { get; set; }
         
        public string EstadoActa { get; set; }
        public string EstadoActaCodigo { get; set; }
        public bool? RegistroCompleto { get; set; }
        public string RegistroCompletoNombre { get; set; }
    }
}