using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels
{
	public class MemberLoginViewModel
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		[StringLength(maximumLength: 30, MinimumLength = 12)]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[StringLength(maximumLength: 30, MinimumLength = 8)]
		public string Password { get; set; }
    }
}
