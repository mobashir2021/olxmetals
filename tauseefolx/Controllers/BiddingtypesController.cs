using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using tauseefolx.Models;

namespace tauseefolx.Controllers
{
    public class BiddingtypesController : Controller
    {
        private olxappEntities db = new olxappEntities();

        // GET: Biddingtypes
        public ActionResult Index()
        {
            return View(db.Biddingtypes.ToList());
        }

        // GET: Biddingtypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biddingtype biddingtype = db.Biddingtypes.Find(id);
            if (biddingtype == null)
            {
                return HttpNotFound();
            }
            return View(biddingtype);
        }

        // GET: Biddingtypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Biddingtypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Biddingtypeid,Biddingtype1")] Biddingtype biddingtype)
        {
            if (ModelState.IsValid)
            {
                db.Biddingtypes.Add(biddingtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(biddingtype);
        }

        // GET: Biddingtypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biddingtype biddingtype = db.Biddingtypes.Find(id);
            if (biddingtype == null)
            {
                return HttpNotFound();
            }
            return View(biddingtype);
        }

        // POST: Biddingtypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Biddingtypeid,Biddingtype1")] Biddingtype biddingtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(biddingtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(biddingtype);
        }

        // GET: Biddingtypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Biddingtype biddingtype = db.Biddingtypes.Find(id);
            if (biddingtype == null)
            {
                return HttpNotFound();
            }
            return View(biddingtype);
        }

        // POST: Biddingtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Biddingtype biddingtype = db.Biddingtypes.Find(id);
            db.Biddingtypes.Remove(biddingtype);
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
