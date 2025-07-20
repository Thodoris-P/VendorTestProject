using FileLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class SupplierToVendorDto
{
    public static IEnumerable<VendorDto> MapMany(IEnumerable<Supplier> suppliers)
    {
        return suppliers.Select(s => new VendorDto
        {
            Id = s.Id,
            Name = s.Name,
            Address = s.Address
        });
    }
    
    public static VendorDto Map(Supplier supplier) =>
        new()
        {
            Id = supplier.Id,
            Name = supplier.Name,
            Address = supplier.Address
        };
}