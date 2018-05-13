using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarParkingLibrary
{
    public class Car
    {
        private int id;
        private double balance;
        private CarType carType;
        private static int count = 1;

        public int Id
        {
            get
            {
                return id;
            }
        }

        public double Balance
        {
            get
            {
                return balance;
            }
            internal set
            {
                balance = value;
            }
        }

        public CarType CarType
        {
            get
            {
                return carType;
            }
        }

        public Car(double balance, CarType carType)
        {
            this.id = count++;
            this.balance = balance;
            this.carType = carType;
        }
    }
}
