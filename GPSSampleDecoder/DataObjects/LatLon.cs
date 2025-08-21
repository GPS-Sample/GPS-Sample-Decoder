using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class LatLon
   {
		public string uuid { get; set; }
		public long creationDate { get; set; }
		public double latitude { get; set; }
      public double longitude { get; set; }

      public LatLon() { }
   }
}
