using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIBLIOTECA_SAPPE
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Precio { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public string Img { get; set; }
        public string NombreCategoria { get; set; }
        public int Categoria { get; set; }
        public string NombreMarca { get; set; }
        public int Marca { get; set; }
        public string UnidadMedida { get; set; }
        public int IdUnidadMedida{ get; set; }
        public string Perecible { get; set; }
        public int IdPerecible { get; set; }
        public int Volumen { get; set; }
    }

    public class UnidadMedidaDTO {
        public int IdUnidadMedida { get; set; }
        public string UnidadMedida { get; set; }
        public int Peso { get; set; }
    }

    public class LoteDTO {
        public string IdLote { get; set; }
        public int Cantidad { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int Producto { get; set; }
        public string NombreProducto { get; set; }
        public int Proveedor { get; set; }
        public string NombreProveedor { get; set; }
    }

    public class ComunaDTO { 
        public int IdComuna { get; set; }
        public string NombreComuna { get; set; }
        public int Region { get; set; }
    }

    public class TiendaDTO
    {
        public int IdTienda { get; set; }
        public string NombreTienda { get; set; }
        public string FraseFooter { get; set; }
        public string Direccion { get; set; }
        public int Comuna { get; set; }
        public string NombreComuna { get; set; }
        public string Logo { get; set; }
        public string Color { get; set; }
        public string ColorSecundario { get; set; }
        public string ImgLogin { get; set; }
        public string ImgRegistro { get; set; }
    }

    public class BodegaDTO {
        public int IdBodega { get; set; }
        public string NombreBodega { get; set; }
        public int CapacidadMax { get; set; }
    }

    public class AdministradorDTO
    {
        public int IdUsuario { get; set; }
        public string Rut { get; set; }
        public string NombreUsuario { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string NombreTipoUsuario { get; set; }
    }

    public class ProveedorDTO
    {
        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int Telefono { get; set; }
        public string Correo { get; set; }
        public string Direccion { get; set; }
        public string Comuna { get; set; }
        public int IdComuna { get; set; }
    }

    public class DescuentoDTO
    {
        public int IdDescuento { get; set; }
        public int DiasParaVencimiento { get; set; }
        public int Descuento { get; set; }
    }

    public class AnuncioDTO
    {
        public int IdAnuncio { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string ImgUrl { get; set; }
    }

    public class BoletaDTO
    {
        public string IdBoleta { get; set; }
        public DateTime Fecha { get; set; }
        public int Subtotal { get; set; }
        public int IVA { get; set; }
        public int Total { get; set; }
        public int Descuentos { get; set; }
        public string EstadoBoleta { get; set; }
    }

    public class SeccionBodegaDTO
    {
        public int IdSeccion { get; set; }
        public string NombreSeccion { get; set; }
    }

    public class OrdenDTO
    {
        public int IdOrden { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CodigoPostal { get; set; }
        public string Rut { get; set; }
        public string BoletaId { get; set; }
        public string EstadoEntrega { get; set; }
        public string MetodoPago { get; set; }
        public string Cliente { get; set; }
    }

    public class MovimientoInventarioDTO
    {
        public int IdMovimiento { get; set; }
        public DateTime Fecha { get; set; }
        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }
        public string ProductoNombre { get; set; }
    }

    public class DevolucionDTO
    {
        public int IdDevolucion { get; set; }
        public DateTime Fecha { get; set; }
        public float Total { get; set; }
        public string Motivo { get; set; }
        public int OrdenId { get; set; }
    }

    public class InventarioDTO
    {
        public int IdInventario { get; set; }
        public string NombreProducto { get; set; }
        public int CantidadDisponible { get; set; }
        public string NombreSeccion { get; set; }
        public string NombreBodega { get; set; }
    }



}
