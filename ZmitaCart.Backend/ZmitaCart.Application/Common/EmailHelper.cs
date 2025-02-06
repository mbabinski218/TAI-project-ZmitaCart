using System.Web;

namespace ZmitaCart.Application.Common;

public static class EmailHelper
{
	public static string CreateConfirmationLink(int userId, string token)
	{
		var id = userId.ToString();
		
		var bytes = System.Text.Encoding.UTF8.GetBytes(token);
		var base64 = Convert.ToBase64String(bytes);
		var encodedToken = base64.Replace("+", "-").Replace("/", "_").Replace("=", "");
		
		var url = "https://mbabinski218.github.io/ZmitaCart/home/confirm-email";
		
		return $"{url}?Id={id}&Token={encodedToken}";
	}
}