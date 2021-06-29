using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nomina2019.Models.ViewModels
{
    public class tblEmpleados
    {
        public int iIdEmpleado { get; set; }
        
        public int iIdentificadorEmpleado { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Solo Puedes Capturar valores Numericos")]
        [Display(Name = "Sueldo Diario")]
        public decimal dSueldoDiario { get; set; }

        public tblPadronPersonas Padron { get; set;}
        
        public tblPeriodosPago PeriodoPago { get; set; }
        
        public tblDepartamentos Departamento { get; set; }
        
        public bool lActivo { get; set; }
        
        public DateTime dtCreacion { get; set; }
        
        public DateTime dtModificacion { get; set; }

        public IEnumerable<SelectListItem> lstPeriodosPago { get; set; }
        public IEnumerable<SelectListItem> lstDepartamentos { get; set; }
    }
}