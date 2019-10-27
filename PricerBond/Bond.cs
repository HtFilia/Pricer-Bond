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

        public double GetPrice()
        {
            double price = 0;
            ConventionDate today = new ConventionDate();
            ConventionDate firstCoupon = GetFirstCouponDate(today, frequency);
            int nbDays = firstCoupon.NumberDays(today);
            double factor = (double)nbDays / (double)today.GetDaysPerYear();
            price += factor * faceValue * annualRate / Math.Pow((1 + marketRate), factor);
            for (int year = 1; year < YearsTillMaturity(); year++)
            {
                price += faceValue * annualRate / Math.Pow((1 + marketRate), factor + year);
            }
            price += faceValue * (1 + annualRate) / Math.Pow((1 + marketRate), factor + YearsTillMaturity());
            return Math.Round(price, 2);
        }

        private ConventionDate GetFirstCouponDate(ConventionDate today, int frequency)
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
            return goodIdx;
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
            return (goodIdx + 1 < months.Count ? goodIdx + 1 : 0);
        }

        private static int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

        private int GetNextYear(ConventionDate today, int nextMonth)
        {
            if (nextMonth < today.Month || frequency == 12)
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
