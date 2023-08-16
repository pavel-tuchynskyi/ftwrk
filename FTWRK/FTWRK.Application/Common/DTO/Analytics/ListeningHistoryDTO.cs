namespace FTWRK.Application.Common.DTO.Analytics
{
    public class ListeningHistoryDTO
    {
        public Guid UserId { get; set; }
        public List<Guid> Albums { get; set; }
        public string Genre { get; set; }
        public int Count { get; set; }
        public DateTime ListeningDate { get; set; }
    }
}
