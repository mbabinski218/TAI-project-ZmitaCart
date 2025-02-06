using FluentResults;
using Microsoft.AspNetCore.Http;

namespace ZmitaCart.Application.Interfaces.Repositories;

public interface IPictureRepository
{
    public Task<Result> AddAsync(int userId, int offerId, IEnumerable<IFormFile> files);
    public Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<int>? imagesIds = null);
    public Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<string> filesNames);
    public Task<Result> DeleteAllAsync(int offerId);
    public Task<Result<IEnumerable<string>>> GetPictureNameByOfferIdAsync(int userId);
}