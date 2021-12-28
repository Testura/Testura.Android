namespace Testura.Android.PageObject
{
    /// <summary>
    /// Defines an interface that to resolve and register types. Used to wrap
    /// containers for dependency injection.
    /// </summary>
    public interface IDependencyContainer
    {
        /// <summary>
        /// Get an instance of the requested type with the given name from the container.
        /// </summary>
        /// <param name="type"><see cref="Type"/> of object to get from the container.</param>
        /// <returns>The retrieved object.</returns>
        object Resolve(Type type);

        /// <summary>
        /// RegisterType an instance with the container.
        /// </summary>
        /// <param name="type">Type of instance to register (may be an implemented interface instead of the full type).</param>
        /// <param name="obj">Object to returned.</param>
        void RegisterInstance(Type type, object obj);

        /// <summary>
        /// RegisterType an instance with the container.
        /// </summary>
        /// <param name="type">Type to register</param>
        void RegisterType(Type type);

        /// <summary>
        /// RegisterType an instance with the container.
        /// </summary>
        /// <typeparam name="T">The type to register.</typeparam>
        void RegisterType<T>();
    }
}
