using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nomina2019.Models.SendModels
{
    public class tblEmpleadosDTO
    {
        public int iIdEmpleado { get; set; }
        public int iIdPadron { get; set; }
        public int iIdPeriodoPago { get; set; }
        public int iIdDepartamento { get; set; }
        public string cNombre { get; set; }
        public string cApellido1 { get; set; }
        public string cApellido2 { get; set; }
        public string cDireccion { get; set; }
        public string cTelefono { get; set; }
    }
}