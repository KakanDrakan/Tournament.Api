namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}