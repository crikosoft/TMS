namespace KleanKart.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bancoes",
                c => new
                    {
                        bancoId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100),
                        descripcion = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.bancoId);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        categoriaId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 1000),
                        descripcion = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.categoriaId);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        productoId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 1000),
                        descripcion = c.String(maxLength: 1000),
                        unidadMedidaId = c.Int(nullable: false),
                        categoriaId = c.Int(),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.productoId)
                .ForeignKey("dbo.Categorias", t => t.categoriaId)
                .ForeignKey("dbo.UnidadMedidas", t => t.unidadMedidaId)
                .Index(t => t.unidadMedidaId)
                .Index(t => t.categoriaId);
            
            CreateTable(
                "dbo.CompraDetalles",
                c => new
                    {
                        compraDetalleId = c.Int(nullable: false, identity: true),
                        productoId = c.Int(nullable: false),
                        cantidad = c.Double(nullable: false),
                        precioUnitario = c.Double(nullable: false),
                        precioTotal = c.Double(nullable: false),
                        compraId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.compraDetalleId)
                .ForeignKey("dbo.Compras", t => t.compraId)
                .ForeignKey("dbo.Productoes", t => t.productoId)
                .Index(t => t.productoId)
                .Index(t => t.compraId);
            
            CreateTable(
                "dbo.Compras",
                c => new
                    {
                        compraId = c.Int(nullable: false, identity: true),
                        tipoDocumentoId = c.Int(nullable: false),
                        serie = c.String(),
                        numero = c.String(),
                        proveedorId = c.Int(nullable: false),
                        subTotal = c.Double(nullable: false),
                        igv = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        comentario = c.String(),
                        estadoCompraId = c.Int(nullable: false),
                        fechaDocumento = c.DateTime(),
                        usuarioCreacion = c.String(),
                        usuarioModificacion = c.String(),
                        fechaCreacion = c.DateTime(nullable: false),
                        fechaModificacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.compraId)
                .ForeignKey("dbo.EstadoCompras", t => t.estadoCompraId)
                .ForeignKey("dbo.Proveedors", t => t.proveedorId)
                .ForeignKey("dbo.TipoDocumentoes", t => t.tipoDocumentoId)
                .Index(t => t.tipoDocumentoId)
                .Index(t => t.proveedorId)
                .Index(t => t.estadoCompraId);
            
            CreateTable(
                "dbo.EstadoCompras",
                c => new
                    {
                        estadoCompraId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.estadoCompraId);
            
            CreateTable(
                "dbo.Proveedors",
                c => new
                    {
                        proveedorId = c.Int(nullable: false, identity: true),
                        razonSocial = c.String(nullable: false, maxLength: 1000),
                        contacto = c.String(maxLength: 1000),
                        ruc = c.String(maxLength: 50),
                        direccion = c.String(maxLength: 1000),
                        telefono = c.String(maxLength: 50),
                        email = c.String(),
                        bancoId = c.Int(),
                        nroCuenta = c.String(maxLength: 200),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.proveedorId)
                .ForeignKey("dbo.Bancoes", t => t.bancoId)
                .Index(t => t.bancoId);
            
            CreateTable(
                "dbo.TipoDocumentoes",
                c => new
                    {
                        tipoDocumentoId = c.Int(nullable: false, identity: true),
                        nombre = c.String(maxLength: 50),
                        descripcion = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.tipoDocumentoId);
            
            CreateTable(
                "dbo.OrdenDetalles",
                c => new
                    {
                        ordenDetalleId = c.Int(nullable: false, identity: true),
                        productoId = c.Int(nullable: false),
                        cantidad = c.Double(nullable: false),
                        precioUnitario = c.Double(nullable: false),
                        precioTotal = c.Double(nullable: false),
                        ordenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ordenDetalleId)
                .ForeignKey("dbo.Ordens", t => t.ordenId)
                .ForeignKey("dbo.Productoes", t => t.productoId)
                .Index(t => t.productoId)
                .Index(t => t.ordenId);
            
            CreateTable(
                "dbo.Ordens",
                c => new
                    {
                        ordenId = c.Int(nullable: false, identity: true),
                        numero = c.String(maxLength: 20),
                        clienteOrigenId = c.Int(nullable: false),
                        clienteDestinatarioId = c.Int(nullable: false),
                        clientePagoId = c.Int(nullable: false),
                        estadoOrdenId = c.Int(),
                        subTotal = c.Double(nullable: false),
                        igv = c.Double(nullable: false),
                        Total = c.Double(nullable: false),
                        direccionOrigenId = c.Int(nullable: false),
                        direccionDestinoId = c.Int(nullable: false),
                        fechaSolicitud = c.DateTime(),
                        fechaEntrega = c.DateTime(),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                        comentario = c.String(maxLength: 1000),
                        Cliente_clienteid = c.Int(),
                    })
                .PrimaryKey(t => t.ordenId)
                .ForeignKey("dbo.Clientes", t => t.Cliente_clienteid)
                .ForeignKey("dbo.Clientes", t => t.clienteDestinatarioId)
                .ForeignKey("dbo.Clientes", t => t.clienteOrigenId)
                .ForeignKey("dbo.Clientes", t => t.clientePagoId)
                .ForeignKey("dbo.Direccions", t => t.direccionDestinoId)
                .ForeignKey("dbo.Direccions", t => t.direccionOrigenId)
                .ForeignKey("dbo.EstadoOrdens", t => t.estadoOrdenId)
                .Index(t => t.clienteOrigenId)
                .Index(t => t.clienteDestinatarioId)
                .Index(t => t.clientePagoId)
                .Index(t => t.estadoOrdenId)
                .Index(t => t.direccionOrigenId)
                .Index(t => t.direccionDestinoId)
                .Index(t => t.Cliente_clienteid);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        clienteid = c.Int(nullable: false, identity: true),
                        razonSocial = c.String(nullable: false, maxLength: 1000),
                        contacto = c.String(maxLength: 1000),
                        ruc = c.String(nullable: false, maxLength: 50),
                        telefono = c.String(maxLength: 50),
                        email = c.String(maxLength: 200),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.clienteid);
            
            CreateTable(
                "dbo.ClienteDireccions",
                c => new
                    {
                        clienteDireccionId = c.Int(nullable: false, identity: true),
                        clienteId = c.Int(nullable: false),
                        direccionId = c.Int(nullable: false),
                        tipoDireccionId = c.Int(nullable: false),
                        fechaInicio = c.DateTime(),
                    })
                .PrimaryKey(t => t.clienteDireccionId)
                .ForeignKey("dbo.Clientes", t => t.clienteId)
                .ForeignKey("dbo.Direccions", t => t.direccionId)
                .ForeignKey("dbo.TipoDireccions", t => t.tipoDireccionId)
                .Index(t => t.clienteId)
                .Index(t => t.direccionId)
                .Index(t => t.tipoDireccionId);
            
            CreateTable(
                "dbo.Direccions",
                c => new
                    {
                        direccionId = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 1000),
                        ubigeoId = c.Int(nullable: false),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.direccionId)
                .ForeignKey("dbo.Ubigeos", t => t.ubigeoId)
                .Index(t => t.ubigeoId);
            
            CreateTable(
                "dbo.Ubigeos",
                c => new
                    {
                        ubigeoId = c.Int(nullable: false, identity: true),
                        codigo = c.String(nullable: false, maxLength: 10),
                        descripcion = c.String(nullable: false, maxLength: 1000),
                    })
                .PrimaryKey(t => t.ubigeoId);
            
            CreateTable(
                "dbo.TipoDireccions",
                c => new
                    {
                        tipoDireccionId = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.tipoDireccionId);
            
            CreateTable(
                "dbo.Envios",
                c => new
                    {
                        envioId = c.Int(nullable: false, identity: true),
                        proveedorId = c.Int(nullable: false),
                        choferId = c.Int(nullable: false),
                        vehiculoId = c.Int(nullable: false),
                        subTotal = c.Double(),
                        igv = c.Double(),
                        Total = c.Double(),
                        formaPagoId = c.Int(),
                        fechaPago = c.DateTime(),
                        comentario = c.String(maxLength: 1000),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.envioId)
                .ForeignKey("dbo.Chofers", t => t.choferId)
                .ForeignKey("dbo.Proveedors", t => t.proveedorId)
                .ForeignKey("dbo.Vehiculoes", t => t.vehiculoId)
                .Index(t => t.proveedorId)
                .Index(t => t.choferId)
                .Index(t => t.vehiculoId);
            
            CreateTable(
                "dbo.Chofers",
                c => new
                    {
                        choferId = c.Int(nullable: false, identity: true),
                        nombres = c.String(nullable: false, maxLength: 200),
                        apellidos = c.String(nullable: false, maxLength: 200),
                        nroDocumentoIdentidad = c.String(maxLength: 20),
                        tipoDocumentoIdentodadId = c.Int(),
                        fechaNacimiento = c.DateTime(),
                        numeroBrevete = c.String(nullable: false, maxLength: 50),
                        proveedorId = c.Int(nullable: false),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.choferId)
                .ForeignKey("dbo.Proveedors", t => t.proveedorId)
                .Index(t => t.proveedorId);
            
            CreateTable(
                "dbo.Vehiculoes",
                c => new
                    {
                        vehiculoId = c.Int(nullable: false, identity: true),
                        placaUnidad = c.String(nullable: false, maxLength: 50),
                        placaCarretera = c.String(maxLength: 50),
                        configuracionVehicular = c.String(maxLength: 50),
                        certificadoInscripcion1 = c.String(maxLength: 50),
                        certificadoInscripcion2 = c.String(maxLength: 50),
                        Marca = c.String(maxLength: 100),
                        Modelo = c.String(maxLength: 100),
                        proveedorId = c.Int(nullable: false),
                        usuarioCreacion = c.String(maxLength: 100),
                        usuarioModificacion = c.String(maxLength: 100),
                        fechaCreacion = c.DateTime(),
                        fechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.vehiculoId)
                .ForeignKey("dbo.Proveedors", t => t.proveedorId)
                .Index(t => t.proveedorId);
            
            CreateTable(
                "dbo.EstadoOrdens",
                c => new
                    {
                        estadoOrdenId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.estadoOrdenId);
            
            CreateTable(
                "dbo.OrdenEstadoOrdens",
                c => new
                    {
                        ordenEstadoOrdenId = c.Int(nullable: false, identity: true),
                        ordenId = c.Int(nullable: false),
                        estadoOrdenId = c.Int(nullable: false),
                        comentario = c.String(),
                        usuarioCreacion = c.String(),
                        fechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ordenEstadoOrdenId)
                .ForeignKey("dbo.EstadoOrdens", t => t.estadoOrdenId)
                .ForeignKey("dbo.Ordens", t => t.ordenId)
                .Index(t => t.ordenId)
                .Index(t => t.estadoOrdenId);
            
            CreateTable(
                "dbo.GuiaSalidas",
                c => new
                    {
                        guiaSalidaId = c.Int(nullable: false, identity: true),
                        serie = c.String(),
                        numero = c.String(),
                        ordenId = c.Int(nullable: false),
                        motivoTrasladoId = c.Int(nullable: false),
                        motivoDetalle = c.String(),
                        fechaTraslado = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.guiaSalidaId)
                .ForeignKey("dbo.MotivoTrasladoes", t => t.motivoTrasladoId)
                .ForeignKey("dbo.Ordens", t => t.ordenId)
                .Index(t => t.ordenId)
                .Index(t => t.motivoTrasladoId);
            
            CreateTable(
                "dbo.MotivoTrasladoes",
                c => new
                    {
                        motivoTrasladoId = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.motivoTrasladoId);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        ventaId = c.Int(nullable: false, identity: true),
                        serie = c.String(),
                        numero = c.String(),
                        ordenId = c.Int(nullable: false),
                        tipoDocumentoId = c.Int(nullable: false),
                        fecha = c.DateTime(nullable: false),
                        estadoVentaId = c.Int(nullable: false),
                        formaPagoId = c.Int(nullable: false),
                        fechaPago = c.DateTime(),
                    })
                .PrimaryKey(t => t.ventaId)
                .ForeignKey("dbo.EstadoVentas", t => t.estadoVentaId)
                .ForeignKey("dbo.FormaPagoes", t => t.formaPagoId)
                .ForeignKey("dbo.Ordens", t => t.ordenId)
                .ForeignKey("dbo.TipoDocumentoes", t => t.tipoDocumentoId)
                .Index(t => t.ordenId)
                .Index(t => t.tipoDocumentoId)
                .Index(t => t.estadoVentaId)
                .Index(t => t.formaPagoId);
            
            CreateTable(
                "dbo.EstadoVentas",
                c => new
                    {
                        estadoVentaId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.estadoVentaId);
            
            CreateTable(
                "dbo.FormaPagoes",
                c => new
                    {
                        formaPagoId = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.formaPagoId);
            
            CreateTable(
                "dbo.Precios",
                c => new
                    {
                        precioId = c.Int(nullable: false, identity: true),
                        precio = c.Double(nullable: false),
                        fecha = c.DateTime(nullable: false),
                        productoId = c.Int(nullable: false),
                        compraDetalleId = c.Int(nullable: false),
                        usuarioCreacion = c.String(),
                        fechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.precioId)
                .ForeignKey("dbo.CompraDetalles", t => t.compraDetalleId)
                .ForeignKey("dbo.Productoes", t => t.productoId)
                .Index(t => t.productoId)
                .Index(t => t.compraDetalleId);
            
            CreateTable(
                "dbo.UnidadMedidas",
                c => new
                    {
                        unidadMedidaId = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100),
                        sigla = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.unidadMedidaId);
            
            CreateTable(
                "dbo.EmpresaAdministradoras",
                c => new
                    {
                        empresaAdministradoraId = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(nullable: false, maxLength: 1000),
                        RUC = c.String(maxLength: 50),
                        Direccion = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.empresaAdministradoraId);
            
            CreateTable(
                "dbo.TipoDocumentoIdentidads",
                c => new
                    {
                        tipoDocumentoIdentidadId = c.Int(nullable: false, identity: true),
                        nombre = c.String(),
                        descripcion = c.String(),
                    })
                .PrimaryKey(t => t.tipoDocumentoIdentidadId);
            
            CreateTable(
                "dbo.EnvioOrdens",
                c => new
                    {
                        Envio_envioId = c.Int(nullable: false),
                        Orden_ordenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Envio_envioId, t.Orden_ordenId })
                .ForeignKey("dbo.Envios", t => t.Envio_envioId, cascadeDelete: true)
                .ForeignKey("dbo.Ordens", t => t.Orden_ordenId, cascadeDelete: true)
                .Index(t => t.Envio_envioId)
                .Index(t => t.Orden_ordenId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "unidadMedidaId", "dbo.UnidadMedidas");
            DropForeignKey("dbo.Precios", "productoId", "dbo.Productoes");
            DropForeignKey("dbo.Precios", "compraDetalleId", "dbo.CompraDetalles");
            DropForeignKey("dbo.OrdenDetalles", "productoId", "dbo.Productoes");
            DropForeignKey("dbo.Ventas", "tipoDocumentoId", "dbo.TipoDocumentoes");
            DropForeignKey("dbo.Ventas", "ordenId", "dbo.Ordens");
            DropForeignKey("dbo.Ventas", "formaPagoId", "dbo.FormaPagoes");
            DropForeignKey("dbo.Ventas", "estadoVentaId", "dbo.EstadoVentas");
            DropForeignKey("dbo.GuiaSalidas", "ordenId", "dbo.Ordens");
            DropForeignKey("dbo.GuiaSalidas", "motivoTrasladoId", "dbo.MotivoTrasladoes");
            DropForeignKey("dbo.OrdenEstadoOrdens", "ordenId", "dbo.Ordens");
            DropForeignKey("dbo.OrdenEstadoOrdens", "estadoOrdenId", "dbo.EstadoOrdens");
            DropForeignKey("dbo.Ordens", "estadoOrdenId", "dbo.EstadoOrdens");
            DropForeignKey("dbo.Envios", "vehiculoId", "dbo.Vehiculoes");
            DropForeignKey("dbo.Vehiculoes", "proveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.Envios", "proveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.EnvioOrdens", "Orden_ordenId", "dbo.Ordens");
            DropForeignKey("dbo.EnvioOrdens", "Envio_envioId", "dbo.Envios");
            DropForeignKey("dbo.Envios", "choferId", "dbo.Chofers");
            DropForeignKey("dbo.Chofers", "proveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.Ordens", "direccionOrigenId", "dbo.Direccions");
            DropForeignKey("dbo.Ordens", "direccionDestinoId", "dbo.Direccions");
            DropForeignKey("dbo.OrdenDetalles", "ordenId", "dbo.Ordens");
            DropForeignKey("dbo.Ordens", "clientePagoId", "dbo.Clientes");
            DropForeignKey("dbo.Ordens", "clienteOrigenId", "dbo.Clientes");
            DropForeignKey("dbo.Ordens", "clienteDestinatarioId", "dbo.Clientes");
            DropForeignKey("dbo.Ordens", "Cliente_clienteid", "dbo.Clientes");
            DropForeignKey("dbo.ClienteDireccions", "tipoDireccionId", "dbo.TipoDireccions");
            DropForeignKey("dbo.ClienteDireccions", "direccionId", "dbo.Direccions");
            DropForeignKey("dbo.Direccions", "ubigeoId", "dbo.Ubigeos");
            DropForeignKey("dbo.ClienteDireccions", "clienteId", "dbo.Clientes");
            DropForeignKey("dbo.CompraDetalles", "productoId", "dbo.Productoes");
            DropForeignKey("dbo.Compras", "tipoDocumentoId", "dbo.TipoDocumentoes");
            DropForeignKey("dbo.Compras", "proveedorId", "dbo.Proveedors");
            DropForeignKey("dbo.Proveedors", "bancoId", "dbo.Bancoes");
            DropForeignKey("dbo.Compras", "estadoCompraId", "dbo.EstadoCompras");
            DropForeignKey("dbo.CompraDetalles", "compraId", "dbo.Compras");
            DropForeignKey("dbo.Productoes", "categoriaId", "dbo.Categorias");
            DropIndex("dbo.EnvioOrdens", new[] { "Orden_ordenId" });
            DropIndex("dbo.EnvioOrdens", new[] { "Envio_envioId" });
            DropIndex("dbo.Precios", new[] { "compraDetalleId" });
            DropIndex("dbo.Precios", new[] { "productoId" });
            DropIndex("dbo.Ventas", new[] { "formaPagoId" });
            DropIndex("dbo.Ventas", new[] { "estadoVentaId" });
            DropIndex("dbo.Ventas", new[] { "tipoDocumentoId" });
            DropIndex("dbo.Ventas", new[] { "ordenId" });
            DropIndex("dbo.GuiaSalidas", new[] { "motivoTrasladoId" });
            DropIndex("dbo.GuiaSalidas", new[] { "ordenId" });
            DropIndex("dbo.OrdenEstadoOrdens", new[] { "estadoOrdenId" });
            DropIndex("dbo.OrdenEstadoOrdens", new[] { "ordenId" });
            DropIndex("dbo.Vehiculoes", new[] { "proveedorId" });
            DropIndex("dbo.Chofers", new[] { "proveedorId" });
            DropIndex("dbo.Envios", new[] { "vehiculoId" });
            DropIndex("dbo.Envios", new[] { "choferId" });
            DropIndex("dbo.Envios", new[] { "proveedorId" });
            DropIndex("dbo.Direccions", new[] { "ubigeoId" });
            DropIndex("dbo.ClienteDireccions", new[] { "tipoDireccionId" });
            DropIndex("dbo.ClienteDireccions", new[] { "direccionId" });
            DropIndex("dbo.ClienteDireccions", new[] { "clienteId" });
            DropIndex("dbo.Ordens", new[] { "Cliente_clienteid" });
            DropIndex("dbo.Ordens", new[] { "direccionDestinoId" });
            DropIndex("dbo.Ordens", new[] { "direccionOrigenId" });
            DropIndex("dbo.Ordens", new[] { "estadoOrdenId" });
            DropIndex("dbo.Ordens", new[] { "clientePagoId" });
            DropIndex("dbo.Ordens", new[] { "clienteDestinatarioId" });
            DropIndex("dbo.Ordens", new[] { "clienteOrigenId" });
            DropIndex("dbo.OrdenDetalles", new[] { "ordenId" });
            DropIndex("dbo.OrdenDetalles", new[] { "productoId" });
            DropIndex("dbo.Proveedors", new[] { "bancoId" });
            DropIndex("dbo.Compras", new[] { "estadoCompraId" });
            DropIndex("dbo.Compras", new[] { "proveedorId" });
            DropIndex("dbo.Compras", new[] { "tipoDocumentoId" });
            DropIndex("dbo.CompraDetalles", new[] { "compraId" });
            DropIndex("dbo.CompraDetalles", new[] { "productoId" });
            DropIndex("dbo.Productoes", new[] { "categoriaId" });
            DropIndex("dbo.Productoes", new[] { "unidadMedidaId" });
            DropTable("dbo.EnvioOrdens");
            DropTable("dbo.TipoDocumentoIdentidads");
            DropTable("dbo.EmpresaAdministradoras");
            DropTable("dbo.UnidadMedidas");
            DropTable("dbo.Precios");
            DropTable("dbo.FormaPagoes");
            DropTable("dbo.EstadoVentas");
            DropTable("dbo.Ventas");
            DropTable("dbo.MotivoTrasladoes");
            DropTable("dbo.GuiaSalidas");
            DropTable("dbo.OrdenEstadoOrdens");
            DropTable("dbo.EstadoOrdens");
            DropTable("dbo.Vehiculoes");
            DropTable("dbo.Chofers");
            DropTable("dbo.Envios");
            DropTable("dbo.TipoDireccions");
            DropTable("dbo.Ubigeos");
            DropTable("dbo.Direccions");
            DropTable("dbo.ClienteDireccions");
            DropTable("dbo.Clientes");
            DropTable("dbo.Ordens");
            DropTable("dbo.OrdenDetalles");
            DropTable("dbo.TipoDocumentoes");
            DropTable("dbo.Proveedors");
            DropTable("dbo.EstadoCompras");
            DropTable("dbo.Compras");
            DropTable("dbo.CompraDetalles");
            DropTable("dbo.Productoes");
            DropTable("dbo.Categorias");
            DropTable("dbo.Bancoes");
        }
    }
}
