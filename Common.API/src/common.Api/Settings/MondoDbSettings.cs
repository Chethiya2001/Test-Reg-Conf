namespace common.Api.Settings;

public class MondoDbSettings
{
    public string? Host { get; init; }
    public string? Port { get; init; }

    public string? ConnectionString => $"mongodb://{Host}:{Port}";
}
