using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.Models;
using Microsoft.Reporting.WebForms;
using TransporteCarga.Helpers;
using System.Globalization;


namespace TransporteCarga.Controllers
{
    public class GuiaSalidaController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /GuiaSalida/
        public ActionResult Index()
        {
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "razonSocial", "razonSocial");

            var guiasalidas = db.GuiaSalidas.Include(g => g.MotivoTraslado).Include(g => g.Orden);
            return View(guiasalidas.ToList());
        }

        // GET: /GuiaSalida/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuiaSalida guiasalida = db.GuiaSalidas.Find(id);
            if (guiasalida == null)
            {
                return HttpNotFound();
            }
            return View(guiasalida);
        }

        // GET: /GuiaSalida/Create
        public ActionResult Create(int? id)
        {
            GuiaSalida GuiaSalida = new GuiaSalida();

            Orden Orden = db.Ordenes.Where(a => a.ordenId == id).FirstOrDefault();
            GuiaSalida.ordenId = Orden.ordenId;
            GuiaSalida.Orden = Orden;

            ViewBag.motivoTrasladoId = new SelectList(db.MotivoTraslado, "motivoTrasladoId", "nombre");
            ViewBag.ordenId = new SelectList(db.Ordenes.Where(a => a.ordenId == id), "ordenId", "ordenId");

            return View(GuiaSalida);
        }

        // POST: /GuiaSalida/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guiaSalidaId,serie,ordenId,numero,motivoTrasladoId,motivoDetalle,fechaEmision")] GuiaSalida guiasalida)
        {
            if (ModelState.IsValid)
            {
                db.GuiaSalidas.Add(guiasalida);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Orden");

            }

            ViewBag.motivoTrasladoId = new SelectList(db.MotivoTraslado, "motivoTrasladoId", "nombre", guiasalida.motivoTrasladoId);
            ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", guiasalida.ordenId);
            return View(guiasalida);
        }

        // GET: /GuiaSalida/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuiaSalida guiasalida = db.GuiaSalidas.Find(id);
            if (guiasalida == null)
            {
                return HttpNotFound();
            }
            ViewBag.motivoTrasladoId = new SelectList(db.MotivoTraslado, "motivoTrasladoId", "nombre", guiasalida.motivoTrasladoId);
            ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", guiasalida.ordenId);
            return View(guiasalida);
        }

        // POST: /GuiaSalida/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="guiaSalidaId,serie,numero,ordenId,motivoTrasladoId,motivoDetalle,fechaEmision")] GuiaSalida guiasalida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guiasalida).State = EntityState.Modified;
                db.Entry(guiasalida).Property(x => x.ordenId).IsModified = false;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "Orden");

            }
            ViewBag.motivoTrasladoId = new SelectList(db.MotivoTraslado, "motivoTrasladoId", "nombre", guiasalida.motivoTrasladoId);
            ViewBag.ordenId = new SelectList(db.Ordenes, "ordenId", "numero", guiasalida.ordenId);
            return View(guiasalida);
        }

        // GET: /GuiaSalida/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuiaSalida guiasalida = db.GuiaSalidas.Find(id);
            if (guiasalida == null)
            {
                return HttpNotFound();
            }
            return View(guiasalida);
        }

        // POST: /GuiaSalida/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuiaSalida guiasalida = db.GuiaSalidas.Find(id);
            db.GuiaSalidas.Remove(guiasalida);

            Orden pedido = db.Ordenes.Find(guiasalida.ordenId);
            pedido.estadoOrdenId = db.EstadosOrden.Single(p => p.nombre == "Creado").estadoOrdenId;

            OrdenEstadoOrden pedidoEstado = new OrdenEstadoOrden();
            pedidoEstado = pedido.Estados.Where(p => p.EstadoOrden.nombre == "Con Guía").FirstOrDefault();
            db.OrdenesEstadoOrden.Remove(pedidoEstado);

            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Index", "Orden");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: /GuiaSalida/Document/5
        public ActionResult Document(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuiaSalida guiaSalida = db.GuiaSalidas.Find(id);
            if (guiaSalida == null)
            {
                return HttpNotFound();
            }
            return View(guiaSalida);
        }

        // GET: /Venta/
        public FileResult DocumentGuiaPrint(int? id, byte empty, int type)
        {

            if (id == null)
            {
                return null;
            }


            if (empty == 1)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                //1. Cambia Estado de Orden
                var ordenOriginal = db.Ordenes.Find(id);
                var estadoOrden = db.EstadosOrden.Single(p => p.nombre == "Con Guía");

                ordenOriginal.estadoOrdenId = estadoOrden.estadoOrdenId;
                ordenOriginal.usuarioModificacion = User.Identity.Name;
                ordenOriginal.fechaModificacion = cstTime;

                //2. Grabar Estado de Pedido
                var ordenEstadoOrden = new OrdenEstadoOrden();

                ordenEstadoOrden.ordenId = ordenOriginal.ordenId;
                ordenEstadoOrden.estadoOrdenId = estadoOrden.estadoOrdenId;
                ordenEstadoOrden.usuarioCreacion = User.Identity.Name;
                ordenEstadoOrden.fechaCreacion = cstTime;

                db.OrdenesEstadoOrden.Add(ordenEstadoOrden);

                db.SaveChanges();

            }

            //List<Venta> venta = db.Ventas.Where(a => a.ventaId == id).ToList();
            List<GuiaSalida> guia = db.GuiaSalidas.Where(a => a.ordenId == id).ToList();

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Serie");
            dt.Columns.Add("Numero");
            dt.Columns.Add("FechaEmision");
            dt.Columns.Add("FechaTraslado");
            dt.Columns.Add("Remitente");
            dt.Columns.Add("Destinatario");
            dt.Columns.Add("direccionRemitente");
            dt.Columns.Add("direccionDestinatario");
            dt.Columns.Add("ciudadRemitente");
            dt.Columns.Add("ciudadDestinatario");
            dt.Columns.Add("RUCRemitente");
            dt.Columns.Add("RUCDestinatario");
            dt.Columns.Add("PuntoPartida");
            dt.Columns.Add("PuntoLlegada");
            dt.Columns.Add("Cod");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("UnidadMedida");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("MarcaVehiculo");
            dt.Columns.Add("PlacaVehiculo");
            dt.Columns.Add("ConfiguracionVehicularVehiculo");
            dt.Columns.Add("CertificadoInscripcionVehiculo");
            dt.Columns.Add("LicenciaConductorVehiculo");
            dt.Columns.Add("RazonSocialSubcontratada");
            dt.Columns.Add("RUCSubcontratada");
            dt.Columns.Add("ChoferSubcontratada");
            dt.Columns.Add("Referencia");
            dt.Columns.Add("DireccionFiscalClientePago");
            dt.Columns.Add("RucClientePago");
            dt.Columns.Add("Importe");
            dt.Columns.Add("Flete");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalTexto");
            dt.Columns.Add("FechaEmisionF");
            dt.Columns.Add("Comentario");
            


            foreach (var item in guia[0].Orden.Detalles.OrderBy(a => a.Producto.nombre))
            {
                DataRow _ravi = dt.NewRow();
                _ravi["Serie"] = guia[0].serie;
                _ravi["Numero"] = guia[0].numero;
                //_ravi["FechaEmision"] = ((DateTime)(guia[0].Orden.Envios.FirstOrDefault().fechaCreacion)).ToShortDateString();
                _ravi["FechaTraslado"] = guia[0].Orden.Envios.FirstOrDefault().fechaTraslado.ToShortDateString();
                _ravi["Remitente"] = guia[0].Orden.ClienteOrigen.razonSocial;
                _ravi["Destinatario"] = guia[0].Orden.ClienteDestinatario.razonSocial;
                _ravi["direccionRemitente"] = guia[0].Orden.ClienteOrigen.Direcciones.Where(a=>a.TipoDireccion.nombre == "Fiscal").FirstOrDefault().Direccion.descripcion;
                _ravi["direccionDestinatario"] = guia[0].Orden.ClienteDestinatario.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").FirstOrDefault().Direccion.descripcion;
                _ravi["ciudadRemitente"] = guia[0].Orden.DireccionOrigen.Ubigeo.UbigeoParent.descripcion + " - " + guia[0].Orden.DireccionOrigen.Ubigeo.descripcion;
                _ravi["ciudadDestinatario"] = guia[0].Orden.DireccionDestino.Ubigeo.UbigeoParent.descripcion + " - " + guia[0].Orden.DireccionDestino.Ubigeo.descripcion; ;
                _ravi["RUCRemitente"] = guia[0].Orden.ClienteOrigen.ruc;
                _ravi["RUCDestinatario"] = guia[0].Orden.ClienteDestinatario.ruc;
                _ravi["PuntoPartida"] = guia[0].Orden.DireccionOrigen.direccionCompleta;
                _ravi["PuntoLlegada"] = guia[0].Orden.DireccionDestino.direccionCompleta;
                _ravi["Cod"] = item.Producto.productoId;
                _ravi["Cantidad"] = item.cantidad;
                _ravi["Comentario"] = item.comentario;
                _ravi["UnidadMedida"] = item.Producto.UnidadMedida.nombre;
                _ravi["Descripcion"] = item.Producto.nombre;
                _ravi["MarcaVehiculo"] = guia[0].Orden.Envios.FirstOrDefault().Vehiculo.Marca;
                _ravi["PlacaVehiculo"] = guia[0].Orden.Envios.FirstOrDefault().Vehiculo.placaUnidad;
                _ravi["ConfiguracionVehicularVehiculo"] = guia[0].Orden.Envios.FirstOrDefault().Vehiculo.placaCarretera;
                _ravi["CertificadoInscripcionVehiculo"] = guia[0].Orden.Envios.FirstOrDefault().Vehiculo.certificadoInscripcion1;
                _ravi["LicenciaConductorVehiculo"] = guia[0].Orden.Envios.FirstOrDefault().Chofer.numeroBrevete;
                _ravi["RazonSocialSubcontratada"] = guia[0].Orden.Envios.FirstOrDefault().Proveedor.razonSocial;
                _ravi["RUCSubcontratada"] = guia[0].Orden.Envios.FirstOrDefault().Proveedor.ruc;
                _ravi["ChoferSubcontratada"] = guia[0].Orden.Envios.FirstOrDefault().Chofer.apellidos + ", " + guia[0].Orden.Envios.FirstOrDefault().Chofer.nombres;
                
                if (type ==2)
                { 
                    _ravi["Referencia"] = guia[0].serie +" " + guia[0].numero; ;
                    _ravi["DireccionFiscalClientePago"] = guia[0].Orden.ClientePago.Direcciones.FirstOrDefault().Direccion.descripcion;
                    _ravi["RucClientePago"] = guia[0].Orden.ClientePago.ruc;
                    _ravi["Importe"] = string.Format("{0:0,0.00}", item.precioTotal);
                    _ravi["Flete"] = string.Format("{0:0,0.00}", guia[0].Orden.subTotal);
                    _ravi["IGV"] = string.Format("{0:0,0.00}", guia[0].Orden.igv);
                    _ravi["Total"] = string.Format("{0:0,0.00}", guia[0].Orden.Total);
                    _ravi["FechaEmisionF"] = guia[0].Orden.Ventas.FirstOrDefault().fecha.ToShortDateString();
                    ConvertirNumero conv = new ConvertirNumero();
                    String numero = guia[0].Orden.Total.ToString();
                    _ravi["TotalTexto"] = conv.Convertir(numero, true);// "CIENTO OCHENTA Y OCHO y 10/100";



                }
                dt.Rows.Add(_ravi);
            }


            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            if (empty == 0 && type == 1) //type =1 guia
            { 
                localreport.ReportPath = Server.MapPath("~/Reports/Guia.rdlc");
            }
            else if (empty == 1 && type == 1)
            {
                localreport.ReportPath = Server.MapPath("~/Reports/GuiaEmpty.rdlc");
            }
            else if (empty == 0 && type == 2)
            {
                localreport.ReportPath = Server.MapPath("~/Reports/Factura.rdlc");
            }
            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }

        public FileResult DocumentGuiaEmptyPrint(int? id)
        {

            if (id == null)
            {
                return null;
            }

            DateTime timeUtc = DateTime.UtcNow;
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

            //1. Cambia Estado
            var pedidoOriginal = db.Ordenes.Find(id);
            var estadoPedido = db.EstadosOrden.Single(p => p.nombre == "Con Guía");

            pedidoOriginal.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoOriginal.usuarioModificacion = User.Identity.Name;
            pedidoOriginal.fechaModificacion = cstTime;

            //2. Grabar Estado de Pedido
            var pedidoEstadoPedido = new OrdenEstadoOrden();

            pedidoEstadoPedido.ordenId = pedidoOriginal.ordenId;
            pedidoEstadoPedido.estadoOrdenId = estadoPedido.estadoOrdenId;
            pedidoEstadoPedido.usuarioCreacion = User.Identity.Name;
            pedidoEstadoPedido.fechaCreacion = cstTime;

            db.OrdenesEstadoOrden.Add(pedidoEstadoPedido);

            db.SaveChanges();


            //List<Venta> venta = db.Ventas.Where(a => a.ventaId == id).ToList();
            List<GuiaSalida> guia = db.GuiaSalidas.Where(a => a.ordenId == id).ToList();

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Cliente");
            dt.Columns.Add("Direccion");
            dt.Columns.Add("RUC");
            dt.Columns.Add("SerieF");
            dt.Columns.Add("NumeroF");
            dt.Columns.Add("SubTotal");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("SerieG");
            dt.Columns.Add("NumeroG");
            dt.Columns.Add("FechaF");
            dt.Columns.Add("FechaG");
            dt.Columns.Add("PuntoDestinoG");
            dt.Columns.Add("PuntoPartidaG");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("DescripcionP");
            dt.Columns.Add("PrecioUnitario");
            dt.Columns.Add("PrecioTotal");
            dt.Columns.Add("TotalTexto");
            dt.Columns.Add("UnidadMedida");
            dt.Columns.Add("MotivoTralado");
            dt.Columns.Add("Comentario");

            foreach (var item in guia[0].Orden.Detalles.OrderBy(a => a.Producto.nombre))
            {
                DataRow _ravi = dt.NewRow();
                _ravi["Cliente"] = guia[0].Orden.ClienteOrigen.razonSocial;
                //Falta
                //_ravi["Direccion"] = guia[0].Orden.Cliente.direccion;
                _ravi["RUC"] = guia[0].Orden.ClienteOrigen.ruc;
                _ravi["SerieF"] = "";
                _ravi["NumeroF"] = "";
                _ravi["SubTotal"] = string.Format("{0:0,0.00}", guia[0].Orden.subTotal);
                _ravi["IGV"] = string.Format("{0:0,0.00}", guia[0].Orden.igv);

                _ravi["Total"] = string.Format("{0:0,0.00}", guia[0].Orden.Total);
                _ravi["FechaF"] = "";//"Lima, " + guia[0].fecha.Day + " de " + guia[0].fecha.ToString("MMMM").ToUpper() + "  del " + guia[0].fecha.Year;

               
                string fechaTraslado = "";
                //fechaTraslado = "Lima, " + guia[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Day + " de Agosto del" + guia[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Year;

                _ravi["SerieG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().serie;
                _ravi["NumeroG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().numero;
               // _ravi["FechaG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.ToString("dd/MM/yyyy").ToUpper();
                _ravi["MotivoTralado"] = guia[0].Orden.GuiasSalida.SingleOrDefault().MotivoTraslado.nombre;

                //Falta
                //_ravi["PuntoDestinoG"] = guia[0].Orden.Cliente.direccion;
                //_ravi["PuntoPartidaG"] = "CALLE AURELIO FERNANDEZ CONCHA 298-MIRAFLORES";
                _ravi["Cantidad"] = item.cantidad;
                _ravi["DescripcionP"] = item.Producto.nombre;
                _ravi["PrecioUnitario"] = string.Format("{0:0,0.00}", item.precioUnitario);
                _ravi["PrecioTotal"] = string.Format("{0:0,0.00}", item.precioTotal);
                _ravi["TotalTexto"] = "CIENTO OCHENTA Y OCHO y 10/100";
                _ravi["UnidadMedida"] = item.Producto.UnidadMedida.nombre;
                dt.Rows.Add(_ravi);
            }


            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            localreport.ReportPath = Server.MapPath("~/Reports/GuiaEmpty.rdlc");
            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
            


        }

        public FileResult DocumentVentaPrint(int? id, byte empty, int type, bool resumen)
        {

            if (id == null)
            {
                return null;
            }


            if (empty == 1)
            {

                DateTime timeUtc = DateTime.UtcNow;
                TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

                //1. Cambia Estado de Orden
                List<Orden> ordenesOriginal = db.Ordenes.Where(a=>a.Ventas.FirstOrDefault().ventaId==id).ToList();
                var estadoOrden = db.EstadosOrden.Single(p => p.nombre == "Con Guía");

                foreach (var ordenOriginal in ordenesOriginal)
                {
                    ordenOriginal.estadoOrdenId = estadoOrden.estadoOrdenId;
                    ordenOriginal.usuarioModificacion = User.Identity.Name;
                    ordenOriginal.fechaModificacion = cstTime;
                    //2. Grabar Estado de Pedido
                    var ordenEstadoOrden = new OrdenEstadoOrden();

                    ordenEstadoOrden.ordenId = ordenOriginal.ordenId;
                    ordenEstadoOrden.estadoOrdenId = estadoOrden.estadoOrdenId;
                    ordenEstadoOrden.usuarioCreacion = User.Identity.Name;
                    ordenEstadoOrden.fechaCreacion = cstTime;

                    db.OrdenesEstadoOrden.Add(ordenEstadoOrden);

                    db.SaveChanges();
                }
            }

            Venta venta = db.Ventas.Find(id);
            //List<GuiaSalida> guias = db.GuiaSalidas.Where(a => a.ordenId == id).ToList();

            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Serie");
            dt.Columns.Add("Numero");
            dt.Columns.Add("SerieFactura");
            dt.Columns.Add("NumeroFactura");
            dt.Columns.Add("FechaEmision");
            dt.Columns.Add("FechaTraslado");
            dt.Columns.Add("Remitente");
            dt.Columns.Add("Destinatario");
            dt.Columns.Add("direccionRemitente");
            dt.Columns.Add("direccionDestinatario");
            dt.Columns.Add("ciudadRemitente");
            dt.Columns.Add("ciudadDestinatario");
            dt.Columns.Add("RUCRemitente");
            dt.Columns.Add("RUCDestinatario");
            dt.Columns.Add("PuntoPartida");
            dt.Columns.Add("PuntoLlegada");
            dt.Columns.Add("Cod");
            dt.Columns.Add("Cantidad");
            dt.Columns.Add("UnidadMedida");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("MarcaVehiculo");
            dt.Columns.Add("PlacaVehiculo");
            dt.Columns.Add("ConfiguracionVehicularVehiculo");
            dt.Columns.Add("CertificadoInscripcionVehiculo");
            dt.Columns.Add("LicenciaConductorVehiculo");
            dt.Columns.Add("RazonSocialSubcontratada");
            dt.Columns.Add("RUCSubcontratada");
            dt.Columns.Add("ChoferSubcontratada");
            dt.Columns.Add("Referencia");
            dt.Columns.Add("DireccionFiscalClientePago");
            dt.Columns.Add("RucClientePago");
            dt.Columns.Add("Importe");
            dt.Columns.Add("Flete");
            dt.Columns.Add("IGV");
            dt.Columns.Add("Total");
            dt.Columns.Add("TotalTexto");
            dt.Columns.Add("FechaEmisionF");
            dt.Columns.Add("Comentario");
            dt.Columns.Add("SPOT");
            dt.Columns.Add("NumeroGuiaIndividual");

            double? subTotal = 0;
            double? igv = 0;
            double? total = 0;
            string guias = "";
            subTotal = venta.Ordenes.AsEnumerable().Sum(a => a.subTotal);
            igv = venta.Ordenes.AsEnumerable().Sum(a => a.igv);
            total = venta.Ordenes.AsEnumerable().Sum(a => a.Total);

            foreach (var orden in venta.Ordenes)
            {
                guias +=  orden.GuiasSalida.FirstOrDefault().serie   + "-"+ orden.GuiasSalida.FirstOrDefault().numero + ", ";
            }

            foreach (var orden in venta.Ordenes)
            {
                List<OrdenDetalle> detalles = new List<OrdenDetalle>();

                //if (resumen == true && orden.resumen != null)
                //{
                //    resumen = false;
                //}



                if (resumen == true && orden.resumen != null)
                {
                    detalles.Add(orden.Detalles.FirstOrDefault());
                }
                else if (resumen == false)
                {
                    detalles = orden.Detalles;
                }

                //foreach (var detalle in orden.Detalles)
                foreach (var detalle in detalles)
                {

                DataRow _ravi = dt.NewRow();
                //_ravi["Serie"] = orden.GuiasSalida[0].serie;
                _ravi["Numero"] = guias;
                _ravi["SerieFactura"] = orden.Ventas.FirstOrDefault().serie; 
                _ravi["NumeroFactura"] = orden.Ventas.FirstOrDefault().numero; 
                //_ravi["FechaEmision"] = ((DateTime)(orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().fechaCreacion)).ToShortDateString();
                _ravi["FechaEmision"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().fechaCreacion.ToString();
                _ravi["FechaTraslado"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().fechaTraslado.ToShortDateString();
                _ravi["Remitente"] = orden.GuiasSalida[0].Orden.ClientePago.razonSocial;
                _ravi["Destinatario"] = orden.GuiasSalida[0].Orden.ClienteDestinatario.razonSocial;
                if (orden.GuiasSalida[0].Orden.ClienteOrigen.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").Count() > 0)
                    _ravi["direccionRemitente"] = orden.GuiasSalida[0].Orden.ClienteOrigen.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").FirstOrDefault().Direccion.direccionCompleta;
                else
                    _ravi["direccionRemitente"] = "";

                if (orden.GuiasSalida[0].Orden.ClienteDestinatario.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").Count() > 0)
                    _ravi["direccionDestinatario"] = orden.GuiasSalida[0].Orden.ClienteDestinatario.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").FirstOrDefault().Direccion.direccionCompleta;
                else
                    _ravi["direccionDestinatario"] = "";
                    
                _ravi["ciudadRemitente"] = orden.GuiasSalida[0].Orden.DireccionOrigen.Ubigeo.descripcion;
                _ravi["ciudadDestinatario"] = orden.GuiasSalida[0].Orden.DireccionDestino.Ubigeo.descripcion;
                _ravi["RUCRemitente"] = orden.GuiasSalida[0].Orden.ClienteOrigen.ruc;
                _ravi["RUCDestinatario"] = orden.GuiasSalida[0].Orden.ClienteDestinatario.ruc;
                _ravi["PuntoPartida"] = orden.GuiasSalida[0].Orden.DireccionOrigen.descripcion;
                _ravi["PuntoLlegada"] = orden.GuiasSalida[0].Orden.DireccionDestino.descripcion;
                _ravi["Cod"] = detalle.Producto.productoId;
                
                _ravi["Comentario"] = detalle.comentario;

                if (resumen == true && orden.resumen != null)
                {
                    _ravi["Cantidad"] = 1;
                    _ravi["UnidadMedida"] = "Unidad";
                    _ravi["Descripcion"] = orden.resumen;

                }
                else if (resumen == false)
                {
                    _ravi["Cantidad"] = detalle.cantidad;
                    _ravi["UnidadMedida"] = detalle.Producto.UnidadMedida.nombre;
                    _ravi["Descripcion"] = "POR EL SERVICIO DE TRASLADO DE " + orden.DireccionOrigen.Ubigeo.descripcion + " A " + orden.DireccionDestino.Ubigeo.descripcion + " SEGÚN DETALLE: " + detalle.Producto.nombre;
                }
                _ravi["MarcaVehiculo"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Vehiculo.Marca;
                _ravi["PlacaVehiculo"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Vehiculo.placaUnidad;
                _ravi["ConfiguracionVehicularVehiculo"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Vehiculo.placaCarretera;
                _ravi["CertificadoInscripcionVehiculo"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Vehiculo.certificadoInscripcion1;
                _ravi["LicenciaConductorVehiculo"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Chofer.numeroBrevete;
                _ravi["RazonSocialSubcontratada"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Proveedor.razonSocial;
                _ravi["RUCSubcontratada"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Proveedor.ruc;
                _ravi["ChoferSubcontratada"] = orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Chofer.apellidos + ", " + orden.GuiasSalida[0].Orden.Envios.FirstOrDefault().Chofer.nombres;
                _ravi["NumeroGuiaIndividual"] = orden.GuiasSalida[0].serie.Trim() + "-" + orden.GuiasSalida[0].numero.Trim();
                //type2: Factura o BOleta
                if (type == 2 || type == 3)
                {
                    _ravi["Referencia"] = orden.GuiasSalida[0].serie + " " + orden.GuiasSalida[0].numero; ;
                    //_ravi["DireccionFiscalClientePago"] = orden.GuiasSalida[0].Orden.ClientePago.Direcciones.FirstOrDefault().Direccion.descripcion;
                    if (orden.GuiasSalida[0].Orden.ClientePago.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").Count() > 0)
                        _ravi["DireccionFiscalClientePago"] = orden.GuiasSalida[0].Orden.ClientePago.Direcciones.Where(a => a.TipoDireccion.nombre == "Fiscal").FirstOrDefault().Direccion.direccionCompleta;
                    else
                        _ravi["DireccionFiscalClientePago"] = "";
                    _ravi["RucClientePago"] = orden.GuiasSalida[0].Orden.ClientePago.ruc;
                    if (resumen == true && orden.resumen != null)
                    {
                        _ravi["Importe"] = string.Format("{0:0,0.00}", orden.subTotal);
                    }
                    else if (resumen == false)
                    {
                        _ravi["Importe"] = string.Format("{0:0,0.00}", detalle.precioTotal);
                    }

                    _ravi["Flete"] = string.Format("{0:0,0.00}", subTotal);
                    _ravi["IGV"] = string.Format("{0:0,0.00}", igv);
                    _ravi["Total"] = string.Format("{0:0,0.00}", total);
                    _ravi["FechaEmisionF"] = orden.GuiasSalida[0].Orden.Ventas.FirstOrDefault().fecha.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);



                    ConvertirNumero conv = new ConvertirNumero();
                    //String numero = orden.GuiasSalida[0].Orden.Total.ToString();
                    _ravi["TotalTexto"] = conv.Convertir(total.ToString(), true);// "CIENTO OCHENTA Y OCHO y 10/100";

                    if (orden.GuiasSalida[0].Orden.Ventas.FirstOrDefault().spot == true)
                        _ravi["SPOT"] = "OPERACIÓN SUJETA A PAGO DE SPOT";
                    else
                        _ravi["SPOT"] = "OPERACIÓN NO SUJETA A PAGO DE SPOT";

                }
    
                dt.Rows.Add(_ravi);
                }
            }


            Microsoft.Reporting.WebForms.LocalReport localreport = new LocalReport();
            ReportDataSource reportdatasource1 = new ReportDataSource("dsMaestroDetalle");
            reportdatasource1.Name = "dsMaestroDetalle";
            if (empty == 0 && type == 1) //type =1 guia
            {
                localreport.ReportPath = Server.MapPath("~/Reports/Guia.rdlc");
            }
            else if (empty == 1 && type == 1)
            {
                localreport.ReportPath = Server.MapPath("~/Reports/GuiaEmpty.rdlc");
            }
            else if (empty == 0 && type == 2)
            {
                localreport.ReportPath = Server.MapPath("~/Reports/Factura.rdlc");
            }
            else if (empty == 0 && type == 3)
            {
                localreport.ReportPath = Server.MapPath("~/Reports/Boleta.rdlc");
            }

            reportdatasource1.Value = dt;
            localreport.DataSources.Add(reportdatasource1);

            string reportType = "pdf";
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;
            string mimeType;
            renderedBytes = localreport.Render(reportType, null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            return File(renderedBytes, mimeType);
        }


    }
}
