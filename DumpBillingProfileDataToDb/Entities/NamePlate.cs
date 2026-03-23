using System;

namespace DumpBillingProfileDataToDb.Entities;

public class NamePlate
{
    public int? ProjectId { get; set; }

    public string NodeId { get; set; }

    public string MeterNumber { get; set; }

    public int? AgeingInDays { get; set; }

    public string CommunicationType { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;

    public int? CtRatio { get; set; }

    public string DeviceId { get; set; }


    public long? DriftInSeconds { get; set; }

    public string FirmwareVersion { get; set; }

    public DateTimeOffset FirstCommunicationAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset InstalledAt { get; set; } = DateTimeOffset.UtcNow;

    public string Latitude { get; set; }

    public string Longitude { get; set; }

    public string Manufacturer { get; set; }

    public string MeterCategory { get; set; }

    public DateTimeOffset MeterClockDateTime { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset MeterClockSyncTime { get; set; } = DateTimeOffset.UtcNow;

    public int? MeterTemplateId { get; set; }

    public string MeterType { get; set; }

    public DateTimeOffset? OriginalInstalledAt { get; set; } = DateTimeOffset.UtcNow;

    public int? PtRatio { get; set; }

    public string Rating { get; set; }

    public string RfVersion { get; set; }

    public DateTimeOffset? SatCompletionAt { get; set; } = DateTimeOffset.UtcNow;

    public string SatNo { get; set; }

    public int? YearOfManufacture { get; set; }

    public int ExpectedBlockloadCounts { get; set; }

    public int ExpectedInstantCounts { get; set; }

    public string GatewayId { get; set; }

    public DateTimeOffset? SatRealizationTime { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? MigratedAt { get; set; } = DateTimeOffset.UtcNow;

    public string TcpIp { get; set; }

    public string Port { get; set; }

    public DateTime? MeterConnectionStatusUpdatedAt { get; set; }

    public int? MeterRcdcStatus { get; set; }

    public string MeterConnectionStatus { get; set; }
}


public class RedisNamePlate
{
    public string SatNo { get; set; }
    public int NameplateId { get; set; }
    public int ProjectId { get; set; }
    public string NodeId { get; set; }
    public string MeterNumber { get; set; }
    public string CommunicationType { get; set; }
    public string MeterCategory { get; set; }
    public int MeterTemplateId { get; set; }
    public string MeterType { get; set; }
    public string Manufacturer { get; set; }
    public int? ExpectedBlockloadCounts { get; set; } = 48;
    public int? ExpectedInstantCounts { get; set; } = 48;
    public DateTimeOffset? MigratedAt { get; set; }
    public bool? IsMagicNumber { get; set; }
    public string TCPip { get; set; }
    public string Port { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string DeviceId { get; set; }
    public string GatewayId { get; set; }
    public DateTimeOffset? FirstCommunicationAt { get; set; }
}
