namespace DumpBillingProfileDataToDb.Entities;

public class RedisConnectionParams
{
    private string _sentinelEndpoints;

    public string sentinelEndpoints
    {
        get => _sentinelEndpoints;
        set
        {
            _sentinelEndpoints = value;
            Endpoints = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }
    }

    public string Password { get; set; }

    public string[] Endpoints { get; private set; }

    public RedisConnectionParams() { }

    public RedisConnectionParams(string sentinelEndpoints, string password)
    {
        this.sentinelEndpoints = sentinelEndpoints;
        Password = password;
    }
}

