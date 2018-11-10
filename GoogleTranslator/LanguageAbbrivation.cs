using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleTranslator
{
    public class LanguageAbbrivation
    {
        public String Title { get; set; }
        public String Abbrivation { get; set; }

        public LanguageAbbrivation(){ }

        public LanguageAbbrivation(string title,string abbrivation)
        {
            Title = title;
            Abbrivation = abbrivation;
        }
    }
}
