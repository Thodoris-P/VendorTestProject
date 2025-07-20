using FileLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class VendorDtoToSupplier
{
    public static Supplier Map(VendorDto vendorDto)
    {
        if (vendorDto == null)
        {
            throw new ArgumentNullException(nameof(vendorDto), "VendorDto cannot be null");
        }

        return new Supplier
        {
            Id = vendorDto.Id,
            Name = vendorDto.Name,
            Address = vendorDto.Address
        };
    }
}