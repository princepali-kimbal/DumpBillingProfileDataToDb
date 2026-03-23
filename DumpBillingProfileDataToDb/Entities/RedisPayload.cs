namespace DumpBillingProfileDataToDb.Entities;
//public class RedisPayload
//{
//    //public string Key { get; set; }
//    //public string NewTemplateId { get; set; }
//    //public List<RedisValues> Values { get; set; }
//    //public List<MeterSecurity> Security { get; set; }
//    //public List<RedisProfileValues> RedisProfileValues { get; set; }
//    //public RedisNamePlate NamePlateDetails { get; set; }
//    //public List<MeterTemplateHeader> meterTemplateHeaders { get; set; }
//    //public List<ProfileDto> profileDtos { get; set; }
//    //public List<EventProfileDto> eventProfileDtos { get; set; }
//    //public List<MeterEntity> meterEntities { get; set; }
//    //public List<EventSubTemplateDto> eventSubTemplates { get; set; }
//    //public List<MeterSubTemplate> meterSubTemplates { get; set; }
//    //public List<MeterValidation> meterValidations { get; set; }
//    //public List<EventsMeterCategoryMapping> eventsMeterCategoryMappings { get; set; }
//    //public List<EventsMappingDto> eventsMappingDtos { get; set; }
//}
//public class RedisValues
//{
//    public int CurrentFrag { get; set; }
//    public byte[] Value { get; set; }
//}
public class RedisProfileValues
{
    public string EntityCode { get; set; }
    public string ObisCode { get; set; }
    public string Value { get; set; }
}
