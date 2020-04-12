namespace PatoghBackend.Dto.Common
{
	using System.ComponentModel.DataAnnotations;

	public class IdStringWrapper
	{
		[Required(ErrorMessage = "idString not found", AllowEmptyStrings = false)]
		public string IdString { get; set; }
	}
}