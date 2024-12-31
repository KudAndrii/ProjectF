namespace ProjectF.DataAccess.Entities;

public class MoviesQueryEntity
{
    public Guid Id { get; } = Guid.CreateVersion7();
    public required string Term { get; init; }
    public required int Year { get; init; }
}