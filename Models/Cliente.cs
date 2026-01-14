using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Examen_IntegraComex.Models
{
    public class Cliente
    {
        [Key] // opcional, pero deja explícito
        public int IdCliente { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression(@"^\d{11}$")]
        public string Cuit { get; set; }

        [StringLength(200)]
        public string RazonSocial { get; set; }

        [RegularExpression(@"^\d*$")]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        public bool Activo { get; set; }
    }
}




