namespace SparepartManagementSystem.Service.DTO;

public class VendPackingSlipJourDto
{
    public string PurchId { get; set; } = string.Empty;
    public string PackingSlipId { get; set; } = string.Empty;
    public string InternalPackingSlipId { get; set; } = string.Empty;
    public DateTime DeliveryDate { get; set; }
    public ICollection<VendPackingSlipTransDto> VendPackingSlipTrans { get; set; } = [];
}