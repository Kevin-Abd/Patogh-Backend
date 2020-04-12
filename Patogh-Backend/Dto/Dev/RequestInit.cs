namespace PatoghBackend.Dto.Dev
{
	using System.Collections.Generic;

	using PatoghBackend.Dto.Dorehami;
	using PatoghBackend.Dto.User;

	public class RequestInit
	{
		public List<DevObjectUser> Users { get; set; }

		public List<DevObjectDorehami> Dorehamies { get; set; }
	}
}