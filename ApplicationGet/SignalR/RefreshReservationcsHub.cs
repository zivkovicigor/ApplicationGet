using ApplicationGet.Data;
using ApplicationGet.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationGet.SignalR
{
    public class RefreshReservationcsHub : Hub
    {
        private
            ApplicationDBContext _db;

        public RefreshReservationcsHub(ApplicationDBContext dB)
        {
            _db = dB;
        }
        public async Task UpdateReservations()
        {
            IEnumerable<Reservation> objReservationtList = _db.Reservations.Include(r => r.User)
                .Include(r => r.Flight).ThenInclude(f => f.FromCity).
                Include(r => r.Flight).ThenInclude(f => f.ToCity);
            Reservation r = objReservationtList.LastOrDefault();

            await Clients.All.SendAsync("UpdatedData", r);
        }
    }
}
