
using ConsultorioDeSeguros.Domain.Asegurados.Models;
using ConsultorioDeSeguros.Domain.Asegurados.Repository;
using ConsultorioDeSeguros.Domain.Seguros.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultorioDeSeguros.Infrastructure.Asegurados.Repository
{
    public class AseguradoRepository: IAseguradoRepository, IDisposable
    {
        private SegurosContexto SegurosContexto { get; }
        private bool disposed = false;

        public AseguradoRepository(SegurosContexto SegurosContexto)
        {
            this.SegurosContexto = SegurosContexto;
        }


        public async Task<AseguradoModel> Create(AseguradoModel asegurado)
        {
            await SegurosContexto.AddAsync(asegurado);
            return asegurado;
        }

        public async Task<List<AseguradoModel>> CreateMany(List<AseguradoModel> asegurados)
        {
            await SegurosContexto.AddRangeAsync(asegurados);
            return asegurados;
        }


        public AseguradoModel? GetByCedula(string cedula)
        {
            AseguradoModel? asegurado = SegurosContexto
                                           .Asegurados
                                           .Where(ase => ase.Cedula == cedula)
                                           .FirstOrDefault();

            return asegurado;
        }
        public async Task<List<AseguradoModel>> GetAll()
        {
            List<AseguradoModel> asegurados = await SegurosContexto
                                                       .Asegurados
                                                       .Include(seg => seg.Seguros) 
                                                       .ToListAsync();

            return asegurados;
        }


        public AseguradoModel Update(AseguradoModel asegurado)
        {
            SegurosContexto.Update(asegurado);
            return asegurado;
        }

        public List<AseguradoModel> UpdateMany(List<AseguradoModel> asegurados)
        {
            SegurosContexto.UpdateRange(asegurados);
            return asegurados;
        }

        public void Remove(string cedula)
        {
            AseguradoModel? asegurado = GetByCedula(cedula);
            if (asegurado != null)
                SegurosContexto.Remove(asegurado);
        }

        public async Task RemoveSeguros(string cedula)
        {
            List<SeguroAseguradoModel> seguroAsegurados = await SegurosContexto
                                                                .SeguroAsegurado
                                                                .Where(seg => seg.AseguradoCedula == cedula)
                                                                .ToListAsync();

            SegurosContexto.RemoveRange(seguroAsegurados);
        }

        public async Task<List<SeguroAseguradoModel>> AddSeguros(List<SeguroAseguradoModel> seguros)
        {
            await SegurosContexto.AddRangeAsync(seguros);
            return seguros;
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
