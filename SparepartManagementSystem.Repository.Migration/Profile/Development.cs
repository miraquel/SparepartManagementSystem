using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace SparepartManagementSystem.Repository.Migration.Profile;

[Profile("Development")]
[ExcludeFromCodeCoverage]
public class Development : FluentMigrator.Migration
{
    public override void Up()
    {
        Insert.IntoTable("Users").Row(new
        {
            Username = "chaidir.ali",
            FirstName = "admin",
            LastName = "admin",
            Email = "chaidir.ali@gandummas.co.id",
            IsAdministrator = true,
            IsEnabled = true,
            LastLogin = SqlDateTime.MinValue.Value,
            CreatedBy = "chaidir.ali",
            CreatedDateTime = DateTime.Now,
            ModifiedBy = "chaidir.ali",
            ModifiedDateTime = DateTime.Now
        });
        Insert.IntoTable("Roles").Row(new
        {
            RoleName = "Administrator",
            Description = "Administrator",
            CreatedBy = "chaidir.ali",
            CreatedDateTime = DateTime.Now,
            ModifiedBy = "chaidir.ali",
            ModifiedDateTime = DateTime.Now
        });
        Insert.IntoTable("UserRoles").Row(new
        {
            UserId = 1,
            RoleId = 1
        });
        Insert.IntoTable("NumberSequences").Row(new
        {
            Name = "Invoice Scan Header",
            Description = "Running number for Invoice Scan Header",
            Module = "InvoiceScanHeaderService",
            Format = "INVSCAN-#####",
            LastNumber = 57,
            CreatedBy = "chaidir.ali",
            CreatedDateTime = DateTime.Now,
            ModifiedBy = "chaidir.ali",
            ModifiedDateTime = DateTime.Now
        });
    }

    public override void Down()
    {
    }
}