using System.ComponentModel.DataAnnotations;

namespace Pustok.ViewModels
{
	public class MemberRegisterViewModel
	{
        [Required]
        
        public string Username{ get; set; }
		[Required]

		public string Fullname{ get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email{ get; set; }
		[Required]

		public string BirthDate{ get; set; }
		[Required]
		[StringLength(maximumLength:30,MinimumLength =8)]
		[DataType(DataType.Password)]	
		public string Password{ get; set; }
		[Required]
		[DataType(DataType.Password)]
		[StringLength(maximumLength: 30, MinimumLength = 8)]
		[Compare(nameof(Password))]
		public string ConfirmPassword{ get; set; }

    }
}
