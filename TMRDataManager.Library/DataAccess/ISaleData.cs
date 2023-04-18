using System.Collections.Generic;
using TRMDataManager.Models;

namespace TMRDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        List<SaleReportModel> GetSaleReport();
        void SaveSale(SaleModel saleInfo, string cashierId);
    }
}