using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using MSVenta.Reportes.DTOs;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using System.Linq;

namespace MSVenta.Reportes.Services
{
    public class PdfService
    {
        private static readonly Color ColorAzul = new DeviceRgb(30, 64, 175);
        private static readonly Color ColorGrisClaro = new DeviceRgb(241, 245, 249);
        private static readonly Color ColorVerde = new DeviceRgb(22, 163, 74);
        private static readonly Color ColorRojo = new DeviceRgb(220, 38, 38);

        public byte[] GenerarReporteVentas(List<NotaVentaDto> ventas, decimal totalVendido, int canceladas)
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            var fuente = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);
            document.SetFont(fuente);

            AgregarTitulo(document, "Reporte de Ventas");
            AgregarFecha(document);

            var tablaResumen = new Table(3).UseAllAvailableWidth();
            AgregarTarjeta(tablaResumen, "TOTAL VENDIDO", $"Bs. {totalVendido:F2}");
            AgregarTarjeta(tablaResumen, "VENTAS CANCELADAS", canceladas.ToString());
            AgregarTarjeta(tablaResumen, "TOTAL REGISTROS", ventas.Count.ToString());
            document.Add(tablaResumen);
            document.Add(new Paragraph(" "));

            AgregarSubtitulo(document, "Detalle de Ventas");
            var tabla = new Table(new float[] { 2, 2, 2, 3, 4, 2, 2 }).UseAllAvailableWidth();

            foreach (var header in new[] { "CÓDIGO", "FECHA", "HORA", "CLIENTE", "ARTÍCULOS", "TOTAL", "ESTADO" })
                AgregarEncabezado(tabla, header);

            foreach (var v in ventas)
            {
                var nombreCliente = v.Cliente != null
                    ? $"{v.Cliente.Nombre} {v.Cliente.Paterno}".Trim()
                    : "—";

                var articulos = (v.Detalles != null && v.Detalles.Count > 0)
                    ? string.Join(", ", v.Detalles.Select(d => $"{d.NombreProducto} x{d.Cantidad:0.##}"))
                    : "N/A";

                tabla.AddCell(CeldaNormal($"VTA-{v.Id:D5}"));
                tabla.AddCell(CeldaNormal(v.Fecha.ToString("dd/MM/yyyy")));
                tabla.AddCell(CeldaNormal(v.Hora.ToString(@"hh\:mm")));
                tabla.AddCell(CeldaNormal(nombreCliente));
                tabla.AddCell(CeldaNormal(articulos));
                tabla.AddCell(CeldaNormal($"Bs. {v.MontoTotal:F2}"));
                tabla.AddCell(CeldaEstado(v.Estado ?? ""));
            }

            document.Add(tabla);
            document.Close();
            return ms.ToArray();
        }
        public byte[] GenerarReporteCompras(List<NotaCompraDto> compras, decimal totalComprado)
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            var fuente = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);
            document.SetFont(fuente);

            AgregarTitulo(document, "Reporte de Compras");
            AgregarFecha(document);

            var tablaResumen = new Table(2).UseAllAvailableWidth();
            AgregarTarjeta(tablaResumen, "TOTAL COMPRADO", $"Bs. {totalComprado:F2}");
            AgregarTarjeta(tablaResumen, "TOTAL REGISTROS", compras.Count.ToString());
            document.Add(tablaResumen);
            document.Add(new Paragraph(" "));

            AgregarSubtitulo(document, "Detalle de Compras");
            var tabla = new Table(new float[] { 2, 3, 2, 2, 2 }).UseAllAvailableWidth();

            foreach (var header in new[] { "FECHA", "PROVEEDOR", "TOTAL", "PAGO", "ESTADO" })
                AgregarEncabezado(tabla, header);

            foreach (var c in compras)
            {
                var nombreProveedor = c.Proveedor != null ? c.Proveedor.Nombre : "—";

                tabla.AddCell(CeldaNormal(c.FechaCompra.ToString("dd/MM/yyyy")));
                tabla.AddCell(CeldaNormal(nombreProveedor));
                tabla.AddCell(CeldaNormal($"Bs. {c.TotalCompra:F2}"));
                tabla.AddCell(CeldaNormal(c.TipoPago ?? ""));
                tabla.AddCell(CeldaEstado(c.Estado ?? ""));
            }

            document.Add(tabla);
            document.Close();
            return ms.ToArray();
        }

        public byte[] GenerarReporteInventario(List<ArticuloReporteDto> articulos)
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            var fuente = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);
            document.SetFont(fuente);

            AgregarTitulo(document, "Reporte de Inventario");
            AgregarFecha(document);

            var stockBajo = articulos.FindAll(a => a.Stock <= a.StockMin);
            var tablaResumen = new Table(2).UseAllAvailableWidth();
            AgregarTarjeta(tablaResumen, "TOTAL ARTÍCULOS", articulos.Count.ToString());
            AgregarTarjeta(tablaResumen, "STOCK BAJO", stockBajo.Count.ToString());
            document.Add(tablaResumen);
            document.Add(new Paragraph(" "));

            AgregarSubtitulo(document, "Detalle de Artículos");
            var tabla = new Table(new float[] { 3, 2, 2, 1, 1, 1, 2 }).UseAllAvailableWidth();

            foreach (var header in new[] { "ARTÍCULO", "CATEGORÍA", "MARCA", "STOCK", "MÍN", "MÁX", "PRECIO" })
                AgregarEncabezado(tabla, header);

            foreach (var a in articulos)
            {
                tabla.AddCell(CeldaNormal(a.Nombre ?? ""));
                tabla.AddCell(CeldaNormal(a.NombreCategoria ?? ""));
                tabla.AddCell(CeldaNormal(a.NombreMarca ?? ""));
                tabla.AddCell(CeldaNormal(a.Stock.ToString()));
                tabla.AddCell(CeldaNormal(a.StockMin.ToString()));
                tabla.AddCell(CeldaNormal(a.StockMax.ToString()));
                tabla.AddCell(CeldaNormal($"Bs. {a.Precio:F2}"));
            }

            document.Add(tabla);
            document.Close();
            return ms.ToArray();
        }
        public byte[] GenerarReporteMovimientos(List<IngresoDto> ingresos, List<EgresoDto> egresos)
        {
            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            var fuente = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.WINANSI);
            document.SetFont(fuente);

            AgregarTitulo(document, "Reporte de Movimientos");
            AgregarFecha(document);

            var tablaResumen = new Table(2).UseAllAvailableWidth();
            AgregarTarjeta(tablaResumen, "TOTAL INGRESOS", ingresos.Count.ToString());
            AgregarTarjeta(tablaResumen, "TOTAL EGRESOS", egresos.Count.ToString());
            document.Add(tablaResumen);
            document.Add(new Paragraph(" "));

            // Ingresos
            AgregarSubtitulo(document, "Detalle de Ingresos");
            var tablaIngresos = new Table(new float[] { 2, 2, 4, 2 }).UseAllAvailableWidth();
            foreach (var header in new[] { "FECHA", "MOTIVO", "ARTÍCULOS", "ESTADO" })
                AgregarEncabezado(tablaIngresos, header);

            foreach (var i in ingresos)
            {
                var articulos = (i.Detalles != null && i.Detalles.Count > 0)
                    ? string.Join(", ", i.Detalles.Select(d =>
                        $"{d.ArticuloAlmacen?.Articulo?.Nombre ?? "—"} x{d.Cantidad}"))
                    : "—";

                tablaIngresos.AddCell(CeldaNormal(i.Fecha.ToString("dd/MM/yyyy")));
                tablaIngresos.AddCell(CeldaNormal(i.Motivo ?? ""));
                tablaIngresos.AddCell(CeldaNormal(articulos));
                tablaIngresos.AddCell(CeldaEstado(i.Estado ?? ""));
            }
            document.Add(tablaIngresos);
            document.Add(new Paragraph(" "));

            // Egresos
            AgregarSubtitulo(document, "Detalle de Egresos");
            var tablaEgresos = new Table(new float[] { 2, 2, 4, 2 }).UseAllAvailableWidth();
            foreach (var header in new[] { "FECHA", "MOTIVO", "ARTÍCULOS", "ESTADO" })
                AgregarEncabezado(tablaEgresos, header);

            foreach (var e in egresos)
            {
                var articulos = (e.Detalles != null && e.Detalles.Count > 0)
                    ? string.Join(", ", e.Detalles.Select(d =>
                        $"{d.ArticuloAlmacen?.Articulo?.Nombre ?? "—"} x{d.Cantidad}"))
                    : "—";

                tablaEgresos.AddCell(CeldaNormal(e.Fecha.ToString("dd/MM/yyyy")));
                tablaEgresos.AddCell(CeldaNormal(e.Motivo ?? ""));
                tablaEgresos.AddCell(CeldaNormal(articulos));
                tablaEgresos.AddCell(CeldaEstado(e.Estado ?? ""));
            }
            document.Add(tablaEgresos);

            document.Close();
            return ms.ToArray();
        }

        // ─── Helpers ───────────────────────────────────────────

        private void AgregarTitulo(Document doc, string texto)
        {
            doc.Add(new Paragraph(texto)
                .SetFontSize(20)
                .SetFontColor(ColorAzul)
                .SetTextAlignment(TextAlignment.CENTER));
        }

        private void AgregarFecha(Document doc)
        {
            doc.Add(new Paragraph($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}")
                .SetFontSize(9)
                .SetFontColor(ColorConstants.GRAY)
                .SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(" "));
        }

        private void AgregarSubtitulo(Document doc, string texto)
        {
            doc.Add(new Paragraph(texto)
                .SetFontSize(13)
                .SetFontColor(ColorAzul));
        }

        private void AgregarTarjeta(Table tabla, string titulo, string valor)
        {
            var celda = new Cell()
                .SetBackgroundColor(ColorGrisClaro)
                .SetPadding(10);
            celda.Add(new Paragraph(titulo).SetFontSize(8).SetFontColor(ColorConstants.GRAY));
            celda.Add(new Paragraph(valor).SetFontSize(16).SetFontColor(ColorAzul));
            tabla.AddCell(celda);
        }

        private void AgregarEncabezado(Table tabla, string texto)
        {
            tabla.AddHeaderCell(new Cell()
                .SetBackgroundColor(ColorAzul)
                .SetFontColor(ColorConstants.WHITE)
                .SetFontSize(9)
                .SetPadding(6)
                .Add(new Paragraph(texto)));
        }

        private Cell CeldaNormal(string texto)
        {
            return new Cell()
                .SetFontSize(8)
                .SetPadding(5)
                .Add(new Paragraph(texto));
        }

        private Cell CeldaEstado(string estado)
        {
            var color = estado switch
            {
                "activo" => new DeviceRgb(220, 252, 231),
                "pagado" => new DeviceRgb(219, 234, 254),
                "cancelado" => new DeviceRgb(254, 226, 226),
                _ => (Color)ColorGrisClaro
            };
            return new Cell()
                .SetFontSize(8)
                .SetPadding(5)
                .SetBackgroundColor(color)
                .Add(new Paragraph(estado));
        }
    }
}