
using ConsultorioDeSeguros.Domain.Asegurados.Models;
using ConsultorioDeSeguros.Domain.Seguros.Models;

namespace ConsultorioDeSeguros.Domain.Asegurados.Repository
{
    public interface IAseguradoRepository
    {
        Task<AseguradoModel> Create(AseguradoModel asegurado);
        Task<List<AseguradoModel>> CreateMany(List<AseguradoModel> asegurados);
        AseguradoModel? GetByCedula(string cedula);
        Task<List<AseguradoModel>> GetAll();
        AseguradoModel Update(AseguradoModel asegurado);
        List<AseguradoModel> UpdateMany(List<AseguradoModel> asegurados);
        void Remove(string cedula);
        Task RemoveSeguros(string cedula);
        Task<List<SeguroAseguradoModel>> AddSeguros(List<SeguroAseguradoModel> seguros);
        Task Save();

    }
}
