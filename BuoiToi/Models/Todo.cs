using BuoiToi.Models.Enums;

namespace BuoiToi.Models
{
    public class Todo
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } =string.Empty;

        public DateTime StartTime { get; set;} = DateTime.Now;

        public DateTime EndTime { get; set; } = DateTime.Now;

        public TodoStatus Status { get; set; } = TodoStatus.TODO;

        public int CategoryId { get; set; }

        public Category Category { get; set; } = new Category();

        public string Link { get; set; } = string.Empty;

        public string GetStartTime () => StartTime.Year + "-" + AddZero(StartTime.Month) + "-" + AddZero(StartTime.Day) + "T" + AddZero(StartTime.Hour) + ":" + AddZero(StartTime.Minute);

        public string GetEndTime() => EndTime.Year + "-" + AddZero(EndTime.Month) + "-" + AddZero(EndTime.Day) + "T" + AddZero(EndTime.Hour) + ":" + AddZero(EndTime.Minute);

        public string AddZero(int num)
        {
            if(num < 10) return "0" + num;
            return num.ToString();
        }
    }
}
