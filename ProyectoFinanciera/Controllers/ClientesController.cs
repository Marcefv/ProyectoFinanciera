using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ProyectoFinanciera.Models;

namespace ProyectoFinanciera.Controllers
{
    public class ClientesController : Controller
    {
        private IndicadoresEntities db = new IndicadoresEntities();


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
                if (!clienteExistente)
                {
                    db.Cliente.Add(cliente);
                    db.SaveChanges();
                    confirmacionCorreo(cliente);
                    return RedirectToAction("Index", "Home");
                }
                
            }
            var idCanton = db.Canton.Where(x => x.Distrito == cliente.Distrito);
            var idProvincia = db.Provincia.Where(x => x.Canton == idCanton);
            ViewBag.Id_Canton = new SelectList(db.Canton, "Id", "Canton1",idCanton );
            ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1", idProvincia);
            ViewBag.Id_distrito = new SelectList(db.Distrito, "Id", "Distrito1", cliente.Distrito);
            return View(cliente);
        }


        //llenar lista de cantones segun provincia

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadCantonesByProvincia(string id)
        {
            try
            {
                var ident = Convert.ToInt32(id);
                ViewBag.Id_canton = db.Canton.Where(x => x.Id_provincia == ident).Select(p => new
                {
                    Value = p.Id,
                    Text = p.Canton1
                }).ToList();
                return Json(ViewBag.Id_canton, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            
        }

        //llenar lista de distritos segun canton
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult LoadDistritosByCanton(string id)
        {
            try
            {
                var ident = Convert.ToInt32(id);
                ViewBag.Id_distrito = db.Distrito.Where(x => x.Id_canton == ident).Select(p => new
                {
                    Value = p.Id,
                    Text = p.Distrito1
                }).ToList();
                return Json(ViewBag.Id_distrito, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("Error", JsonRequestBehavior.AllowGet);
            }
           
        }

        //envio de correo de confirmacion una vez suscrito
        public void confirmacionCorreo(Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                        var body = "<p>Email From: elprogresofinanciera@gmail.com</p>" +
                        "       <p>Estimad@ " + cliente.Nombre + " " + cliente.Apellidos + " </p>" +
                        "       <p>   Financiera El Progreso le da la bienvenida a su servicio de envío de los indicadores económicos:</p>" +
                                 "<ul> <li>Tipo de Cambio de Venta del Dólar</li><li>Tipo de Cambio de Compra del Dólar</li>" +
                                 "<li>Tasa de Política Monetaria</li><li>Tasa Básica Pasiva</li>" +
                                 "</ul>";
                        var message = new MailMessage();
                        message.To.Add(new MailAddress(cliente.Coreo));
                        message.Subject = "Bienvenid@ a el servicio de indicadores económicos de Financiera El Progreso";
                        message.Body = body;
                        message.IsBodyHtml = true;

                        using (var smtp = new SmtpClient())
                        {
                        var credential = new NetworkCredential
                        {
                            UserName = "elprogresofinanciera@gmail.com",  
                            Password = "passwordprogra5"  
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(message);
                    }              
                    
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
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
