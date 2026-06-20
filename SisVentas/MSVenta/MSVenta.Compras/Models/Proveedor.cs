using System;
using System.Collections.Generic;

namespace MSVenta.Compras.Models
{
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Nit { get; set; }
        public string Contacto { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public string Estado { get; set; } = "activo";

        public ICollection<NotaCompra> NotasCompra { get; set; }
        public ICollection<CatalogoProveedor> Catalogo { get; set; }
    }
}