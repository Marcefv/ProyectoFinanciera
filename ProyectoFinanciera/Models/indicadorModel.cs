using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ProyectoFinanciera.Models
{
    [XmlRoot("Datos_de_INGC011_CAT_INDICADORECONOMIC")]
    public class indicadorModel
    {
        [XmlElement("INGC011_CAT_INDICADORECONOMIC")]
        public List<ResultadosIndicador> Resultados { get; set; }
    }

    public class ResultadosIndicador
    {
        public int COD_INDICADORINTERNO { get; set; }
        public DateTime DES_FECHA { get; set; }

        public decimal NUM_VALOR { get; set; }
    }
}