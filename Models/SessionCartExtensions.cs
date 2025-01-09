using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.Json;

namespace ZamawianiePosiłkowOnline.Models
{
    public static class SessionCartExtensions
    {
        public static void Set<T>(this ISession session,string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
}
