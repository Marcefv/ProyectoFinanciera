using ProyectoFinanciera.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                        dsBasicaPasiva = objRef.ObtenerIndicadoresEconomicos("19654", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsPoliticaMonetaria = objRef.ObtenerIndicadoresEconomicos("3541", hace2anios, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");

                    }
                    else//busca informacion de indicadores desde la ulitma fecha consultada
                    {
                        var uFecha = db.Indicadores.OrderByDescending(x => x.DES_FECHA).FirstOrDefault().DES_FECHA;
                        var ultimaFecha = uFecha.AddDays(1).ToString("dd/MM/yyyy");
                        indicadoresReference.wsindicadoreseconomicos objRef = new indicadoresReference.wsindicadoreseconomicos();
                        dsDolarCompra = objRef.ObtenerIndicadoresEconomicos("317", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsDolarVenta = objRef.ObtenerIndicadoresEconomicos("318", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
                        dsBasicaPasiva = objRef.ObtenerIndicadoresEconomicos("19654", ultimaFecha, ahora, "Marcela", "N", "marcefv.89@gmail.com", "C0C81E7IFF");
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


    }
}