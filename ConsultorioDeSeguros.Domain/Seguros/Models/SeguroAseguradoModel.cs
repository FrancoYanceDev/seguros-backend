

using ConsultorioDeSeguros.Domain.Asegurados.Models;

namespace ConsultorioDeSeguros.Domain.Seguros.Models
{
    public class SeguroAseguradoModel
    {
        public int Id { get; set; }
        public int SeguroCodigo { get; set; }
        public string AseguradoCedula { get; set; }
        public SeguroModel Seguro { get; set; }
        public AseguradoModel Asegurado { get; set; }
    }
}
