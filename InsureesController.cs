using System;
using System.Linq;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureesController : Controller
    {
        private InsuranceContext db = new InsuranceContext();

        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Insuree insuree)
        {
            decimal quote = 50;

            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear) age--;

            if (age <= 18) quote += 100;
            else if (age <= 25) quote += 50;
            else quote += 25;

            if (insuree.CarYear < 2000) quote += 25;
            if (insuree.CarYear > 2015) quote += 25;

            if (insuree.CarMake.ToLower() == "porsche")
            {
                quote += 25;
                if (insuree.CarModel.ToLower() == "911 carrera")
                    quote += 25;
            }

            quote += insuree.SpeedingTickets * 10;

            if (insuree.DUI) quote *= 1.25m;
            if (insuree.CoverageType) quote *= 1.50m;

            insuree.Quote = quote;

            db.Insurees.Add(insuree);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
