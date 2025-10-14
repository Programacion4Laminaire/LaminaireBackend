using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetPagedQuery
{
    public class GetPagedHandler(IUnitOfWork uow)
    : IQueryHandler<GetPagedQuery, IEnumerable<AccessoryEquivalenceResponseDto>>
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<BaseResponse<IEnumerable<AccessoryEquivalenceResponseDto>>> Handle(
            GetPagedQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<AccessoryEquivalenceResponseDto>>();

            try
            {
                var (items, total) = await _uow.AccessoryEquivalence.GetPagedAsync(
                    request.Records, request.NumPage, request.Sort, request.Order, request.NumFilter, request.TextFilter);

                response.IsSuccess = true;
                response.Data = items;
                response.TotalRecords = total;
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
