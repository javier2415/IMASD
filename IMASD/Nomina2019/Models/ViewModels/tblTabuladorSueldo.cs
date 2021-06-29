using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomina2019.Models.ViewModels
{
    public class tblTabuladorSueldo
    {
		public int iIdTabuladorSueldo { get; set; }
		public int iIdEmpleado { get; set; }
		public decimal dSueldoDiario { get; set; }
		public bool lActivo { get; set; }
		public DateTime dtCreacion { get; set; }
		public DateTime dtModificacion { get; set; }
	}
}