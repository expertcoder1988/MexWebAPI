namespace MexCoreWebAPI.Model
{
    public class VisaRequest
    {
        public VisaRequest() { }
        public long Id { get; set; }
        public long CountryId { get; set; }
        public long VisaId { get; set; }
        public long VisaDovcumentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? DOB { get; set; }
        public string? BirthCountry { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string? BirthCity { get; set; }
        public string? BirthState { get; set; }
        public string? Gender { get; set; }
        public string? HomeAddress1 { get; set; }
        public string? HomeAddress2 { get; set; }
        public string? PostalCode { get; set; }
        public string? ResidenceContact { get; set; }
        public string? ResidenceCity { get; set; }
        public string? ResidenceCountry { get; set; }
        public string? OfficeAddress1 { get; set; }
        public string? OfficeAddress2 { get; set; }
        public string? OfficePostalCode { get; set; }
        public string? OfficeContact { get; set; }
        public string? OfficeCity { get; set; }
        public string? OfficeCountry { get; set; }
        public DateTime AppliedDate { get; set; }
        public int IsOpened { get; set; }
        //public List<Images> Images { get; set; }
    }
    public class Images
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IFormFile? file { get; set; }
    }
    public class Files
    {
        public int Id { get; set; }
        public IFormFile? file { get; set; }

    }
}
