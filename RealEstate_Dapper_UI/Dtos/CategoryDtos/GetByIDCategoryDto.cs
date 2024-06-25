namespace RealEstate_Dapper_UI.Dtos.CategoryDtos
{
    public class GetByIDCategoryDto
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool CategoryStatus { get; set; }
    }
}
