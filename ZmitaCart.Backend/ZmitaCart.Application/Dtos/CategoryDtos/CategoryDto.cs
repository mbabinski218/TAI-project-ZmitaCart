namespace ZmitaCart.Application.Dtos.CategoryDtos;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? IconName { get; set; }
    public int? ParentId { get; set; }
    public IEnumerable<CategoryDto?>? Children { get; set; }
}