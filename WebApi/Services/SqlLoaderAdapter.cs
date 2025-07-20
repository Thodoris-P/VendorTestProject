using WebApi.Contracts;
using WebApi.Exceptions;
using WebApi.Mappers;
using WebApi.Services.Abstractions;
using SqlServerLoader;

namespace WebApi.Services;

public class SqlLoaderAdapter : IVendorLoader
{
    private readonly DataLoader _loader;
    private readonly ILogger<SqlLoaderAdapter> _logger;

    public SqlLoaderAdapter(DataLoader loader, ILogger<SqlLoaderAdapter> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        _loader = loader ?? throw new ArgumentNullException(nameof(loader), "Loader cannot be null");
    }
    
    public async Task<IEnumerable<VendorDto>> GetAllAsync()
    {
        var traders = await _loader.LoadTraders();
        return TraderToVendorDto.MapMany(traders);
    }

    public async Task<VendorDto> GetByIdAsync(string vendorId)
    {
        try
        {
            var trader = await _loader.LoadTrader(vendorId);
            var vendorDto = TraderToVendorDto.Map(trader);
            return vendorDto;
        }
        catch (Exception e)
        {
            var msg = e.Message.ToLowerInvariant();
            if (msg.Contains("not found"))
            {
                throw new VendorNotFoundException(vendorId, e);
            }
            throw;
        }
    }

    public async Task<VendorDto> CreateAsync(CreateVendorRequest newVendor)
    {
        var trader = CreateVendorRequestToTrader.Map(newVendor);
        trader.Code = Guid.NewGuid().ToString();

        try
        {
            await _loader.InsertTrader(trader);
        }
        catch (Exception e)
        {
            var msg = e.Message.ToLowerInvariant();
            if (msg.Contains("already exists"))
            {
                throw new ConflictingIdException(e.Message, e);
            }
            if (msg.Contains("are required"))
            {
                throw new BadRequestException(e.Message, e);
            }
            throw;
        }

        var vendorDto = TraderToVendorDto.Map(trader);
        return vendorDto;
    }

    public async Task DeleteAsync(string vendorId)
    {
        try
        {
            await _loader.DeleteTrader(vendorId);
        }
        catch (Exception e)
        {
            var msg = e.Message.ToLowerInvariant();
            if (msg.Contains("not found"))
            {
                throw new VendorNotFoundException(vendorId, e);
            }
            throw;
        }
    }

    public async Task UpdateAsync(VendorDto updatedVendor)
    {
        var trader = VendorDtoToTrader.Map(updatedVendor);
        
        try
        {
            await _loader.UpdateTrader(trader);
        }
        catch (Exception e)
        {
            var msg = e.Message.ToLowerInvariant();
            if (msg.Contains("not found"))
            {
                throw new VendorNotFoundException(e.Message, e);
            }
            throw;
        }
    }
}