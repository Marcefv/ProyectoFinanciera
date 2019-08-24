using ProyectoFinanciera.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;


namespace ProyectoFinanciera.Controllers
{
    public class llamadasIndicadores
    {
        private IndicadoresEntities db = new IndicadoresEntities();

        //llena la base de datos con los valores de indicadores hasta los del día actual
        public void baseDatos()
        {

            DateTime theDate = DateTime.Today;
            String ahora = theDate.ToString("dd/MM/yyyy");
            DateTime twoyearsago = theDate.AddYears(-2);
            String hace2anios = twoyearsago.ToString("dd/MM/yyyy");      
            DateTime ultimafecha = db.Indicadores.OrderByDescending(x => x.DES_FECHA).FirstOrDefault().DES_FECHA;


            //verifica si hay almacenados indicadores con la fecha actual
            var indicadoresHoy = db.Indicadores.Where(x => x.DES_FECHA == theDate).Select(x => x).ToList();

            //verifica si hay almacenados indicadores de hace dos anios
            var hace2 = db.Indicadores.Where(x=>x.DES_FECHA==twoyearsago).Select(x=>x).ToList();

            List<Indicadores> indicadores = null;
            DataSet dsDolarCompra = null;
            DataSet dsDolarVenta = null;
            DataSet dsBasicaPasiva = null;
            DataSet dsPoliticaMonetaria = null;

            if (indicadoresHoy.Count() == 0)
            {
                try
                {
                    if (hace2.Count() == 0)//si no hay informacion de hace dos anios la busca en el web service
                    {
                        indicadoresReference.wsindicadoreseconomicos objRef = new indicadoresReference.wsindicadoreseconomicos();
                        dsDolarCompra = objRef.ObtenerIndicadoresEconomicos("317", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsDolarVenta = objRef.ObtenerIndicadoresEconomicos("318", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsBasicaPasiva = objRef.ObtenerIndicadoresEconomicos("423", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsPoliticaMonetaria = objRef.ObtenerIndicadoresEconomicos("3541", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        
                    }
                    else//busca informacion de indicadores desde la ulitma fecha consultada
                    {
                        var uFecha = db.Indicadores.OrderByDescending(x => x.DES_FECHA).FirstOrDefault().DES_FECHA;
                        var ultimaFecha = uFecha.AddDays(1).ToString("dd/MM/yyyy");
                        indicadoresReference.wsindicadoreseconomicos objRef = new indicadoresReference.wsindicadoreseconomicos();
                        dsDolarCompra = objRef.ObtenerIndicadoresEconomicos("317", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsDolarVenta = objRef.ObtenerIndicadoresEconomicos("318", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsBasicaPasiva = objRef.ObtenerIndicadoresEconomicos("423", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsPoliticaMonetaria = objRef.ObtenerIndicadoresEconomicos("3541", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                      
                    }

                    //Convierte indicador dolar compra y almacena en BD
                    indicadores = dsDolarCompra.Tables[0].AsEnumerable().
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

                    //Convierte indicador dolar venta y almacena en BD
                    indicadores = dsDolarVenta.Tables[0].AsEnumerable().
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

                    //Convierte indicador tasa basica pasiva y almacena en BD
                    indicadores = dsBasicaPasiva.Tables[0].AsEnumerable().
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


                    //Convierte indicador tasa politica monetaria y almacena en BD
                    indicadores = dsPoliticaMonetaria.Tables[0].AsEnumerable().
                         Select(dataRow => new Indicadores()
                         {
                             COD_INDICADORINTERNO = Convert.ToInt32(dataRow["COD_INDICADORINTERNO"]),
                             DES_FECHA = Convert.ToDateTime(dataRow["DES_FECHA"]),
                             NUM_VALOR = Convert.ToDecimal(dataRow["NUM_VALOR"])
                         }).ToList();

                    //Guarda cada indicador en la base de datos
                    foreach (var item in indicadores)
                    {
                        db.Indicadores.Add(item);
                        db.SaveChanges();
                    }

                    //envia correo con los indicadores
                    if (ultimafecha==theDate)
                    {
                        correoIndicadores(ultimafecha, theDate);
                    }
                    else
                    {
                        correoIndicadores(ultimafecha.AddDays(1), theDate);
                    }
                    
                }
                catch (Exception)
                {

                    throw;
                }
                
            }
            
        }

        //obtiene los datos de la base de datos según fecha de inicio , final y el indicador deseado
        public List<Indicadores> obtieneDatosDeBD(DateTime inicio, DateTime final, int indicador)
        {
            List<Indicadores> lista = new List<Indicadores>();
            lista = db.Indicadores.Where(x => x.DES_FECHA >= inicio && x.DES_FECHA <= final 
                        && x.COD_INDICADORINTERNO==indicador).Select(x => x).ToList();
            return lista;
        }

        //Envia correo con indicadores
        public void correoIndicadores(DateTime fechaDesde, DateTime fechaHasta)
        {
            List<Cliente> clientes = new List<Cliente>();
            clientes = db.Cliente.ToList();
            try
            {
                var indicadores = "";
                do
                {
                    var tbp=db.Indicadores.Where(x => (x.DES_FECHA == fechaDesde) && (x.COD_INDICADORINTERNO == 423)).First().NUM_VALOR;
                    var tpm= db.Indicadores.Where(x => (x.DES_FECHA == fechaDesde) && (x.COD_INDICADORINTERNO == 3541)).First().NUM_VALOR;
                    var cd= db.Indicadores.Where(x => (x.DES_FECHA == fechaDesde) && (x.COD_INDICADORINTERNO == 317)).First().NUM_VALOR;
                    decimal vd= db.Indicadores.Where(x => x.DES_FECHA == fechaDesde && x.COD_INDICADORINTERNO == 318).First().NUM_VALOR;
                    indicadores += "<p>Indicadores del "+fechaDesde.ToShortDateString()+": </p>" +
                        "<ul> <li>Tipo de Cambio de Venta del Dólar: "+vd+"</li><li>Tipo de Cambio de Compra del Dólar: "+cd+"</li>" +
                             "<li>Tasa de Política Monetaria: "+tpm+"</li><li>Tasa Básica Pasiva: "+tbp+"</li>" +
                             "</ul>";
                    fechaDesde = fechaDesde.AddDays(1);
                } while (fechaDesde<=fechaHasta);
                foreach (var cliente in clientes)
                {
                    
                    var body = "<p>Email From: elprogresofinanciera@gmail.com</p>" +
                    "       <p>Estimad@ " + cliente.Nombre + " " + cliente.Apellidos + ", Cédula: "+cliente.Cedula+" </p>" +
                    "       <p>   Financiera El Progreso le presenta los indicadores económicos:</p>" +
                             indicadores;
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(cliente.Coreo));
                    message.Subject = "Reporte de indicadores económicos de Financiera El Progreso";
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


    }
}