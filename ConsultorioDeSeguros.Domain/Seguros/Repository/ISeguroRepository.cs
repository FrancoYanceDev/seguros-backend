
using ConsultorioDeSeguros.Domain.Seguros.Models;

namespace ConsultorioDeSeguros.Domain.Seguros.Repository
{
    public interface ISeguroRepository: IDisposable
    {
        Task<SeguroModel> Create(SeguroModel seguro);
        SeguroModel? GetByCode(int codigo);
        Task<List<SeguroModel>> GetAll();
        SeguroModel Update(SeguroModel seguro);
        List<SeguroModel> UpdateMany(List<SeguroModel> seguros);
        Task<List<SeguroAseguradoModel>> GetAsegurados(int codigo);
        Task<List<SeguroAseguradoModel>> GetByCedula(string cedula);
        void Remove(int codigo);
        Task Save();
    }
}
