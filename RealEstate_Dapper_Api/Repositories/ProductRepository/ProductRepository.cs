using Dapper;
using RealEstate_Dapper_Api.Dtos.CategoryDtos;
using RealEstate_Dapper_Api.Dtos.ProductDetailDtos;
using RealEstate_Dapper_Api.Dtos.ProductDtos;
using RealEstate_Dapper_Api.Models.DapperContext;

namespace RealEstate_Dapper_Api.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;
        public ProductRepository(Context context)
        {
            _context = context;
        }

        public async Task CreateProduct(CreateProductDto createProductDto)
        {
            string query = "insert into Product (Title, Price, City,District,CoverImage,Address,Description,Type,DealOfTheDay,AdvertisementDate,ProductStatus,ProductCategory,EmployeeID) values (@Title, @Price, @City,@District,@CoverImage,@Address,@Description,@Type,@DealOfTheDay,@AdvertisementDate,@ProductStatus,@ProductCategory,@EmployeeID)";
            var parameters = new DynamicParameters();
            parameters.Add("@Title", createProductDto.Title);
            parameters.Add("@Price", createProductDto.Price);
            parameters.Add("@City", createProductDto.City);
            parameters.Add("@District", createProductDto.District);
            parameters.Add("@CoverImage", createProductDto.CoverImage);
            parameters.Add("@Address", createProductDto.Address);
            parameters.Add("@Description", createProductDto.Description);
            parameters.Add("@Type", createProductDto.Type);
            parameters.Add("@DealOfTheDay", createProductDto.DealOfTheDay);
            parameters.Add("@AdvertisementDate", createProductDto.AdvertisementDate);
            parameters.Add("@ProductStatus", createProductDto.ProductStatus);
            parameters.Add("@ProductCategory", createProductDto.ProductCategory);
            parameters.Add("@EmployeeID", createProductDto.EmployeeID);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            string query = "Select * From Product";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResultProductWithCategoryDto>> GetAllProductWithCategoryAsync()
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName, CoverImage, Type, Address, DealOfTheDay, SlugUrl From Product inner join Category on Product.ProductCategory=Category.CategoryID";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithCategoryDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResultLast3ProductWithCategoryDto>> GetLast3ProductAsync()
        {
            string query = "Select Top(3) ProductID, Title, Price, City, District, ProductCategory, CategoryName, AdvertisementDate,Description, CoverImage From Product Inner Join Category on Product.ProductCategory=Category.CategoryID Order By ProductID Desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultLast3ProductWithCategoryDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResultLast5ProductWithCategoryDto>> GetLast5ProductAsync()
        {
            string query = "Select Top(5) ProductID, Title, Price, City, District, ProductCategory, CategoryName, AdvertisementDate From Product Inner Join Category on Product.ProductCategory=Category.CategoryID Where Type = 'Kiralık' Order By ProductID Desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultLast5ProductWithCategoryDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResultProductAdvertListWithCategoryByEmployeeDto>> GetProductAdvertListByEmployeeAsyncByFalse(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName, CoverImage, Type, Address, DealOfTheDay From Product inner join Category on Product.ProductCategory=Category.CategoryID Where EmployeeID = @employeeId And ProductStatus=0";
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductAdvertListWithCategoryByEmployeeDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<List<ResultProductAdvertListWithCategoryByEmployeeDto>> GetProductAdvertListByEmployeeAsyncByTrue(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName, CoverImage, Type, Address, DealOfTheDay From Product inner join Category on Product.ProductCategory=Category.CategoryID Where EmployeeID = @employeeId And ProductStatus=1";
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductAdvertListWithCategoryByEmployeeDto>(query,parameters);
                return values.ToList();
            }
        }

        public async Task<GetProductByProductIDDto> GetProductByProductID(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,Description, CategoryName, CoverImage, Type, Address, DealOfTheDay, AdvertisementDate, SlugUrl From Product inner join Category on Product.ProductCategory=Category.CategoryID where ProductID = @productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<GetProductByProductIDDto>(query, parameters);
                return values.FirstOrDefault();
            }
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductDealOfTheDayProductTrueWithCategoryAsync()
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName, CoverImage, Type, Address, DealOfTheDay From Product inner join Category on Product.ProductCategory=Category.CategoryID Where DealOfTheDay=1";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithCategoryDto>(query);
                return values.ToList();
            }
        }

        public async Task<GetProductDetailByIdDto> GetProductDetailByProductID(int id)
        {
            string query = "Select * From ProductDetails Where ProductDetailID = @productDetailId";
            var parameters = new DynamicParameters();
            parameters.Add("@productDetailId", id);

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<GetProductDetailByIdDto>(query, parameters);
                return values.FirstOrDefault();
            }
        }

        public async Task ProductDealOfTheDatStatusChangeToFalse(int id)
        {
            string query = "Update Product Set DealOfTheDay=0 Where ProductID= @productID";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task ProductDealOfTheDatStatusChangeToTrue(int id)
        {
            string query = "Update Product Set DealOfTheDay=1 Where ProductID= @productID";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ResultProductWithSearchListDto>> ResultProductWithSearchList(string searchKeyValue, int propertyCategoryID, string city)
        {
            string query = "Select * From Product Where Title like '%" + @searchKeyValue + "%' And ProductCategory = @propertyCategoryID and City = @city";
            var parameters = new DynamicParameters();
            //parameters.Add("@searchKeyValue", searchKeyValue);
            parameters.Add("@propertyCategoryID", propertyCategoryID);
            parameters.Add("@city", city);

            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithSearchListDto>(query, parameters);
                return values.ToList();
            }
        }
    }
}
