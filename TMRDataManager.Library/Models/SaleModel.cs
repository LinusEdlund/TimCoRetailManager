using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMRDataManager.Library.Models;

namespace TRMDataManager.Models
{
    public class SaleModel
    {
        // inte insilsinget för han vill vet om det är null
        public List<SaleDetailModel> SaleDetails { get; set; }
    }
}