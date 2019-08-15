using ProyectoFinanciera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinanciera.Controllers
{
    public class IndicadoresController : Controller
    {
       
        public ActionResult graphTipoDeCambio(DateTime inicio, DateTime final, int indicador, string titulo)
        {
            llamadasIndicadores ind = new llamadasIndicadores();
            List<Indicadores> lista = new List<Indicadores>();
            lista = ind.obtieneDatosDeBD(inicio, final, indicador);
            ViewBag.y = new List<decimal>();
            ViewBag.x = new List<DateTime>();

            foreach (var item in lista)
            {
                ViewBag.y.Add(item.NUM_VALOR);
                ViewBag.x.Add(item.DES_FECHA);
            }
            ViewBag.x.ToArray();
            ViewBag.y.ToArray();
            ViewBag.Title = titulo;
            return View();
        }

        public ActionResult graphTasas(DateTime inicio, DateTime final, int indicador, string titulo, float mayor, float menor)
        {
            llamadasIndicadores ind = new llamadasIndicadores();
            List<Indicadores> lista = new List<Indicadores>();
            lista = ind.obtieneDatosDeBD(inicio, final, indicador);
            ViewBag.y = new List<decimal>();
            ViewBag.x = new List<DateTime>();
            ViewBag.mayor = mayor;
            ViewBag.menor = menor;

            foreach (var item in lista)
            {
                ViewBag.y.Add(item.NUM_VALOR);
                ViewBag.x.Add(item.DES_FECHA);
            }
            ViewBag.x.ToArray();
            ViewBag.y.ToArray();
            ViewBag.Title = titulo;
            return View();
        }


        public ActionResult compraDolar()
        {
            DateTime theDate = DateTime.Today;
            DateTime twoyearsago = theDate.AddYears(-2);
            ViewBag.inicio = twoyearsago;
            ViewBag.final = theDate;
            ViewBag.indicador = 317;
            ViewBag.titulo = "Tipo de Cambio de Compra del Dólar " + twoyearsago.Year + " - " + theDate.Year;
            return View();
        }

        public ActionResult ventaDolar()
        {
            DateTime theDate = DateTime.Today;
            DateTime twoyearsago = theDate.AddYears(-2);
            ViewBag.inicio = twoyearsago;
            ViewBag.final = theDate;
            ViewBag.indicador = 318;
            ViewBag.titulo = "Tipo de Cambio de Venta del Dólar " + twoyearsago.Year + " - " + theDate.Year;
            return View();
        }

        public ActionResult basicaPasiva()
        {
            DateTime theDate = DateTime.Today;
            DateTime twoyearsago = theDate.AddYears(-2);
            ViewBag.inicio = twoyearsago;
            ViewBag.final = theDate;
            ViewBag.indicador = 19654;
            ViewBag.mayor = 7.5;
            ViewBag.menor = 5.5;
            ViewBag.titulo = "Tasa Básica Pasiva " + twoyearsago.Year + " - " + theDate.Year;
            return View();
        }

        public ActionResult politicaMonetaria()
        {
            DateTime theDate = DateTime.Today;
            DateTime twoyearsago = theDate.AddYears(-2);
            ViewBag.inicio = twoyearsago;
            ViewBag.final = theDate;
            ViewBag.mayor =5.5;
            ViewBag.menor = 3.25;
            ViewBag.indicador = 3541;
            ViewBag.titulo = "Tasa de Política Monetaria " + twoyearsago.Year+ " - "+theDate.Year;
            return View();
        }


        public ActionResult comportamientoIndicadores()
        {
            DateTime theDate = DateTime.Today;
            DateTime sixmonths = theDate.AddMonths(-5);
            ViewBag.inicio =sixmonths;
            ViewBag.final = theDate;
            ViewBag.mayorbp = 7.5;
            ViewBag.menorbp= 5.5;
            ViewBag.mayorpm = 5.5;
            ViewBag.menorpm = 3.25;
            ViewBag.indicadorpm = 3541;
            ViewBag.indicadorbp = 19654;
            ViewBag.indicadorvd = 318;
            ViewBag.indicadorcd = 317;
            ViewBag.titulo = "Tasa de Política Monetaria " + sixmonths.Month + " - " + theDate.Month+ ", "+ theDate.Year;
            return View();
        }



    }
}