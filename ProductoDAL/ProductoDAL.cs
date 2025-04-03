using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using SysInventory.EN;

namespace SysInventory.DAL
{
    public class ProductoDAL
    {
        readonly InventoryDBContext _dbContext;
        public ProductoDAL(InventoryDBContext context)
        {
            _dbContext = context;
        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            Producto producto = new Producto()
            {
                Nombre = pProducto.Nombre,
            };

            _dbContext.Add(producto);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(Producto pProducto)
        {
            var producto = _dbContext.Productos.FirstOrDefault(s => s.IdProducto == pProducto.IdProducto);
            if (producto != null)
            {
                _dbContext.Productos.Remove(producto);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(Producto pProducto)
        {
            var producto = _dbContext.Productos.FirstOrDefault(s => s.IdProducto == pProducto.IdProducto);
            if (producto != null && producto.IdProducto > 0)
            {
                _dbContext.Productos.Update(pProducto);
                return await _dbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            var producto = await _dbContext.Productos.FirstOrDefaultAsync(s => s.IdProducto == pProducto.IdProducto);
            if (producto != null && producto.IdProducto != 0)
            {
                return new Producto
                {
                    IdProducto = producto.IdProducto,
                    Nombre = producto.Nombre,
                };
            }
            else
                return new Producto();
        }

        public List<Producto> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Producto>> ObtenerTodosAsync(Producto pProducto)
        {
            var productos = await _dbContext.Productos.ToListAsync();

            if (pProducto != null && productos.Count > 0)
            {
                var lista = new List<Producto>();
                productos.ForEach(s => lista.Add(new Producto
                {
                    IdProducto = s.IdProducto,
                    Nombre = s.Nombre,
                }));
                return lista;
            }
            else
            {
                return new List<Producto>();
            }
        }
        public async Task AgregarTodosAsync(List<Producto> pProductos)
        {
            await _dbContext.Productos.AddRangeAsync(pProductos);
            await _dbContext.SaveChangesAsync();
        }
    }
}

