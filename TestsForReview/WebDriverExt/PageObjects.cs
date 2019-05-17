using System.Linq;
using System.Reflection;
using Autofac;
using TestsForReview.Interfaces;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public static class PageObjects
    {
        public static readonly IContainer Container;
        private static readonly object LockObj = new object();

        static PageObjects()
        {
            lock (LockObj)
            {
                var builder = new ContainerBuilder();

                var assembly = Assembly.GetExecutingAssembly();
                var typesToRegister = assembly.GetTypes().Where(t => t.DoesImplement<IHasLocator>());

                foreach (var type in typesToRegister)
                {
                    var regBuilder = builder.RegisterType(type).OnActivated(args =>
                    {
                        var basePageElement = args.Instance as BasePageElement;
                        CustomPageFactory.InitElements(basePageElement);
                    });
                    if (typeof(ISingleInstance).IsAssignableFrom(type))
                        regBuilder.SingleInstance();
                    else
                        regBuilder.InstancePerDependency();
                }

                Container = builder.Build();
            }
        }

        public static T Get<T>() where T : BasePageElement
        {
            return Container.Resolve<T>();
        }
    }
}
