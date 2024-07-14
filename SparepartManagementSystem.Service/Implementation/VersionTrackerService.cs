using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.EventHandlers;
using SparepartManagementSystem.Service.Interface;
using SparepartManagementSystem.Service.Mapper;

namespace SparepartManagementSystem.Service.Implementation;

public class VersionTrackerService : IVersionTrackerService
{
    private readonly MapperlyMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RepositoryEvents _repositoryEvents;
    private readonly ILogger _logger = Log.ForContext<RoleService>();

    public VersionTrackerService(MapperlyMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, RepositoryEvents repositoryEvents)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _repositoryEvents = repositoryEvents;
    }

    public async Task<ServiceResponse> AddVersionTracker(VersionTrackerDto dto)
    {
        try
        {
            var newRecord = _mapper.MapToVersionTracker(dto);
            await _unitOfWork.VersionTrackerRepository.Add(newRecord, _repositoryEvents.OnBeforeAdd);
            var versionTrackerId = await _unitOfWork.GetLastInsertedId();
            
            _logger.Information($"Version Tracker added successfully with ID: {versionTrackerId}");
            
            await _unitOfWork.Commit();

            return new ServiceResponse
            {
                Success = true, 
                Message = "Version Tracker added successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> DeleteVersionTracker(int id)
    {
        try
        {
            await _unitOfWork.VersionTrackerRepository.Delete(id);
            
            await _unitOfWork.Commit();

            _logger.Information($"Version Tracker with ID: {id} deleted successfully");

            return new ServiceResponse
            {
                Success = true,
                Message = "Version Tracker deleted successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<VersionTrackerDto>>> GetAllVersionTracker()
    {
        try
        {
            var versionTrackers = await _unitOfWork.VersionTrackerRepository.GetAll();
            
            

            return new ServiceResponse<IEnumerable<VersionTrackerDto>>
            {
                Data = _mapper.MapToListOfVersionTrackerDto(versionTrackers),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<VersionTrackerDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<VersionTrackerDto>> GetVersionTrackerById(int id)
    {
        try
        {
            var versionTracker = await _unitOfWork.VersionTrackerRepository.GetById(id);

            return new ServiceResponse<VersionTrackerDto>
            {
                Data = _mapper.MapToVersionTrackerDto(versionTracker),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<VersionTrackerDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse> UpdateVersionTracker(VersionTrackerDto dto)
    {
        try
        {
            var record = await _unitOfWork.VersionTrackerRepository.GetById(dto.VersionTrackerId, true);
            record.AcceptChanges();
            
            if (record.ModifiedDateTime > dto.ModifiedDateTime)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Record has been modified by another user, please refresh and try again"
                };
            }

            record.UpdateProperties(_mapper.MapToVersionTracker(dto));
            
            if (!record.IsChanged)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "No changes detected in the Version Tracker record"
                };
            }
            
            await _unitOfWork.VersionTrackerRepository.Update(record, _repositoryEvents.OnBeforeUpdate);
            
            await _unitOfWork.Commit();

            _logger.Information($"Version Tracker with ID: {record.VersionTrackerId} updated successfully");

            return new ServiceResponse
            {
                Success = true,
                Message = "Version Tracker updated successfully"
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<VersionTrackerDto>>> GetVersionTrackerByParams(Dictionary<string, string> parameters)
    {
        try
        {
            var versionTrackers = await _unitOfWork.VersionTrackerRepository.GetByParams(parameters);

            return new ServiceResponse<IEnumerable<VersionTrackerDto>>
            {
                Data = _mapper.MapToListOfVersionTrackerDto(versionTrackers),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<VersionTrackerDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<VersionTrackerDto>> GetLatestVersionTracker()
    {
        try
        {
            var versionTracker = await _unitOfWork.VersionTrackerRepository.GetLatestVersionTracker();

            return new ServiceResponse<VersionTrackerDto>
            {
                Data = _mapper.MapToVersionTrackerDto(versionTracker),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<VersionTrackerDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<FileContentResult> DownloadApkByVersion(string version)
    {
        var versionTracker = await _unitOfWork.VersionTrackerRepository.GetByVersion(version);
            
        if (!Uri.TryCreate(versionTracker.PhysicalLocation, UriKind.Absolute, out var result) || result.Scheme != "file")
        {
            throw new InvalidOperationException("Invalid file URI format");
        }

        var filePath = result.AbsolutePath;
        if (!File.Exists(filePath))
        {
            throw new InvalidOperationException("File does not exist");
        }
            
        if (!MimeTypes.TryGetMimeType(filePath, out var contentType) || !contentType.StartsWith("application"))
        {
            throw new InvalidOperationException("File is not an apps");
        }

        var fileContents = await File.ReadAllBytesAsync(filePath);
        
        return new FileContentResult(fileContents, contentType)
        {
            FileDownloadName = $"SAMSON_v{version}.apk"
        };
    }

    public async Task<string> GetVersionFeed()
    {
        var versionTrackers = await _unitOfWork.VersionTrackerRepository.GetAll();
        
        XNamespace sparkle = "http://www.andymatuschak.org/xml-namespaces/sparkle";
        var feed = new SyndicationFeed("SparepartManagementSystem", null, null);
        feed.AttributeExtensions.Add(new XmlQualifiedName("sparkle", XNamespace.Xmlns.NamespaceName), sparkle.NamespaceName);
        
        var items = new List<SyndicationItem>();

        foreach (var versionTracker in versionTrackers)
        {
            var item = new SyndicationItem($"Version {versionTracker.Version}", versionTracker.Description, null)
            {
                PublishDate = versionTracker.PublishedDateTime
            };
            
            var domainName = _httpContextAccessor.HttpContext?.Request.Host.Host ?? "";
            var port = _httpContextAccessor.HttpContext?.Request.Host.Port;
            var scheme = _httpContextAccessor.HttpContext?.Request.Scheme ?? "";
            var uriBuilder = new UriBuilder
            {
                Scheme = scheme,
                Host = domainName,
                Path = $"/versions/{versionTracker.Version}"
            };

            if (port is not null)
            {
                uriBuilder.Port = (int)port;
            }
            
            var enclosure = SyndicationLink.CreateMediaEnclosureLink(uriBuilder.Uri, null, 0);
            enclosure.AttributeExtensions.Add(new XmlQualifiedName("version", sparkle.NamespaceName), versionTracker.Version);
            enclosure.AttributeExtensions.Add(new XmlQualifiedName("os", sparkle.NamespaceName), "android");
            item.Links.Add(enclosure);
            
            items.Add(item);
        }

        feed.Items = items;

        var settings = new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            Indent = true,
            Async = true
        };
        using var stream = new MemoryStream();
        await using (var xmlWriter = XmlWriter.Create(stream, settings))
        {
            var rssFormatter = new Rss20FeedFormatter(feed, false);
            rssFormatter.WriteTo(xmlWriter);
            await xmlWriter.FlushAsync();
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    public async Task<ServiceResponse<VersionTrackerDto>> GetVersionTrackerByVersion(string version)
    {
        try
        {
            var versionTracker = await _unitOfWork.VersionTrackerRepository.GetByVersion(version);
            return new ServiceResponse<VersionTrackerDto>
            {
                Data = _mapper.MapToVersionTrackerDto(versionTracker),
                Message = "Version Tracker successfully retrieved",
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null)
            {
                errorMessages.Add(ex.StackTrace);
            }

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<VersionTrackerDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}