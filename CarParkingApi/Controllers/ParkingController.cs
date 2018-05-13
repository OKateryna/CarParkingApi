using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarParkingLibrary;

namespace CarParkingApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Parking")]
    public class ParkingController : Controller
    {
        private readonly CarParkingService carParkingService;
        public ParkingController()
        {
            carParkingService = new CarParkingService();
        }

        // GET: api/Parking/freeParkingCount
        [HttpGet("freeParkingCount")]
        public int GetFreeParkingCount()
        {
            return carParkingService.GetFreeParkingCount();
        }

        // GET: api/Parking/occupiedParkingCount
        [HttpGet("occupiedParkingCount")]
        public int GetOccupiedParkingCount()
        {
            return carParkingService.GetOccupiedParkingCount();
        }

        // GET: api/Parking/totalParkingEarnings
        [HttpGet("totalParkingEarnings")]
        public double GetTotalParkingEarnings()
        {
            return carParkingService.GetTotalParkingEarnings();
        }
      
    }
}
