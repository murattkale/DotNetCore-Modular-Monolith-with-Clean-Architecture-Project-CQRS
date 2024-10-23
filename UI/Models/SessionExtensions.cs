using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace UI.Models;

public static class SessionExtensions
{
    public static void Set(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);

        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}