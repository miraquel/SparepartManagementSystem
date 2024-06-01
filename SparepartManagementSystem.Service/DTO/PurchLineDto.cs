using SparepartManagementSystem.Domain.Enums;

namespace SparepartManagementSystem.Service.DTO;

public class PurchLineDto
{
    public string ItemId { get; init; } = string.Empty;
    public int LineNumber { get; init; }
    public string ItemName { get; init; } = string.Empty;
    public ProductType ProductType { get; init; }
    public decimal RemainPurchPhysical { get; init; }
    public decimal PurchQty { get; init; }
    public string PurchUnit { get; init; } = string.Empty;
    public decimal PurchPrice { get; init; }
    public decimal LineAmount { get; init; }
}