using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BankApp.Models;

namespace BankApp.Controllers
{
    [Authorize]
    public class BusinessTransactionsController : Controller
    {
        private BankDBContext db = new BankDBContext();

        // GET: BusinessTransactions
        public ActionResult Index(int? id)
        {
            var businessTransaction = db.BusinessTransactions.Include(b => b.BusinessAccount);
            ViewBag.CustomerId = businessTransaction.First().CustomerId;
            return View(businessTransaction.ToList().Where(b => (b.AccountId == id )));
        }

        // GET: BusinessTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTransaction businessTransaction = db.BusinessTransactions.Find(id);
            if (businessTransaction == null)
            {
                return HttpNotFound();
            }
            return View(businessTransaction);
        }

        // GET: BusinessTransactions/Create
        public BusinessTransaction CreateTransaction(int? customerId, int? accountId, double amount, string type)
        {
            BusinessTransaction businessTransaction = new BusinessTransaction
            {
                DateOfTransaction = DateTime.Now,
                CustomerId =(int) customerId,
                AccountId =(int) accountId,
                Amount = (double) amount,
                Type = type
            };
            return businessTransaction;
        }

        //// POST: BusinessTransactions/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,AccountId,CustomerId,DateOfTransaction,Amount")] BusinessTransaction businessTransaction)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.BusinessTransactions.Add(businessTransaction);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(businessTransaction);
        //}

        // GET: BusinessTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTransaction businessTransaction = db.BusinessTransactions.Find(id);
            if (businessTransaction == null)
            {
                return HttpNotFound();
            }
            return View(businessTransaction);
        }

        // POST: BusinessTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,CustomerId,DateOfTransaction,Amount")] BusinessTransaction businessTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(businessTransaction);
        }

        // GET: BusinessTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessTransaction businessTransaction = db.BusinessTransactions.Find(id);
            if (businessTransaction == null)
            {
                return HttpNotFound();
            }
            return View(businessTransaction);
        }

        // POST: BusinessTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusinessTransaction businessTransaction = db.BusinessTransactions.Find(id);
            db.BusinessTransactions.Remove(businessTransaction);
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

        public ActionResult BackToAccounts(int id)
        {
            BusinessAccount ba = new BusinessAccount();
            ba.CustomerId = (int)id;
            return RedirectToAction("Index", "BusinessAccounts", new { id = ba.CustomerId });
        }
    }
}
