namespace AdvertisingPlatforms.Entities.Models
{
    public class LocationData
    {
        public string Title { get; set; }
        public IEnumerable<LocationSegments> Locations { get; set; }
    }
}
