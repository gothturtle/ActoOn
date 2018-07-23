using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActonTranslation
{
    public class TranslationJob
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public string OriginalWord { get; set; }
        public string TranslatedWord { get; set; }
    }
}
