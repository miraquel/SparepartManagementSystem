using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using FluentMigrator;

namespace SparepartManagementSystem.Repository.Migration.Profile;

[Profile("Production")]
[ExcludeFromCodeCoverage]
public class Production : FluentMigrator.Migration
{
    public override void Up()
    {
        Insert.IntoTable("Users").Row(new
        {
            Username = "chaidir.ali",
            FirstName = "Chaidir",
            LastName = "Ali",
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
        Insert.IntoTable("UserWarehouses").Row(new
        {
            UserId = 1,
            InventLocationId = "TK",
            InventSiteId = "TGR",
            Name = "Tehnik WH",
            IsDefault = 1,
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