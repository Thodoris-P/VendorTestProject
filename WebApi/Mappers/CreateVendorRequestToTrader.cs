using SqlServerLoader;
using WebApi.Contracts;

namespace WebApi.Mappers;

public static class CreateVendorRequestToTrader
{
    public static Trader Map(CreateVendorRequest request)
    {
        return new Trader
        {
            Description = request.Name,
            Street = request.Address
        };
    }
}