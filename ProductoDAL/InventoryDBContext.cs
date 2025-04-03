using Microsoft.EntityFrameworkCore;
using SysInventory.EN;

namespace SysInventory.DAL
{
    public class InventoryDBContext : DbContext

    {
        public InventoryDBContext(DbContextOptions<InventoryDBContext> Options) : base(Options)
        {
        }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
    }
}