using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand
{
    public class UpdateUserWithImageCommand : ICommand<bool>
    {
        [FromForm] public int UserId { get; set; }
        [FromForm] public string Identification { get; set; } = null!;
        [FromForm] public DateTime BirthDate { get; set; }
        [FromForm] public string FirstName { get; set; } = null!;
        [FromForm] public string LastName { get; set; } = null!;
        [FromForm] public string UserName { get; set; } = null!;
        [FromForm] public string Email { get; set; } = null!;
        [FromForm] public string? Password { get; set; }
        [FromForm] public string State { get; set; } = null!;
        [FromForm] public IFormFile? Image { get; set; }
    }
}
