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
    public class SellItemsController : Controller
    {
        private olxappEntities db = new olxappEntities();

        // GET: SellItems
        public ActionResult Index()
        {
            return View(db.SellItems.ToList());
        }

        // GET: SellItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellItem sellItem = db.SellItems.Find(id);
            if (sellItem == null)
            {
                return HttpNotFound();
            }
            return View(sellItem);
        }

        // GET: SellItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SellItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SellItemid,Usertableid,Productname,Productdescription,Productimageone,Productimagetwo,Productimagethree,Productimagefour,Price,Createddate,Sold,LowestBiddingPrice,Auctiondate,AuctionPlace,AuctionZipcode,Auctionaddress")] SellItem sellItem)
        {
            if (ModelState.IsValid)
            {
                db.SellItems.Add(sellItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sellItem);
        }

        // GET: SellItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellItem sellItem = db.SellItems.Find(id);
            if (sellItem == null)
            {
                return HttpNotFound();
            }
            return View(sellItem);
        }

        // POST: SellItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SellItemid,Usertableid,Productname,Productdescription,Productimageone,Productimagetwo,Productimagethree,Productimagefour,Price,Createddate,Sold,LowestBiddingPrice,Auctiondate,AuctionPlace,AuctionZipcode,Auctionaddress")] SellItem sellItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sellItem);
        }

        // GET: SellItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SellItem sellItem = db.SellItems.Find(id);
            if (sellItem == null)
            {
                return HttpNotFound();
            }
            return View(sellItem);
        }

        // POST: SellItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SellItem sellItem = db.SellItems.Find(id);
            db.SellItems.Remove(sellItem);
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
