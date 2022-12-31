namespace API.Data.Entities
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicPhotoId { get; set; }
        public int UserId { get; set; }
        public virtual AppUser AppUser {get; set;}
    }
}