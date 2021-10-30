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
    public class BuyItemsController : Controller
    {
        private olxappEntities db = new olxappEntities();

        // GET: BuyItems
        public ActionResult Index()
        {
            return View(db.BuyItems.ToList());
        }

        // GET: BuyItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyItem buyItem = db.BuyItems.Find(id);
            if (buyItem == null)
            {
                return HttpNotFound();
            }
            return View(buyItem);
        }

        // GET: BuyItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BuyItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BuyItemid,SellItemid,Buyerid,DiscountPrice,Finalprice,Boughtdate")] BuyItem buyItem)
        {
            if (ModelState.IsValid)
            {
                db.BuyItems.Add(buyItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(buyItem);
        }

        // GET: BuyItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyItem buyItem = db.BuyItems.Find(id);
            if (buyItem == null)
            {
                return HttpNotFound();
            }
            return View(buyItem);
        }

        // POST: BuyItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BuyItemid,SellItemid,Buyerid,DiscountPrice,Finalprice,Boughtdate")] BuyItem buyItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(buyItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(buyItem);
        }

        // GET: BuyItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyItem buyItem = db.BuyItems.Find(id);
            if (buyItem == null)
            {
                return HttpNotFound();
            }
            return View(buyItem);
        }

        // POST: BuyItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BuyItem buyItem = db.BuyItems.Find(id);
            db.BuyItems.Remove(buyItem);
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
