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
            [RegularExpression(@"^([0-9]{9})$", ErrorMessage ="El número debe tener 9 caracteres")]
            [DisplayName("Cédula")]
            public int Cedula { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [Range(1,130, ErrorMessage ="Solo puede colocar valores entre 1 y 130")]
            public int Edad { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [DisplayName("Correo")]
            [EmailAddress(ErrorMessage ="Ingrese un correo electrónico válido. Ej: persona@gmail.com")]
            public string Coreo { get; set; }
            [Required(ErrorMessage = "Este dato es obligatorio")]
            [DisplayName("Profesión")]
            public string Profesion { get; set; }
        [DisplayName("Provincia")]
        [Required(ErrorMessage = "Este dato es obligatorio")]
        public int Id_provincia { get; set; }
        [DisplayName("Canton")]
        [Required(ErrorMessage = "Este dato es obligatorio")]
        public int Id_canton { get; set; }
        [Required(ErrorMessage = "Este dato es obligatorio")]
        [DisplayName("Distrito")]
            public int Id_distrito { get; set; }

      

           

    }
}