using Engineering.Application.Dtos.StateChanges;

namespace Engineering.Application.Interfaces.Persistence;

public interface ILogMovementRepository
{
    
    Task<IEnumerable<StateChangeLogResponseDto>> GetByFilterAsync(StateChangeLogFilterDto filter);
}
