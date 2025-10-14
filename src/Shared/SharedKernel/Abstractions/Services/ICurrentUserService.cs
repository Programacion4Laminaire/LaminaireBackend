namespace SharedKernel.Abstractions.Services;

public interface ICurrentUserService
{
    int? UserId { get; }
}
