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
    public class CheckingTransactionsController : Controller
    {
        private BankDBContext db = new BankDBContext();

        // GET: CheckingTransactions
        public ActionResult Index(int? id)
        {
            var checkingTransaction = db.CheckingTransaction.Include(b => b.Account);
            ViewBag.CustomerId = checkingTransaction.First().CustomerId;
            return View(checkingTransaction.ToList().Where(b => (b.AccountId == id)));
        }

        // GET: CheckingTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingTransaction checkingTransaction = db.CheckingTransaction.Find(id);
            if (checkingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(checkingTransaction);
        }

        public CheckingTransaction CreateTransaction(int? customerId, int? accountId, double amount, string type)
        {
            CheckingTransaction checkingTransaction = new CheckingTransaction
            {
                DateOfTransaction = DateTime.Now,
                CustomerId = (int)customerId,
                AccountId = (int)accountId,
                Amount = (double)amount,
                Type = type
            };
            return checkingTransaction;
        }

        // GET: CheckingTransactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.CheckingAccounts, "Id", "Id");
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            return View();
        }

        // POST: CheckingTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,CustomerId,DateOfTransaction,Amount")] CheckingTransaction checkingTransaction)
        {
            if (ModelState.IsValid)
            {
                db.CheckingTransaction.Add(checkingTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.CheckingAccounts, "Id", "Id", checkingTransaction.AccountId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingTransaction.CustomerId);
            return View(checkingTransaction);
        }

        // GET: CheckingTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingTransaction checkingTransaction = db.CheckingTransaction.Find(id);
            if (checkingTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.CheckingAccounts, "Id", "Id", checkingTransaction.AccountId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingTransaction.CustomerId);
            return View(checkingTransaction);
        }

        // POST: CheckingTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,CustomerId,DateOfTransaction,Amount")] CheckingTransaction checkingTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkingTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.CheckingAccounts, "Id", "Id", checkingTransaction.AccountId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingTransaction.CustomerId);
            return View(checkingTransaction);
        }

        // GET: CheckingTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingTransaction checkingTransaction = db.CheckingTransaction.Find(id);
            if (checkingTransaction == null)
            {
                return HttpNotFound();
            }
            return View(checkingTransaction);
        }

        // POST: CheckingTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckingTransaction checkingTransaction = db.CheckingTransaction.Find(id);
            db.CheckingTransaction.Remove(checkingTransaction);
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

        //Returns the view of the CheckingAccounts for the given customerId
        public ActionResult BackToAccounts(int id)
        {
            CheckingAccount ca = new CheckingAccount();
            ca.CustomerId = (int)id;
            return RedirectToAction("Index", "CheckingAccounts", new { id = ca.CustomerId });
        }

    }
}
