using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Domain.Enums;
using SparepartManagementSystem.Repository.EventHandlers;

namespace SparepartManagementSystem.Repository.Tests;

internal static class RepositoryTestsHelper
{
    internal static Random Random { get; } = new();

    internal static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
    
    internal static DateTime RandomDateTime()
    {
        var start = new DateTime(1995, 1, 1);
        var range = (DateTime.Today - start).Days;
        return start.AddDays(Random.Next(range)).TrimMiliseconds();
    }
    
    internal static void OnBeforeAdd(object? _, AddEventArgs args)
    {
        args.Entity.CreatedBy = RandomString(12);
        args.Entity.CreatedDateTime = RandomDateTime();
        args.Entity.ModifiedBy = RandomString(12);
        args.Entity.ModifiedDateTime = RandomDateTime();
    }
    
    internal static void OnBeforeUpdate(object? _, UpdateEventArgs args)
    {
        args.Entity.ModifiedBy = RandomString(12);
        args.Entity.ModifiedDateTime = RandomDateTime();
    }
    
    internal static GoodsReceiptHeader CreateGoodsReceiptHeader()
    {
        return new GoodsReceiptHeader
        {
            PurchId = RandomString(12),
            PackingSlipId = RandomString(12),
            Description = RandomString(12),
            InvoiceAccount = RandomString(12),
            OrderAccount = RandomString(12),
            IsSubmitted = true,
            SubmittedDate = RandomDateTime(),
            PurchName = RandomString(12),
            PurchStatus = RandomString(12),
            SubmittedBy = RandomString(12),
            TransDate = RandomDateTime()
        };
    }

    private static GoodsReceiptLine CreateGoodsReceiptLine()
    {
        return new GoodsReceiptLine
        {
            ItemId = RandomString(12),
            CreatedBy = RandomString(12),
            CreatedDateTime = DateTime.Now.TrimMiliseconds(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = DateTime.Now.TrimMiliseconds(),
            ItemName = RandomString(12),
            LineAmount = Random.Next(),
            LineNumber = Random.Next(),
            ProductType = (ProductType)Random.Next(0, 2),
            PurchPrice = Random.Next(),
            PurchQty = Random.Next(),
            PurchUnit = RandomString(3),
            ReceiveNow = Random.Next(),
            InventLocationId = RandomString(12),
            RemainPurchPhysical = Random.Next(),
            WMSLocationId = RandomString(12)
        };
    }

    internal static GoodsReceiptLine CreateGoodsReceiptLine(int goodsReceiptHeaderId)
    {
        var result = CreateGoodsReceiptLine();
        result.GoodsReceiptHeaderId = goodsReceiptHeaderId;
        return result;
    }
    
    internal static RefreshToken CreateRefreshToken()
    {
        return new RefreshToken
        {
            Token = RandomString(12),
            Expires = DateTime.Now.TrimMiliseconds(),
            Created = DateTime.Now.TrimMiliseconds(),
            Revoked = DateTime.Now.TrimMiliseconds(),
            ReplacedByToken = RandomString(12),
            UserId = Random.Next(),
            RefreshTokenId = Random.Next()
        };
    }
    
    internal static User CreateUser()
    {
        return new User
        {
            Username = RandomString(12),
            FirstName = RandomString(12),
            LastName = RandomString(12),
            Email = RandomString(12),
            IsAdministrator = true,
            IsEnabled = true,
            LastLogin = RandomDateTime(),
            CreatedBy = RandomString(12),
            CreatedDateTime = RandomDateTime(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = RandomDateTime()
        };
    }
    
    internal static RowLevelAccess CreateRowLevelAccess()
    {
        return new RowLevelAccess
        {
            UserId = Random.Next(),
            Query = RandomString(12),
            AxTable = (AxTable)Random.Next(0, 2),
            RowLevelAccessId = Random.Next(),
            CreatedBy = RandomString(12),
            CreatedDateTime = DateTime.Now.TrimMiliseconds(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = DateTime.Now.TrimMiliseconds(),
        };
    }
    
    internal static RowLevelAccess CreateRowLevelAccess(int userId)
    {
        var rowLevelAccess = CreateRowLevelAccess();
        rowLevelAccess.UserId = userId;
        return rowLevelAccess;
    }
    
    internal static NumberSequence CreateNumberSequence()
    {
        return new NumberSequence
        {
            Name = RandomString(12),
            Description = RandomString(12),
            Format = string.Join("-", [RandomString(5), "#######"]),
            LastNumber = Random.Next(1, 100),
            Module = RandomString(12),
            CreatedBy = RandomString(12),
            CreatedDateTime = DateTime.Now.TrimMiliseconds(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = DateTime.Now.TrimMiliseconds(),
        };
    }

    internal static Permission CreatePermission()
    {
        return new Permission
        {
            RoleId = Random.Next(1, 1000),
            PermissionName = RandomString(12),
            Module = RandomString(12),
            Type = RandomString(12)
        };
    }
    
    internal static Permission CreatePermission(int roleId)
    {
        var result = CreatePermission();
        result.RoleId = roleId;
        return result;
    }
    
    internal static Role CreateRole()
    {
        return new Role
        {
            RoleName = RandomString(12),
            Description = RandomString(12),
            CreatedBy = RandomString(12),
            CreatedDateTime = DateTime.Now.TrimMiliseconds(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = DateTime.Now.TrimMiliseconds(),
        };
    }
    
    internal static UserWarehouse CreateUserWarehouse()
    {
        return new UserWarehouse
        {
            UserId = Random.Next(),
            InventLocationId = RandomString(12),
            InventSiteId = RandomString(12),
            Name = RandomString(12),
            IsDefault = true,
            CreatedBy = RandomString(12),
            CreatedDateTime = DateTime.Now.TrimMiliseconds(),
            ModifiedBy = RandomString(12),
            ModifiedDateTime = DateTime.Now.TrimMiliseconds(),
        };
    }
    
    internal static UserWarehouse CreateUserWarehouse(int userId)
    {
        var result = CreateUserWarehouse();
        result.UserId = userId;
        return result;
    }
}