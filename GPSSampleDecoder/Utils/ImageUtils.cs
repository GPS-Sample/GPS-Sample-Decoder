using System;
using GPSSampleDecoder.DataObjects;
using GPSSampleDecoder.Static;
using System.Collections.Generic;

namespace GPSSampleDecoder.Utils
{
   public class ImageUtils
   {
      public ImageUtils() { }

      public Dictionary<string, byte[]> GetImagesFromConfiguration(Configuration config)
      {

      Dictionary<string, byte[]> output = new Dictionary<string, byte[]>();
      //   foreach(var enumArea in config.enumAreas) 
      //   {
      //      foreach(var location in enumArea.locations) 
      //      {
      //         if(location != null)
      //         {
      //            if(location.isLandmark)
      //            {
      //               // Check image content and description.  if there is no description what is the image of.
      //               // fix the configuration
      //               if (!string.IsNullOrEmpty(location.imageData) && !string.IsNullOrEmpty(location.description))
      //               {
      //                  output.Add(location.description, Convert.FromBase64String(location.imageData));
      //               }
      //            }
      //         }
      //      }
      //   }

      //   foreach (var enumArea in config.enumAreas)
      //   {
      //      foreach (var team in enumArea.collectionTeams)
      //      {
      //         foreach (var locationUuid in team.locationUuids)
      //         {
						//DataObjects.Location location = null;

						//foreach (var loc in enumArea.locations)
						//{
						//	if (locationUuid == loc.uuid)
						//	{
						//		location = loc;
						//		break;
						//	}
						//}

						//if ( !string.IsNullOrEmpty(location.imageData) &&!location.isLandmark)
      //            {
      //               string imageName = location.uuid;
      //               if(location.enumerationItems.Count > 0 )
      //               {
      //                  imageName = location.enumerationItems[0].uuid;
      //                  if (!output.ContainsKey(imageName))
      //                  {
						//			output.Add(imageName, Convert.FromBase64String(location.imageData));
						//		}
						//	}
						//	else
      //               {
						//		if (!output.ContainsKey(imageName))
						//		{
						//			output.Add(imageName, Convert.FromBase64String(location.imageData));
						//		}
						//	}
						//}
      //         }
      //      }
      //   }
         return output;

      }
   }
}
