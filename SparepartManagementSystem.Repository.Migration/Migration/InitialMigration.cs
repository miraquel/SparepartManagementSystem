using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace SparepartManagementSystem.Repository.Migration.Migration;

[Migration(20022024)]
[ExcludeFromCodeCoverage]
public class InitialMigration : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Roles")
            .WithColumn("RoleId").AsInt32().PrimaryKey().Identity()
            .WithColumn("RoleName").AsString(50).NotNullable().Unique("IX_RoleName")
            .WithColumn("Description").AsString(100).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.Table("Permissions")
            .WithColumn("PermissionId").AsInt32().PrimaryKey().Identity()
            .WithColumn("RoleId").AsInt32().NotNullable().ForeignKey("FK_Permissions_Roles", "Roles", "RoleId")
            .WithColumn("Module").AsString(50).NotNullable()
            .WithColumn("Type").AsString(50).NotNullable()
            .WithColumn("PermissionName").AsString(50).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.UniqueConstraint("IX_Permission")
            .OnTable("Permissions").Columns("RoleId", "PermissionName");
        
        Create.Table("Users")
            .WithColumn("UserId").AsInt32().PrimaryKey().Identity()
            .WithColumn("Username").AsString(50).NotNullable().Unique("IX_UserName")
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("LastName").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable().Unique("IX_Email")
            .WithColumn("IsAdministrator").AsBoolean().NotNullable()
            .WithColumn("IsEnabled").AsBoolean().NotNullable()
            .WithColumn("LastLogin").AsDateTime().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();

        Create.Table("UserWarehouses")
            .WithColumn("UserWarehouseId").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("FK_UserWarehouses_Users", "Users", "UserId").OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("InventLocationId").AsString(50).NotNullable()
            .WithColumn("InventSiteId").AsString(50).NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("IsDefault").AsBoolean().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.UniqueConstraint("IX_UserWarehouse")
            .OnTable("UserWarehouses").Columns("UserId", "InventLocationId");
        
        // Create.UniqueConstraint("IX_Email")
        //     .OnTable("Users").Columns("Email");
        //
        // Create.UniqueConstraint("IX_UserName")
        //     .OnTable("Users").Columns("Username");
        
        // Create.UniqueConstraint("IX_RoleName")
        //     .OnTable("Roles").Columns("RoleName");

        Create.Table("UserRoles")
            .WithColumn("UserId").AsInt32().NotNullable().PrimaryKey("PK_UserRoles").ForeignKey("FK_UserRoles_UserId", "Users", "UserId")
            .WithColumn("RoleId").AsInt32().NotNullable().PrimaryKey("PK_UserRoles").ForeignKey("FK_UserRoles_RoleId", "Roles", "RoleId");
        
        // Create.PrimaryKey("PK_UserRoles")
        //     .OnTable("UserRoles").Columns("UserId", "RoleId");
        
        // Create.ForeignKey("FK_UserRoles_RoleId")
        //     .FromTable("UserRoles").ForeignColumn("RoleId")
        //     .ToTable("Roles").PrimaryColumn("RoleId");
        //
        // Create.ForeignKey("FK_UserRoles_UserId")
        //     .FromTable("UserRoles").ForeignColumn("UserId")
        //     .ToTable("Users").PrimaryColumn("UserId");
        
        Create.Table("NumberSequences")
            .WithColumn("NumberSequenceId").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("Description").AsString(100).NotNullable()
            .WithColumn("Module").AsString(200).NotNullable()
            .WithColumn("Format").AsString(50).NotNullable()
            .WithColumn("LastNumber").AsInt32().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.Table("RefreshTokens")
            .WithColumn("RefreshTokenId").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("FK_RefreshTokens_Users1", "Users", "UserId")
            .WithColumn("Token").AsString(512).NotNullable()
            .WithColumn("Created").AsDateTime().NotNullable()
            .WithColumn("Expires").AsDateTime().NotNullable()
            .WithColumn("Revoked").AsDateTime().NotNullable()
            .WithColumn("ReplacedByToken").AsString(512).NotNullable();
        
        // Create.ForeignKey("FK_RefreshTokens_Users1")
        //     .FromTable("RefreshTokens").ForeignColumn("UserId")
        //     .ToTable("Users").PrimaryColumn("UserId");
        
        Create.Table("GoodsReceiptHeaders")
            .WithColumn("GoodsReceiptHeaderId").AsInt32().PrimaryKey().Identity()
            .WithColumn("PackingSlipId").AsString(50).NotNullable().Unique("IX_PackingSlipId")
            .WithColumn("TransDate").AsDateTime().NotNullable()
            .WithColumn("Description").AsString(50).NotNullable()
            .WithColumn("PurchId").AsString(50).NotNullable()
            .WithColumn("PurchName").AsString(50).NotNullable()
            .WithColumn("OrderAccount").AsString(50).NotNullable()
            .WithColumn("InvoiceAccount").AsString(50).NotNullable()
            .WithColumn("PurchStatus").AsString(50).NotNullable()
            .WithColumn("IsSubmitted").AsBoolean().NotNullable()
            .WithColumn("SubmittedDate").AsDateTime().NotNullable()
            .WithColumn("SubmittedBy").AsString(50).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        // Create.UniqueConstraint("IX_PackingSlipId")
        //     .OnTable("GoodsReceiptHeaders").Columns("PackingSlipId");
        
        Create.Table("GoodsReceiptLines")
            .WithColumn("GoodsReceiptLineId").AsInt32().PrimaryKey().Identity()
            .WithColumn("GoodsReceiptHeaderId").AsInt32().NotNullable().ForeignKey("FK_GoodsReceiptLines_GoodsReceiptHeaders", "GoodsReceiptHeaders", "GoodsReceiptHeaderId")
            .WithColumn("ItemId").AsString(50).NotNullable()
            .WithColumn("LineNumber").AsInt32().NotNullable()
            .WithColumn("ItemName").AsString(50).NotNullable()
            .WithColumn("ProductType").AsInt32().NotNullable()
            .WithColumn("RemainPurchPhysical").AsDecimal().NotNullable()
            .WithColumn("ReceiveNow").AsDecimal().NotNullable()
            .WithColumn("PurchQty").AsDecimal().NotNullable()
            .WithColumn("PurchUnit").AsString(50).NotNullable()
            .WithColumn("PurchPrice").AsDecimal().NotNullable()
            .WithColumn("LineAmount").AsDecimal().NotNullable()
            .WithColumn("InventLocationId").AsString(50).NotNullable()
            .WithColumn("WMSLocationId").AsString(50).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        // Create.ForeignKey("FK_GoodsReceiptLines_GoodsReceiptHeaders")
        //     .FromTable("GoodsReceiptLines").ForeignColumn("GoodsReceiptHeaderId")
        //     .ToTable("GoodsReceiptHeaders").PrimaryColumn("GoodsReceiptHeaderId");
        
        Create.UniqueConstraint("IX_GoodsReceiptLines_LineNumber")
            .OnTable("GoodsReceiptLines").Columns("GoodsReceiptHeaderId", "LineNumber");
        
        Create.Table("RowLevelAccesses")
            .WithColumn("RowLevelAccessId").AsInt32().PrimaryKey().Identity()
            .WithColumn("UserId").AsInt32().NotNullable().ForeignKey("FK_RowLevelAccess_Users", "Users", "UserId")
            .WithColumn("AxTable").AsInt32().NotNullable()
            .WithColumn("Query").AsString(50).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        // Create.ForeignKey("FK_RowLevelAccess_Users")
        //     .FromTable("RowLevelAccesses").ForeignColumn("UserId")
        //     .ToTable("Users").PrimaryColumn("UserId");
        
        Create.Table("WorkOrderHeaders")
            .WithColumn("WorkOrderHeaderId").AsInt32().PrimaryKey().Identity()
            .WithColumn("IsSubmitted").AsBoolean().NotNullable()
            .WithColumn("SubmittedDate").AsDateTime().NotNullable()
            .WithColumn("AGSEAMWOID").AsString(50).NotNullable().Unique()
            .WithColumn("AGSEAMWRID").AsString(50).NotNullable()
            .WithColumn("AGSEAMEntityID").AsString(50).NotNullable()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("HeaderTitle").AsString(50).NotNullable()
            .WithColumn("AGSEAMPriorityID").AsString(50).NotNullable()
            .WithColumn("AGSEAMWOTYPE").AsString(50).NotNullable()
            .WithColumn("AGSEAMWOStatusID").AsString(50).NotNullable()
            .WithColumn("AGSEAMPlanningStartDate").AsDateTime().NotNullable()
            .WithColumn("AGSEAMPlanningEndDate").AsDateTime().NotNullable()
            .WithColumn("EntityShutDown").AsInt32().NotNullable()
            .WithColumn("WOCloseDate").AsDateTime().NotNullable()
            .WithColumn("AGSEAMSuspend").AsInt32().NotNullable()
            .WithColumn("Notes").AsString(int.MaxValue).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();

        Create.Table("WorkOrderLines")
            .WithColumn("WorkOrderLineId").AsInt32().PrimaryKey().Identity()
            .WithColumn("WorkOrderHeaderId").AsInt32().NotNullable().ForeignKey("FK_WorkOrderLines_WorkOrderHeaders", "WorkOrderHeaders", "WorkOrderHeaderId")
            .WithColumn("Line").AsInt32().NotNullable()
            .WithColumn("LineTitle").AsString(60).NotNullable()
            .WithColumn("EntityId").AsString(50).NotNullable()
            .WithColumn("EntityShutdown").AsInt32().NotNullable()
            .WithColumn("WorkOrderType").AsString(25).NotNullable()
            .WithColumn("TaskId").AsString(25).NotNullable()
            .WithColumn("Condition").AsString(20).NotNullable()
            .WithColumn("PlanningStartDate").AsDateTime().NotNullable()
            .WithColumn("PlanningEndDate").AsDateTime().NotNullable()
            .WithColumn("Supervisor").AsString(20).NotNullable()
            .WithColumn("CalendarId").AsString(20).NotNullable()
            .WithColumn("WorkOrderStatus").AsString(20).NotNullable()
            .WithColumn("Suspend").AsInt32().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.Table("ItemRequisitions")
            .WithColumn("ItemRequisitionId").AsInt32().PrimaryKey().Identity()
            .WithColumn("WorkOrderLineId").AsInt32().NotNullable().ForeignKey("FK_ItemRequisitions_WorkOrderLine", "WorkOrderLines", "WorkOrderLineId")
            .WithColumn("ItemId").AsString(50).NotNullable()
            .WithColumn("ItemName").AsString(50).NotNullable()
            .WithColumn("RequiredDate").AsDateTime().NotNullable()
            .WithColumn("Quantity").AsDecimal().NotNullable()
            .WithColumn("RequestQuantity").AsDecimal().NotNullable()
            .WithColumn("InventLocationId").AsString(50).NotNullable()
            .WithColumn("WMSLocationId").AsString(50).NotNullable()
            .WithColumn("JournalId").AsString(50).NotNullable()
            .WithColumn("IsSubmitted").AsBoolean().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        // Create.ForeignKey("FK_WorkOrderLines_WorkOrderHeaders")
        //     .FromTable("WorkOrderLines").ForeignColumn("WorkOrderHeaderId")
        //     .ToTable("WorkOrderHeaders").PrimaryColumn("WorkOrderHeaderId");
    }

    public override void Down()
    {
        Delete.Table("ItemRequisitions");
        Delete.Table("WorkOrderLines");
        Delete.Table("WorkOrderHeaders");
        Delete.Table("RowLevelAccesses");
        Delete.Table("GoodsReceiptLines");
        Delete.Table("GoodsReceiptHeaders");
        Delete.Table("RefreshTokens");
        Delete.Table("NumberSequences");
        Delete.Table("UserRoles");
        Delete.Table("UserWarehouses");
        Delete.Table("Users");
        Delete.Table("Permissions");
        Delete.Table("Roles");
    }
}