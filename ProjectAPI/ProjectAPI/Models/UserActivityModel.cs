namespace ProjectAPI.Models
{
    public class UserActivityModel
    {
        public string Id { get; set; }
        public int Count { get; set; }

        public UserActivityModel(string id, int count)
        {
            Id = id;
            Count = count;
        }
    }
}
