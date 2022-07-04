

using ConsultorioDeSeguros.Domain.Seguros.Models;

namespace ConsultorioDeSeguros.Domain.Asegurados.Models
{
    public class AseguradoModel
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public byte Edad { get; set; }
        public List<SeguroAseguradoModel> Seguros { get; set; }
    }
}
