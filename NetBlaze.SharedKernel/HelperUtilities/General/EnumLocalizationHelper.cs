using Microsoft.Extensions.Localization;
using NetBlaze.SharedKernel.SharedResources;

namespace NetBlaze.SharedKernel.HelperUtilities.General
{
    public static class EnumLocalizationHelper
    {
        public static string LocalizeEnum<TEnum>(
            TEnum value,
            IStringLocalizer<Messages> localizer)
            where TEnum : Enum
        {
            var key = $"{typeof(TEnum).Name}_{value}";
            return localizer[key];
        }
    }
}
