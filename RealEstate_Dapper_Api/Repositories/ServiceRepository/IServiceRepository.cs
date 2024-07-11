using RealEstate_Dapper_Api.Dtos.ServiceDtos;

namespace RealEstate_Dapper_Api.Repositories.ServiceRepository
{
    public interface IServiceRepository
    {
        Task<List<ResultServiceDto>> GetAllService();
        Task CreateService(CreateServiceDto serviceDto);

        Task DeleteService(int id);

        Task UpdateService(UpdateServiceDto serviceDto);

        Task<GetByIDServiceDto> GetService(int id);
    }
}
