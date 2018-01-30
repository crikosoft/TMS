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
        public ActionResult Create([Bind(Include = "guiaSalidaId,serie,ordenId,numero,motivoTrasladoId,motivoDetalle,fechaTraslado")] GuiaSalida guiasalida)
        {
            if (ModelState.IsValid)
            {

                //Actualiza estados de Ordenes
                var estadoEnvioGuia = db.EstadosOrden.Single(p => p.nombre == "Con Guía");
                var orden = db.Ordenes.Find(guiasalida.ordenId);
                orden.estadoOrdenId = estadoEnvioGuia.estadoOrdenId;
                guiasalida.Orden = orden;
               

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
        public ActionResult Edit([Bind(Include="guiaSalidaId,serie,numero,ordenId,motivoTrasladoId,motivoDetalle,fechaTraslado")] GuiaSalida guiasalida)
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


            foreach (var item in guia[0].Orden.Detalles.OrderBy(a => a.Producto.nombre))
            {
                DataRow _ravi = dt.NewRow();
                _ravi["Serie"] = guia[0].serie;
                _ravi["Numero"] = guia[0].numero;
                //_ravi["FechaEmision"] = ((DateTime)(guia[0].Orden.Envios.FirstOrDefault().fechaCreacion)).ToShortDateString();
                _ravi["FechaTraslado"] = guia[0].Orden.Envios.FirstOrDefault().fechaTraslado.ToShortDateString();
                _ravi["Remitente"] = guia[0].Orden.ClienteOrigen.razonSocial;
                _ravi["Destinatario"] = guia[0].Orden.ClienteDestinatario.razonSocial;
                _ravi["direccionRemitente"] = guia[0].Orden.ClienteOrigen.Direcciones.FirstOrDefault().Direccion.descripcion;
                _ravi["direccionDestinatario"] = guia[0].Orden.ClienteDestinatario.Direcciones.FirstOrDefault().Direccion.descripcion;
                _ravi["ciudadRemitente"] = guia[0].Orden.DireccionOrigen.Ubigeo.descripcion;
                _ravi["ciudadDestinatario"] = guia[0].Orden.DireccionDestino.Ubigeo.descripcion;
                _ravi["RUCRemitente"] = guia[0].Orden.ClienteOrigen.ruc;
                _ravi["RUCDestinatario"] = guia[0].Orden.ClienteDestinatario.ruc;
                _ravi["PuntoPartida"] = guia[0].Orden.DireccionOrigen.descripcion;
                _ravi["PuntoLlegada"] = guia[0].Orden.DireccionDestino.descripcion;
                _ravi["Cod"] = item.Producto.productoId;
                _ravi["Cantidad"] = item.cantidad;
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
                //_ravi["SubTotal"] = string.Format("{0:0,0.00}", guia[0].Orden.subTotal);
                //_ravi["IGV"] = string.Format("{0:0,0.00}", guia[0].Orden.igv);

                //_ravi["Total"] = string.Format("{0:0,0.00}", guia[0].Orden.Total);
                //_ravi["FechaF"] = "";//"Lima, " + guia[0].fecha.Day + " de " + guia[0].fecha.ToString("MMMM").ToUpper() + "  del " + guia[0].fecha.Year;

               
                //string fechaTraslado = "";
                ////fechaTraslado = "Lima, " + ((DateTime)guia[0].Orden.fechaEntrega).Day + " de Agosto del" + guia[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.Year;

                //_ravi["SerieG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().serie;
                //_ravi["NumeroG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().numero;
                ////_ravi["FechaG"] = guia[0].Orden.GuiasSalida.SingleOrDefault().fechaTraslado.ToString("dd/MM/yyyy").ToUpper();
                ////_ravi["MotivoTralado"] = guia[0].Orden.GuiasSalida.SingleOrDefault().MotivoTraslado.nombre;


                ////_ravi["PuntoDestinoG"] = guia[0].Orden.Cliente.direccion;
                //_ravi["PuntoPartidaG"] = "CALLE AURELIO FERNANDEZ CONCHA 298-MIRAFLORES";
                //_ravi["Cantidad"] = item.cantidad;
                //_ravi["DescripcionP"] = item.Producto.nombre;
                //_ravi["PrecioUnitario"] = string.Format("{0:0,0.00}", item.precioUnitario);
                //_ravi["PrecioTotal"] = string.Format("{0:0,0.00}", item.precioTotal);
                //_ravi["TotalTexto"] = "CIENTO OCHENTA Y OCHO y 10/100";
                //_ravi["UnidadMedida"] = item.Producto.UnidadMedida.nombre;
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

    }
}
