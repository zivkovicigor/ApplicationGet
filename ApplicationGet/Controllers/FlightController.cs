using ApplicationGet.Data;
using ApplicationGet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ApplicationGet.Controllers
{
    public class FlightController : Controller
    {
        private readonly ApplicationDBContext _db;
        public FlightController(ApplicationDBContext dB, UserManager<User> userManager)
        {
            _db = dB;
        }

        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            IEnumerable<Flight> objFlightList = _db.Flights.Include(f=>f.FromCity).Include(f=>f.ToCity)
                .Where(f => f.Status == "Active");
            return View(objFlightList);
        }

        [Authorize(Roles = "agent")]
        public IActionResult Overview()
        {
            IEnumerable<Flight> objFlightList = _db.Flights.Include(f => f.FromCity).Include(f => f.ToCity)
                .Where(f => f.Status == "Active");
            return View(objFlightList);
        }

        //GET
        [Authorize(Roles = "agent")]
        public IActionResult Create()
        {
            IEnumerable<City> listOfCities = _db.Cities;
            Flight f = new Flight();
            FlightCities fc = new FlightCities();
            fc.ListOfCities = listOfCities;
            fc.Flight = f;
            return View(fc);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "agent")]
        public IActionResult Create(FlightCities obj)
        {
            if (ModelState.IsValid)
            {
                obj.Flight.FromCity = _db.Cities.Find(obj.Flight.FromCityId);
                obj.Flight.ToCity = _db.Cities.Find(obj.Flight.ToCityId);
                obj.ListOfCities = _db.Cities;

                if (obj.Flight.Date < DateTime.Now)
                {
                    ModelState.AddModelError("CustomError", "Date of flight cannot be in the past.");
                }

                if (obj.Flight.NumberOfSeats <= 0)
                {
                    ModelState.AddModelError("CustomError", "Number of seats cannot be less than or equal to zero.");
                }

                if (obj.Flight.NumberOfLayovers < 0)
                {
                    ModelState.AddModelError("CustomError", "Number of layovers cannot be less than zero.");
                }

                if (obj.Flight.FromCity.Id == obj.Flight.ToCity.Id)
                {
                    ModelState.AddModelError("CustomError", "City from cannot be the same as city to.");
                }

                if (ModelState.ErrorCount > 0)
                    return View(obj);

                _db.Flights.Add(obj.Flight);
                _db.SaveChanges();
                return RedirectToAction("Overview");
            }

            return View(obj);
        }

        //GET
        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            IEnumerable<Flight> objFlightList = _db.Flights.Include(f => f.FromCity).Include(f => f.ToCity).Where(f => f.Id == id);
            Flight f = objFlightList.FirstOrDefault();

            if (f == null)
            {
                return NotFound();
            }

            return View(f);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public IActionResult Edit(Flight fl)
        {
            if (fl.Date < DateTime.Now)
            {
                ModelState.AddModelError("CustomError", "You cannot cancel flight from the past.");

            }

            if (ModelState.ErrorCount > 0)
                return View(fl);

            _db.Flights.Where(x => x.Id == fl.Id).ExecuteUpdate(b => b.SetProperty(u => u.Status, "Cancelled"));
            IEnumerable<Reservation> objReservationList = _db.Reservations.Where(f => f.Flight.Id == fl.Id);

            if (objReservationList != null)
            {
                foreach (Reservation r in objReservationList)
                {
                    _db.Reservations.Where(x => x.UserId == r.UserId && x.Id == r.Id).
                        ExecuteUpdate(b => b.SetProperty(u => u.Status, "Cancelled"));
                }
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
