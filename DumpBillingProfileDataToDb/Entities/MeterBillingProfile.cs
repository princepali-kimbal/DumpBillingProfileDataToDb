namespace DumpBillingProfileDataToDb.Entities;

public class MeterBillingProfile
{
    public int Projectid { get; set; }
    public string? Meternumber { get; set; }
    public int? Typeofmeter { get; set; }
    public string? Sourcetype { get; set; }
    public bool? Isvalid { get; set; }
    public string? Mdmrequestid { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Obisdata { get; set; }
    public string? ProfileName { get; set; }
    public string? ProfileObisCode { get; set; }
    public DateTime? Rtcdateat { get; set; }
}