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
    public class CheckingAccountsController : Controller
    { 

        private BankDBContext db = new BankDBContext();

        // GET: CheckingAccounts
        public ActionResult Index(int? id)
        {
            ViewBag.CustomerId = id;
            var checkingAccounts = db.CheckingAccounts.Include(b => b.Customer);
            return View(checkingAccounts.ToList().Where(b => (b.CustomerId == id && b.Active == true)));
        }

        public ActionResult ViewTransactions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "CheckingTransactions", checkingAccount);
            }


        public ActionResult Withdraw(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            TempData["CheckingBalance"] = checkingAccount.Balance;
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        //Post for Withdraw
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                CheckingTransactionsController c = new CheckingTransactionsController();
                CheckingTransaction transaction;

                var withdrawAmount = checkingAccount.Balance;
                checkingAccount.Balance = (double)TempData["CheckingBalance"];
                if(withdrawAmount > checkingAccount.Balance)
                {
                    return RedirectToAction("WithdrawDenied", new { id = checkingAccount.CustomerId });
                }
                else
                {
                    checkingAccount.Balance -= withdrawAmount;
                    transaction = c.CreateTransaction(checkingAccount.CustomerId, checkingAccount.Id, withdrawAmount,"Withdraw");
                    db.CheckingTransaction.Add(transaction);
                    db.Entry(checkingAccount).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
        }




        //GET deposit
        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            TempData["AccountBalance"] = checkingAccount.Balance;
            TempData["TempBalance"] = 0.00;
            checkingAccount.Balance = (double)TempData["TempBalance"];
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        //Post for Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                CheckingTransactionsController c = new CheckingTransactionsController();
                CheckingTransaction transaction;
                double depositAmount = checkingAccount.Balance;
                checkingAccount.Balance += (double) TempData["AccountBalance"];
                transaction = c.CreateTransaction(checkingAccount.CustomerId, checkingAccount.Id, depositAmount,"Deposit");
                db.CheckingTransaction.Add(transaction);

                db.Entry(checkingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount.CustomerId);
        }

        // GET: CheckingAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        // GET: CheckingAccount/Create
        //create an account object and pass it the id parameter
        public ActionResult Create(int? id)
        {
            //ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            var account = new CheckingAccount();
            account.CustomerId = (int)id;

            return View(account);
        }

        // POST: CheckingAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                checkingAccount.Active = true;
                db.CheckingAccounts.Add(checkingAccount);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        // GET: CheckingAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        // POST: CheckingAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CheckingAccount checkingAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkingAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", checkingAccount.CustomerId);
            return View(checkingAccount);
        }

        // GET: CheckingAccount/Close/5
        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            if (checkingAccount == null)
            {
                return HttpNotFound();
            }
            return View(checkingAccount);
        }

        // POST: CheckingAccounts/Delete/5
        //Sets the status checkingAccount with the given id to closed, so that it will not show up
        //in the index view
        [HttpPost, ActionName("Close")]
        [ValidateAntiForgeryToken]
        public ActionResult Close(int id)
        {
            CheckingAccount checkingAccount = db.CheckingAccounts.Find(id);
            checkingAccount.Active = false;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = checkingAccount.CustomerId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //returns View that shows potential interest of the account with the given id
        public ActionResult CalculateInterest(int? id)
        {
            CheckingAccount account = db.CheckingAccounts.Find(id);
            account.Balance *= CheckingAccount.Interest;
            return View(account);
        }

        //returns Details view of customer with given id
        public ActionResult BackToCustomer(int? id)
        {
            Customer c = new Customer();
            c.Id = (int)id;
            return RedirectToAction("Details", "Customers", new { id = c.Id });
        }

        //redirects to WithdrawDenied View
        public ActionResult WithdrawDenied(int? id)
        {
            var c = new Customer();
            c.Id =(int) id;
            return View(c);            
        }


    }
}
