using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinanciera.Models
{
      [MetadataType(typeof(ClienteMetaData))]
    public class ClientesDir
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public int Cedula { get; set; }
        public int Edad { get; set; }
        public string Coreo { get; set; }
        public string Profesion { get; set; }
        public int Id_distrito { get; set; }
        public int Id_canton { get; set; }
        public int Id_provincia { get; set; }

        public virtual Distrito Distrito { get; set; }
        public virtual Canton Canton { get; set; }
        public virtual Provincia Provincia { get; set; }
    }
}