using FluentResults;
using Mapster;
using MapsterMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ZmitaCart.Application.Common;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Dtos.OfferDtos;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.DbContexts;

namespace ZmitaCart.Infrastructure.Repositories;

public class OfferRepository : IOfferRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IMapper _mapper;

	public OfferRepository(ApplicationDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<Result<int>> CreateAsync(CreateOfferDto offerDto)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == offerDto.UserId);
		if (user == null)
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == offerDto.CategoryId);
		if (category == null)
		{
			return Result.Fail(new NotFoundError("Category does not exist"));
		}

		var offer = _mapper.Map<Offer>(offerDto);
		offer.User = user;
		offer.Category = category;

		await _dbContext.Offers.AddAsync(offer);
		await _dbContext.SaveChangesAsync();

		return Result.Ok(offer.Id);
	}

	public async Task<Result<int>> UpdateAsync(UpdateOfferDto offerDto)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerDto.Id);
		if (offer == null)
		{
			return Result.Fail(new NotFoundError("Offer does not exist"));
		}

		if (offer.UserId != offerDto.UserId)
		{
			return Result.Fail(new UnauthorizedError("User does not have access to this offer"));
		}

		offer.Title = offerDto.Title ?? offer.Title;
		offer.Description = offerDto.Description ?? offer.Description;
		offer.Price = offerDto.Price ?? offer.Price;
		offer.Quantity = offerDto.Quantity ?? offer.Quantity;
		offer.Condition = offerDto.Condition ?? offer.Condition;
		offer.IsAvailable = offerDto.IsAvailable ?? offer.IsAvailable;

		await _dbContext.SaveChangesAsync();
		return Result.Ok(offer.Id);
	}

	public async Task<Result> DeleteAsync(int userId, int offerId)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
		if (offer == null)
		{
			return Result.Fail(new NotFoundError("Offer does not exist"));
		}

		if (offer.UserId != userId)
		{
			return Result.Fail(new UnauthorizedError("User does not have access to this offer"));
		}

		offer.IsAvailable = false;
		//_dbContext.Offers.Remove(offer);
		await _dbContext.SaveChangesAsync();
		return Result.Ok();
	}

	public async Task<Result<OfferDto>> GetOfferAsync(int id)
	{
		var offer = await _dbContext.Offers
			.Where(o => o.Id == id)
			.Include(o => o.User)
			.Include(o => o.Pictures)
			.Include(o => o.Favorites)
			.AsNoTracking()
			.ProjectToType<OfferDto>()
			.FirstOrDefaultAsync();

		return offer is null
			? Result.Fail(new NotFoundError("Offer does not exist"))
			: offer;
	}

	public async Task<Result<OfferDataDto>> GetOfferDataAsync(int id)
	{
		var offer = await _dbContext.Offers
			.Where(o => o.Id == id)
			.Include(o => o.User)
			.Include(o => o.Pictures)
			.AsNoTracking()
			.ProjectToType<OfferDataDto>()
			.FirstOrDefaultAsync();
		
		return offer is null
			? Result.Fail(new NotFoundError("Offer does not exist"))
			: offer;
	}

	public async Task<Result> AddToFavoritesAsync(int userId, int offerId)
	{
		var offer = await _dbContext.Offers.FindAsync(offerId);
		if (offer == null)
		{
			return Result.Fail(new NotFoundError("Offer does not exist"));
		}

		var user = await _dbContext.Users
			.Include(u => u.Favorites)
			.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null)
		{
			return Result.Fail(new NotFoundError("User does not exist"));
		}

		var offerInFavorites = user.Favorites.FirstOrDefault(f => f.OfferId == offerId);

		if (offerInFavorites is null)
		{
			await _dbContext.Favorites.AddAsync(new UserOffer
			{
				UserId = user.Id,
				OfferId = offer.Id
			});
		}
		else
		{
			_dbContext.Favorites.Remove(offerInFavorites);
		}

		await _dbContext.SaveChangesAsync();
		return Result.Ok();
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> GetFavoritesOffersAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		return await _dbContext.Users
			.Where(u => u.Id == userId)
			.Include(u => u.Favorites)
				.ThenInclude(uc => uc.Offer)
					.ThenInclude(o => o.User)
						.ThenInclude(u => u.Address)
			.SelectMany(u => u.Favorites.Select(uc => uc.Offer))
			.AsNoTracking()
			.ProjectToType<OfferInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
	}

	public async Task<Result<IEnumerable<int>>> GetFavoritesOffersIdsAsync(int userId)
	{
		return await _dbContext.Favorites
			.Where(uc => uc.UserId == userId)
			.Select(uc => uc.OfferId)
			.ToListAsync();
	}

	public async Task<Result> BuyAsync(int userId, int offerId, int quantity)
	{
		var offer = await _dbContext.Offers.FindAsync(offerId);
		if (offer == null) return Result.Fail(new NotFoundError("Offer does not exist"));

		var user = await _dbContext.Users.FindAsync(userId);
		if (user == null) return Result.Fail(new NotFoundError("User does not exist"));

		if (offer.UserId == userId) return Result.Fail(new InvalidDataError("You cannot buy your own offer"));
		if (!offer.IsAvailable) return Result.Fail(new InvalidDataError("Offer is not available"));
		if (offer.Quantity < quantity) return Result.Fail(new InvalidDataError("Offer is not available"));

		offer.Quantity -= quantity;
		if (offer.Quantity == 0) offer.IsAvailable = false;

		var bought = new Bought
		{
			UserId = user.Id,
			User = user,
			OfferId = offer.Id,
			Offer = offer,
			Quantity = quantity,
			BoughtAt = DateTimeOffset.Now,
			TotalPrice = offer.Price * quantity
		};

		await _dbContext.Bought.AddAsync(bought);
		await _dbContext.SaveChangesAsync();
		return Result.Ok();
	}

	public async Task<Result<PaginatedList<BoughtOfferDto>>> GetBoughtAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		return await _dbContext.Bought
			.Where(b => b.UserId == userId)
			.Include(b => b.Offer)
				.ThenInclude(o => o.User)
					.ThenInclude(u => u.Address)
			.ProjectToType<BoughtOfferDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> SearchOffersAsync(SearchOfferDto search, int? pageNumber = null,
		int? pageSize = null)
	{
		var categoriesId = Enumerable.Empty<int>();
		if (search.CategoryId != null)
		{
			var categoryId = new SqlParameter("categoryId", search.CategoryId);
			
			categoriesId = await _dbContext.Database
				.SqlQuery<int>
				($@"
					WITH Subcategories AS
					(
			            SELECT DISTINCT Id, ParentId
			            FROM Categories
			            WHERE ParentId = {categoryId}

			            UNION ALL

			            SELECT Categories.Id, Categories.ParentId
			            FROM Subcategories, Categories
			            WHERE Categories.ParentId = Subcategories.Id
					)

					SELECT Id FROM Subcategories
				")
				.ToListAsync();
		}

		var offers = await _dbContext.Offers
			.Where(o => EF.Functions.Like(o.Title, $"%{search.Title}%")
				&& (categoriesId.Contains(o.CategoryId) || o.CategoryId == (search.CategoryId ?? o.CategoryId))
				&& o.Price >= (search.MinPrice ?? o.Price)
				&& o.Price <= (search.MaxPrice ?? o.Price)
				&& (search.Conditions == null || search.Conditions.Contains(o.Condition))
				&& o.IsAvailable == true
				&& o.UserId != search.UserId)
			.Include(o => o.User)
				.ThenInclude(u => u.Address)
			.Include(o => o.Pictures)
			.Include(o => o.Favorites)
			.SortBy(search.SortBy)
			.AsNoTracking()
			.ProjectToType<OfferInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);

		return Result.Ok(offers);
	}

	public async Task<Result<List<NamedList<string, OfferInfoDto>>>> GetOffersByCategoriesNameAsync(
		IEnumerable<string> categoriesNames, int userId, int size)
	{
		var offersId = await _dbContext.Offers
			.Include(o => o.Category)
			.Where(o => o.IsAvailable && o.UserId != userId && categoriesNames.Contains(o.Category.Name))
			.Select(o => o.Id)
			.ToListAsync();

		var offers = await _dbContext.Offers
			.Where(o => offersId.Contains(o.Id))
			.Include(o => o.Category)
			.Include(o => o.User)
				.ThenInclude(u => u.Address)
			.Include(o => o.Pictures)
			.Include(o => o.Favorites)
			.OrderByDescending(o => o.CreatedAt)
			.AsNoTracking()
			.ToUniqueListAsync(o => o.Category.Name, o => o.Adapt<OfferInfoDto>());

		return offers;
	}

	public async Task<Result<PaginatedList<OfferInfoDto>>> GetUserOffersAsync(int userId, int? pageNumber = null, int? pageSize = null)
	{
		return await _dbContext.Offers
			.Where(o => o.UserId == userId)
			.Include(o => o.User)
			.Include(o => o.Pictures)
			.Include(o => o.Favorites)
			.AsNoTracking()
			.ProjectToType<OfferInfoDto>()
			.ToPaginatedListAsync(pageNumber, pageSize);
	}

	public async Task<Result<int>> GetFavoritesOffersCountAsync(int userId)
	{
		if (!await _dbContext.Users.AnyAsync(u => u.Id == userId))
		{
			return Result.Fail(new NotFoundError("User not found"));
		}

		return await _dbContext.Favorites
			.Where(f => f.UserId == userId)
			.CountAsync();
	}

	public async Task<Result<int>> GetUserIdByOfferIdAsync(int offerId)
	{
		var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
		if (offer is null)
		{
			return Result.Fail(new NotFoundError("Offer not found"));
		}

		return offer.UserId;
	}
}