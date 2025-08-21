using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class MapTileRegion
   {
		public string uuid { get; set; }
      public LatLon northEast { get; set; }
      public LatLon southWest { get; set; }
      
      public MapTileRegion() { }
   }
}
