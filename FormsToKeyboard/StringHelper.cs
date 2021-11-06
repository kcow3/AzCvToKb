namespace FormsToKeyboard
{
    public static class StringHelper
    {
        /// <summary>
        /// Convert a string to be safe for the SendKeys method of System.Windows.Forms
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Converted string</returns>
        public static string FormatStringForKeyboardBuffer(this string input)
        {
            return input
                .Replace("+", "{+}")
                .Replace("?", "{?}")
                .Replace("%", "{%}")
                .Replace("^", "{^}")
                .Replace("(", "{(}")
                .Replace(")", "{)}")
                .Replace("{", "{{}")
                .Replace("}", "{}}")
                .Replace("~", "{~}");
        }
    }
}
