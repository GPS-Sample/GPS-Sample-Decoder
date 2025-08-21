using GPSSampleDecoder.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class Filter
   {
		public string uuid { get; set; }
      public string name { get; set; }
      public string samplingType { get; set; }
      public int sampleSize { get; set; }
      public Rule rule { get; set; }

      public Filter() { }
   }
}
