using SqlServerLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class TraderToVendorDto
{
    public static IEnumerable<VendorDto> MapMany(List<Trader> trader)
    {
        return trader.Select(t => new VendorDto
        {
            Id = t.Code,
            Name = t.Description,
            Address = t.Street
        });
    }
    
    public static VendorDto Map(Trader trader) =>
        new()
        {
            Id = trader.Code,
            Name = trader.Description,
            Address = trader.Street
        };
}
