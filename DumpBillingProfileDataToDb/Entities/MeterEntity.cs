namespace DumpBillingProfileDataToDb.Entities;

public class MeterEntity
{
    public int ObisId { get; set; }
    public string MeterCategory { get; set; }
    public string ProfileName { get; set; }
    public string ObisCode { get; set; }
    public int AttributeId { get; set; }
    public string EntityCode { get; set; }
}