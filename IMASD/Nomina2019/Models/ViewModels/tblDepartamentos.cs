using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomina2019.Models.ViewModels
{
    public class tblDepartamentos
    {
        public int iIdDepartamento { get; set; }
		public string cNombreDepartamento { get; set; }
        public DateTime dtCreacion { get; set; }
        public DateTime dtModificacion { get; set; }
    }
}