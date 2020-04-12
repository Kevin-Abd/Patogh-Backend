namespace PatoghBackend.Entity.Models
{
	public class GeoLocation : Entity<long>
	{
		public long DorehamiId { get; set; }

		public Dorehami Dorehami { get; set; }

		public decimal Latitude { get; set; }

		public decimal Longitude { get; set; }
	}
}