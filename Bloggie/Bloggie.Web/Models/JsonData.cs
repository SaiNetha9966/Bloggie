namespace Bloggie.Web.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ErrorMessage
    {
        public int ErrorCode { get; set; }
        public string ErrorDetails { get; set; }
        public object OfflineMessage { get; set; }
    }

    public class GetClientStore
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int ClientStoreId { get; set; }
        public string ClientStoreName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string StateName { get; set; }
        public bool StoreEcomStatus { get; set; }
        public string StoreEmail { get; set; }
        public int StoreNumber { get; set; }
        public string StorePhoneNumber { get; set; }
        public string StoreTimings { get; set; }
        public int WeeklyAdStoreId { get; set; }
        public string ZipCode { get; set; }
    }

    public class Root
    {
        public bool EnableStoreManagement { get; set; }
        public ErrorMessage ErrorMessage { get; set; }
        public List<GetClientStore> GetClientStores { get; set; }
    }


}
