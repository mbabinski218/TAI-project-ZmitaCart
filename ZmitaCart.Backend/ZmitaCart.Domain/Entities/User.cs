using ZmitaCart.Domain.Common.Models;
using ZmitaCart.Domain.ValueObjects;
namespace ZmitaCart.Domain.Entities;

public class User : IdentityEntity<int>
{
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public Address Address { get; set; } = null!;
	public virtual List<Bought> Bought { get; set; } = new();
	public virtual List<UserOffer> Favorites { get; set; } = new();
	public virtual List<Feedback> Feedbacks { get; set; } = new();
	public virtual List<Offer> Offers { get; set; } = new();

	public static User Create(string email, string firstName, string lastName)
	{
		var user = new User
		{
			Email = email,
			UserName = email,
			FirstName = firstName,
			LastName = lastName,
			EmailConfirmed = false
		};

		return user;
	}
}