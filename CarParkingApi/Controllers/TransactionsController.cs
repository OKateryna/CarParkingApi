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
    [Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        private readonly CarParkingService carParkingService;
        public TransactionsController()
        {
            carParkingService = new CarParkingService();
        }
        // GET: api/Transactions/transactionLog
        [HttpGet("transactionLog")]
        public string GetTransactionLog()
        { 
            return carParkingService.GetTransactionLog();
        }

        // GET: api/Transactions/transactionsForLastMinute
        [HttpGet("transactionsForLastMinute")]
        public IEnumerable<Transaction> GetTransactionsForLastMinute()
        {
            return carParkingService.GetTransactionsForLastMinute();
        }

        // GET: api/Transactions/lastMinuteTransactionsForCar/1
        [HttpGet("lastMinuteTransactionsForCar/{id}")]
        public IEnumerable<Transaction> GetLastMinuteTransactionsForCar(int id)
        {
            return carParkingService.GetLastMinuteTransactionsForCar(id);
        }

        // PUT: api/Transactions/carBalance/5
        [HttpPut("carBalance/{id}")]
        public IActionResult PutCarBalance(int id, [FromBody]double balance)
        {
            bool result = carParkingService.PutCarBalance(id, balance);
            if (!result)
            {
                return BadRequest($"Could not find car with Id: {id}");
            }

            return Ok("Successfully added balance");
        }
      
    }
}
