using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RealEstate_Dapper_UI.Dtos.ProductDetailDtos;
using RealEstate_Dapper_UI.Dtos.ProductDtos;

namespace RealEstate_Dapper_UI.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PropertyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44373/api/Products/ProductListWithCategory");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        public async Task<IActionResult> PropertyListWithSearch(string searchKeyValue, int propertyCategoryID, string city)
        {
            searchKeyValue = TempData["searchKeyValue"].ToString();
            propertyCategoryID = int.Parse(TempData["propertyCategoryID"].ToString());
            city = TempData["city"].ToString();
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:44373/api/Products/ResultProductWithSearchList?searchKeyValue={searchKeyValue}&propertyCategoryID={propertyCategoryID}&city={city}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultProductWithSearchListDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet("property/{slug}/{id}")]
        public async Task<IActionResult> PropertySingle(string slug, int id)
        {
            ViewBag.i = id;
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44373/api/Products/GetProductByProductID?id=" + id);
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResultProductDto>(jsonData);

            var responseMessage2 = await client.GetAsync("https://localhost:44373/api/ProductDetails/GetProductDetailByProductID?id=" + id);
            var jsonData2 = await responseMessage2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<GetProductDetailByIdDto>(jsonData2);

            if (values2 == null)
            {
                // Null kontrolü burada yapılır
                values2 = new GetProductDetailByIdDto
                {
                    bathCount = 0,
                    bedRoomCount = 0,
                    productSize = 0,
                    roomCount = 0,
                    garageSize = 0,
                    buildYear = DateTime.Now.ToString(),
                    location = string.Empty,
                    videoUrl = string.Empty
                };
            }

            ViewBag.productID = values.productID;
            ViewBag.title1 = values.title.ToString();
            ViewBag.price = values.price;
            ViewBag.city = values.city;
            ViewBag.district = values.district;
            ViewBag.address = values.address;
            ViewBag.type = values.type;
            ViewBag.description = values.description;
            ViewBag.slugUrl = values.SlugUrl;

            ViewBag.bathCount = values2.bathCount;
            ViewBag.badRoomCount = values2.bedRoomCount;
            ViewBag.size = values2.productSize;
            ViewBag.roomCount = values2.roomCount;
            ViewBag.garageCount = values2.garageSize;
            ViewBag.buildYear = values2.buildYear;
            ViewBag.date = values.AdvertisementDate;
            ViewBag.location = values2.location;
            ViewBag.videoUrl = values2.videoUrl;

            TimeSpan timeSpan = DateTime.Now - values.AdvertisementDate;

            ViewBag.dateDiff = timeSpan.Days.ToString();

            string slugFromTitle = CreateSlug(values.title);
            ViewBag.slugUrl = slugFromTitle;

            return View();
        }

        private string CreateSlug(string title)
        {
            title = title.ToLowerInvariant();//Küçük Harfe Çevirir
            title = title.Replace(" ", "-"); // Boşlukları - işareti ile değiştir
            title = System.Text.RegularExpressions.Regex.Replace(title, @"[^a-z0-9\s-]", ""); // Geçersiz karakterleri kaldır
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s+"," ").Trim(); // Birden fazla boşluğu tek boşluğa indirir ve kenar boşluklarını kaldırır.
            title = System.Text.RegularExpressions.Regex.Replace(title, @"\s", "-"); // Boşlukları - ile değiştirir

            return title;
        }
    }
}
