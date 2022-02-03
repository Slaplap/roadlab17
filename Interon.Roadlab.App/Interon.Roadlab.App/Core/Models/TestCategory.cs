using SQLite;

namespace Interon.Roadlab.App.Core.Models
{
    public class TestCategory

    {
        [PrimaryKey] public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class Test

    {
        [PrimaryKey] public int Id { get; set; }
        public int TestCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}