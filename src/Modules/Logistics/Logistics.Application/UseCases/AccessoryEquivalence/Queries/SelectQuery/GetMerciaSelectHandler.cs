using Logistics.Application.Interfaces.Persistence;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Dtos.Commons;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.SelectQuery
{
    public class GetMerciaSelectHandler(IAccessoryEquivalenceRepository repo)
      : IQueryHandler<GetMerciaSelectQuery, IEnumerable<SelectResponseDto>>
    {
        public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetMerciaSelectQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<SelectResponseDto>>();
            try
            {
                var items = await repo.GetMerciaSelectAsync(request.SearchTerm, request.Kind);
                response.IsSuccess = true;
                response.Data = items;
                response.Message = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
