using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace AtividadeCtedsD3.Models;

public enum EventType
{
    Login,
    Logoff
}

public class LoginInfo
{
    public LoginInfo(EventType eventType, DateTime time, string userName)
    {
        EventType = eventType;
        Time = time;
        UserName = userName;
    }

    public DateTime Time { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public EventType EventType { get; set; }

    public string UserName { get; set; }
}