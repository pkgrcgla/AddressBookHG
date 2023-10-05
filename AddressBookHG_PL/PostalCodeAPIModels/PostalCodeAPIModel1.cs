namespace AddressBookHG_PL.PostalCodeAPIModels
{
    public class PostalCodeAPIModel1
    {
        public bool success { get; set; }
        public string status { get; set; }
        public DateTime dataupdatedate { get; set; }
        public string description { get; set; }
        public DateTime pagecreatedate { get; set; }
        public List<PostalCodeAPIModel2> postakodu { get; set; } = new List<PostalCodeAPIModel2>();
    }
}
