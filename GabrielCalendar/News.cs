using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GabrielCalendar
{
    public class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imagePath { get; set; }
        public string link { get; set; }
        public DateTime date { get; set; }
    }
}
