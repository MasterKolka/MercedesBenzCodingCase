using System.Reflection;

namespace UrlShortener.Helpers;

internal class SwaggerHelpers
{
    public static string XmlCommentsFilePath
    {
        get
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            var fileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            return Path.Combine(basePath, fileName);
        }
    }
}
