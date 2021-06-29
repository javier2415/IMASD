using Nomina2019.Data;
using Nomina2019.Models.SendModels;
//using Nomina2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nomina2019.Controllers.Empleados
{
    public class tblEmpleadosController : Controller
    {
        // GET: tblEmpleados
        public async Task<ActionResult> Index(int iIdPeriodoPago = 0, int iIdDepartamento = 0)
        {
            var respuesta = new Nomina2019.Models.ViewModels.tblEmpleadoswithList();
            List <Models.ViewModels.tblEmpleados> lstEmpleados = new List<Models.ViewModels.tblEmpleados>();
            using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
            {
                var lstDepartamentosTMP = db.tblEmpleados.Select(x => new Models.ViewModels.tblEmpleados
                {
                    iIdEmpleado = x.iIdEmpleado,
                    Padron = new Models.ViewModels.tblPadronPersonas { 
                        cNombre = x.tblPadronPersonas.cNombre,
                        cApellido1 = x.tblPadronPersonas.cApellido1,
                        cApellido2 = x.tblPadronPersonas.cApellido2
                    },
                    Departamento = new Models.ViewModels.tblDepartamentos
                    {
                        iIdDepartamento = x.tblDepartamentos.iIdDepartamento,
                        cNombreDepartamento = x.tblDepartamentos.cNombreDepartamento
                    },
                    dSueldoDiario = (decimal)x.tblTabuladorSueldo.Where(w => (bool)w.lActivo).FirstOrDefault().dSueldoDiario,
                    PeriodoPago = new Models.ViewModels.tblPeriodosPago
                    {
                        iIdPeriodoPago = x.tblPeriodosPago.iIdPeriodoPago,
                        cNombrePeriodo = x.tblPeriodosPago.cNombrePeriodo,
                        iDiasxPeriodo = (int)x.tblPeriodosPago.iDiasxPeriodo
                    },
                    lActivo = (bool)x.lActivo,
                    dtCreacion = (DateTime)x.dtCreacion,
                    dtModificacion = (DateTime)x.dtModificacion
                }).Where(f=>f.lActivo).ToList();

                if (iIdPeriodoPago > 0 && iIdDepartamento > 0)
                {
                    lstEmpleados = lstDepartamentosTMP.Where(x => x.PeriodoPago.iIdPeriodoPago == iIdPeriodoPago && x.Departamento.iIdDepartamento == iIdDepartamento).ToList();
                }
                else
                {
                    if (iIdPeriodoPago > 0)
                    {
                        lstEmpleados = lstDepartamentosTMP.Where(x => x.PeriodoPago.iIdPeriodoPago == iIdPeriodoPago).ToList();
                    }
                    else
                    {
                        if (iIdDepartamento > 0)
                        {
                            lstEmpleados = lstDepartamentosTMP.Where(x => x.Departamento.iIdDepartamento == iIdDepartamento).ToList();
                        }
                        else
                        {
                            lstEmpleados = lstDepartamentosTMP;
                        }
                    }

                }


            }

            respuesta.lsttblEmpleados = lstEmpleados;
            respuesta.lstDepartamentos = await obtenerDepartamentos();
            respuesta.lstPeriodosPago = await obtenerPeriodosPago();

            return View(@"~/Views\tblEmpleados\tblEmpleados.cshtml", respuesta);
        }

        /// <summary>
        /// Metodo para optener los departamentos disponibles
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> obtenerDepartamentos()
        {
            
            using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
            {
                var departamentos = db
                            .tblDepartamentos
                            .Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.iIdDepartamento.ToString(),
                                        Text = x.cNombreDepartamento
                                    }).ToList();

                return new SelectList(departamentos, "Value", "Text");
            }
        }

        /// <summary>
        /// Metodo para optener los periodos de pago disponibles
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<SelectListItem>> obtenerPeriodosPago()
        {

            using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
            {
                var periodos = db
                            .tblPeriodosPago
                            .Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.iIdPeriodoPago.ToString(),
                                        Text = x.cNombrePeriodo
                                    }).ToList();

                return new SelectList(periodos, "Value", "Text");
            }
        }


        /// <summary>
        /// Metodo de carga de la vista Nuevo
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Nuevo()
        {
            var objEmpleados = new Models.ViewModels.tblEmpleados();
            objEmpleados.lstDepartamentos = await obtenerDepartamentos();
            objEmpleados.lstPeriodosPago = await obtenerPeriodosPago();

            return View(@"~/Views\tblEmpleados\Nuevo.cshtml",objEmpleados);
        }


        /// <summary>
        /// Metodo para crear un nuevo empleado
        /// </summary>
        /// <param name="_empleado"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Nuevo(Models.ViewModels.tblEmpleados _empleado)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
                    {
                        var respuesta = db.empleados_crud(
                                        0/*identificador empleado vacio*/,
                                        _empleado.Padron.iIdPadron,
                                        _empleado.Padron.cNombre,
                                        _empleado.Padron.cApellido1,
                                        _empleado.Padron.cApellido2,
                                        _empleado.Padron.cDireccion,
                                        _empleado.Padron.cTelefono,
                                        _empleado.PeriodoPago.iIdPeriodoPago,
                                        _empleado.Departamento.iIdDepartamento,
                                        _empleado.dSueldoDiario,
                                        1/*validador Crear Nuevo*/);
                    }
                    return Redirect("../tblempleados");
                }


                _empleado.lstDepartamentos = await obtenerDepartamentos();
                _empleado.lstPeriodosPago = await obtenerPeriodosPago();
                return View(_empleado);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        /// <summary>
        /// Metodo de carga de la vista Editar
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Editar(int Id)
        {
            var objEmpleados = new Models.ViewModels.tblEmpleados();

            using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
            {
                List<Models.ViewModels.tblEmpleados> lstTblEmpleados = db.tblEmpleados.Select(x => new Models.ViewModels.tblEmpleados
                {
                    iIdEmpleado = x.iIdEmpleado,
                    Padron = new Models.ViewModels.tblPadronPersonas
                    {
                        iIdPadron = x.tblPadronPersonas.iIdPadron,
                        cNombre = x.tblPadronPersonas.cNombre,
                        cApellido1 = x.tblPadronPersonas.cApellido1,
                        cApellido2 = x.tblPadronPersonas.cApellido2,
                        cDireccion = x.tblPadronPersonas.cDireccion,
                        cTelefono = x.tblPadronPersonas.cTelefono
                    },
                    Departamento = new Models.ViewModels.tblDepartamentos
                    {
                        iIdDepartamento = x.tblDepartamentos.iIdDepartamento,
                        cNombreDepartamento = x.tblDepartamentos.cNombreDepartamento
                    },
                    dSueldoDiario = (decimal)x.tblTabuladorSueldo.Where(w => (bool)w.lActivo).FirstOrDefault().dSueldoDiario,
                    PeriodoPago = new Models.ViewModels.tblPeriodosPago
                    {
                        iIdPeriodoPago = x.tblPeriodosPago.iIdPeriodoPago,
                        cNombrePeriodo = x.tblPeriodosPago.cNombrePeriodo,
                        iDiasxPeriodo = (int)x.tblPeriodosPago.iDiasxPeriodo
                    },
                    lActivo = (bool)x.lActivo,
                    dtCreacion = (DateTime)x.dtCreacion,
                    dtModificacion = (DateTime)x.dtModificacion
                }).Where(f => f.lActivo && f.iIdEmpleado == Id).ToList();

                objEmpleados = lstTblEmpleados.FirstOrDefault();
                objEmpleados.lstDepartamentos = await obtenerDepartamentos();
                objEmpleados.lstPeriodosPago = await obtenerPeriodosPago();

            }
            
            return View(@"~/Views\tblEmpleados\Editar.cshtml", objEmpleados);

        }


        /// <summary>
        /// Metodo para editar un empleado
        /// </summary>
        /// <param name="_empleado"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Editar(Models.ViewModels.tblEmpleados _empleado)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
                    {
                        var respuesta = db.empleados_crud(
                                        _empleado.iIdEmpleado /*Identificador Empleado*/,
                                        _empleado.Padron.iIdPadron,
                                        _empleado.Padron.cNombre,
                                        _empleado.Padron.cApellido1,
                                        _empleado.Padron.cApellido2,
                                        _empleado.Padron.cDireccion,
                                        _empleado.Padron.cTelefono,
                                        _empleado.PeriodoPago.iIdPeriodoPago,
                                        _empleado.Departamento.iIdDepartamento,
                                        _empleado.dSueldoDiario,
                                        2/*validador Editar*/);
                    }
                    return Redirect("../");
                }


                _empleado.lstDepartamentos = await obtenerDepartamentos();
                _empleado.lstPeriodosPago = await obtenerPeriodosPago();
                return View(_empleado);


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para eliminar un empleado
        /// </summary>
        /// <param name="_empleado"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Eliminar(int Id)
        {
            try
            {
                    using (BDNOMINA2019Entities db = new BDNOMINA2019Entities())
                    {
                        var respuesta = db.empleados_crud(
                                        Id/*Id del empleado*/,
                                        0,
                                        "",
                                        "",
                                        "",
                                        "",
                                        "",
                                        0,
                                        0,
                                        0,
                                        3/*validador Eliminar*/);
                    }

                return Redirect("../");

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}