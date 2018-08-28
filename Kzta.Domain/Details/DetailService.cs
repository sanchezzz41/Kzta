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
            var detail = Mapper.Map<Detail>(model);
            detail.DetailtGuid = Guid.NewGuid();
            _context.Details.Add(detail);
            await _context.SaveChangesAsync();

            return detail.DetailtGuid;
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


        /// <summary>
        /// Устанавливает картинку
        /// </summary>
        /// <param name="detailGuid"></param>
        /// <param name="imageGuid"></param>
        /// <returns></returns>
        public async Task SetImage(Guid detailGuid, Guid imageGuid)
        {
            var detail = await _context.Details.SingleOrDefaultAsync(x => x.DetailtGuid == detailGuid);
            var image = await _context.Files.SingleOrDefaultAsync(x => x.FileGuid == imageGuid);
            if (detail == null || image == null)
                return;
            detail.ImageGuid = image.FileGuid;
            await _context.SaveChangesAsync();
        }

        public async Task<Detail> GetSingle(Guid detailGuid)
        {
            return _context.Details.SingleOrDefault(x => x.DetailtGuid == detailGuid);
        }
    }
}