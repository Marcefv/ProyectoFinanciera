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
    public class RespuestasController : Controller
    {
        private IndicadoresEntities db = new IndicadoresEntities();

        // GET: Respuestas
        public ActionResult Index()
        {

            List<Respuestas> lista = new List<Respuestas>();
            lista = db.Respuestas.Where(x => x.PreguntaId == x.Preguntas.ID
                        ).Select(x => x).OrderBy(x => x.PreguntaId).ToList();


            return View(lista);
        }
        // GET: Respuestas/Respuestas/5
        public ActionResult Respuestas(int? id)
        {
            List<Respuestas> lista = new List<Respuestas>();
            lista = db.Respuestas.Where(x => x.PreguntaId == id
                        ).Select(x => x).OrderBy(x => x.PreguntaId).ToList();
            ViewBag.Nombre = db.Preguntas.Find(id).Pregunta;
            return View(lista);
        }

        // GET: Respuestas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Respuestas respuestas = db.Respuestas.Find(id);
            if (respuestas == null)
            {
                return HttpNotFound();
            }
            return View(respuestas);
        }

        // GET: Respuestas/Create
        public ActionResult Create()
        {

            ViewBag.PreguntaId = new SelectList(db.Preguntas, "ID", "Pregunta");
            return View();
        }

        // POST: Respuestas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Respuesta,nombreResponde,PreguntaId")] Respuestas respuestas)
        {
            if (ModelState.IsValid)
            {
                db.Respuestas.Add(respuestas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PreguntaId = new SelectList(db.Preguntas, "ID", "Nombre", respuestas.PreguntaId);

            return View(respuestas);
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
