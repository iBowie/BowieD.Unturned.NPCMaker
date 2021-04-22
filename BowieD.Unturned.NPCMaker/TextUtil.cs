namespace BowieD.Unturned.NPCMaker
{
    public static class TextUtil
    {
        public static string Shortify(this string original, int maxLength = 40)
        {
            if (string.IsNullOrEmpty(original))
                return original ?? string.Empty;
            string shortened = original.Substring(0, original.Length < maxLength ? original.Length : maxLength);
            if (original.Length > maxLength)
                return shortened + "...";
            return shortened;
        }
    }
}
