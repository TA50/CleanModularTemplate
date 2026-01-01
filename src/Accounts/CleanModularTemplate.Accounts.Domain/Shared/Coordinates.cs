namespace CleanModularTemplate.Accounts.Domain.Shared;

public record Coordinates
{
  public double Latitude { get; set; }
  public double Longitude { get; set; }

  public const int MaxLongitude = 180;
  public const int MinLongitude = -180;

  public const int MaxLatitude = 90;
  public const int MinLatitude = -90;
}
