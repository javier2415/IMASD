using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nomina2019.Models.ViewModels
{
    public class tblEmpleadoswithList
    {
        public List<Nomina2019.Models.ViewModels.tblEmpleados> lsttblEmpleados { get; set; }
        public int iIdPeriodoPago { get; set; }
        public int iIdDepartamento { get; set; }
        public IEnumerable<SelectListItem> lstPeriodosPago { get; set; }
        public IEnumerable<SelectListItem> lstDepartamentos { get; set; }
    }
}