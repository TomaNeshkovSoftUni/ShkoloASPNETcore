using System.ComponentModel.DataAnnotations;
using ShkoloASPNETcore.Infrastructure.Data.Enums;
using ShkoloASPNETcore.Infrastructure.Data.Models;

namespace ShkoloASPNETcore.Web.Models
{
    public class RemarkFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Моля, изберете ученик.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Моля, изберете предмет.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "Моля, изберете вид.")]
        public RemarkType Type { get; set; }

        [Required(ErrorMessage = "Моля, изберете конкретна забележка/похвала.")]
        public string SelectedText { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "Коментарът е твърде дълъг.")]
        public string? Comment { get; set; }

        public static List<string> Praises => new()
        {
            "Активно участие", "Изпълнена задача", "Старание", "Отлично представяне", "Обща похвала",
            "Работа в екип", "Прогрес", "Любознание", "Концентрация", "Бодър ум", "Креативност",
            "Двигателна култура", "Комуникативност", "Музикална култура", "Толерантност",
            "Презентационни умения", "Емоционална интелигентност", "Родолюбие", "Дигитални умения",
            "Олимпиец", "Лидерство", "Следобеден сън"
        };

        public static List<string> Penalties => new()
        {
            "Без домашна работа", "Лоша дисциплина", "Липса на внимание", "Без учебно помагало",
            "Без учебни пособия", "Без подготовка", "Обща забележка", "Неуважение", "Слабо представяне",
            "Без екип", "Официална забележка", "Отстранен от час", "Агресия", "Закъснение",
            "Отсъствие", "Без униформа", "Обяд", "Закуска"
        };
    }
}