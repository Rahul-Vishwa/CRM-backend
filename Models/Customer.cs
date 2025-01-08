using System.ComponentModel.DataAnnotations;

namespace Artico.Models
{
    public record AddCustomer
    {
        internal int userid { get; set; }
        [StringLength(100, MinimumLength = 3)]
        public required string name { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        [StringLength(100, MinimumLength = 5)]
        public string? emailaddress { get; set; }
        public string? phonenumber { get; set; }
        public string? notes { get; set; }
    }

    public record AllCustomers
    {
        public int id { get; set; }
        public required string createdby { get; set; }
        public DateTime createdat { get; set; }
        public required string name { get; set; }
        public string type { get; set; } = "";
        public string status { get; set; } = "";
        public required string emailaddress { get; set; }
        public required string phonenumber { get; set; }
        public string? notes { get; set; }
    }
}
