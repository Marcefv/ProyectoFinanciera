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
            ViewBag.Id_Canton = new SelectList(db.Canton.Where(x => x.Id_provincia == 1), "Id", "Canton1");
            ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
            ViewBag.Id_distrito = new SelectList(db.Distrito.Where(x => x.Id_canton == 1), "Id", "Distrito1");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,Apellidos,Cedula,Edad,Coreo,Profesion,Id_provincia, Id_canton, Id_distrito")] ClientesDir cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var clientesLista = db.Cliente.ToList();
                    var clienteExistente = false;
                    foreach (var item in clientesLista)
                    {
                        if (cliente.Coreo == item.Coreo)
                        {
                            clienteExistente = true;
                            ViewBag.correo = "Error";
                            ViewBag.Id_Canton = new SelectList(db.Canton.Where(x => x.Id_provincia == 1), "Id", "Canton1");
                            ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
                            ViewBag.Id_distrito = new SelectList(db.Distrito.Where(x => x.Id_canton == 1), "Id", "Distrito1");
                            return View();
                        }
                    }
                    if (!clienteExistente)
                    {
                        var client = new Cliente();
                        client.Apellidos = cliente.Apellidos;
                        client.Cedula = cliente.Cedula;
                        client.Coreo = cliente.Coreo;
                        client.Id_distrito = cliente.Id_distrito;
                        client.Edad = cliente.Edad;
                        client.Nombre = cliente.Nombre;
                        client.Profesion = cliente.Profesion;
                        db.Cliente.Add(client);
                        db.SaveChanges();
                        confirmacionCorreo(client);
                        ViewBag.message = "Mensaje";
                        ViewBag.Id_Canton = new SelectList(db.Canton.Where(x => x.Id_provincia == 1), "Id", "Canton1");
                        ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
                        ViewBag.Id_distrito = new SelectList(db.Distrito.Where(x => x.Id_canton == 1), "Id", "Distrito1");
                        ModelState.Clear();
                        return View();
                    }
                }
                catch (Exception)
                {
                    ViewBag.error = "Error";
                    ViewBag.Id_Canton = new SelectList(db.Canton.Where(x => x.Id_provincia == 1), "Id", "Canton1");
                    ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
                    ViewBag.Id_distrito = new SelectList(db.Distrito.Where(x => x.Id_canton == 1), "Id", "Distrito1");
                    return View();
                }    
            }
            ViewBag.Id_Canton = new SelectList(db.Canton.Where(x => x.Id_provincia == 1), "Id", "Canton1");
            ViewBag.Provincia = new SelectList(db.Provincia, "Id", "Provincia1");
            ViewBag.Id_distrito = new SelectList(db.Distrito.Where(x => x.Id_canton == 1), "Id", "Distrito1");
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
                        message.Subject = "Bienvenid@ al servicio de indicadores económicos de Financiera El Progreso";
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
