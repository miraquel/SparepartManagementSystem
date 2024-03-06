using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptHeaderService : IGoodsReceiptHeaderService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptHeaderService>();

    public GoodsReceiptHeaderService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Add(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Add(_mapper.Map<GoodsReceiptHeader>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header added successfully", result?.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line added successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Delete(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header deleted successfully", result?.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line deleted successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetAll()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAll();

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptHeaderDto>>(result),
                Message = "Journal Line retrieved successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetById(id);

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header retrieved successfully", result?.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line added successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>> GetByParams(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParams(_mapper.Map<GoodsReceiptHeader>(dto));

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptHeaderDto>>(result),
                Message = "Journal Line retrieved successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<IEnumerable<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<GoodsReceiptHeaderDto>> Update(GoodsReceiptHeaderDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.Update(_mapper.Map<GoodsReceiptHeader>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptHeaderId}, Goods Receipt Header updated successfully", result?.GoodsReceiptHeaderId);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Data = _mapper.Map<GoodsReceiptHeaderDto>(result),
                Message = "Journal Line updated successfully",
                Success = true
            };
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<GoodsReceiptHeaderDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetAllPagedList(int pageNumber, int pageSize)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetAllPagedList(pageNumber, pageSize);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<PagedListDto<GoodsReceiptHeaderDto>>(result),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }

    public async Task<ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>> GetByParamsPagedList(int pageNumber, int pageSize, GoodsReceiptHeaderDto entity)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptHeaderRepository.GetByParamsPagedList(pageNumber, pageSize, _mapper.Map<GoodsReceiptHeader>(entity));

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Data = _mapper.Map<PagedListDto<GoodsReceiptHeaderDto>>(result),
                Success = true
            };
        }
        catch (Exception ex)
        {
            var errorMessages = new List<string>
            {
                ex.Message
            };

            if (ex.StackTrace is not null) errorMessages.Add(ex.StackTrace);

            _logger.Error(ex, ex.Message);

            return new ServiceResponse<PagedListDto<GoodsReceiptHeaderDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}