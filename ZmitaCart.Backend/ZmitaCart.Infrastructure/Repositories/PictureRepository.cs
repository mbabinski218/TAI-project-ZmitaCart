using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using ZmitaCart.Application.Common.Errors;
using ZmitaCart.Application.Interfaces.Repositories;
using ZmitaCart.Domain.Entities;
using ZmitaCart.Infrastructure.Persistence.DbContexts;

namespace ZmitaCart.Infrastructure.Repositories;

public class PictureRepository : IPictureRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PictureRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddAsync(int userId, int offerId, IEnumerable<IFormFile> files)
    {
        var offer = await _dbContext.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }

        if (offer.UserId != userId)
        {
            return Result.Fail(new UnauthorizedError("You are not authorized to update an offer."));
        }

        foreach (var file in files)
        {
            if (file.Length <= 0) continue;

            var creationTime = DateTimeOffset.Now;
            var imageName = $"{offerId}_{creationTime:ddMMyyyyhhmmssfff}_{new Random().Next(0, 10000000)}.{file.FileName.Split('.').Last()}";
            var filePath = Path.Combine(Path.GetFullPath("wwwroot"), imageName);
            
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                if(image.Size.Width > 2000 || image.Size.Height > 2000)
                    image.Mutate(op => op.Resize(new ResizeOptions
                    {
                        Size = new Size(2000, 2000),
                        Mode = ResizeMode.Max,
                        Sampler = LanczosResampler.Lanczos3
                    }));

                await image.SaveAsync(filePath);
            }

            var picture = new Picture
            {
                OfferId = offerId,
                Offer = offer,
                Name = imageName,
                CreatedAt = creationTime
            };

            await _dbContext.Pictures.AddAsync(picture);
        }

        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<int>? imagesIds = null)
    {
        var offer = await _dbContext.Offers
            .Include(o => o.Pictures)
            .FirstOrDefaultAsync(o => o.Id == offerId);
        
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }

        if (offer.UserId != userId)
        {
            return Result.Fail(new UnauthorizedError("You are not authorized to remove an offer."));
        }

        if (!offer.Pictures.Any())
        {
            return Result.Ok();
        }

        var pictures = offer.Pictures.Where(picture => imagesIds?.Contains(picture.Id) ?? true).ToList();

        if (!pictures.Any())
        {
            return Result.Ok();
        }

        foreach (var filePath in pictures.Select(picture => Path.Combine(Path.GetFullPath("wwwroot"), picture.Name)).Where(File.Exists))
        {
            File.Delete(filePath);
        }

        _dbContext.Pictures.RemoveRange(pictures);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(int userId, int offerId, IEnumerable<string> filesNames)
    {
        var offer = await _dbContext.Offers
            .Include(o => o.Pictures)
            .FirstOrDefaultAsync(o => o.Id == offerId);
        
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }

        if (offer.UserId != userId)
        {
            return Result.Fail(new UnauthorizedError("You are not authorized to remove an offer."));
        }

        if (!offer.Pictures.Any())
        {
            return Result.Ok();
        }

        var pictures = offer.Pictures.Where(picture => filesNames.Contains(picture.Name)).ToList();

        if (!pictures.Any())
        {
            return Result.Ok();
        }

        foreach (var filePath in pictures.Select(picture => Path.Combine(Path.GetFullPath("wwwroot"), picture.Name)).Where(File.Exists))
        {
            File.Delete(filePath);
        }

        _dbContext.Pictures.RemoveRange(pictures);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result> DeleteAllAsync(int offerId)
    {
        var offer = await _dbContext.Offers
            .Include(o => o.Pictures)
            .FirstOrDefaultAsync(o => o.Id == offerId);
        
        if (offer is null)
        {
            return Result.Fail(new NotFoundError("Offer not found."));
        }
        
        if (!offer.Pictures.Any())
        {
            return Result.Ok();
        }
        
        foreach (var filePath in offer.Pictures.Select(picture => Path.Combine(Path.GetFullPath("wwwroot"), picture.Name)).Where(File.Exists))
        {
            File.Delete(filePath);
        }
        
        _dbContext.Pictures.RemoveRange(offer.Pictures);
        await _dbContext.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<string>>> GetPictureNameByOfferIdAsync(int userId)
    {
        return await _dbContext.Pictures
            .Where(p => p.Offer.UserId == userId)
            .Select(p => p.Name)
            .ToListAsync();
    }
}