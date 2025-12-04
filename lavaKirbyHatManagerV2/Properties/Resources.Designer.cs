using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace lKHM.Properties
{
    /// <summary>
    /// Strongly-typed resource class for looking up localized strings.
    /// </summary>
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [DebuggerNonUserCode]
    [CompilerGenerated]
    internal class Resources
    {
        private static ResourceManager? resourceMan;
        private static CultureInfo? resourceCulture;

        internal Resources() { }

        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (resourceMan == null)
                {
                    resourceMan = new ResourceManager("lKHM.Properties.Resources", typeof(Resources).Assembly);
                }
                return resourceMan;
            }
        }

        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all resource lookups.
        /// </summary>
        internal static CultureInfo? Culture
        {
            get => resourceCulture;
            set => resourceCulture = value;
        }

        /// <summary>
        /// Looks up a localized string similar to v1.0.1.
        /// </summary>
        internal static string Version
        {
            get => ResourceManager.GetString("Version", resourceCulture) ?? string.Empty;
        }
    }

    /// <summary>
    /// Avalonia markup extension to use Resources.cs strings in XAML.
    /// </summary>
    public class ResxExtension : MarkupExtension
    {
        /// <summary>
        /// Name of the resource property to retrieve from Resources.cs
        /// </summary>
        public string Key { get; set; } = string.Empty;

        public ResxExtension() { }

        public ResxExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                return string.Empty;

            // Look for the static property in Resources class
            var prop = typeof(Resources).GetProperty(Key, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop == null)
                return string.Empty;

            return prop.GetValue(null) ?? string.Empty;
        }
    }
}
