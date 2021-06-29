using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nomina2019.Models.ViewModels
{
    public class tblPeriodosPago
    {
        public int iIdPeriodoPago { get; set; }
        public string cNombrePeriodo { get; set; }
		public int iDiasxPeriodo { get; set; }
        public DateTime dtCreacion { get; set; }
        public DateTime dtModificacion { get; set; }
    }
}