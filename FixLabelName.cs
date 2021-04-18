using System.Collections.Generic;
using System.Linq;

namespace cryptoFinance
{
    public static class FixLabelName
    {
        public static string ReturnName(string name)
        {
            var chars = name.ToCharArray().ToList();
            string fixedName = ValidateChars(chars, name);
            return fixedName;
        }

        private static string ValidateChars(List<char> chars, string name)
        {
            int caseNr = -1;
            int test;
            string fixedName = "";

            for (int i = 0; i < 3; i++)
            {
                if(int.TryParse(chars[i].ToString(), out test))
                {
                    caseNr = i;
                }                
            }

            if (caseNr < 3 && (chars[caseNr + 1] == '.' && chars[caseNr + 2] == ' '))
            {
                for (int i = caseNr + 3; i < chars.Count; i++)
                {
                    fixedName += chars[i];
                }
            }
            else
            {
                fixedName = name;
            }
            
            return fixedName;
        }

    }
}
