using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoFinanciera.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            llamadasIndicadores cargarIndicadores = new llamadasIndicadores();
            cargarIndicadores.baseDatos();

            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        




 
    }
}