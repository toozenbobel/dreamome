using Common.Helpers;
using WeatherAlerts.Models;
using WeatherAlerts.Models.Http;

namespace WeatherAlerts.Services;

internal static class MeteoWarningMapper
{
    public static MeteoWarning Map(MeteoWarningItem entity)
    {
        var startsAt = DateTimeHelper.FromUnixTimeToLocal(entity.From);
        var endsAt = DateTimeHelper.FromUnixTimeToLocal(entity.To);

        var type = MeteoWarningType.General;
        var level = MeteoWarningLevel.Trivial;

        var iconString = entity.IconId;

        if (string.IsNullOrWhiteSpace(iconString))
        {
            return new MeteoWarning
            {
                StartsAt = startsAt,
                EndsAt = endsAt,
                Description = entity.Description,
                Name = entity.Name,
                Type = type,
                Level = level
            };
        }
        
        if (iconString == "000" || iconString == "001")
        {
            level = MeteoWarningLevel.Trivial;
            type = MeteoWarningType.None;
        }
        else
        {
            var firstDigit = iconString[..1];

            switch (firstDigit)
            {
                case "0":
                case "1":
                    level = MeteoWarningLevel.Trivial;
                    break;

                case "2":
                    level = MeteoWarningLevel.Yellow;
                    break;

                case "3":
                    level = MeteoWarningLevel.Orange;
                    break;

                case "4":
                    level = MeteoWarningLevel.Red;
                    break;
            }

            var typeValue = iconString.Substring(1);

            if (int.TryParse(typeValue, out var typeId))
            {
                type = (MeteoWarningType)typeId;
            }
        }
        
        return new MeteoWarning
        {
            StartsAt = startsAt,
            EndsAt = endsAt,
            Description = entity.Description,
            Name = entity.Name,
            Type = type,
            Level = level
        };
    }
}