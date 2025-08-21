using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
	public class ImageList
	{
		public string configUuid { get; set; }
		public List<Image> images { get; set; }

		public ImageList() { }
	}
}