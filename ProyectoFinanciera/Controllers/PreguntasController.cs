using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProyectoFinanciera.Models;

namespace ProyectoFinanciera.Controllers
{
    public class PreguntasController : Controller
    {
        private IndicadoresEntities db = new IndicadoresEntities();

        // GET: Preguntas
        public ActionResult Index()
        {
            return View(db.Preguntas.ToList());
        }

        // GET: Preguntas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Preguntas preguntas = db.Preguntas.Find(id);
            if (preguntas == null)
            {
                return HttpNotFound();
            }
            return View(preguntas);
        }

        // GET: Preguntas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Preguntas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nombre,Pregunta,Descripcion")] Preguntas preguntas)
        {
            if (ModelState.IsValid)
            {
                db.Preguntas.Add(preguntas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(preguntas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
