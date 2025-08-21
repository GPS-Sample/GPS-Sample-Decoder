using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class EnumerationTeam
   {
		public string uuid { get; set; }
		public long creationDate { get; set; }
      public string enumAreaUuid { get; set; }
      public string name { get; set; }
		public List<LatLon> polygon { get; set; }
		public List<String> locationUuids { get; set; }

      public EnumerationTeam() { }

		public void merge(EnumerationTeam newEnumerationTeam)
		{
		}
	}
}
