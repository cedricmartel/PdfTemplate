
namespace Moon.PDFDraw.Helpers
{
    public class IntHelper
    {
        /// <summary>
        /// fait un int.tryparse avec prise en charge d'une valeur par defaut
        /// </summary>
        public static int TryParseDefault(string stringToParse, int defaultValue)
        {
            return TryParseDefault(stringToParse, (int?)defaultValue) ?? defaultValue;
        }

        public static int? TryParseDefault(string stringToParse, int? defaultValue)
        {
            int res;
            if (!string.IsNullOrEmpty(stringToParse) && int.TryParse(stringToParse, out res))
                return res;

            return defaultValue;
        }
    }
}
