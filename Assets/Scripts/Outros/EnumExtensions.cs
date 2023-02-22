using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Linq;

public static class EnumExtensions
{
    public static string GetTipoItemEnum<T>(this T item)
    {
        var enumMemberAttr = item.GetType()
            .GetField(item.ToString())
            .GetCustomAttributes(false)
            .OfType<EnumMemberAttribute>()
            .FirstOrDefault();

        return enumMemberAttr?.Value;
    }

}
