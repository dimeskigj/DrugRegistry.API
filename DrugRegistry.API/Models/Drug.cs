using System.ComponentModel.DataAnnotations;

namespace DrugRegistry.API.Models;

public class Drug
{
    [Key]
    public string DecisionNumber { get; set; }
    public string ATC { get; set; }
    public string LatinName { get; set; }
    public string GenericName { get; set; }
    public IssuingType IssuingType { get; set; }
    public string Ingredients { get; set; }
    public string Packaging { get; set; }
    public string Strength { get; set; }
    public string PharmaceuticalForm { get; set; }
    public string URL { get; set; }
    public string ManualURL { get; set; }
    public string ReportURL { get; set; }
    public DateTime DecisionDate { get; set; }
    public DateTime? ValidityDate { get; set; }
    public string ApprovalCarrier { get; set; }
    public string Manufacturer { get; set; }
    public int PriceWithVAT { get; set; }
    public int PriceWIthoutVAT { get; set; }
    public DateTime LastUpdate { get; set; }


    public Drug(string decisionNumber, string atc, string latinName,
        string genericName, IssuingType issuingType, string ingredients,
        string packaging, string strength, string pharmaceuticalForm,
        string url, string manualURL, string reportURL,
        DateTime decisionDate, string approvalCarrier,
        string manufacturer, int priceWithVAT, int priceWIthoutVAT, DateTime lastUpdate = default, DateTime? validityDate = null) {
        DecisionNumber = decisionNumber;
        ATC = atc;
        LatinName = latinName;
        GenericName = genericName;
        IssuingType = issuingType;
        Ingredients = ingredients;
        Packaging = packaging;
        Strength = strength;
        PharmaceuticalForm = pharmaceuticalForm;
        URL = url;
        ManualURL = manualURL;
        ReportURL = reportURL;
        ValidityDate = validityDate;
        DecisionDate = decisionDate;
        ApprovalCarrier = approvalCarrier;
        Manufacturer = manufacturer;
        PriceWithVAT = priceWithVAT;
        PriceWIthoutVAT = priceWIthoutVAT;
        LastUpdate = lastUpdate == default
                    ? DateTime.Now.Date
                    : lastUpdate;
    }
}

