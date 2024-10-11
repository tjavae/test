using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUDC.Common.Dtos.CuSearch
{
    public class CreditUnionDto
    {
        public int CuNumber { get; set; }
        public string CuType { get; set; }
        public string? Region { get; set; }
        public int JoinNumber { get; set; }
        public Int16 StateCode { get; set; }        
        public Int16 CountyCode { get; set; }       
        public string? Status { get; set; }
        public string? LastEventCode { get; set; }        
        public string? CuName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? AttentionOf { get; set; }
        public string? ActualStreet { get; set; }
        public string? ActualCity { get; set; }
        public string? ActualState { get; set; }
        public string? ActualZipCode { get; set; }
        public string? Phone { get; set; }
        public string? Principal { get; set; }
        public string? WhenOpen { get; set; }
        public Int16 CongressionalDistrict { get; set; }
        public Int16 District { get; set; }
        public Int16 Smsa { get; set; }
        public string? TomCode { get; set; }
        public Int16 YearOpened { get; set; }
        public Int16 OpenedBy { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? InsuredDate { get; set; }
        public Byte LimitedIncCode { get; set; }       
        public string? SeCode { get; set; }        
        public string? CuAssignmentCode { get; set; }        
        public string? InsuranceCarrierCode { get; set; }        
        public string? PresidingBoardMember { get; set; }
        public string? EdpVendor { get; set; }        
    }
}
