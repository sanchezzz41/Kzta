using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Databas;
using Databas.Models;

namespace Kzta.Domain.PropertyTypes
{
    /// <summary/>
    public class PropertyTypeService
    {
        private readonly DatabaseContext _context;

        /// <summary/>
        public PropertyTypeService(DatabaseContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Guid> Create(PropertyTypeInfo model)
        {
            var propertyType = new PropertyType(model.Name, model.Description);
            _context.PropertyTypes.Add(propertyType);
            _context.SaveChanges();

            return propertyType.PropertyTypeGuid;
        }

        /// <inheritdoc />
        public async Task Update(Guid propertyTypeId, PropertyTypeInfo model)
        {
            var propertyType = await _context.PropertyTypes.SingleOrDefaultAsync(x => x.PropertyTypeGuid == propertyTypeId);
            if (propertyType == null)
            {
                return;
            }
            if (!String.Equals(propertyType.Name, model.Name, StringComparison.CurrentCultureIgnoreCase)
                && await _context.PropertyTypes.AnyAsync(x =>
                    x.Name.ToLower().Equals(model.Name.ToLower())))
            {
            }

            propertyType.Name = model.Name;
            propertyType.Description = model.Description;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task Delete(Guid propertyTypeGuid)
        {
            var propertyType = await _context.PropertyTypes
                .Include(x => x.Details)
                .SingleOrDefaultAsync(x => x.PropertyTypeGuid == propertyTypeGuid);

            if (propertyType.Details.Any())
                return;

            _context.PropertyTypes.Remove(propertyType);
            _context.SaveChanges();
        }

        public IQueryable<PropertyType> Get()
        {
            return _context.PropertyTypes.AsNoTracking().OrderBy(x => x.Name);
        }

        public async Task<PropertyType> GetSingle(Guid propertyTypeGuid)
        {
            return _context.PropertyTypes.SingleOrDefault(x => x.PropertyTypeGuid == propertyTypeGuid);
        }
    }
}