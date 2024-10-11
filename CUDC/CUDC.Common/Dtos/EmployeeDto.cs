using System;

namespace CUDC.Api.Data
{
    public class EmployeeDto
    {
        public string EmployeeNumber { get; set; }
        public string Status { get; set; }
        public string Region { get; set; }
        public string SeCode { get; set; }
        public string Office { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string? SoundexName { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? City2 { get; set; }
        public string? State { get; set; }
        public string? State2 { get; set; }
        public string? ZipCode { get; set; }
        public string? ZipCode2 { get; set; }
        public string EmployeeType { get; set; }
        public string? District { get; set; }
        public string Division { get; set; }
        public string GsGrade { get; set; }
        public string LanId { get; set; }
        public string MailBox { get; set; }
        public string? Phone { get; set; }
        public string? Phone2 { get; set; }
        public Int16? StateCode { get; set; }
        public string? AspenNumber { get; set; }
        public string? AspenRegion { get; set; }
        public string? MailAlias { get; set; }
    }
}
