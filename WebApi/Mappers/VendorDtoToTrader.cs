using SqlServerLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class VendorDtoToTrader
{
    public static Trader Map(VendorDto updatedVendor)
    {
        if (updatedVendor == null)
        {
            throw new ArgumentNullException(nameof(updatedVendor), "VendorDto cannot be null");
        }

        return new Trader
        {
            Code = updatedVendor.Id,
            Description = updatedVendor.Name,
            Street = updatedVendor.Address
        };
    }
}