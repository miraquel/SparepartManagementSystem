using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Service.GMKSMSServiceGroup;

namespace SparepartManagementSystem.Service.DTO;

public class PurchLineDto
{
    public string ItemId { get; init; } = "";
    public int LineNumber { get; init; }
    public string ItemName { get; init; } = "";
    public ProductType ProductType { get; init; }
    public decimal RemainPurchPhysical { get; init; }
    public decimal PurchQty { get; init; }
    public string PurchUnit { get; init; } = "";
    public decimal PurchPrice { get; init; }
    public decimal LineAmount { get; init; }
}