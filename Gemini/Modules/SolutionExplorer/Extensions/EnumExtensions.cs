using System;
using System.ComponentModel;
using System.Linq;

namespace FactrIDE.Gemini.Modules.SolutionExplorer.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum @enum)
        {
            var genericEnumType = @enum.GetType();
            var memberInfo = genericEnumType.GetMember(@enum.ToString());
            if (memberInfo?.Length > 0)
            {
                var _attribs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (_attribs?.Count() > 0)
                    return ((DescriptionAttribute) _attribs[0]).Description;
            }
            return @enum.ToString();
        }
    }
}