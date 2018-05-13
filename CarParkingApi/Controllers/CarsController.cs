using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarParkingLibrary;
using CarParkingApi.Models;

namespace CarParkingApi.Controllers
{
    [Produces("application/json")]
    [Route("api/Cars")]
    public class CarsController : Controller
    {
        private readonly CarParkingService carParkingService;

        public CarsController()
        {
            carParkingService = new CarParkingService();
        }

        // GET: api/Cars
        [HttpGet]
        public IEnumerable<Car> Get()
        {
            IEnumerable<Car> result = carParkingService.GetAllCars();
            return result;
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public Car GetById(int id)
        {
            Car result = carParkingService.GetCarById(id);
            return result;
        }
        
        // POST: api/Cars
        [HttpPost]
        public IActionResult Post([FromBody] CarRequestModel carRequestModel)
        {
            if (carRequestModel == null)
            {
                return BadRequest("carRequestModel can`t be null");
            }

            if (!Enum.IsDefined(typeof(CarType), carRequestModel.CarType))
            {
                return BadRequest("Invalid car type, please try again.");
            }

            if (carRequestModel.Balance <= 0)
            {
                return BadRequest("Balance can not be less or equal zero.");
            }
            // returns -1 when car parking is full
            int createdCarId = carParkingService.CreateCar(carRequestModel.Balance, carRequestModel.CarType);

            if (createdCarId == -1)
            {
                return BadRequest("Car parking is full.");
            }

            return Ok(createdCarId);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool success = carParkingService.DeleteCar(id);
            if (!success)
            {
                return BadRequest("Please make sure that car ID is valid and car balance is > 0");
            }

            return Ok("Car deleted.");
        }
    }
}
