

using ConsultorioDeSeguros.Domain.Seguros.Models;
using ConsultorioDeSeguros.Domain.Seguros.Repository;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioDeSeguros.Infrastructure.Seguros.Repository
{
    public class SeguroRepository : ISeguroRepository, IDisposable
    {
        private SegurosContexto SegurosContexto { get;}
        private bool disposed = false;

        public SeguroRepository(SegurosContexto SegurosContexto)
        {
            this.SegurosContexto = SegurosContexto;
        }

        public async Task<SeguroModel> Create(SeguroModel seguro)
        {
            await SegurosContexto.AddAsync(seguro);
            return seguro;
        }

        public SeguroModel? GetByCode(int codigo)
        {
            SeguroModel? seguro = SegurosContexto
                                   .Seguro
                                   .Where(segu => segu.Codigo == codigo)
                                   .FirstOrDefault();

            return seguro;
        }

        public async Task<List<SeguroAseguradoModel>> GetAsegurados(int codigo)
        {
            return await SegurosContexto
                               .SeguroAsegurado
                               .Where(seg => seg.SeguroCodigo == codigo)
                               .Include(seg => seg.Asegurado)
                               .ToListAsync();
        }

        public async Task<List<SeguroAseguradoModel>> GetByCedula(string cedula)
        {
            return await SegurosContexto
                               .SeguroAsegurado
                               .Where(seg => seg.AseguradoCedula == cedula)
                               .Include(seg => seg.Seguro)
                               .ToListAsync();
        }

        public async Task<List<SeguroModel>> GetAll()
        {
            return await SegurosContexto
                               .Seguro
                               .ToListAsync();
        }

        public SeguroModel Update(SeguroModel seguro)
        {
            SegurosContexto.Update(seguro);
            return seguro;
        }
        public List<SeguroModel> UpdateMany(List<SeguroModel> seguros)
        {
            SegurosContexto.UpdateRange(seguros);
            return seguros;
        }

        public void Remove(int codigo)
        {
            SeguroModel? seguro  = GetByCode(codigo);
            if(seguro != null)
                SegurosContexto.Remove(seguro);
        }


        public async Task Save()
        {
            await SegurosContexto.SaveChangesAsync();
        }



        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    SegurosContexto.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
