using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace SparepartManagementSystem.Repository.Migration.Migration;

[Migration(20022024)]
[ExcludeFromCodeCoverage]
public class InitialMigration : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Permissions")
            .WithColumn("PermissionId").AsInt32().PrimaryKey().Identity()
            .WithColumn("RoleId").AsInt32().NotNullable()
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
            .WithColumn("Username").AsString(50).NotNullable()
            .WithColumn("FirstName").AsString(50).NotNullable()
            .WithColumn("LastName").AsString(50).NotNullable()
            .WithColumn("Email").AsString(50).NotNullable()
            .WithColumn("IsAdministrator").AsBoolean().NotNullable()
            .WithColumn("IsEnabled").AsBoolean().NotNullable()
            .WithColumn("LastLogin").AsDateTime().NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.UniqueConstraint("IX_Email")
            .OnTable("Users").Columns("Email");
        
        Create.UniqueConstraint("IX_UserName")
            .OnTable("Users").Columns("Username");
        
        Create.Table("Roles")
            .WithColumn("RoleId").AsInt32().PrimaryKey().Identity()
            .WithColumn("RoleName").AsString(50).NotNullable()
            .WithColumn("Description").AsString(100).NotNullable()
            .WithColumn("CreatedBy").AsString(50).NotNullable()
            .WithColumn("CreatedDateTime").AsDateTime().NotNullable()
            .WithColumn("ModifiedBy").AsString(50).NotNullable()
            .WithColumn("ModifiedDateTime").AsDateTime().NotNullable();
        
        Create.UniqueConstraint("IX_RoleName")
            .OnTable("Roles").Columns("RoleName");
        
        Create.Table("UserRoles")
            .WithColumn("UserId").AsInt32().NotNullable()
            .WithColumn("RoleId").AsInt32().NotNullable();
        
        Create.PrimaryKey("PK_UserRoles")
            .OnTable("UserRoles").Columns("UserId", "RoleId");
        
        Create.ForeignKey("FK_UserRoles_RoleId")
            .FromTable("UserRoles").ForeignColumn("RoleId")
            .ToTable("Roles").PrimaryColumn("RoleId");
        
        Create.ForeignKey("FK_UserRoles_UserId")
            .FromTable("UserRoles").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("UserId");
        
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
            .WithColumn("UserId").AsInt32().NotNullable()
            .WithColumn("Token").AsString(512).NotNullable()
            .WithColumn("Created").AsDateTime().NotNullable()
            .WithColumn("Expires").AsDateTime().NotNullable()
            .WithColumn("Revoked").AsDateTime().NotNullable()
            .WithColumn("ReplacedByToken").AsString(512).NotNullable();
        
        Create.ForeignKey("FK_RefreshTokens_Users1")
            .FromTable("RefreshTokens").ForeignColumn("UserId")
            .ToTable("Users").PrimaryColumn("UserId");
        
        Create.Table("GoodsReceiptHeaders")
            .WithColumn("GoodsReceiptHeaderId").AsInt32().PrimaryKey().Identity()
            .WithColumn("PackingSlipId").AsString(50).NotNullable()
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
        
        Create.Table("GoodsReceiptLines")
            .WithColumn("GoodsReceiptLineId").AsInt32().PrimaryKey().Identity()
            .WithColumn("GoodsReceiptHeaderId").AsInt32().NotNullable()
            .WithColumn("ItemId").AsString(50).NotNullable()
            .WithColumn("LineNumber").AsInt32().NotNullable()
            .WithColumn("ItemName").AsString(50).NotNullable()
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
        
        Create.ForeignKey("FK_GoodsReceiptLines_GoodsReceiptHeaders")
            .FromTable("GoodsReceiptLines").ForeignColumn("GoodsReceiptHeaderId")
            .ToTable("GoodsReceiptHeaders").PrimaryColumn("GoodsReceiptHeaderId");
    }

    public override void Down()
    {
        Delete.Table("GoodsReceiptLines");
        Delete.Table("GoodsReceiptHeaders");
        Delete.Table("RefreshTokens");
        Delete.Table("NumberSequences");
        Delete.Table("UserRoles");
        Delete.Table("Roles");
        Delete.Table("Users");
        Delete.Table("Permissions");
    }
}