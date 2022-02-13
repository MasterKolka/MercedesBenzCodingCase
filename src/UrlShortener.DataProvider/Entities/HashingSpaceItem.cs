using Microsoft.EntityFrameworkCore;
using UrlShortener.DataProvider.Entities.Base;

namespace UrlShortener.DataProvider.Entities;

[Index(nameof(RangeStart), nameof(RangeEnd))]
public class HashingSpaceItem: Entity
{
    public long RangeStart { get; set; }
    public long RangeEnd { get; set; }
}
