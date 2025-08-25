using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Engineering.Application.UseCases.Products.Queries.GetSelectQuery;

public class GetProductSelectQuery : IQuery<IEnumerable<SelectResponseDto>>
{
    public string? SearchTerm { get; }

    public GetProductSelectQuery(string? searchTerm)
    {
        SearchTerm = searchTerm;
    }
}
