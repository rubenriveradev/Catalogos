using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Products
    {

        public int IdProduct { get; set; }
        public int UserId { get; set; }
        public string DateCreated { get; set; }
        public string ItemID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Store { get; set; }
        public string Mattel { get; set; }
        public string UPC { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal WFS { get; set; }
        public decimal StorageFee_WFS { get; set; }
        public decimal FactorPoundFeeShipment { get; set; }
        
        public decimal Miscelaneous_Shipment { get; set; }
        public decimal PercentageCommission { get; set; }
        public decimal Commission { get; set; } 
        public decimal PoundFee_Shipment { get; set; }
        public decimal Profit { get; set; }
        public decimal ROI { get; set; }
        public decimal WFS_RefFee { get; set; }
        public string Observation { get; set; }
        public bool IsActive { get; set; }
        public int IdImage { get; set; }
        public byte[] ImageData { get; set; }



    }
}
