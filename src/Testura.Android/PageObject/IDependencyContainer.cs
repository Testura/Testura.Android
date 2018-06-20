using System;

namespace Testura.Android.PageObject
{
    public interface IDependencyContainer
    {
        object Resolve(Type type);

        void RegisterType(Type type);
    }
}
