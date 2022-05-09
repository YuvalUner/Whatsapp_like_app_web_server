namespace AdvancedProgrammingProjectsServer.Models {

    public class Rating {

        public int Id { get; set; }

        public int Stars { get; set; }

        public string? Feedback { get; set; }

        public string? Name { get; set; }

        public DateTime TimeSubmitted { get; set; }
    }
}
