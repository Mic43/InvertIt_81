using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace NoNameGame.Helpers
{
    public class ResourcesHelper
    {
        public static ResourceDictionary OpenResourceDictionary(string fileName)
        {
            StreamReader reader;
            string path = @"NoNameGame;component/Themes/" + fileName;
            Uri uri = new Uri(path, UriKind.Relative);
            ResourceDictionary resourceDictionary;
            using (reader = new System.IO.StreamReader((System.Windows.Application.GetResourceStream(uri)).Stream))
            {
                resourceDictionary = (ResourceDictionary)XamlReader.Load(reader.ReadToEnd());
            }
            return resourceDictionary;
        }
        //public static T FindResource<T>(string key)
        //{
//            object obj = Application.Current.Resources[key];
//            if (obj != null)
//                return (T)obj ;
             //Application.Current.Resources.MergedDictionaries.SelectMany(x=>x)
        //}
    }
}