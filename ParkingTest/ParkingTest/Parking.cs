using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkingTest
{
    public class Parking
    {
        private static Dictionary<CarType, double> PriceList = Settings.getPriceList();
        private static int Timeout = Settings.Timeout;
        private static double Fine = Settings.Fine;
        private static int ParkingSpace = Settings.ParkingSpace;

        private List<Car> Cars;
        private List<Transaction> Transactions;
        private double ParkingBalance;

        private static Parking instance;

        private Parking()
        {

            Cars = new List<Car>(ParkingSpace);
            Transactions = new List<Transaction>();
            ParkingBalance = 0;

            TimerCallback tm = new TimerCallback(calculateFundsForAllCars);
            Timer timer = new Timer(tm, null, 0, Timeout);


        }

        public static Parking getInstance()
        {

                if (instance == null)
                {
                    instance = new Parking();
                }
                return instance;
        }

        public double getParkingRevenue()
        {
            return ParkingBalance;
        }

        public void addCar(Car car)
        {
            if(Cars.Count() < ParkingSpace)
            {
                Cars.Add(car);
            }
            else
            {
                Console.WriteLine("The parking is overloaded!");
            }
        }

        public List<Car> getAllCars()
        {
            return Cars;
        }

        public List<Transaction> getAllTransactions()
        {
            return Transactions;
        }

        public void addCarBalance(double newBalance, Car car)
        {
            car.Balance += newBalance;
        }

        public void removeCar(Car car)
        {
            Cars.Remove(car);
        }

        public int availablePlaces()
        {
            return ParkingSpace - Cars.Count;
        }


        public void calculateFee(double newBalance, Car car)
        {
            ParkingBalance += newBalance;
            Transactions.Add(new Transaction
            {
                transactionDate = DateTime.Now,
                carId = car.Id,
                funds = newBalance
            });
        }

        private void calculateFundsForAllCars(object obj)
        {
           foreach(Car car in Cars)
            {
                double priceForCar = PriceList[car.Type];
                if (car.Balance < priceForCar)
                {
                    car.Balance -= priceForCar * Fine;
                }
                else
                {
                    car.Balance -= priceForCar;
                    ParkingBalance += priceForCar;
                    Transactions.Add(new Transaction
                    {
                        transactionDate = DateTime.Now,
                        carId = car.Id,
                        funds = priceForCar
                    });
                }
            }
        }

    }
}
