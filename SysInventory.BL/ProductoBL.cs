using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using SysInventory.DAL;
using SysInventory.EN;

namespace SysInventory.BL
{
    public class ProductoBL
    {
        private readonly ProductoDAL _productoDAL;

        public ProductoBL(ProductoDAL productoDAL)
        {
            _productoDAL = productoDAL;
        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            return await _productoDAL.CrearAsync(pProducto);
        }

        public async Task<int> ModificarAsync(Producto pProducto)
        {
            return await _productoDAL.ModificarAsync(pProducto);
        }

        public async Task<int> EliminarAsync(Producto pProducto)
        {
            return await _productoDAL.EliminarAsync(pProducto);
        }

        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            return await _productoDAL.ObtenerPorIdAsync(pProducto);
        }
        public Task AgregarTodosAsync(List<Producto> pProductos)
        {
            return _productoDAL.AgregarTodosAsync(pProductos);
        }

        public async Task<IEnumerable<object>> ObtenerTodosAsync()
        {
            throw new NotImplementedException();
        }
    }
}