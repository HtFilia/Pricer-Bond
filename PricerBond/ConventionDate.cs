using System;
using System.Collections.Generic;
using System.Text;

namespace PricerBond
{
    internal class ConventionDate
    {
        private static int nbDaysPerMonth = 30;
        private static int nbDaysPerYear = 360;
        public int Year;
        public int Month;
        public int Day;

        public ConventionDate()
        {
            DateTime today = DateTime.Today;
            if (today.Day > 30)
            {
                this.Day = 30;
                if (today.Month == 12)
                {
                    this.Month = 1;
                    this.Year = today.Year + 1;
                } else
                {
                    this.Month = today.Month + 1;
                    this.Year = today.Year;
                }
            } else
            {
                this.Day = today.Day;
                this.Month = today.Month;
                this.Year = today.Year;
            }
        }

        public ConventionDate(string date)
        {
            int day = int.Parse(date.Substring(0, 2));
            int month = int.Parse(date.Substring(3, 2));
            int year = int.Parse(date.Substring(6, 4));
            if (!isValidDate(year, month, day)) throw new ArgumentException("Not a valid 30/360 date.");
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public ConventionDate(int year, int month, int day)
        {
            if (!isValidDate(year, month, day)) throw new ArgumentException("Not a valid 30/360 date.");
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        public int GetDaysPerMonth()
        {
            return nbDaysPerMonth;
        }

        public int GetDaysPerYear()
        {
            return nbDaysPerYear;
        }

        public bool isValidDate(int year, int month, int day)
        {
            if (month <= 0 || month > 12 || day <= 0 || day > 30) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || ! this.GetType().Equals(obj.GetType()))
            {
                return false;
            }

            ConventionDate date = (ConventionDate)obj;
            if ((date.Day != this.Day) || (date.Month != this.Month) || (date.Year != this.Year))
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int NumberDays(ConventionDate endDate)
        {
            return (360 * (endDate.Year - this.Year) + 30 * (endDate.Month - this.Month) + (endDate.Day - this.Day));
        }
    }
}
