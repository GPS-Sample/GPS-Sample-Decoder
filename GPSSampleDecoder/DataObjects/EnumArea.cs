using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSSampleDecoder.DataObjects
{
   public class EnumArea
   {
      public string uuid { get; set; }
		public long creationDate { get; set; }
		public string configUuid { get; set; }
		public string name { get; set; }
      public string selectedEnumerationTeamUuid { get; set; }
		public string selectedCollectionTeamUuid { get; set; }
		public List<LatLon> vertices { get; set; }
      public List<Location> locations { get; set; }
      public List<EnumerationTeam> enumerationTeams { get; set; }
      public List<CollectionTeam> collectionTeams { get; set; }
		public MapTileRegion mapTileRegion { get; set; }

		public EnumArea() { }

		public void merge( EnumArea newEnumArea )
		{
			// merge locations

			List<Location> newLocations = new List<Location>();

			foreach (Location newLocation in newEnumArea.locations)
			{
				bool found = false;

				foreach (Location oldLocation in this.locations)
				{
					if (newLocation.uuid == oldLocation.uuid)
					{
						found = true;
						oldLocation.merge(newLocation);
					}
				}

				if (!found)
				{
					newLocations.Add(newLocation);
				}
			}

			foreach (Location location in newLocations)
			{
				this.locations.Add(location);
			}

			// merge enumerationTeams

			List<EnumerationTeam> newEnumerationTeams = new List<EnumerationTeam>();

			foreach (EnumerationTeam newEnumerationTeam in newEnumArea.enumerationTeams)
			{
				bool found = false;

				foreach (EnumerationTeam oldEnumerationTeam in this.enumerationTeams)
				{
					if (newEnumerationTeam.uuid == oldEnumerationTeam.uuid)
					{
						found = true;
						oldEnumerationTeam.merge(newEnumerationTeam);
					}
				}

				if (!found)
				{
					newEnumerationTeams.Add(newEnumerationTeam);
				}
			}

			foreach (EnumerationTeam enumerationTeam in newEnumerationTeams)
			{
				this.enumerationTeams.Add(enumerationTeam);
			}

			// merge collectionTeams

			List<CollectionTeam> newCollectionTeams = new List<CollectionTeam>();

			foreach (CollectionTeam newCollectionTeam in newEnumArea.collectionTeams)
			{
				bool found = false;

				foreach (CollectionTeam oldCollectionTeam in this.collectionTeams)
				{
					if (newCollectionTeam.uuid == oldCollectionTeam.uuid)
					{
						found = true;
						oldCollectionTeam.merge(newCollectionTeam);
					}
				}

				if (!found)
				{
					newCollectionTeams.Add(newCollectionTeam);
				}
			}

			foreach (CollectionTeam collectionTeam in newCollectionTeams)
			{
				this.collectionTeams.Add(collectionTeam);
			}
		}
	}
}
