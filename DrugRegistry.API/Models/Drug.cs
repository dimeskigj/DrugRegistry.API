using System.ComponentModel.DataAnnotations;

namespace DrugRegistry.API.Models;

public class Drug
{
    [Key] public Guid Id { get; set; }
    public string DecisionNumber { get; set; }
    public string Atc { get; set; }
    public string LatinName { get; set; }
    public string GenericName { get; set; }
    public IssuingType IssuingType { get; set; }
    public string Ingredients { get; set; }
    public string Packaging { get; set; }
    public string Strength { get; set; }
    public string PharmaceuticalForm { get; set; }
    public string Url { get; set; }
    public string ManualUrl { get; set; }
    public string ReportUrl { get; set; }
    public DateTime DecisionDate { get; set; }
    public DateTime? ValidityDate { get; set; }
    public string ApprovalCarrier { get; set; }
    public string Manufacturer { get; set; }
    public int PriceWithVat { get; set; }
    public int PriceWIthoutVat { get; set; }
    public DateTime LastUpdate { get; set; }


    public Drug(string decisionNumber, string atc, string latinName,
        string genericName, IssuingType issuingType, string ingredients,
        string packaging, string strength, string pharmaceuticalForm,
        string url, string manualUrl, string reportUrl,
        DateTime decisionDate, string approvalCarrier,
        string manufacturer, int priceWithVat, int priceWithoutVat, DateTime lastUpdate = default,
        DateTime? validityDate = null)
    {
        DecisionNumber = decisionNumber;
        Atc = atc;
        LatinName = latinName;
        GenericName = genericName;
        IssuingType = issuingType;
        Ingredients = ingredients;
        Packaging = packaging;
        Strength = strength;
        PharmaceuticalForm = pharmaceuticalForm;
        Url = url;
        ManualUrl = manualUrl;
        ReportUrl = reportUrl;
        ValidityDate = validityDate;
        DecisionDate = decisionDate;
        ApprovalCarrier = approvalCarrier;
        Manufacturer = manufacturer;
        PriceWithVat = priceWithVat;
        PriceWIthoutVat = priceWithoutVat;
        LastUpdate = lastUpdate == default
            ? DateTime.Now.Date
            : lastUpdate;
    }
}