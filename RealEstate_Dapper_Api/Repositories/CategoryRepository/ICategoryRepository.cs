using RealEstate_Dapper_Api.Dtos.CategoryDtos;
using RealEstate_Dapper_Api.Dtos.WhoWeAreDetailDtos;

namespace RealEstate_Dapper_Api.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<List<ResultCategoryDto>> GetAllCategory();
        Task CreateCategory(CreateCategoryDto categoryDto);

        Task DeleteCategory(int id);

        Task UpdateCategory(UpdateCategoryDto categoryDto);

        Task<GetByIDCategoryDto> GetCategory(int id);
    }
}
