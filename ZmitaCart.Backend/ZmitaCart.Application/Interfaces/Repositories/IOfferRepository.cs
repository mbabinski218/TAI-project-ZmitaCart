using FluentResults;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Dtos.OfferDtos;

namespace ZmitaCart.Application.Interfaces.Repositories;

public interface IOfferRepository
{
    Task<Result<int>> CreateAsync(CreateOfferDto offerDto);
    Task<Result<int>> UpdateAsync(UpdateOfferDto offerDto);
    Task<Result> DeleteAsync(int userId, int offerId);
    Task<Result<OfferDto>> GetOfferAsync(int id);
    Task<Result<OfferDataDto>> GetOfferDataAsync(int id);
    Task<Result> AddToFavoritesAsync(int userId, int offerId);
    Task<Result<PaginatedList<OfferInfoDto>>> GetFavoritesOffersAsync(int userId, int? pageNumber = null, int? pageSize = null);
    Task<Result<IEnumerable<int>>> GetFavoritesOffersIdsAsync(int userId);
    Task<Result> BuyAsync(int userId, int offerId, int quantity);
    Task<Result<PaginatedList<BoughtOfferDto>>> GetBoughtAsync(int userId, int? pageNumber = null, int? pageSize = null);
    Task<Result<PaginatedList<OfferInfoDto>>> SearchOffersAsync(SearchOfferDto search, int? pageNumber = null, int? pageSize = null);
    Task<Result<List<NamedList<string, OfferInfoDto>>>> GetOffersByCategoriesNameAsync(IEnumerable<string> categoriesNames, int userId, int size);
    Task<Result<PaginatedList<OfferInfoDto>>> GetUserOffersAsync(int userId, int? pageNumber = null, int? pageSize = null);
    Task<Result<int>> GetFavoritesOffersCountAsync(int userId);
    Task<Result<int>> GetUserIdByOfferIdAsync(int offerId);
}
