using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Text;

namespace CarParkingLibrary
{
    public sealed class Parking
    {
        public const int transactionWritePeriod = 60;
        private readonly List<Car> cars;
        private readonly List<Transaction> transactions;
        private double balance;
        private Timer withdrawTimer;
        private Timer transactionTimer;
        public string TransactionLogFile { get; } = Settings.TransactionLogFileName;

        public List<Car> Cars
        {
            get
            {
                return cars;
            }
        }

        public List<Transaction> Transactions
        {
            get
            {
                return transactions;
            }
        }

        public double Balance
        {
            get
            {
                return balance;
            }
        }

        private Parking()
        {
            cars = new List<Car>(Settings.ParkingSpace);
            transactions = new List<Transaction>();
            withdrawTimer = new Timer(OnWithdrawTimer, null, Settings.Timeout * 1000, Settings.Timeout * 1000);
            transactionTimer = new Timer(OnTransactionTimer, null, transactionWritePeriod * 1000, transactionWritePeriod * 1000);

            using (var file = File.Create(TransactionLogFile))
            {
            }
        }

        private void OnWithdrawTimer(object state)
        {
            foreach (var car in cars)
            {
                double amount = Settings.Prices[car.CarType];
                if (car.Balance < 0)
                {
                    amount *= Settings.Fine;
                }

                car.Balance -= amount;
                balance += amount;
                Transaction transaction = new Transaction(car.Id, amount);
                transactions.Add(transaction);
            }
        }

        private IEnumerable<Transaction> GetTransactionsForLastPeriod()
        {
            return transactions.Where(transaction => transaction.TransactionDateTime > DateTime.Now.AddSeconds(transactionWritePeriod * -1));
        }

        private void OnTransactionTimer(object state)
        {
            var lastMinuteTransactions = GetTransactionsForLastPeriod();
            LogTransactions(lastMinuteTransactions);
        }

        private void LogTransactions(IEnumerable<Transaction> lastMinuteTransactions)
        {
            var totalAmount = lastMinuteTransactions.ToList().Sum(x => x.Amount);
            using (StreamWriter writer = new StreamWriter(TransactionLogFile, true))
            {
                string lineToWrite = string.Format("{0}-{1}", totalAmount, DateTime.Now);
                writer.WriteLine(lineToWrite);
            }
        }
        
        private static readonly Lazy<Parking> instance = new Lazy<Parking>(() => new Parking());

        public static Parking Instance
        {
            get
            {
                return instance.Value;
            }
        }
    }
}
