using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace PricerBond
{
    [DataContract]
    class Action
    {
        [DataMember]
        public string name;

        [DataMember]
        public double drift;

        [DataMember]
        public double vol;

        [DataMember]
        public Dictionary<ConventionDate, double> spots;

        private static Random random = new Random();

        public Action(string name, double initial, ConventionDate today, ConventionDate maturity)
        {
            this.name = name;
            this.drift = 0.1;
            this.vol = 0.2;
            this.spots = new Dictionary<ConventionDate, double>();
            this.spots.Add(today, initial);
            BuildTrajectory(today, maturity);
        }

        private void BuildTrajectory(ConventionDate today, ConventionDate maturity)
        {
            double dt = GetDt();
            double spotValue;
            bool afterMaturity = false;
            ConventionDate currentDate = today.GetNextDate();
            ConventionDate previousDate = today;
            while (! afterMaturity)
            {
                if (currentDate.Equals(maturity))
                {
                    afterMaturity = true;
                }
                spotValue = spots[previousDate] * Math.Exp((drift - Math.Pow(vol, 2) / 2) * dt + vol * Math.Sqrt(dt) * Normal.Sample(random, 0.0, 1.0));
                spots.Add(currentDate, spotValue);
                previousDate = currentDate;
                currentDate = currentDate.GetNextDate();

            }
        }

        private double GetDt()
        {
            return 1;
        }

        public double GetPerf(ConventionDate currentDate, ConventionDate previousDate)
        {
            double perf = (spots[currentDate] - spots[previousDate]) / spots[previousDate];
            return (perf > 0 ? perf : 0);
        }
    }
}
