using DrugRegistry.API.Domain;

namespace DrugRegistry.API.Extensions;

public static class PharmacyTypeExtensions
{
    private const string PharmacyStation = "АПТЕКАРСКА СТАНИЦА";
    private const string Hospital = "БОЛНИЧКА";
    private const string Insulin = "ИНСУЛИНСКА";
    private const string PrivateHealthInstitution = "ПЗУ";
    private const string MobilePharmacy = "ПОДВИЖНА АПТЕКА";

    public static PharmacyType ParseType(string input)
        => input switch
        {
            PharmacyStation => PharmacyType.PharmacyStation,
            Hospital => PharmacyType.Hospital,
            Insulin => PharmacyType.Insulin,
            PrivateHealthInstitution => PharmacyType.PrivateHealthInstitution,
            MobilePharmacy => PharmacyType.MobilePharmacy,
            _ => throw new Exception()
        };
}