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
            UnityContainer = new UnityContainer();
        }

        protected UnityContainer UnityContainer { get; set; }

        /// <summary>
        /// Go through all properties in the class that inherit from the view class
        /// and register them in our unity container
        /// </summary>
        protected void RegisterViewDependencies()
        {
            var pages = GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(View)));
            pages.ForEach(p => UnityContainer.RegisterType(p.PropertyType));
        }

        /// <summary>
        /// Go through all properties in the classt that inherit from the view class
        /// and intialize and solve their dependencies with our unity container
        /// </summary>
        protected virtual void SolveViewDependencies()
        {
            var views = GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(View)));
            foreach (var view in views)
            {
                view.SetValue(this, UnityContainer.Resolve(view.PropertyType));
            }
        }
    }
}
