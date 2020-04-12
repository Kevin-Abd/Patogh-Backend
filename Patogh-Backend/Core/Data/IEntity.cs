namespace PatoghBackend.Entity
{
	public interface IEntity
	{
		object Id { get; }
	}

	public interface IEntity<T> : IEntity
	{
		new T Id { get; set; }
	}
}