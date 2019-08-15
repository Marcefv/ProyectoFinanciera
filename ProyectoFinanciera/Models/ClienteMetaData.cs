using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinanciera.Models
{
    public class ClienteMetaData
    {
      
            public int Id { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")] 
            public string Nombre { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]   
            public string Apellidos { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [DisplayName("Cédula")]
            public int Cedula { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [Range(0,130)]
            public int Edad { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [DisplayName("Correo")]
            public string Coreo { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [DisplayName("Profesión")]
            public string Profesion { get; set; }
            [DisplayName("Distrito")]
            [Required(ErrorMessage = "Este dato es obligatorio")]   
            public int Id_distrito { get; set; }

            public virtual Distrito Distrito { get; set; }
       
    }
}