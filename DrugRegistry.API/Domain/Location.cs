using System.ComponentModel.DataAnnotations;

namespace DrugRegistry.API.Domain;

public class Location
{
    [Key] public Guid Id { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}