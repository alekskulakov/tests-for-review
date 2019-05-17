using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenQA.Selenium;
using TestsForReview.WebDriverExt;

namespace TestsForReview.Utils
{
    public static class Helper
    {
        public static T TryGetItem<T>(Func<T> func, string errorMessage)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            while (stopWatch.Elapsed < Config.ElementLoadTimeoutTimeSpan)
            {
                var item = func.Invoke();
                if (item != null)
                    return item;
                else
                    Thread.Sleep(500);
            }
            throw new TimeoutException(errorMessage);
        }

        public static Uri UrlCombine(string url)
        {
            return new Uri(Config.HomePageUrl, url);
        }

        internal static T TryGet<T>(Func<T> func)
        {
            var attempts = 0;
            while (attempts < 5)
            {
                try
                {
                    attempts++;
                    return func.Invoke();
                }
                catch (StaleElementReferenceException)
                {
                    if (attempts == 2)
                        throw;
                    else
                    {
                        Thread.Sleep(200);
                    }

                }
            }
            throw new TestingFrameworkException();
        }

        internal static bool InheritsFrom(this Type type, Type baseType)
        {
            var currentType = type;

            while (currentType != null)
            {
                if (currentType.BaseType == baseType)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }

            return false;
        }

        internal static bool IsPageElement(this Type type)
        {
            return IsPageElement<BasePageElement>(type);
        }

        internal static bool IsPageElement<T>(this Type type)
        {
            return type == typeof(T) || type.InheritsFrom(typeof(T));
        }

        internal static bool DoesImplement<T>(this Type type)
        {
            return type.GetInterfaces().Any(t => t == typeof(T));
        }

        internal static bool IsPageElementList(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(ElementListProxy<>) || type.GetGenericTypeDefinition().InheritsFrom(typeof(ElementListProxy<>));
        }

        internal static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                        "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }

        public static string RemoveInvalidChars(string rawString)
        {
            var invalids = Path.GetInvalidFileNameChars();
            return string.Join("_", rawString.Split(invalids, StringSplitOptions.RemoveEmptyEntries)).TrimEnd('.');
        }
    }
}
