using System;

namespace Testura.Android.PageObject
{
    public interface IDependencyContainer
    {
        object Resolve(Type type);

        void RegisterInstance(Type type, object obj);

        void RegisterType(Type type);
    }
}
