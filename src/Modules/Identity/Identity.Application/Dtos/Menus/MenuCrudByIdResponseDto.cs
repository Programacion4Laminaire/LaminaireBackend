namespace Identity.Application.Dtos.Menus
{
    public record MenuCrudByIdResponseDto
    {
        public int MenuId { get; init; }
        public int Position { get; init; }
        public string Name { get; init; } = null!;
        public string? Icon { get; init; }
        public string? Url { get; init; }
        public int? FatherId { get; init; }
        public string? State { get; init; }
        public bool IsNew { get; init; }
    }
}
