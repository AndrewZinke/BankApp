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
    public class TermDepositsController : Controller
    {
        private BankDBContext db = new BankDBContext();

        // GET: TermDeposits
        public ActionResult Index(int? id)
        {
            ViewBag.CustomerId = id;
            return View(db.TermDeposits.ToList().Where(td => td.CustomerId == id));
        }

        // GET: TermDeposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermDeposit termDeposit = db.TermDeposits.Find(id);
            if (termDeposit == null)
            {
                return HttpNotFound();
            }
            return View(termDeposit);
        }

        // GET: TermDeposits/Create
        public ActionResult Create(int? id)
        {
           // ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName");
            var termDeposit = new TermDeposit();
            termDeposit.CustomerId = (int) id;
            return View(termDeposit);
        }

        // POST: TermDeposits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( TermDeposit termDeposit)
        {
            if (ModelState.IsValid)
            {
                termDeposit.DateOpened = DateTime.Now;
                db.TermDeposits.Add(termDeposit);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = termDeposit.CustomerId });
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "FirstName", termDeposit.CustomerId);
            return View(termDeposit);
        }

        // GET: TermDeposits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermDeposit termDeposit = db.TermDeposits.Find(id);
            if (termDeposit == null)
            {
                return HttpNotFound();
            }
            return View(termDeposit);
        }

        // POST: TermDeposits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DateOpened,Amount,InterestRate")] TermDeposit termDeposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(termDeposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(termDeposit);
        }

        // GET: TermDeposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TermDeposit termDeposit = db.TermDeposits.Find(id);
            if (termDeposit == null)
            {
                return HttpNotFound();
            }
            return View(termDeposit);
        }

        // POST: TermDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TermDeposit termDeposit = db.TermDeposits.Find(id);
            db.TermDeposits.Remove(termDeposit);
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

        public ActionResult CalculateInterest(int? id)
        {
            TermDeposit td = db.TermDeposits.Find(id);
            double daysOpen = (DateTime.Now - td.DateOpened).TotalDays;
            {
                if(daysOpen < 90 )
                {
                    td.InterestRate = .01;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                    td.Amount *= td.InterestRate;
                    return View(td);
                }
                else if(daysOpen < 90 && daysOpen < 360)
                {
                    td.InterestRate = .03;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                    td.Amount *= td.InterestRate;
                    return View(td);
                }
                else if(daysOpen >360 && daysOpen < 700)
                {
                    td.InterestRate = .05;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                    td.Amount *= td.InterestRate;
                    return View(td);
                }
                else
                {
                    td.InterestRate = .1;
                    td.HasMatured = true;
                    db.Entry(td).State = EntityState.Modified;
                    db.SaveChanges();
                    td.Amount *= td.InterestRate;
                    return View(td);
                }
            }
                                 
        }// end of CalculateInterest Method

        //withdraw GET
        public ActionResult Withdraw(int? id)
        {
            TermDeposit td = db.TermDeposits.Find(id);
            TempData["tdAmount"] = td.Amount;
            return View(td);
        } //end of Withdraw

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Withdraw(TermDeposit termDeposit)
        {
            if(ModelState.IsValid)
            {
                if(termDeposit.HasMatured)
                {
                    double withDrawAmount = termDeposit.Amount;
                    termDeposit.Amount = (double) TempData["tdAmount"];
                    if (withDrawAmount > termDeposit.Amount)
                    {
                        return RedirectToAction("Index", new { id = termDeposit.CustomerId });
                    }
                    else
                    {
                        termDeposit.Amount -= withDrawAmount;
                        db.Entry(termDeposit).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", new { id = termDeposit.CustomerId });
                    }
                }
            }
            return RedirectToAction("Index", new { id = termDeposit.CustomerId });
        }

        public ActionResult BackToCustomer(int? id)
        {
            Customer c = new Customer();
            c.Id = (int)id;
            return RedirectToAction("Details", "Customers", new { id = c.Id });
        }

    }//end of controller
}//end of namespace
