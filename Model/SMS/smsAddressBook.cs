//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cabincrew.Model.SMS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("smsAddressBook")]
    public partial class smsAddressBook
    {
        [Key]
        public int AddressBookID { get; set; }
        public string ContactCode { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string Email { get; set; }
        public string PrivateEmail { get; set; }
        public string MainBase { get; set; }
        public string Group { get; set; }
        public string Course { get; set; }
        public string CrewType { get; set; }
        public string OnPlane { get; set; }
        public string WorkingStatus { get; set; }
        public string AdditionalInfo { get; set; }
        public Nullable<bool> bSync { get; set; }
        public Nullable<bool> isWhiteList { get; set; }
        public Nullable<bool> isBlackList { get; set; }
    }
}
