using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class FieldDataOption
   {
		public string uuid { get; set; }
		public string name { get; set; }
      public bool value {get;set;}

      public FieldDataOption() { }

		public void merge(FieldDataOption newFieldDataOption)
      {
         this.name = newFieldDataOption.name;
         this.value = newFieldDataOption.value;
      }
	}
}
