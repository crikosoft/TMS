using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.Models;

namespace TransporteCarga.Controllers
{
    public class CalendarController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();
        //
        // GET: /Calendar/
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetEvents()
        {
            //var results = db.Database.SqlQuery<Calendar>("select fecha2, cantidad, monto from Points").ToArray();
            var guias = db.GuiaSalidas.Where(a=>a.fechaEmision != null);
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                Request.ApplicationPath.TrimEnd('/') + "/";

            List<Calendar> events = new List<Calendar>();
            foreach (var _event in guias)
            {

                Calendar evento = new Calendar();
                evento.title = _event.numero;
                evento.start = _event.fechaEmision.ToString();
                //evento.end = _event.end;
                evento.allDay = true;
                evento.url = baseUrl + "GuiaSalida/Document/"+ _event.guiaSalidaId;
                events.Add(evento);
            }

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
	}
}