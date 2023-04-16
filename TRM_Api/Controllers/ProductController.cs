using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TMRDataManager.Library.DataAccess;
using TMRDataManager.Library.Models;

namespace TRM_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        // TODO: Det här är hur vi får producterna som vi vissar    
        [HttpGet]
        public List<ProductModel> Get()
        {
            ProductData data = new ProductData(_config);

            return data.GetProducts();
        }
    }
}
