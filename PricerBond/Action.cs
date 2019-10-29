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
        public List<double> spots;
    }
}
