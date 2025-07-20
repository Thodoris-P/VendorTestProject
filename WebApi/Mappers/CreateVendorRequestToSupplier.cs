using FileLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class CreateVendorRequestToSupplier
{
    public static Supplier Map(CreateVendorRequest vendorDto)
    {
        if (vendorDto == null)
        {
            throw new ArgumentNullException(nameof(vendorDto), "VendorDto cannot be null");
        }

        return new Supplier
        {
            Name = vendorDto.Name,
            Address = vendorDto.Address
        };
    }
}