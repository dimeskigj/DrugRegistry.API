using System.ComponentModel.DataAnnotations;

namespace DrugRegistry.API.Domain;

public class Drug
{
    [Key] public Guid Id { get; set; }
    public string? DecisionNumber { get; set; }
    public string? Atc { get; set; }
    public string? LatinName { get; set; }
    public string? GenericName { get; set; }
    public IssuingType IssuingType { get; set; }
    public string? Ingredients { get; set; }
    public string? Packaging { get; set; }
    public string? Strength { get; set; }
    public string? PharmaceuticalForm { get; set; }
    public string? Url { get; set; }
    public string? ManualUrl { get; set; }
    public string? ReportUrl { get; set; }
    public DateTime DecisionDate { get; set; }
    public DateTime? ValidityDate { get; set; }
    public string? ApprovalCarrier { get; set; }
    public string? Manufacturer { get; set; }
    public int PriceWithVat { get; set; }
    public int PriceWithoutVat { get; set; }
    public DateTime LastUpdate { get; set; }
}