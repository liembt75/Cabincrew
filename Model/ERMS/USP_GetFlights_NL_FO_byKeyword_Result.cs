using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cabincrew.Model.ERMS
{
    public partial class USP_GetFlights_NL_FO_byKeyword_Result
    {
        public int FoFlightID { get; set; }
        public System.DateTime FoDate { get; set; }
        public string FoFlightNo { get; set; }
        public string FoRouting { get; set; }
        public Nullable<System.DateTime> FoDepart { get; set; }
        public Nullable<System.DateTime> FoArrive { get; set; }
        public string FoCarry { get; set; }
        public string FoAircraft { get; set; }
        public Nullable<long> LBid { get; set; }
        public Nullable<bool> FoStatus { get; set; }
        public int NlID { get; set; }
        public Nullable<System.DateTime> NlUTCDate { get; set; }
        public string NlFlightNo { get; set; }
        public string NlRouting { get; set; }
        public Nullable<System.DateTime> NlUTCDepart { get; set; }
        public Nullable<System.DateTime> NlUTCArrive { get; set; }
        public string NlStatus { get; set; }
        public string NlAC { get; set; }
        public string NlCarry { get; set; }
        public Nullable<bool> NlAuto { get; set; }
        public string NlCreator { get; set; }
        public Nullable<System.DateTime> NlCreated { get; set; }
        public Nullable<System.DateTime> NlModified { get; set; }
        public string NlModifier { get; set; }
    }

}
