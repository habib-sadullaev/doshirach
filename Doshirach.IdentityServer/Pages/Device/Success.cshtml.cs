using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Doshirach.IdentityServer.Pages.Device
{
	[SecurityHeaders]
	[Authorize]
	public class SuccessModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}