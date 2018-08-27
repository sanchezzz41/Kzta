using System.Data.Entity;
using Databas.Models;

namespace Databas
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext():base("Context")
        {
            
        }

        /// <summary>
        /// Таблица деталий
        /// </summary>
        public DbSet<Detail> Details { get; set; }

        /// <summary>
        /// Таблица файлов
        /// </summary>
        public DbSet<File> Files { get; set; }

        /// <summary>
        /// Таблица типа имущества
        /// </summary>
        public DbSet<PropertyType> PropertyTypes { get; set; }

        /// <summary>
        /// Таблица продуктов(изделий)
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Связывающая таблица для продукта и детали
        /// </summary>
        public DbSet<DetailInProduct> DetailsInProducts { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetailInProduct>()
                .HasKey(x => new {x.DetailGuid, x.ProductGuid});
            base.OnModelCreating(modelBuilder);
        }
    }
}
