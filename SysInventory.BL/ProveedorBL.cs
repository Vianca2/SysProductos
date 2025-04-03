using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysInventory.DAL;
using SysInventory.EN;

namespace SysInventory.BL
{
    public class ProveedorBL
    {
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorBL(ProveedorDAL proveedorDAL)
        {
            _proveedorDAL = proveedorDAL;
        }

        public async Task<int> CrearAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.CrearAsync(pProveedor);
        }

        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.ModificarAsync(pProveedor);
        }

        public async Task<int> EliminarAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.EliminarAsync(pProveedor);
        }

        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.ObtenerPorIdAsync(pProveedor);
        }

        public async Task<List<Proveedor>> ObtenerTodosAsync() 
        {
            return await _proveedorDAL.ObtenerTodosAsync();
        }
    }
}

