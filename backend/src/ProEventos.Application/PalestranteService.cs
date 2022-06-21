using System;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class PalestranteService : IPalestranteService
    {
        private readonly IGeralPersist _geralPersist;
        private readonly IPalestrantePersist _palestrantePersist;

        public PalestranteService(IGeralPersist geralPersist, IPalestrantePersist palestrantePersist)
        {
            _geralPersist = geralPersist;
            _palestrantePersist = palestrantePersist;
        }

        public async Task<Palestrante> AddPalestrante(Palestrante model)
        {
            try
            {
                _geralPersist.Add<Palestrante>(model);
                if (await _geralPersist.SaveChangesAsync())
                    return await _palestrantePersist.GetPalestranteById(model.Id, false);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Palestrante> UpdatePalestrante(int palestranteId, Palestrante model)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteById(palestranteId, false);
                if (palestrante == null) return null;

                model.Id = palestrante.Id;

                _geralPersist.Update(model);
                if (await _geralPersist.SaveChangesAsync())
                    return await _palestrantePersist.GetPalestranteById(model.Id, false);
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletePalestrante(int palestranteId)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteById(palestranteId, false);
                if (palestrante == null) throw new Exception("Palestrante a ser deletrado não foi encontrado.");

                _geralPersist.Delete(palestrante);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesAsync(includeEventos);
                return palestrantes ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestrantesByNomeAsync(nome, includeEventos);
                return palestrantes ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Palestrante> GetPalestranteById(int palestranteId, bool includeEventos)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteById(palestranteId, includeEventos);
                return palestrante ?? null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
