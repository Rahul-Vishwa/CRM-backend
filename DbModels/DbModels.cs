namespace Artico.DbModels
{
    public record Users
    {
        public int id { get; set; }
        public required string username { get; set; }
        public required string email { get; set; }
        public required string password { get; set; }
    }

    public record Customers
    {
        public int id { get; set; }
        public int userid { get; set; }
        public required string name { get; set; }
        public int type { get; set; }
        public int status { get; set; }
        public required string emailaddress { get; set; }
        public required string phonenumber { get; set; }
        public string? notes { get; set; }
        public DateTime create_at { get; set; }
    }
}