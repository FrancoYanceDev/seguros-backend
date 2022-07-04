

using ConsultorioDeSeguros.Domain.Asegurados.Models;

namespace ConsultorioDeSeguros.Domain.Seguros.Models
{
    public class SeguroModel
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal SumaAsegurada { get; set; }
        public decimal Prima { get; set; }

        public List<SeguroAseguradoModel> Asegurados { get; set; }
    }
}
