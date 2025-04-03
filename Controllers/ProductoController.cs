using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using SysInventory.BL;
using SysInventory.EN;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SysInventoryAppWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ProductoBL _productoBL;

        public ProductoController(ProductoBL pProductoBL)
        {
            _productoBL = pProductoBL;
        }

        public async Task<ActionResult> Index()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            return View(productos);
        }

        public async Task<ActionResult> Details(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(new Producto { IdProducto = id });
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Producto pProducto)
        {
            if (!ModelState.IsValid) return View(pProducto);

            try
            {
                await _productoBL.CrearAsync(pProducto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View(pProducto);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(new Producto { IdProducto = id });
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Producto pProducto)
        {
            if (!ModelState.IsValid) return View(pProducto);

            try
            {
                await _productoBL.ModificarAsync(pProducto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View(pProducto);
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var producto = await _productoBL.ObtenerPorIdAsync(new Producto { IdProducto = id });
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _productoBL.EliminarAsync(new Producto { IdProducto = id });
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return View();
            }
        }

        public async Task<ActionResult> ReporteProductos()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpProductos", productos);
        }

        public async Task<ActionResult> ProductosJsonPrecio()
        {
            var productos = await _productoBL.ObtenerTodosAsync();
            var groupedData = productos
                .GroupBy(p => p.Fechacreacion.ToString("yyyy-MM-dd"))
                .Select(g => new
                {
                    fecha = g.Key,
                    precioPromedio = g.Average(p => p.Precio)
                })
                .OrderBy(g => g.fecha)
                .ToList();

            return Json(groupedData);
        }

        public async Task<IActionResult> ReporteProductosExcel()
        {
            var productos = await _productoBL.ObtenerTodosAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Productos");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Precio";
                hojaExcel.Cells["C1"].Value = "Cantidad";
                hojaExcel.Cells["D1"].Value = "Fecha";

                int row = 2;

                foreach (var producto in productos)
                {
                    if (producto == null) continue;

                    hojaExcel.Cells[row, 1].Value = producto.Nombre ?? "N/A"; 
                    hojaExcel.Cells[row, 2].Value = producto.Precio;
                    hojaExcel.Cells[row, 3].Value = producto.CantidadDisponible;
                    hojaExcel.Cells[row, 4].Value = producto.FechaCreacion?.ToString("yyyy-MM-dd") ?? "Sin fecha";

                    hojaExcel.Cells[row, 2].Style.Numberformat.Format = "#,##0.00"; 
                    hojaExcel.Cells[row, 3].Style.Numberformat.Format = "0"; 

                    row++;
                }

                hojaExcel.Cells["A:D"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteProductosExcel.xlsx");
            }
        }



        [HttpPost]
        public async Task<IActionResult> SubirExcelProductos(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }

            var productos = new List<Producto>();

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hojaExcel = package.Workbook.Worksheets[0];
                    int rowCount = hojaExcel.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nombre = hojaExcel.Cells[row, 1].Text;
                        var precio = hojaExcel.Cells[row, 2].GetValue<decimal>();
                        var cantidad = hojaExcel.Cells[row, 3].GetValue<int>();
                        var fecha = hojaExcel.Cells[row, 4].GetValue<DateTime>();

                        if (string.IsNullOrEmpty(nombre) || precio <= 0 || cantidad < 0) continue;

                        productos.Add(new Producto
                        {
                            Nombre = nombre,
                            Precio = precio,
                            CantidadDisponible = cantidad,
                            FechaCreacion = fecha
                        });
                    }
                }

                if (productos.Any())
                {
                    await _productoBL.AgregarTodosAsync(productos);
                }
                return RedirectToAction("Index");
            }
        }
    }
}




