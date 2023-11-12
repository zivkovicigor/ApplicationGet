using ApplicationGet.Data;
using ApplicationGet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ApplicationGet.Controllers
{
    public class ReservationController : Controller
    {
        private 
            ApplicationDBContext _db;

        public ReservationController(ApplicationDBContext dB)
        {
            _db = dB;
        }

        [Authorize(Roles = "agent")]
        public IActionResult Index()
        {
            IEnumerable<Reservation> objReservationtList = _db.Reservations.Include(r => r.User)
                .Include(r => r.Flight).ThenInclude(f => f.FromCity).
                Include(r => r.Flight).ThenInclude(f => f.ToCity).Where(r => r.Status == "Pending");
            return View(objReservationtList);
        }

        //GET
        [Authorize(Roles = "visitor")]
        public IActionResult IndexFlights ([FromQuery(Name = "fromCity.Id")]int? fromCityId, [FromQuery(Name = "toCity.Id")] int? toCityId, bool withoutLayovers)
        {
            FlightsCities fc = new FlightsCities();

            if (fromCityId == null && toCityId == null)
            {
                fc.ListOfFlights = _db.Flights.Include(f => f.FromCity).Include(f => f.ToCity).Where(f => f.Status == "Active" && f.Date >= DateTime.Now);
                fc.ListOfCities = _db.Cities;
                fc.FromCity = new City();
                fc.ToCity = new City();
            }
            else
            {
                if (withoutLayovers) fc.ListOfFlights = _db.Flights.Include(f => f.FromCity).Include(f => f.ToCity).Where(f => f.FromCity.Id == fromCityId && f.ToCity.Id == toCityId && f.NumberOfLayovers == 0 && f.NumberOfSeats > 0 && f.Status == "Active" && f.Date >= DateTime.Now);
                else fc.ListOfFlights = _db.Flights.Include(f => f.FromCity).Include(f => f.ToCity).Where(f => f.FromCity.Id == fromCityId && f.ToCity.Id == toCityId && f.NumberOfSeats > 0 && f.Status == "Active" && f.Date >= DateTime.Now);

                fc.ListOfCities = _db.Cities;
                fc.FromCity = new City();
                fc.ToCity = new City();
            }

            return View(fc);
        }

        [Authorize(Roles = "visitor")]
        public IActionResult IndexUser()
        {
            User user = _db.Users.Find(User.Claims.FirstOrDefault().Value);
            IEnumerable<Reservation> objReservationtList = _db.Reservations.Include(r => r.User).Where(r=>r.User.Id == user.Id)
                .Include(r => r.Flight).ThenInclude(f => f.FromCity).
                Include(r => r.Flight).ThenInclude(f => f.ToCity);

            return View(objReservationtList);
        }

        //GET
        [Authorize(Roles = "visitor")]
        public IActionResult Create(int? id)
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

            FlightReservation fr = new FlightReservation();
            fr.Reservation = new Reservation();
            fr.Flight = f;

            return View(fr);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "visitor")]
      public IActionResult Create(FlightReservation fr)
        {
            fr.Reservation.User = _db.Users.Find(User.Claims.FirstOrDefault().Value);
            fr.Reservation.Flight = _db.Flights.Find(fr.Flight.Id);
            fr.Reservation.Flight.FromCity = _db.Cities.Find(fr.Reservation.Flight.FromCityId);
            fr.Reservation.Flight.ToCity = _db.Cities.Find(fr.Reservation.Flight.ToCityId);

            if (ModelState.IsValid)
            {
                if (fr.Reservation.NumberOfSeats <= 0)
                {
                    ModelState.AddModelError("CustomError", "Number of seats cannot be less then or equal to zero.");
                }

                if (fr.Reservation.Flight.NumberOfSeats - fr.Reservation.NumberOfSeats < 0)
                {
                    ModelState.AddModelError("CustomError", "There is not enough seats on the flight.");
                }

                if (fr.Reservation.Flight.Date.AddDays(-3) < DateTime.Now.Date)
                {
                    ModelState.AddModelError("CustomError", "Flight is in less than tree days, reservation cannot be made.");
                }

                if (ModelState.ErrorCount > 0)
                    return View();

                IEnumerable<Reservation> listOfReservationsForUser = _db.Reservations.
                    Where(r => r.UserId == fr.Reservation.User.Id);

                if (!listOfReservationsForUser.Any<Reservation>())
                {
                    fr.Reservation.Id = 1;
                }
                else
                {
                    fr.Reservation.Id = listOfReservationsForUser.Count() + 1;
                }

                _db.Reservations.Add(fr.Reservation);
                _db.SaveChanges();

                return RedirectToAction("IndexUser");
            }

             return View(fr);
        }

        //[HttpGet]
        ////[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "visitor")]
        //public Reservation CreateRes([Bind("id", "numberOfSeats")] int id, int numberOfSeats)
        //{
        //    Reservation r = new Reservation();
        //    r.User = _db.Users.Find(User.Claims.FirstOrDefault().Value);
        //    r.Flight = _db.Flights.Find(id);
        //    r.Flight.FromCity = _db.Cities.Find(r.Flight.FromCityId);
        //    r.NumberOfSeats = numberOfSeats;

        //    IEnumerable<Reservation> listOfReservationsForUser = _db.Reservations.Where(res => res.UserId == r.User.Id);
        //    if (!listOfReservationsForUser.Any<Reservation>())
        //    {
        //        r.Id = 1;
        //    }
        //    else
        //    {
        //        r.Id = listOfReservationsForUser.Count() + 1;
        //    }

        //    _db.Reservations.Add(r);
        //    _db.SaveChanges();
        //    return r;
        //}

        //GET
        [Authorize(Roles = "agent")]
        public IActionResult Edit(string? userID, int? id)
        {
            if (id == null || id == 0 || userID == null)
            {
                return NotFound();
            }

            IEnumerable<Reservation> objReservations = _db.Reservations.Include(r => r.Flight).
                ThenInclude(f => f.FromCity).Include(r => r.Flight).
                ThenInclude(f => f.ToCity).Where(x => x.UserId == userID && x.Id == id);
            Reservation r = objReservations.FirstOrDefault();
            
            return View(r);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "agent")]
        public IActionResult Edit(Reservation r)
        {
            if (r.Status != "Pending")
            {
                ModelState.AddModelError("CustomError", "Reservation is not in appropriate status.");
            }

            Flight flight = _db.Flights.Find(r.Flight.Id);

            if (flight.NumberOfSeats - r.NumberOfSeats >= 0)
            {
                flight.NumberOfSeats -= r.NumberOfSeats;
            }
            else
            {
                ModelState.AddModelError("CustomError", "There is not enough seats on the flight.");
            }

            if (ModelState.ErrorCount > 0)
                return View(r);

            r.User = _db.Users.Find(r.UserId);
            r.UserId = r.UserId;
            r.Flight.Id = flight.Id;

            _db.Reservations.Where(x => x.UserId == r.UserId && x.Id == r.Id).
                ExecuteUpdate(b => b.SetProperty(u => u.Status, "Approved"));
            _db.Flights.Where(x => x.Id == r.Flight.Id).
                ExecuteUpdate(b => b.SetProperty(u => u.NumberOfSeats, flight.NumberOfSeats));
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
