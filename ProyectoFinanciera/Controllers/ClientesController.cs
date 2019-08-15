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
    public class ClientesController : Controller
    {
        private IndicadoresEntities db = new IndicadoresEntities();

        // GET: Clientes
        public ActionResult Index()
        {
            var cliente = db.Cliente.Include(c => c.Distrito);
            return View(cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.Id_Canton = new SelectList(db.Canton, "Id", "Canton1");
            ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
            ViewBag.Id_distrito = new SelectList(db.Distrito, "Id", "Distrito1");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellidos,Cedula,Edad,Coreo,Profesion,Id_distrito")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                var clientesLista= db.Cliente.ToList();
                var clienteExistente= false;
                foreach (var item in clientesLista)
                {
                    if (cliente.Coreo==item.Coreo)
                    {
                        clienteExistente = true;   
                    }
                }
                if (clienteExistente)
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
            }
            ViewBag.Id_Canton = new SelectList(db.Canton, "Id");
            ViewBag.Provincia = new SelectList(db.Provincia, "Id");
            ViewBag.Id_distrito = new SelectList(db.Distrito, "Id", "Distrito1");
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id_distrito = new SelectList(db.Distrito, "Id", "Distrito1", cliente.Id_distrito);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Apellidos,Cedula,Edad,Coreo,Profesion,Id_distrito")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_distrito = new SelectList(db.Distrito, "Id", "Distrito1", cliente.Id_distrito);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //llenar lista de cantones segun provincia

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadCantonesByProvincia(string id)
        {
         
            var ident = Convert.ToInt32(id);
            ViewBag.Id_canton = db.Canton.Where(x => x.Id_provincia == ident).Select(p => new
            {
                Value = p.Id,
                Text = p.Canton1
            }).ToList();
            return Json(ViewBag.Id_canton, JsonRequestBehavior.AllowGet);
        }

        //llenar lista de distritos segun canton
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadDistritosByCanton(string id)
        {
            var ident = Convert.ToInt32(id);
            ViewBag.Id_distrito = db.Distrito.Where(x => x.Id_canton == ident).Select(p => new
            {
                Value = p.Id,
                Text = p.Distrito1
            }).ToList();
            return Json(ViewBag.Id_distrito, JsonRequestBehavior.AllowGet);
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
