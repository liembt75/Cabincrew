using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.DDKT
{
    public partial class lbaytv2
    {
        public int flightID { get; set; }
        public string code_fly { get; set; }
        public string code_tv { get; set; }
        public string loai { get; set; }
        public string from_place { get; set; }
        public string end_place { get; set; }
        public string fly_no { get; set; }
        public string cfg { get; set; }
        public DateTime start_date { get; set; }
        public string start_time { get; set; }
        public DateTime end_date { get; set; }
        public string end_time { get; set; }
        public string job { get; set; }
        public string note { get; set; }
        public string type_apl { get; set; }
        public string acf { get; set; }
        public string status { get; set; }
    }
}
