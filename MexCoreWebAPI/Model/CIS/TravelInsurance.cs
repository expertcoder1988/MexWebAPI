
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace MexCoreWebAPI.Model.CIS
{
    [Table("TravelInsurances")]
    public class TravelInsurance
    {
        public int Id { get; set; }

        public string? Duration { get; set; }
        public string? CountryCode { get; set; }
        public string? CountryDescrtiption { get; set; }
        public string? InsuranceType { get; set; }
        public decimal? MedicalExpense { get; set; }
        public decimal? SingleFee { get; set; }
        public decimal? FamilyFee { get; set; }
        public string? Vendor { get; set; }
        public string? MaxStay { get; set; }
    }

    [Table("InsuranceBenefits")]
    public class InsuranceBenefit
    {
        public int Id { get; set; }
        public string? Currency { get; set; }
        public int CountryCode { get; set; }
        public string? InsuranceType { get; set; }
        public string? Coverage { get; set; }
        public string? Benefit { get; set; }
        public string? Vendor { get; set; }
    }

    [Table("InsurancePaxs")]
    public class InsurancePax
    {
        public int Id { get; set; }
        public int InsuranceBookingId { get; set; }
        public string? FullName { get; set; }
        public string? DOB { get; set; }
        public string? PassportNo { get; set; }
        public string? Type { get; set; }
        public string? Vendor { get; set; }

    }

    [Table("InsuranceBookings")]
    public class InsuranceBooking
    {
        public int Id { get; set; }
        public int InsuranceCode { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CNIC { get; set; }
        public string? CNICIssuanceDate { get; set; }
        public string? DOB { get; set; }
        public string? ContactNumber { get; set; }
        public string? PurposeOfVisit { get; set; }
        public string? EmailAddress { get; set; }
        public string? PassportNumber { get; set; }
        public string? ResidentialAddress { get; set; }
        public string? CityofResidence { get; set; }
        public string? BeneficiaryFirstName { get; set; }
        public string? BeneficiaryLastName { get; set; }
        public string? BeneficiaryRelation { get; set; }
        public string? BeneficiaryCNIC { get; set; }
        public string? BeneficiaryContactNumber { get; set; }
        public string? BeneficiaryAddress { get; set; }
        public bool Status { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public string? PassengerType { get; set; }
        public string? DepartureDate { get; set; }
        public string? ArrivalDate { get; set; }
        public string? FirstCountry { get; set; }
        public string? SecondCountry { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? PaymentMessage { get; set; }
        public string? DiscountedPrice { get; set; }
        public string? OriginalPrice { get; set; }
        public string? PolicyNumber { get; set; }
        public string? Vendor { get; set; }
        public string? AreaShortCode { get; set; }
        public string? NoOfDays { get; set; }

        public int? userId {get; set; }
    }

    [Table("InsuranceEnquiries")]
    public class InsuranceEnquiry
    {
        public int Id { get; set; }
        public string? Full_Name { get; set; }
        public string? Mobile_no { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? InsuranceId { get; set; }
        public string? Passengertype { get; set; }

    }

    [Table("UicBookings")]
    public class UicBooking
    {
        public int Id { get; set; }
        public int? PDays { get; set; }
        public int? Duration { get; set; }
        public string? Vendor { get; set; }
        public string? DurationUnit { get; set; }
        public string? AreaShortCode { get; set; }
        public string? Area { get; set; }
        public string Plan { get; set; }
        public string? PlanType { get; set; }
        public decimal? Premium { get; set; }
        public decimal? AgePremIncrease { get; set; }
        public decimal? GrossPremium { get; set; }
        public decimal? AdvanceTax { get; set; }
        public string? TravelerName { get; set; }
        public string? TravelerNICNo { get; set; }
        public string? TravelerNTNNo { get; set; }
        public bool? IsTaxPayer { get; set; }
        public decimal? TotalPayablePremium { get; set; }
        public DateTime? TravelerDOB { get; set; }
        public int? TravelerAge { get; set; }
        public string? Covid { get; set; }
        public string? MedicalExpense { get; set; }
        public string? DepartureDate { get; set; }
        public string? ArrivalDate { get; set; }
        public string? FirstCountry { get; set; }
        public string? SecondCountry { get; set; }
        public string? ContactNumber { get; set; }
        public string? ResidentialAddress { get; set; }
        public string? CityofResidence { get; set; }
        public string? EmailAddress { get; set; }
        public DateTime? EntryDateTime { get; set; }
        public string? BeneficiaryFirstName { get; set; }
        public string? BeneficiaryLastName { get; set; }
        public string? BeneficiaryRelation { get; set; }
        public string? BeneficiaryCNIC { get; set; }
        public string? BeneficiaryContactNumber { get; set; }
        public string? BeneficiaryAddress { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string? PaymentMessage { get; set; }
        public string? PolicyNumber { get; set; }
        public bool? PaymentStatus { get; set; }
        public string? Passport { get; set; }
    }
}
