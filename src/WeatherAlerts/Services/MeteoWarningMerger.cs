using WeatherAlerts.Models;

namespace WeatherAlerts.Services;

internal class MeteoWarningMerger : IMeteoWarningMerger
{
    public IEnumerable<MeteoWarning> Merge(IEnumerable<MeteoWarning> items)
    {
        foreach (var group in items.GroupBy(x => new {x.Name, x.Type}))
        {
            if (group.Count() == 1)
            {
                yield return group.Single();
            }
            else
            {
                var minDate = group.Min(x => x.StartsAt);
                var maxDate = group.Max(x => x.EndsAt);
                var maxLevel = group.Max(x => x.Level);
                var name = group.Key.Name;
                var type = group.Key.Type;
                var description = group.First(x => x.Level == maxLevel).Description;

                yield return new MeteoWarning
                {
                    StartsAt = minDate,
                    EndsAt = maxDate,
                    Type = type,
                    Name = name,
                    Description = description,
                    Level = maxLevel
                };
            }
        }
    }
}