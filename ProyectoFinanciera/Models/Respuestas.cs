//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinanciera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class Respuestas
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string Respuesta { get; set; }
        [DisplayName("Usuario Respuesta")]
        [Required(ErrorMessage = "Este dato es obligatorio")]
        public string nombreResponde { get; set; }
        [DisplayName("Pregunta a Responder")]
        public Nullable<int> PreguntaId { get; set; }
    
        public virtual Preguntas Preguntas { get; set; }
    }
}