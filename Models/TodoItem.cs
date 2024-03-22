using SQLite;

namespace TodoApp.Models
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool Done { get; set; }
        public bool IsPinned { get; set; }
        public DateTime Deadline { get; set; }

        public bool IsImportant { get; set; }
        public TodoItem()
        {
            Deadline = DateTime.Today;
            string dateOnlyString = Deadline.ToString("yyyy-MM-dd"); // Format to display date only
            Console.WriteLine("Date only: " + dateOnlyString);
        }
    }
}