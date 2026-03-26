namespace DumpBillingProfileDataToDb.Entities;

public class BillingProfileDto
{
    public string Meternumber { get; set; }
    public string Obisdata { get; set; }
    public DateTime CreatedAt { get; set; }   // 🔴 this exists
    public DateTime Rtcdateat { get; set; }
}