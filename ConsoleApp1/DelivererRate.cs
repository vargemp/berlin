using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class DelivererRate
    {
        public Deliverer Deliverer { get; set; }
        public int InversedCost { get; set; }

        public DelivererRate(Deliverer d, int ic)
        {
            Deliverer = d;
            InversedCost = ic;
        }
    }
}
