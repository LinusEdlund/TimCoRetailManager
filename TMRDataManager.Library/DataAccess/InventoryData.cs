using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMRDataManager.Library.Internal.DataAccess;

namespace TMRDataManager.Library.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration _config;

        public InventoryData(IConfiguration config)
        {
            _config = config;
        }
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            var output = sql.LoadData<InventoryModel, dynamic>("spInventory_GetAll", new { }, "TRMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel item)
        {
            SqlDataAccess sql = new SqlDataAccess(_config);

            sql.SaveData("dbo.spInventory_Insert", item, "TRMData");

        }
    }
}
