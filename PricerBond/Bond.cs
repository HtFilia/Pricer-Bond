using System;
using System.Collections.Generic;
using System.Text;

namespace PricerBond
{
    class Bond
    {
        public double faceValue;
        public double annualRate;
        public double marketRate;
        public int frequency;
        public ConventionDate maturity;

        public Bond(double faceValue, double annualRate, double marketRate, int frequency,
            ConventionDate maturity)
        {
            this.faceValue = faceValue;
            this.annualRate = annualRate;
            this.marketRate = marketRate;
            this.frequency = frequency;
            this.maturity = maturity;
        }

        public double GetPrice(Action action)
        {
            double perf;
            double perfFactor = 1;
            double price = 0;
            int year = 1;
            ConventionDate previousDate = new ConventionDate();
            ConventionDate currentDate = GetNextCouponDate(previousDate);
            int nbDays = previousDate.NumberDays(currentDate);
            double factor = (double)nbDays / (double)previousDate.GetDaysPerYear();
            price += factor * faceValue * annualRate / Math.Pow((1 + marketRate), factor);
            while (!currentDate.Equals(maturity))
            {
                
                if (action != null)
                {
                    perf = action.GetPerf(currentDate, previousDate);
                    if (perf > 0.2)
                    {
                        perfFactor = 1;
                    } else if (perf > 0.1)
                    {
                        perfFactor = 0.7;
                    } else
                    {
                        perfFactor = 0.5;
                    }
                }
                price += perfFactor * faceValue * annualRate / Math.Pow(1 + marketRate, factor + (year / frequency)) / (double)frequency;
                year += 1;
                previousDate = currentDate;
                currentDate = GetNextCouponDate(currentDate);
            }
            /*for (int year = 1; year < frequency * YearsTillMaturity(); year++)
            {
                if (action != null)
                {
                    perf = action.GetPerf(year);
                }
                price += perf * faceValue * annualRate / Math.Pow((1 + marketRate), factor + (year / frequency)) / (double)frequency;
            }*/
            if (action != null)
            {
                perf = action.GetPerf(previousDate, maturity);
                if (perf > 0.2)
                {
                    perfFactor = 1;
                }
                else if (perf > 0.1)
                {
                    perfFactor = 0.7;
                }
                else
                {
                    perfFactor = 0.5;
                }
            }
            price += perfFactor * faceValue * (1 + annualRate) / Math.Pow((1 + marketRate), factor + YearsTillMaturity());
            return price;
            
        }

        private ConventionDate GetNextCouponDate(ConventionDate today)
        {
            int nextDay = maturity.Day;
            int nextMonth = GetNextMonth(today);
            int nextYear = GetNextYear(today, nextMonth);
            return new ConventionDate(nextYear, nextMonth, nextDay);
        }

        private int GetNextMonth(ConventionDate today)
        {
            List<int> months = GetMonthsCoupon();
            if (today.Day < maturity.Day)
            {
                return FindClosestBefore(months, today.Month);
            }
            else
            {
                return FindClosestAfter(months, today.Month);
            }
        }

        private List<int> GetMonthsCoupon()
        {
            List<int> months = new List<int>();
            for (int month = 1; month <= 12; month++)
            {
                if (mod(month - maturity.Month, frequency) == 0)
                {
                    months.Add(month);
                }
            }
            return months;
        }

        private int FindClosestBefore(List<int> months, int month)
        {
            //TODO optimize O(ln n) instead O(n)
            int distance = month - months[0];
            int idx = 0;
            int goodIdx = 0;
            while (distance > 0 && idx < months.Count)
            {
                if (month - months[idx] < distance)
                {
                    distance = month - months[idx];
                    goodIdx = idx;
                }
                idx++;
            }
            return months[goodIdx];
        }

        private int FindClosestAfter(List<int> months, int month)
        {
            //TODO optimize O(ln n) instead O(n)
            int distance = month - months[0];
            int idx = 0;
            int goodIdx = 0;
            while (distance > 0 && idx < months.Count)
            {
                if (month - months[idx] < distance)
                {
                    distance = month - months[idx];
                    goodIdx = idx;
                }
                idx++;
            }
            return months[goodIdx + 1 < months.Count ? goodIdx + 1 : 0];
        }

        private static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private int GetNextYear(ConventionDate today, int nextMonth)
        {
            if (nextMonth < today.Month)
            {
                return today.Year + 1;
            }
            else
            {
                return today.Year;
            }
        }

        private int YearsTillMaturity()
        {
            ConventionDate today = new ConventionDate();
            return (maturity.Year - today.Year);
        }
    }
}
