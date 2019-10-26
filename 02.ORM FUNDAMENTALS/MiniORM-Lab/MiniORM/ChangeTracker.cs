
namespace MiniORM
{
    using System.Collections.Generic;
	
    internal class ChangeTracker<T>
        where T : class, new()
    {
        private readonly List<T> allEntities;

        private readonly List<T> added;

        private readonly List<T> removed;
    }
}