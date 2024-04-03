using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SparepartManagementSystem.API.Permission;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Implementation;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GMKSMSServiceGroupController : ControllerBase
{
    private readonly IGMKSMSServiceGroup _gmkSmsServiceGroup;

    public GMKSMSServiceGroupController(IGMKSMSServiceGroup gmkSmsServiceGroup)
    { 
        _gmkSmsServiceGroup = gmkSmsServiceGroup;
    }

    [HttpGet(nameof(GetInventTablePagedList))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetInventTablePagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] InventTableSearchDto dto)
    {
        var response = await _gmkSmsServiceGroup.GetInventTablePagedList(pageNumber, pageSize, dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    [HttpGet(nameof(GetImageFromNetworkUri))]
    public async Task<IActionResult> GetImageFromNetworkUri(string networkUri)
    {
        try
        {
            if (!Uri.TryCreate(networkUri, UriKind.Absolute, out var result) || result.Scheme != "file") 
                throw new InvalidOperationException("Invalid file URI format");
            var filePath = result.LocalPath;
            if (!System.IO.File.Exists(filePath)) throw new InvalidOperationException("File does not exist");
            if (!MimeTypes.TryGetMimeType(filePath, out var contentType) || !contentType.StartsWith("image"))
                throw new InvalidOperationException("File is not an image");
            var fileContents = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(fileContents, contentType, Path.GetFileName(filePath));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet(nameof(GetImageWithResolutionFromNetworkUri))]
    public async Task<IActionResult> GetImageWithResolutionFromNetworkUri(string networkUri, int maxLength)
    {
        try
        {
            if (!Uri.TryCreate(networkUri, UriKind.Absolute, out var result) || result.Scheme != "file")
                throw new InvalidOperationException("Invalid file URI format");
            var filePath = result.LocalPath;
            if (!System.IO.File.Exists(filePath))
                throw new InvalidOperationException("File does not exist");
            if (!MimeTypes.TryGetMimeType(filePath, out var mimeType) || !mimeType.StartsWith("image"))
                throw new InvalidOperationException("File is not an image");
            using var inStream = new MemoryStream(await System.IO.File.ReadAllBytesAsync(filePath));
            using var outStream = new MemoryStream();
            using var image = await Image.LoadAsync((Stream) inStream);
            var width = image.Width;
            var height = image.Height;
            if (width > maxLength || height > maxLength)
            {
                var options = new ResizeOptions()
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(maxLength)
                };
        
                image.Mutate(x => x.Resize(options));
            }
            await image.SaveAsync(outStream, new JpegEncoder()
            {
                Quality = 50
            });
            var array = outStream.ToArray();
            return File(array, "image/jpeg", Path.GetFileNameWithoutExtension(filePath) + ".jpg");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet(nameof(GetPurchTablePagedList))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetPurchTablePagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] PurchTableSearchDto dto)
    {
        var response = await _gmkSmsServiceGroup.GetPurchTablePagedList(pageNumber, pageSize, dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet(nameof(GetPurchLineList))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetPurchLineList([FromQuery] string purchId)
    {
        var response = await _gmkSmsServiceGroup.GetPurchLineList(purchId);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet(nameof(GetWMSLocationPagedList))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetWMSLocationPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] WMSLocationSearchDto dto)
    {
        var response = await _gmkSmsServiceGroup.GetWMSLocationPagedList(pageNumber, pageSize, dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("[action]/{purchId}")]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetPurchTable(string purchId)
    {
        var response = await _gmkSmsServiceGroup.GetPurchTable(purchId);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet(nameof(GetInventSumList))]
    [TypeFilter(typeof(ClaimRequirementFilter), Arguments = new object[] { new[] { PermissionType.GMKSMSServiceGroup.Read } })]
    public async Task<IActionResult> GetInventSumList([FromQuery] InventSumSearchDto dto)
    {
        var response = await _gmkSmsServiceGroup.GetInventSumList(dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet(nameof(GetWorkOrderPagedList))]
    public async Task<IActionResult> GetWorkOrderPagedList([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] WorkOrderSearchDto dto)
    {
        var response = await _gmkSmsServiceGroup.GetWorkOrderPagedList(pageNumber, pageSize, dto);
        if (response.Success)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}