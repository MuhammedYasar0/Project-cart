using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectCar.Data;
using ProjectCar.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectCar.Controllers
{
    public class InsureeController : Controller
    {
        private readonly InsuranceDbContext _context;

        public InsureeController(InsuranceDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Insurees.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,SpeedingTickets,HasDUI,IsFullCoverage")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                insuree.Quote = CalculateQuote(insuree);
                _context.Add(insuree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(insuree);
        }

        private decimal CalculateQuote(Insuree insuree)
        {
            decimal quote = 50.0M; // Base quote

            // Calculate age
            int age = DateTime.Now.Year - insuree.DateOfBirth.Year;
            if (DateTime.Now.DayOfYear < insuree.DateOfBirth.DayOfYear)
                age--;

            // Age-based calculations
            if (age <= 18)
                quote += 100;
            else if (age <= 25)
                quote += 50;
            else
                quote += 25;

            // Car year calculations
            if (insuree.CarYear < 2000)
                quote += 25;
            if (insuree.CarYear > 2015)
                quote += 25;

            // Porsche calculations
            if (insuree.CarMake.ToLower() == "porsche")
            {
                quote += 25;
                if (insuree.CarModel.ToLower() == "911 carrera")
                    quote += 25;
            }

            // Speeding ticket calculations
            quote += 10 * insuree.SpeedingTickets;

            // DUI calculation
            if (insuree.HasDUI)
                quote *= 1.25M;

            // Full coverage calculation
            if (insuree.IsFullCoverage)
                quote *= 1.50M;

            return quote;
        }

        public async Task<IActionResult> Admin()
        {
            return View(await _context.Insurees.ToListAsync());
        }
    }
} 