using System.Collections.Generic;
using TMRDataManager.Library.Models;

namespace TMRDataManager.Library.DataAccess
{
    public interface IProductData
    {
        ProductModel GetProductById(int productId);
        List<ProductModel> GetProducts();
    }
}