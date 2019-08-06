using ProyectoFinanciera.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace ProyectoFinanciera.Controllers
{
    public class SerializeController : Controller
    {

        private IndicadoresEntities db = new IndicadoresEntities();
        // GET: Serialize

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BasicaPasiva()
        {
            return View();
        }

        public ActionResult DolarVenta()
        {
            return View();
        }

        public ActionResult DolarCompra()
        {
            return View();
        }

        public ActionResult PoliticaMonetaria()
        {
            return View();
        }

        public ActionResult Graph(string codigo, string Title)
        {
            DateTime theDate = DateTime.Now;
            String ahora = theDate.ToString("dd/MM/yyyy");
            ViewBag.ahora = ahora;
            DateTime twoyearsago = theDate.AddYears(-2);
            String hace2anios = twoyearsago.ToString("dd/MM/yyyy");
            ViewBag.hace2anios = hace2anios;

            var indicador = (from s in db.Indicadores
                             where (s.DES_FECHA == theDate && s.COD_INDICADORINTERNO == Convert.ToInt32(codigo))
                             select s).ToList();
            List<Indicadores> indicadores=null;
            if (indicador.Count()==0)
            {
                indicadoresReference.wsindicadoreseconomicos objRef = new indicadoresReference.wsindicadoreseconomicos();
                DataSet ds = objRef.ObtenerIndicadoresEconomicos(codigo, hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
               indicadores = ds.Tables[0].AsEnumerable().
                    Select(dataRow => new Indicadores()
                    {
                        COD_INDICADORINTERNO = Convert.ToInt32(dataRow["COD_INDICADORINTERNO"]),
                        DES_FECHA = Convert.ToDateTime(dataRow["DES_FECHA"]),
                        NUM_VALOR = Convert.ToDecimal(dataRow["NUM_VALOR"])
                    }).ToList();

                foreach (var item in indicadores)
                {
                    db.Indicadores.Add(item);
                    db.SaveChanges();
                }
            }
            else
            {
                
            }
            ViewBag.y = new List<decimal>();
            ViewBag.x = new List<DateTime>();

            foreach (var item in indicadores)
            {
                ViewBag.y.Add(item.NUM_VALOR);
                ViewBag.x.Add(item.DES_FECHA);
            }
            ViewBag.x.ToArray();
            ViewBag.y.ToArray();
            ViewBag.Title = Title;
            return View();

        }

     
    }
}