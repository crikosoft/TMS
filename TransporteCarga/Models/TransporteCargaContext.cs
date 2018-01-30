using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace TransporteCarga.Models
{
    public class TransporteCargaContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TransporteCargaContext() : base("name=DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Orden>()
                    .HasRequired(m => m.DireccionOrigen)
                    .WithMany(t => t.OrigenesOrden)
                    .HasForeignKey(m => m.direccionOrigenId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                    .HasRequired(m => m.DireccionDestino)
                    .WithMany(t => t.DestinosOrden)
                    .HasForeignKey(m => m.direccionDestinoId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                    .HasRequired(m => m.ClienteDestinatario)
                    .WithMany(t => t.OrdenesDestinos)
                    .HasForeignKey(m => m.clienteDestinatarioId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                    .HasRequired(m => m.ClienteOrigen)
                    .WithMany(t => t.OrdenesOrigenes)
                    .HasForeignKey(m => m.clienteOrigenId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orden>()
                    .HasRequired(m => m.ClientePago)
                    .WithMany(t => t.OrdenesPagos)
                    .HasForeignKey(m => m.clientePagoId)
                    .WillCascadeOnDelete(false);


        }

        public DbSet<EmpresaAdministradora> EmpresaAdministradora { get; set; }
       
        public DbSet<Cliente> Clientes { get; set; }
        //public DbSet<Presentacion> Presentaciones { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Orden> Ordenes { get; set; }
        public DbSet<OrdenDetalle> OrdenDetalles { get; set; }
        public DbSet<EstadoOrden> EstadosOrden { get; set; }

        public DbSet<Venta> Ventas { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }
        public DbSet<EstadoVenta> EstadoVentas { get; set; }
        public DbSet<MotivoTraslado> MotivoTraslado { get; set; }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraDetalle> CompraDetalles { get; set; }

        public System.Data.Entity.DbSet<TransporteCarga.Models.GuiaSalida> GuiaSalidas { get; set; }

        public DbSet<FormaPago> FormaPagos { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<EstadoCompra> EstadoCompras { get; set; }

        public DbSet<OrdenEstadoOrden> OrdenesEstadoOrden { get; set; }
        public DbSet<Precio> Precios { get; set; }

        // Transporte
        public DbSet<Chofer> Choferes { get; set; }
        public DbSet<Direccion> Direcciones { get; set; }
        public DbSet<Ubigeo> Ubigeo { get; set; }
        public DbSet<Vehiculo> Vehiculo { get; set; }
        public DbSet<TipoDocumentoIdentidad> TiposDocumentoIdentidad { get; set; }
        public DbSet<ClienteDireccion> ClienteDireccion { get; set; }
        public DbSet<TipoDireccion> TipoDireccion { get; set; }
        public DbSet<UnidadMedida> UnidadMedida { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<EstadoEnvio> EstadoEnvios { get; set; }

        public DbSet<EstadoPago> EstadoPagos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        //public System.Data.Entity.DbSet<TransporteCarga.ViewModels.PedidoViewModel> PedidoViewModels { get; set; }

    }
}
