using FileLoader;
using WebApi.Contracts;
using WebApi.Exceptions;
using WebApi.Mappers;
using WebApi.Services.Abstractions;

namespace WebApi.Services;

public class FileLoaderAdapter : IVendorLoader
{
    private readonly Loader _loader;
    private readonly ILogger<FileLoaderAdapter> _logger;

    public FileLoaderAdapter(Loader loader, ILogger<FileLoaderAdapter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        _loader = loader ?? throw new ArgumentNullException(nameof(loader), "Loader cannot be null");
    }
    
    public Task<IEnumerable<VendorDto>> GetAllAsync()
    {
        var suppliers =  _loader.LoadSuppliers();
        var vendorDtos = SupplierToVendorDto.MapMany(suppliers);
        return Task.FromResult(vendorDtos);
    }

    public Task<VendorDto> GetByIdAsync(string vendorId)
    {
        try
        {
            var supplier = _loader.LoadSupplier(vendorId);
            var vendorDto = SupplierToVendorDto.Map(supplier);
            return Task.FromResult(vendorDto);
        }
        catch (ApiException ex)
        {
            var msg = ex.Message.ToLowerInvariant();
            if (msg.Contains("supplier not found"))
            {
                throw new VendorNotFoundException(vendorId);
            }

            throw;
        }
    }

    public Task<VendorDto> CreateAsync(CreateVendorRequest newVendor)
    {
        var supplier = CreateVendorRequestToSupplier.Map(newVendor);
        supplier.Id = Guid.NewGuid().ToString();

        try
        {
            _loader.InsertSupplier(supplier);
        }
        catch (ApiException ex)
        {
            var msg = ex.Message.ToLowerInvariant();

            if (msg.Contains("already exists"))
            {
                throw new ConflictingIdException(ex.Message, ex);
            }
            if (msg.Contains("are required"))
            {
                throw new BadRequestException(ex.Message, ex);
            }
            throw;
        }
        
        var vendorDto = SupplierToVendorDto.Map(supplier);
        return Task.FromResult(vendorDto);
    }

    public Task DeleteAsync(string vendorId)
    {
        try
        {
            _loader.DeleteSupplier(vendorId);
        }
        catch (ApiException ex)
        {
            var msg = ex.Message.ToLowerInvariant();
            if (msg.Contains("supplier not found"))
            {
                throw new VendorNotFoundException(vendorId);
            }

            throw;
        }

        return Task.CompletedTask;
    }

    public Task UpdateAsync(VendorDto updatedVendor)
    {
        var supplier = VendorDtoToSupplier.Map(updatedVendor);

        try
        {
            _loader.UpdateSupplier(supplier);
        }
        catch (ApiException ex)
        {
            var msg = ex.Message.ToLowerInvariant();
            if (msg.Contains("supplier not found"))
            {
                throw new VendorNotFoundException(updatedVendor.Id);
            }
            
            throw;
        }

        return Task.CompletedTask;
    }
}