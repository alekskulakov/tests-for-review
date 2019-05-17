using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestsForReview.Interfaces;
using TestsForReview.Utils;

namespace TestsForReview.WebDriverExt
{
    public sealed class CustomPageFactory
    {
        public static IContainer Container;

        static CustomPageFactory()
        {
            var builder = new ContainerBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            var typesToRegister = assembly.GetTypes().Where(t => t.IsPageElement());

            foreach (var type in typesToRegister)
            {
                builder.RegisterType(type).InstancePerDependency().OnActivated(args =>
                {
                    var basePageElement = args.Instance as BasePageElement;
                    InitElements(basePageElement);
                });
            }

            Container = builder.Build();
        }
        public static void InitElements(BasePageElement pageObject)
        {
            if (pageObject == null)
            {
                throw new ArgumentNullException(nameof(pageObject), "Page cannot be null");
            }

            const BindingFlags PublicBindingOptions = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var type = pageObject.GetType();
            var members = new List<MemberInfo>();
            members.AddRange(type.GetFields(PublicBindingOptions));
            members.AddRange(type.GetProperties(PublicBindingOptions));

            foreach (var member in members.Where(m => m.GetUnderlyingType().DoesImplement<IHasLocator>()))
            {
                #region prep

                FieldInfo field = member as FieldInfo;
                PropertyInfo property = member as PropertyInfo;

                Type targetType = null;

                if (field != null)
                {
                    targetType = field.FieldType;
                }

                bool hasPropertySet = false;
                if (property != null)
                {
                    hasPropertySet = property.CanWrite;
                    targetType = property.PropertyType;
                }

                if (field == null & (property == null || !hasPropertySet))
                {
                    continue;
                }
                #endregion

                var by = CreateLocator(member);

                var decoratedObject = Activator.CreateInstance(targetType);
                var decoratedValue = (IHasLocator)decoratedObject;
                var parentLocator = IsRoot(member) ? null : pageObject.Locator;

                if (by == null)
                {
                    if (decoratedValue.Locator == null)
                        decoratedValue.Locator = parentLocator;
                    else
                        decoratedValue.Locator.SetParentLocator(parentLocator);
                }
                else
                {
                    var tempLocator = LocatorFactory.Create(parentLocator, by);

                    decoratedValue.Locator = tempLocator;
                }
                if (decoratedObject is BasePageElement memberObject)
                    InitElements(memberObject);

                if (field != null)
                {
                    field.SetValue(pageObject, decoratedValue);
                }
                else if (property != null)
                {
                    property.SetValue(pageObject, decoratedValue, null);
                }
            }
        }

        private static By CreateLocator(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member", "memeber cannot be null");
            }

            var useSequenceAttributes = Attribute.GetCustomAttributes(member, typeof(FindsBySequenceAttribute), true);
            bool useSequence = useSequenceAttributes.Length > 0;

            var useFindAllAttributes = Attribute.GetCustomAttributes(member, typeof(FindsByAllAttribute), true);
            bool useAll = useFindAllAttributes.Length > 0;

            if (useSequence && useAll)
            {
                throw new ArgumentException("Cannot specify FindsBySequence and FindsByAll on the same member");
            }

            var bys = new List<By>();
            var attributes = Attribute.GetCustomAttributes(member, typeof(FindsByAttribute), true);
            if (attributes.Length > 0)
            {
                Array.Sort(attributes);
                foreach (var attribute in attributes)
                {
                    var castedAttribute = (FindsByAttribute)attribute;
                    if (castedAttribute.Using == null)
                    {
                        castedAttribute.Using = member.Name;
                    }

                    bys.Add(BuildBy(castedAttribute));
                }

                if (useSequence)
                {
                    ByChained chained = new ByChained(bys.ToArray());
                    bys.Clear();
                    bys.Add(chained);
                }

                if (useAll)
                {
                    ByAll all = new ByAll(bys.ToArray());
                    bys.Clear();
                    bys.Add(all);
                }
            }

            return bys.LastOrDefault();
        }
        private static By BuildBy(FindsByAttribute findsBy)
        {
            How how = findsBy.How;
            string usingStr = findsBy.Using;
            Type customType = findsBy.CustomFinderType;

            switch (how)
            {
                case How.Id:
                    return By.Id(usingStr);
                case How.Name:
                    return By.Name(usingStr);
                case How.TagName:
                    return By.TagName(usingStr);
                case How.ClassName:
                    return By.ClassName(usingStr);
                case How.CssSelector:
                    return By.CssSelector(usingStr);
                case How.LinkText:
                    return By.LinkText(usingStr);
                case How.PartialLinkText:
                    return By.PartialLinkText(usingStr);
                case How.XPath:
                    return By.XPath(usingStr);
                case How.Custom:
                    ConstructorInfo ctor = customType.GetConstructor(new Type[] { typeof(string) });
                    By finder = ctor.Invoke(new object[] { usingStr }) as By;
                    return finder;
                default:
                    throw new ArgumentException($"Did not know how to construct How from how {how}, using {usingStr}");
            }
        }

        private static bool IsRoot(MemberInfo member)
        {
            return Attribute.GetCustomAttributes(member, typeof(RootByAttribute), true).Length > 0;
        }
    }
}
