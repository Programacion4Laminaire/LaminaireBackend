using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetDescripcionQuery
{
    public class GetDescripcionHandler(IUnitOfWork uow)
     : IQueryHandler<GetDescripcionQuery, string>
    {
        private readonly IUnitOfWork _uow = uow;

        public async Task<BaseResponse<string>> Handle(GetDescripcionQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<string>();
            try
            {
                var desc = await _uow.AccessoryEquivalence.GetDescripcionByCodigoAsync(request.Codigo);
                response.IsSuccess = true;
                response.Data = desc ?? string.Empty;
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
