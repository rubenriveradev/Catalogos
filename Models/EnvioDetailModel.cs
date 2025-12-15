using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class EnvioDetailModel
    {
        public int Nro { get; set; }
        public int IdDespacho { get; set; }
        public int IdDetalle { get; set; }
        public string FechaDespacho { get; set; }
        public int IdProduct { get; set; }
        public string ItemID { get; set; }
        public string UPC { get; set; }
        public string ProductName { get; set; }
        public string Store { get; set; }
        public string Mattel { get; set; }
        public decimal Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal WFS { get; set; }
        public decimal StorageFee_WFS { get; set; }
        public decimal Miscelaneous_Shipment { get; set; }
        public decimal PercentageCommission { get; set; }
        public decimal FactorPoundFeeShipment { get; set; }
        public bool IsActive { get; set; }       
        public decimal Commission { get; set; }
        public decimal PoundFee_Shipment { get; set; }
        public decimal Profit { get; set; }
        public decimal ROI { get; set; }
        public decimal WFS_RefFee { get; set; }
        public int IdEstado { get; set; }
        public int IdImage { get; set; }
        public string FechaCreacion { get; set; }
        public string Estado { get; set; }
    }
}
