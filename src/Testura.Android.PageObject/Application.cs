using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Testura.Android.PageObject
{
    /// <summary>
    /// Represent the base class for an application under test
    /// </summary>
    public abstract class Application
    {
        protected Application()
        {
            Container = new UnityContainer();
        }

        protected UnityContainer Container { get; set; }

        /// <summary>
        /// Go through all properties in the class that inherit from the view class
        /// and register them in our unity container
        /// </summary>
        protected void RegisterViewDependencies()
        {
            var pages = GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(View)));
            pages.ForEach(p => Container.RegisterType(p.PropertyType));
        }

        /// <summary>
        /// Go through all properties in the class that inherit from the view class
        /// and initialize and solve their dependencies with our unity container
        /// </summary>
        protected virtual void SolveViewDependencies()
        {
            var views = GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(View)));
            foreach (var view in views)
            {
                view.SetValue(this, Container.Resolve(view.PropertyType));
            }
        }
    }
}
