using WebApi.Contracts;

namespace WebApi.Services.Abstractions;

public interface IVendorLoader
{
    Task<IEnumerable<VendorDto>> GetAllAsync();
    Task<VendorDto> GetByIdAsync(string vendorId);
    Task<VendorDto> CreateAsync(CreateVendorRequest newVendor);
    Task DeleteAsync(string vendorId);
    Task UpdateAsync(VendorDto updatedVendor);
}