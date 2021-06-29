using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomina2019.Models.ViewModels
{
	public class tblPadronPersonas
	{
		public int iIdPadron { get; set; }
		[Required]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo Puedes capturar Letras")]
		[Display(Name = "Nombre")]
		public string cNombre { get; set; }
		[Required]
		[Display(Name = "Apellido Paterno")]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo Puedes capturar Letras")]
		public string cApellido1 { get; set;}
		[Required]
		[Display(Name = "Apellido Materno")]
		[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Solo Puedes capturar Letras")]
		public string cApellido2 { get; set; }
		[Required]
		[Display(Name = "Direccion")]
		public string cDireccion { get; set; }
		[Required]
		[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Solo Puedes Capturar valores Numericos")]
		[Display(Name = "Telefono")]
		public string cTelefono { get; set; }
		public DateTime dtCreacion { get; set;}
		public DateTime dtModificacion { get; set;}
	}
}