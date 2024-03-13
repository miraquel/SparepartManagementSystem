using AutoMapper;
using Serilog;
using SparepartManagementSystem.Domain;
using SparepartManagementSystem.Repository.UnitOfWork;
using SparepartManagementSystem.Service.DTO;
using SparepartManagementSystem.Service.Interface;

namespace SparepartManagementSystem.Service.Implementation;

public class GoodsReceiptLineService : IGoodsReceiptLineService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger = Log.ForContext<GoodsReceiptHeaderService>();

    public GoodsReceiptLineService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ServiceResponse<GoodsReceiptLineDto>> Add(GoodsReceiptLineDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.Add(_mapper.Map<GoodsReceiptLine>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptLineId}, Goods Receipt Line added successfully", result?.GoodsReceiptLineId);

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Data = _mapper.Map<GoodsReceiptLineDto>(result),
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

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<GoodsReceiptLineDto>> Delete(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.Delete(id);

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptLineId}, Goods Receipt Line deleted successfully", id);

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Data = _mapper.Map<GoodsReceiptLineDto>(result),
                Message = "Goods Receipt Line deleted successfully",
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

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<IEnumerable<GoodsReceiptLineDto>>> GetAll()
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.GetAll();

            return new ServiceResponse<IEnumerable<GoodsReceiptLineDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptLineDto>>(result),
                Message = "Goods Receipt Line retrieved successfully",
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

            return new ServiceResponse<IEnumerable<GoodsReceiptLineDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<GoodsReceiptLineDto>> GetById(int id)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.GetById(id);

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Data = _mapper.Map<GoodsReceiptLineDto>(result),
                Message = "Goods Receipt Line retrieved successfully",
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

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<IEnumerable<GoodsReceiptLineDto>>> GetByParams(GoodsReceiptLineDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.GetByParams(_mapper.Map<GoodsReceiptLine>(dto));

            return new ServiceResponse<IEnumerable<GoodsReceiptLineDto>>
            {
                Data = _mapper.Map<IEnumerable<GoodsReceiptLineDto>>(result),
                Message = "Goods Receipt Line retrieved successfully",
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

            return new ServiceResponse<IEnumerable<GoodsReceiptLineDto>>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
    public async Task<ServiceResponse<GoodsReceiptLineDto>> Update(GoodsReceiptLineDto dto)
    {
        try
        {
            var result = await _unitOfWork.GoodsReceiptLineRepository.Update(_mapper.Map<GoodsReceiptLine>(dto));

            _unitOfWork.Commit();

            _logger.Information("id: {GoodsReceiptLineId}, Goods Receipt Line updated successfully", result?.GoodsReceiptLineId);

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Data = _mapper.Map<GoodsReceiptLineDto>(result),
                Message = "Goods Receipt Line updated successfully",
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

            return new ServiceResponse<GoodsReceiptLineDto>
            {
                Error = ex.GetType().Name,
                ErrorMessages = errorMessages,
                Success = false
            };
        }
    }
}