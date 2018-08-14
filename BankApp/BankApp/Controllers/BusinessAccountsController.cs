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
    public class BusinessAccountsController : Controller
    {
        private BankDBContext db = new BankDBContext();

        // GET: BusinessAccounts
        public ActionResult Index(int? id)
        {
            ViewBag.CustomerId = id;
            var businessAccounts = db.BusinessAccounts.Include(b => b.Customer);
            return View(businessAccounts.ToList().Where(b => (b.CustomerId == id && b.Active == true)));
        }

        public ActionResult ViewTransactions(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index", "BusinessTransactions", businessAccount);
            //return View("~/Views/BusinessAccounts/Index.cshtml");
        }


        public ActionResult Withdraw(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            TempData["AccountBalance"] = businessAccount.Balance;
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        //Post for Withdraw
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(BusinessAccount businessAccount)
        {
            if (ModelState.IsValid)
            {
                BusinessTransactionsController c = new BusinessTransactionsController();
                BusinessTransaction transaction;

                double withDrawAmount = businessAccount.Balance;
                businessAccount.Balance = (double)TempData["AccountBalance"];
                double negativeAmount;
                if(businessAccount.Balance - withDrawAmount < -100)
                {
                    ViewBag.WithdrawDenied = "You have attempted to withdraw over your limit";
                    return RedirectToAction("Index", new { id = businessAccount.CustomerId });
                }
                else if (businessAccount.Balance - withDrawAmount < 0)
                {
                    negativeAmount = businessAccount.Balance - withDrawAmount;
                    negativeAmount += negativeAmount * BusinessAccount.OverdraftFee;
                    businessAccount.Balance = negativeAmount;
                    db.Entry(businessAccount).State = EntityState.Modified;
                    transaction = c.CreateTransaction(businessAccount.CustomerId,businessAccount.Id,-withDrawAmount, "Withdraw");
                    db.BusinessTransactions.Add(transaction);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = businessAccount.CustomerId });
                }
                else
                {
                    businessAccount.Balance -= withDrawAmount;
                    db.Entry(businessAccount).State = EntityState.Modified;
                    transaction = c.CreateTransaction(businessAccount.CustomerId, businessAccount.Id, withDrawAmount, "Withdraw");
                    db.BusinessTransactions.Add(transaction);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = businessAccount.CustomerId });
                }

                
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount.CustomerId);
        }




        //GET deposit
        public ActionResult Deposit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            TempData["AccountBalance"] = businessAccount.Balance;
            TempData["TempBalance"] = 0.00;
            businessAccount.Balance = (double) TempData["TempBalance"];
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        //Post for Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deposit(BusinessAccount businessAccount)
        {
            if (ModelState.IsValid)
            {
                BusinessTransactionsController c = new BusinessTransactionsController();
                BusinessTransaction transaction;
       
                double depositAmount = businessAccount.Balance;
                businessAccount.Balance = (double)TempData["AccountBalance"];
                businessAccount.Balance += depositAmount;

                db.Entry(businessAccount).State = EntityState.Modified;
                transaction = c.CreateTransaction(businessAccount.CustomerId, businessAccount.Id, depositAmount, "Deposit");
                db.BusinessTransactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = businessAccount.CustomerId });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount.CustomerId);
        }

        // GET: BusinessAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        // GET: BusinessAccounts/Create
        //create an account object and pass it the id parameter
        public ActionResult Create(int? id)
        {
            //ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            var account = new BusinessAccount();
            account.CustomerId = (int)id;

            return View(account);
        }

        // POST: BusinessAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BusinessAccount businessAccount)
        {
            if (ModelState.IsValid)
            {
                businessAccount.Active = true;
                db.BusinessAccounts.Add(businessAccount);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = businessAccount.CustomerId });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        // GET: BusinessAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        // POST: BusinessAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BusinessAccount businessAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(businessAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = businessAccount.CustomerId });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", businessAccount.CustomerId);
            return View(businessAccount);
        }

        // GET: BusinessAccounts/Close/5
        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            if (businessAccount == null)
            {
                return HttpNotFound();
            }
            return View(businessAccount);
        }

        // POST: BusinessAccounts/Delete/5
        [HttpPost, ActionName("Close")]
        [ValidateAntiForgeryToken]
        public ActionResult Close(int id)
        {
            BusinessAccount businessAccount = db.BusinessAccounts.Find(id);
            businessAccount.Active = false;
            db.SaveChanges();
            return RedirectToAction("Index", new { id = businessAccount.CustomerId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult BackToCustomer(int? id)
        {
            Customer c = new Customer();
            c.Id = (int) id;
            return RedirectToAction("Details", "Customers", new { id = c.Id });
        }

    }
}
