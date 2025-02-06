using FluentResults;
using ZmitaCart.Application.Dtos.CategoryDtos;

namespace ZmitaCart.Application.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Result<int>> Create(string name, int? parentId, string? iconName);
    Task<Result<int>> Update(int id, string? name, int? parentId, string? iconName);
    Task<Result> Delete(int id);
    Task<Result<IEnumerable<SuperiorCategoryDto>>> GetAllSuperiors();
    Task<Result<IEnumerable<CategoryDto>>> GetCategoriesBySuperiorId(int superiorId, int? childrenCount);
    Task<Result<IEnumerable<CategoryDto>>> GetSuperiorsWithFewChildren(int? childrenCount);
    Task<Result<CategoryDto?>> GetParentCategory(int id);
    Task<Result<List<string>>> GetMostPopularCategoriesAsync(int numberOfCategories);
}