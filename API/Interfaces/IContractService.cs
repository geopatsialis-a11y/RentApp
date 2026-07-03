using API.DTOs.Contract;
using API.Helper;

namespace API.Interfaces;

public interface IContractService
{
    Task<PaginatedResult<ContractListItemDto>> GetAllAsync(ContractParams p);
    Task<ContractDetailDto?> GetByIdAsync(Guid id);
    Task<List<AvailableAssetDto>> GetAvailableAssetsAsync(DateTime start, DateTime end, Guid? excludeContractId = null);
    Task<ContractDetailDto> CreateAsync(ContractCreateDto dto, string memberId);
    Task<ContractDetailDto> UpdateAsync(Guid id, ContractUpdateDto dto, string memberId);
    Task DeleteAsync(Guid id, string memberId);
}
