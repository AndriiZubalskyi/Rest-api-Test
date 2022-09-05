namespace RestAPI.Helpers
{
    public class Functions
    {
        static string Reverse(string s)
        {
            string res = "";
            for (int i = s.Length - 1; i >= 0; i--)
                res += s[i];
            return res;
        }

        static bool IsFibbonachi(int x)
        {
            int a = 1, b = 1, c = 1;
            while (c < x)
            {
                c = a + b;
                a = b;
                b = c;
            }
            return c == x && c != 0;
        }

        public static string[] TextEdit(string[] text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (IsFibbonachi(i + 1)) text[i] = Reverse(text[i]);
            }            
            return text;
        }
    }
}
