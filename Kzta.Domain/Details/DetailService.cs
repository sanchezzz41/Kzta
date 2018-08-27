using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Databas;
using Databas.Models;

namespace Kzta.Domain.Details
{
    /// <summary/>
    public class DetailService
    {
        private readonly DatabaseContext _context;
        /// <summary/>
        public DetailService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> Create(DetailInfo model)
        {
            var propertyType = Mapper.Map<Detail>(model);
            _context.Details.Add(propertyType);
            await _context.SaveChangesAsync();

            return propertyType.DetailtGuid;
        }

        /// <inheritdoc />
        public async Task Update(Guid detailGuid, DetailInfo model)
        {
            var detail = await _context.Details.SingleOrDefaultAsync(x => x.DetailtGuid == detailGuid);

            if (detail == null)
                return;

            Mapper.Map(model, detail);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid detailGuid)
        {
            var detail = await _context.Details
                .SingleOrDefaultAsync(x => x.DetailtGuid == detailGuid);

            if (detail == null)
                return;
            _context.Details.Remove(detail);
            _context.SaveChanges();
        }

        public IQueryable<Detail> Get()
        {
            return _context.Details.AsNoTracking().OrderBy(x => x.Name);
        }

        public async Task<Detail> GetSingle(Guid detailGuid)
        {
            return _context.Details.SingleOrDefault(x => x.DetailtGuid == detailGuid);
        }
    }
}