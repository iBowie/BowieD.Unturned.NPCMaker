using System.Linq;

namespace BowieD.NPCMaker.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string origin)
        {
            switch (origin)
            {
                case null:
                case "":
                    return origin;
                default:
                    return char.ToUpper(origin[0]) + new string(origin.Skip(1).Select(d => char.ToLower(d)).ToArray());
            }
        }
    }
}
