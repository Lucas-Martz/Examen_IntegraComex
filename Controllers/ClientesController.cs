using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Examen_IntegraComex.Data;
using Examen_IntegraComex.Models;

namespace Examen_IntegraComex.Controllers
{
    public class ClientesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Clientes
        public ActionResult Index(bool mostrarInactivos = false)
        {
            var query = db.Clientes.AsQueryable();

            if (!mostrarInactivos)
                query = query.Where(c => c.Activo);

            return View(query.OrderBy(c => c.IdCliente).ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        [HttpGet]
        public JsonResult GetRazonSocialByCuit(string cuit)
        {
            if (string.IsNullOrWhiteSpace(cuit) || cuit.Length != 11)
                return Json(new { ok = false, error = "CUIT inválido." }, JsonRequestBehavior.AllowGet);

            foreach (char ch in cuit)
                if (!char.IsDigit(ch))
                    return Json(new { ok = false, error = "El CUIT debe ser numérico." }, JsonRequestBehavior.AllowGet);

            try
            {
                var url = "https://sistemaintegracomex.com.ar/Account/GetNombreByCuit?cuit=" + cuit;

                using (var wc = new WebClient())
                {
                    wc.Encoding = Encoding.UTF8;
                    var raw = wc.DownloadString(url);
                    var razon = (raw ?? "").Trim().Trim('"');

                    if (string.IsNullOrWhiteSpace(razon))
                        return Json(new { ok = false, error = "No se encontró razón social para ese CUIT." }, JsonRequestBehavior.AllowGet);

                    return Json(new { ok = true, razonSocial = razon }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { ok = false, error = "No se pudo consultar el servicio." }, JsonRequestBehavior.AllowGet);
            }
        }
    
        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Cuit,RazonSocial,Telefono,Direccion,Activo")] Cliente cliente)
        {
            if (db.Clientes.Any(c => c.Cuit == cliente.Cuit))
            {
                ModelState.AddModelError("Cuit", "Ya existe un cliente con ese CUIT.");
                return View(cliente);
            }

            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Cuit,Telefono,Direccion,Activo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;

                var existente = db.Clientes.AsNoTracking().FirstOrDefault(x => x.IdCliente == cliente.IdCliente);
                if (existente == null) return HttpNotFound();

                cliente.RazonSocial = existente.RazonSocial; // conservar

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null) return HttpNotFound();

            cliente.Activo = false;
            db.Entry(cliente).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivar(int id)
        {
            var cliente = db.Clientes.Find(id);
            if (cliente == null) return HttpNotFound();

            cliente.Activo = true;
            db.Entry(cliente).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { mostrarInactivos = true });
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
