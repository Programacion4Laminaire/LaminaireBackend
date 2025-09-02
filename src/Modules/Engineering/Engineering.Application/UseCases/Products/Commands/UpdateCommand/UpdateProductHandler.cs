using Engineering.Application.Dtos.Products;
using Engineering.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Engineering.Application.UseCases.Products.Commands.UpdateCommand;

public class UpdateProductHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateProductCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var dto = request.Adapt<ProductUpdateRequestDto>();
            var success = await _unitOfWork.Product.UpdateProductAsync(dto);

            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
                response.IsSuccess = true;
                response.Data = true;
                response.Message = "✅ Producto actualizado correctamente";
            }
            else
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = "❌ No se pudo actualizar el producto";
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Data = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
