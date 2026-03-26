namespace DumpBillingProfileDataToDb.Entities;

public class MeterSubTemplate
{
    public int TemplateId { get; set; }
    public string SubjectTypeName { get; set; }
    public string MeterCategory { get; set; }
    public string ProfileName { get; set; }
    public string ObisCode { get; set; }

    //public string DataFormat { get; set; }
}