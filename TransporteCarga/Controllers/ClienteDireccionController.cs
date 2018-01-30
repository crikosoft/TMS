using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransporteCarga.Models;

namespace TransporteCarga.Controllers
{
    public class ClienteDireccionController : Controller
    {
        private TransporteCargaContext db = new TransporteCargaContext();

        // GET: /ClienteDireccion/
        public ActionResult Index()
        {
            var clientedireccion = db.ClienteDireccion.Include(c => c.Cliente).Include(c => c.Direccion).Include(c => c.TipoDireccion);
            return View(clientedireccion.ToList());
        }

        // GET: /ClienteDireccion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClienteDireccion clientedireccion = db.ClienteDireccion.Find(id);
            if (clientedireccion == null)
            {
                return HttpNotFound();
            }
            return View(clientedireccion);
        }

        // GET: /ClienteDireccion/Create
        public ActionResult Create()
        {
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial");
            ViewBag.direccionId = new SelectList(db.Direcciones, "direccionId", "descripcion");
            ViewBag.tipoDireccionId = new SelectList(db.TipoDireccion, "tipoDireccionId", "nombre");
            return View();
        }

        // POST: /ClienteDireccion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="clienteDireccionId,clienteId,direccionId,tipoDireccionId,fechaInicio")] ClienteDireccion clientedireccion)
        {
            if (ModelState.IsValid)
            {
                db.ClienteDireccion.Add(clientedireccion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", clientedireccion.clienteId);
            ViewBag.direccionId = new SelectList(db.Direcciones, "direccionId", "descripcion", clientedireccion.direccionId);
            ViewBag.tipoDireccionId = new SelectList(db.TipoDireccion, "tipoDireccionId", "nombre", clientedireccion.tipoDireccionId);
            return View(clientedireccion);
        }

        // GET: /ClienteDireccion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClienteDireccion clientedireccion = db.ClienteDireccion.Find(id);
            if (clientedireccion == null)
            {
                return HttpNotFound();
            }
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", clientedireccion.clienteId);
            ViewBag.direccionId = new SelectList(db.Direcciones, "direccionId", "descripcion", clientedireccion.direccionId);
            ViewBag.tipoDireccionId = new SelectList(db.TipoDireccion, "tipoDireccionId", "nombre", clientedireccion.tipoDireccionId);
            return View(clientedireccion);
        }

        // POST: /ClienteDireccion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="clienteDireccionId,clienteId,direccionId,tipoDireccionId,fechaInicio")] ClienteDireccion clientedireccion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientedireccion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.clienteId = new SelectList(db.Clientes.OrderBy(a => a.razonSocial), "clienteid", "razonSocial", clientedireccion.clienteId);
            ViewBag.direccionId = new SelectList(db.Direcciones, "direccionId", "descripcion", clientedireccion.direccionId);
            ViewBag.tipoDireccionId = new SelectList(db.TipoDireccion, "tipoDireccionId", "nombre", clientedireccion.tipoDireccionId);
            return View(clientedireccion);
        }

        // GET: /ClienteDireccion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClienteDireccion clientedireccion = db.ClienteDireccion.Find(id);
            if (clientedireccion == null)
            {
                return HttpNotFound();
            }
            return View(clientedireccion);
        }

        // POST: /ClienteDireccion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClienteDireccion clientedireccion = db.ClienteDireccion.Find(id);
            db.ClienteDireccion.Remove(clientedireccion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
