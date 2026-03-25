namespace DumpBillingProfileDataToDb.Entities;

public class RedisPayload
{
    public string Key { get; set; }
    public string NewTemplateId { get; set; }
    public List<RedisValues> Values { get; set; }
    public List<MeterSecurity> Security { get; set; }
    public List<RedisProfileValues> RedisProfileValues { get; set; }
    public RedisNamePlate NamePlateDetails { get; set; }
    public List<MeterTemplateHeader> meterTemplateHeaders { get; set; }
    public List<ProfileDto> profileDtos { get; set; }
    public List<EventProfileDto> eventProfileDtos { get; set; }
    public List<MeterEntity> meterEntities { get; set; }
    public List<EventSubTemplateDto> eventSubTemplates { get; set; }
    public List<MeterSubTemplate> meterSubTemplates { get; set; }
    public List<MeterValidation> meterValidations { get; set; }
    public List<EventsMeterCategoryMapping> eventsMeterCategoryMappings { get; set; }
    public List<EventsMappingDto> eventsMappingDtos { get; set; }
}
public class RedisValues
{
    public int CurrentFrag { get; set; }
    public byte[] Value { get; set; }
}
public class RedisProfileValues
{
    public string EntityCode { get; set; }
    public string ObisCode { get; set; }
    public string Value { get; set; }
    public string DataFormat { get; set; }
}
public class MeterSecurity
{
    public string NodeId { get; set; }
    public string GlobalKey { get; set; }
}
public class MeterTemplateHeader
{
    public int TemplateId { get; set; }
    public string SubjectTypeName { get; set; }
    public bool IsNewTemplate { get; set; }
    public string HeaderId { get; set; }
    public string HeaderName { get; set; }
    public int Length { get; set; }
    public int StartIndex { get; set; }
}

public class BaseProfileDto
{
    public int ProfileId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public int ParentID { get; set; }
    public string ProfileObisCode { get; set; }
}
public class ProfileDto : BaseProfileDto
{
    public bool IsWithSubTemplate { get; set; } = false;
}
public class EventProfileDto
{
    public int ProfileId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public int ParentID { get; set; }
}
public class EventSubTemplateDto
{
    public int Length { get; set; }
    public int StartIndex { get; set; }
    public double Scaler { get; set; }
    public string DataFormat { get; set; }
    public string ObisCode { get; set; }
    public string MeterCategory { get; set; }
    public int Precision { get; set; }
}
public class MeterValidation
{
    public string MeterCategory { get; set; }
    public string ProfileName { get; set; }
    public string ValidationName { get; set; }
    public string ValidationExpression { get; set; }
    public int SequenceID { get; set; }
    public bool IsActive { get; set; }
    public Guid ValidationId { get; set; }
}
public class EventsMeterCategoryMapping
{
    public int EventId { get; set; }
    public string MeterCategory { get; set; }
}
public class EventsMappingDto
{
    public int ProfileId { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public string ProfileObisCode { get; set; } = string.Empty;
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventObisCode { get; set; } = string.Empty;
    public bool IsAlarm { get; set; } = false;

}
