using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
	public class Image
	{
		public string uuid { get; set; }
		public long creationDate { get; set; }
		public string locationUuid { get; set; }
		public string data { get; set; }

		public Image() { }
	}
}