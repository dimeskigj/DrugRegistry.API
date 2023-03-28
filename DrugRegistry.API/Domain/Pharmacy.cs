using System.ComponentModel.DataAnnotations;

namespace DrugRegistry.API.Domain;

public class Pharmacy
{
    [Key] public Guid Id { get; set; }
    public string? IdNumber { get; set; }
    public string? TaxNumber { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Municipality { get; set; }
    public string? Place { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Decision { get; set; }
    public string? Email { get; set; }
    public string? Pharmacists { get; set; }
    public string? Technicians { get; set; }
    public string? Comment { get; set; }
    public PharmacyType? PharmacyType { get; set; }
    public bool? Central { get; set; }
    public bool? Active { get; set; }
    public Location? Location { get; set; }
    public Uri? Url { get; set; }
}