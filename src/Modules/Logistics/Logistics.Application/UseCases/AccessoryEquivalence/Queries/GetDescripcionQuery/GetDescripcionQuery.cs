using SharedKernel.Abstractions.Messaging;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.GetDescripcionQuery
{
    public class GetDescripcionQuery : IQuery<string>
    {
        public string Codigo { get; init; } = default!;
    }
}
