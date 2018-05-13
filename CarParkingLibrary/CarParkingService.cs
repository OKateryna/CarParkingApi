using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace CarParkingLibrary
{
    public class CarParkingService
    {
        public IEnumerable<Car> GetAllCars()
        {
            return Parking.Instance.Cars;
        }

        public Car GetCarById(int id)
        {
            return Parking.Instance.Cars.SingleOrDefault(x => x.Id == id);
        }

        public int CreateCar(double balance, CarType carType)
        {
            if (Parking.Instance.Cars.Count >= Settings.ParkingSpace)
            {
                return -1;
            }

            Car carToAdd = new Car(balance, carType);
            Parking.Instance.Cars.Add(carToAdd);
            return carToAdd.Id;
        }

        public string GetTransactionLog()
        {
            StringBuilder logStringBuilder = new StringBuilder();
            using (StreamReader reader = new StreamReader(Parking.Instance.TransactionLogFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var lineSplit = line.Split('-');
                    logStringBuilder.AppendLine($"Earnings amount - {lineSplit[0]} on {lineSplit[1]}");
                }
            }

            return logStringBuilder.ToString();
        }

        public int GetOccupiedParkingCount()
        {
            return Parking.Instance.Cars.Count;
        }

        public IEnumerable<Transaction> GetTransactionsForLastMinute()
        {
            return Parking.Instance.Transactions.Where(transaction => transaction.TransactionDateTime > DateTime.Now.AddSeconds(60 * -1));
        }
        
        public IEnumerable<Transaction> GetLastMinuteTransactionsForCar(int carId)
        {
            return Parking.Instance.Transactions.Where(transaction => transaction.CarId == carId && transaction.TransactionDateTime > DateTime.Now.AddSeconds(60 * -1));
        }

        public int GetFreeParkingCount()
        {
            return Settings.ParkingSpace - Parking.Instance.Cars.Count;
        }

        public double GetTotalParkingEarnings()
        {
            return Parking.Instance.Balance;
        }

        public bool PutCarBalance(int id, double balance)
        {
            var car = GetCarById(id);
            if (car == null)
            {
                return false;
            }

            car.Balance += balance;
            return true;
        }

        public bool DeleteCar(int id)
        {
            var car = GetCarById(id);
            if (car == null || car.Balance < 0)
            {
                return false;
            }

            Parking.Instance.Cars.Remove(car);
            return true;
        }
    }
}
