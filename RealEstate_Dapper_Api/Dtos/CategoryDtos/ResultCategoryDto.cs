namespace RealEstate_Dapper_Api.Dtos.CategoryDtos
{
    public class ResultCategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool CategoryStatus { get; set; }
    }
}
