using Quartz;

namespace Rekindle.UserGroups.Infrastructure.Email.Extensions;

public static class JobDataMapExtensions
{
    public static bool TryGetString(this JobDataMap map, string key, out string? value)
    {
        if (map.ContainsKey(key) && map[key] is string strValue)
        {
            value = strValue;
            return true;
        }
        
        value = null;
        return false;
    }
    
    public static bool GetBoolean(this JobDataMap map, string key, bool defaultValue = false)
    {
        if (map.ContainsKey(key) && map[key] is bool boolValue)
        {
            return boolValue;
        }
        
        return defaultValue;
    }
}
