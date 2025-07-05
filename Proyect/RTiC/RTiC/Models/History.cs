using System.Xml.Serialization;

namespace RTIC.Models
{
    [XmlRoot("RunnerHistory")]
    public class RunnerHistory
    {
        [XmlElement("Runner")]
        public List<Runner> Runners { get; set; } = new List<Runner>();
    }

    [XmlRoot("TrainingUserHistory")]
    public class TrainingUserHistory
    {
        [XmlElement("TrainingHistories")]
        public List<TrainingHistory> TrainingHistories { get; set; } = new List<TrainingHistory>();
    }

    [XmlRoot("ExercisesWeekHistory")]
    public class ExercisesWeekHistory
    {
        [XmlElement("TrainingHistories")]
        public List<TrainingHistory> ExercisesHistory { get; set; } = new List<TrainingHistory>();
    }
}
