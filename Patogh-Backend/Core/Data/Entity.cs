namespace PatoghBackend.Entity
{
	using System.ComponentModel.DataAnnotations.Schema;

	public abstract class Entity<T> : IEntity<T>
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public T Id { get; set; }

		object IEntity.Id => Id;
	}
}