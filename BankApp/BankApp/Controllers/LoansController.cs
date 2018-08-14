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
    public class LoansController : Controller
    {
        private BankDBContext db = new BankDBContext();

        // GET: Loans
        public ActionResult Index(int? id)
        {
            ViewBag.CustomerId = id;
            var loans = db.Loans.Include(l => l.Customer).Where(l => l.CustomerId == id && l.Active == true);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // GET: Loans/Create
        public ActionResult Create(int? id)
        {
             ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            var loan = new Loan
            {
                CustomerId = (int)id
            };
            return View(loan);
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoanId,Amount,CustomerId,Active")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                loan.Active = true;
                db.Loans.Add(loan);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = loan.CustomerId });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", loan.CustomerId);
            return View(loan);
        }
        //Get MakePayment Method
        public ActionResult MakePayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            TempData["Amount"] = loan.Amount;
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", loan.CustomerId);
            return View(loan);
        }


        //Post MakePayment Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MakePayment([Bind(Include = "LoanId,Amount,CustomerId,Active")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                double paymentAmount = loan.Amount;
                loan.Amount = (double) TempData["Amount"];
                if (paymentAmount > loan.Amount)
                {
                    return RedirectToAction("Index", new { id = loan.CustomerId });
                }
                else if(paymentAmount < loan.Amount)
                {
                    loan.Amount -= paymentAmount;
                    db.Entry(loan).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = loan.CustomerId });
                }
                else if(paymentAmount == loan.Amount)
                {
                    loan.Amount = 0;
                    loan.Active = false;
                    db.Entry(loan).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", new { id = loan.CustomerId });
                }
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", loan.CustomerId);
            return View(loan.CustomerId);
        }

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", loan.CustomerId);
            return View(loan);
        }

        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoanId,Amount,CustomerId,Active")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = loan.CustomerId });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", loan.CustomerId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = loan.CustomerId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //returns Details view of customer with given id
        public ActionResult BackToCustomer(int? id)
        {
            Customer c = new Customer();
            c.Id = (int)id;
            return RedirectToAction("Details", "Customers", new { id = c.Id });
        }

    }
}
