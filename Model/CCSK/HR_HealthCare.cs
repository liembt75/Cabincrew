using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.CCSK
{
    public partial class HR_HealthCare
    {
        public int ID { get; set; }
        public string CrewID { get; set; }
        public DateTime Dotkham { get; set; }
        public DateTime Expired { get; set; }
    }
}
