using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.ERMS
{
    public partial class CR_Flight_Crew
    {
        public int ID { get; set; }
        public int FlightID { get; set; }
        public string CrewID { get; set; }
        public string Job { get; set; }
        public string ca { get; set; }
        public string Dutyfree { get; set; }
        public bool IsDeleted { get; set; }
    }
}
