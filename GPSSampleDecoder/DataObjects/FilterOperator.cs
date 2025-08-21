using GPSSampleDecoder.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace GPSSampleDecoder.DataObjects
{
   public class FilterOperator
   {
		public string uuid { get; set; }
      public int order { get; set; }
      public string connector { get; set; }
      public Rule rule { get; set; }

      public FilterOperator() { }
   }
}
