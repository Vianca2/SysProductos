namespace SysInventory.EN
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public Decimal Precio { get; set; }
        public int CantidadDisponible { get; set; }
        public DateTime FechaCreacion { get; set; }
        public static void Find(int idProducto)
        {
            throw new NotImplementedException();
        }
        public static void Remove(object productO)
        {
            throw new NotImplementedException();
        }
        public static List<Producto> ToList()
        {
            throw new NotImplementedException();
        }
        public static void Update(Producto producto)
        {
            throw new NotImplementedException();
        }
        public void Add(Producto producto)
        {
            throw new NotImplementedException();
        }
    }
}
