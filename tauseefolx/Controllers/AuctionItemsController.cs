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
    public class AuctionItemsController : Controller
    {
        private olxappEntities db = new olxappEntities();

        // GET: AuctionItems
        public ActionResult Index()
        {
            return View(db.AuctionItems.ToList());
        }

        // GET: AuctionItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItem = db.AuctionItems.Find(id);
            if (auctionItem == null)
            {
                return HttpNotFound();
            }
            return View(auctionItem);
        }

        // GET: AuctionItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuctionItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuctionItemid,SellItemid,Buyerid,Auctionprice,SoldStatus,Createddate")] AuctionItem auctionItem)
        {
            if (ModelState.IsValid)
            {
                db.AuctionItems.Add(auctionItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auctionItem);
        }

        // GET: AuctionItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItem = db.AuctionItems.Find(id);
            if (auctionItem == null)
            {
                return HttpNotFound();
            }
            return View(auctionItem);
        }

        // POST: AuctionItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuctionItemid,SellItemid,Buyerid,Auctionprice,SoldStatus,Createddate")] AuctionItem auctionItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auctionItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auctionItem);
        }

        // GET: AuctionItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItem = db.AuctionItems.Find(id);
            if (auctionItem == null)
            {
                return HttpNotFound();
            }
            return View(auctionItem);
        }

        // POST: AuctionItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuctionItem auctionItem = db.AuctionItems.Find(id);
            db.AuctionItems.Remove(auctionItem);
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
