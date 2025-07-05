using System.Xml.Serialization;

namespace RTIC.Models
{
    [XmlRoot("TrainingHistory")]
    public class TrainingHistory
    {
        public string? TimeDefault { get; set; }
        public string? TimeUser { get; set; }
        public string? Fecha { get; set; }
        public string? ExerciseDefault { get; set; }
        public TimeSpan? TimeDefaultParsed => TimeSpan.TryParse(TimeDefault, out var t) ? t : null;
        public TimeSpan? TimeUserParsed => TimeSpan.TryParse(TimeUser, out var t) ? t : null;

        public string FechaCorta => DateTime.TryParse(Fecha, out var f) ? f.ToString("dd/MM") : Fecha;

        public string TimeDefaultFormatted => TimeDefaultParsed?.ToString(@"hh\:mm\:ss") ?? "00:00:00";
        public string TimeUserFormatted => TimeUserParsed?.ToString(@"hh\:mm\:ss") ?? "00:00:00";

        public bool IsUserTimeBetter =>
        TimeUserParsed.HasValue && TimeDefaultParsed.HasValue && TimeUserParsed < TimeDefaultParsed;

        public string DisplayText => $"{ExerciseDefault} - {TimeDefaultFormatted} - {TimeUserFormatted}";
    }
}