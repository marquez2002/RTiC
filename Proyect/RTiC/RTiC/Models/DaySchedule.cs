using System.Collections.ObjectModel;
using System.ComponentModel;

namespace RTIC.Models
{
    public class DaySchedule : INotifyPropertyChanged
    {
        private ObservableCollection<string> _exercises = new() { "", "", "", "", "", "", "", "" };
        private ObservableCollection<string> _time= new() { "", "", "", "", "", "", "", "" };
        private ObservableCollection<string> _timeUser = new() { "", "", "", "", "", "", "", "" };

        public ObservableCollection<string> Exercises
        {
            get => _exercises;
            set
            {
                _exercises = value;
                OnPropertyChanged(nameof(Exercises));
            }
        }

        public ObservableCollection<string> Time
        {
            get => _time;
            set
            {
                _time= value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public ObservableCollection<string> TimeUser
        {
            get => _timeUser;
            set
            {
                _timeUser = value;
                OnPropertyChanged(nameof(TimeUser));
            }
        }

        public static readonly Dictionary<string, List<(string, string)>> ExercisesByLevel = new()
        {
            ["Inicial"] = new()
            {
                ("🏃 1 km caminata de recuperación", "00:10:00"),
                ("🏃 2 km caminata de recuperación", "00:20:00"),
                ("🏃 3 km caminata de recuperación", "00:30:00"),
                ("🏃 4 km caminata de recuperación", "00:40:00"),
                ("🏃 5 km caminata de recuperación", "00:50:00"),
                ("🏃 6 km caminata de recuperación", "01:00:00"),
                ("🏃 7 km caminata de recuperación", "01:10:00"),
                ("🏃 2 km trote suave", "00:16:00"),
                ("🏃 2 km trote continuo", "00:14:00"),
                ("🏃 2 km trote suave + sprint final", "00:12:00"),
                ("🏃 3 km trote suave", "00:24:00"),
                ("🏃 3 km trote continuo", "00:21:00"),
                ("🏃 3 km trote suave + sprint final", "00:19:00"),
                ("🏃 4 km trote suave", "00:32:00"),
                ("🏃 4 km trote continuo", "00:28:00"),
                ("🏃 4 km trote suave + sprint final", "00:26:00"),
                ("🏃 5 series de 200 m + 30 seg. descanso", "00:24:30"),
                ("🏃 6 series de 200 m + 30 seg. descanso", "00:26:30"),
                ("🏃 5 series de 300 m + 30 seg. descanso", "00:25:30"),
                ("🏃 4 series de 400 m + 30 seg. descanso", "00:24:30"),
                ("🏃 4 series de 500 m + 30 seg. descanso", "00:25:30"),
                ("🏃 3 km alternando ritmos (continuo/suave)", "00:22:30"),
                ("🏃 4 km alternando ritmos (continuo/suave)", "00:30:00"),
                ("🏃 5 km alternando ritmos (continuo/suave)", "00:38:30"),
                ("🏃 1 km trote + 1 km caminata", "00:20:00"),
                ("🏃 2 km trote + 1 km caminata", "00:28:00"),
                ("🏃 2 km trote + 2 km caminata", "00:36:00"),
                ("🏃 3 km trote + 2 km caminata", "00:44:00"),
                ("🏃 20 min trote suave continuo", "00:20:00"),
                ("🏃 25 min trote suave + 5 min caminata", "00:30:00"),
                ("🏃 30 min alternando 3 min trote / 2 min caminata", "00:30:00"),
                ("🏃 40 min trote suave progresivo", "00:40:00"),
                ("🏃 Técnica de carrera + 2 km trote suave", "00:25:00"),
                ("🏃 20 min de técnica + 3 series de 200 m", "00:30:00"),
                ("🏃 15 min trote + 10 min ejercicios de fuerza", "00:25:00"),
                ("🏃 Técnica de skipping, talones, multisaltos + 1 km trote", "00:20:00"),
                ("🏃 15 min caminata + estiramientos", "00:20:00"),
                ("🏃 10 min trote muy suave + 10 min movilidad", "00:20:00"),
                ("🏃 20 min caminata en terreno irregular", "00:20:00"),
            },


            ["Aficionado"] = new()
            {
                ("🏃 3 km caminata de recuperación", "00:27:00"),
                ("🏃 4 km caminata de recuperación", "00:36:00"),
                ("🏃 5 km caminata de recuperación", "00:45:00"),
                ("🏃 6 km caminata de recuperación", "00:55:00"),
                ("🏃 7 km caminata de recuperación", "01:05:00"),
                ("🏃 3 km trote suave", "00:21:00"),
                ("🏃 3 km trote continuo", "00:19:00"),
                ("🏃 3 km trote suave + sprint final", "00:18:00"),
                ("🏃 4 km trote suave", "00:28:00"),
                ("🏃 4 km trote continuo", "00:26:00"),
                ("🏃 4 km trote suave + sprint final", "00:25:00"),
                ("🏃 5 km trote suave", "00:40:00"),
                ("🏃 5 km trote continuo", "00:37:00"),
                ("🏃 5 km trote suave + sprint final", "00:35:00"),
                ("🏃 6 km trote suave", "00:40:00"),
                ("🏃 6 km trote continuo", "00:37:00"),
                ("🏃 6 km trote suave + sprint final", "00:38:00"),
                ("🏃 7 km trote suave", "00:49:00"),
                ("🏃 7 km trote continuo", "00:46:00"),
                ("🏃 5 x 200 m + 30 seg. descanso", "00:20:00"),
                ("🏃 6 x 200 m + 30 seg. descanso", "00:22:30"),
                ("🏃 5 x 300 m + 30 seg. descanso", "00:23:30"),
                ("🏃 4 x 400 m + 30 seg. descanso", "00:22:30"),
                ("🏃 4 x 500 m + 30 seg. descanso", "00:23:30"),
                ("🏃 3 x 600 m + 45 seg. descanso", "00:24:00"),
                ("🏃 3 x 800 m + 1 min descanso", "00:26:00"),
                ("🏃 3 km alternando ritmos (continuo/suave)", "00:19:30"),
                ("🏃 4 km alternando ritmos (continuo/suave)", "00:24:00"),
                ("🏃 5 km alternando ritmos (continuo/suave)", "00:28:30"),
                ("🏃 6 km alternando ritmos (continuo/suave)", "00:34:00"),
                ("🏃 4 km progresivo", "00:26:00"),
                ("🏃 5 km progresivo", "00:32:00"),
                ("🏃 6 km progresivo", "00:38:00"),
                ("🏃 20 min trote continuo", "00:20:00"),
                ("🏃 25 min trote suave", "00:25:00"),
                ("🏃 30 min trote continuo", "00:30:00"),
            },

            ["Profesional"] = new()
            {
                ("🏃 3 km caminata de recuperación", "00:24:00"),
                ("🏃 4 km caminata de recuperación", "00:32:00"),
                ("🏃 5 km caminata de recuperación", "00:40:00"),
                ("🏃 6 km caminata de recuperación", "00:48:00"),
                ("🏃 7 km caminata de recuperación", "00:56:00"),
                ("🏃 3 km trote suave", "00:18:00"),
                ("🏃 3 km trote continuo", "00:15:00"),
                ("🏃 3 km trote suave + sprint final", "00:16:30"),
                ("🏃 4 km trote suave", "00:24:00"),
                ("🏃 4 km trote continuo", "00:20:00"),
                ("🏃 4 km trote suave + sprint final", "00:21:30"),
                ("🏃 5 km trote suave", "00:30:00"),
                ("🏃 5 km trote continuo", "00:25:00"),
                ("🏃 5 km trote suave + sprint final", "00:25:30"),
                ("🏃 6 km trote suave", "00:36:00"),
                ("🏃 6 km trote continuo", "00:30:00"),
                ("🏃 7 km trote continuo", "00:35:00"),
                ("🏃 8 km trote suave", "00:48:00"),
                ("🏃 9 km trote continuo", "00:45:00"),
                ("🏃 10 km trote suave", "00:60:00"),
                ("🏃 12 km trote continuo", "01:00:00"),
                ("🏃 14 km trote suave", "01:12:00"),
                ("🏃 5 x 200 m + 30 seg. descanso", "00:10:00"),
                ("🏃 6 x 200 m + 30 seg. descanso", "00:12:00"),
                ("🏃 5 x 300 m + 30 seg. descanso", "00:12:00"),
                ("🏃 4 x 400 m + 30 seg. descanso", "00:10:30"),
                ("🏃 4 x 500 m + 30 seg. descanso", "00:12:30"),
                ("🏃 3 x 600 m + 45 seg. descanso", "00:13:30"),
                ("🏃 3 x 800 m + 1 min descanso", "00:15:00"),
                ("🏃 4 x 1000 m + 90 seg. descanso", "00:20:00"),
                ("🏃 3 km alternando ritmos", "00:17:30"),
                ("🏃 4 km alternando ritmos", "00:21:30"),
                ("🏃 5 km alternando ritmos", "00:25:30"),
                ("🏃 6 km alternando ritmos", "00:30:00"),
                ("🏃 8 km alternando ritmos", "00:40:00"),
                ("🏃 10 km alternando ritmos", "00:50:00"),
                ("🏃 5 km progresivo", "00:24:00"),
                ("🏃 6 km progresivo", "00:28:00"),
                ("🏃 7 km progresivo", "00:32:00"),
                ("🏃 10 km progresivo", "00:45:00"),
                ("🏃 12 km progresivo", "00:54:00"),
                ("🏃 30 min trote continuo", "00:30:00"),
                ("🏃 35 min trote suave", "00:35:00"),
                ("🏃 40 min trote progresivo", "00:40:00"),
                ("🏃 60 min trote suave", "01:00:00"),
            },

            ["Senior"] = new()
            {
                ("🏃 3 km caminata de recuperación", "00:21:00"),
                ("🏃 4 km caminata de recuperación", "00:28:00"),
                ("🏃 5 km caminata de recuperación", "00:35:00"),
                ("🏃 6 km caminata de recuperación", "00:42:00"),
                ("🏃 7 km caminata de recuperación", "00:49:00"),
                ("🏃 8 km caminata de recuperación", "00:56:00"),
                ("🏃 9 km caminata de recuperación", "01:03:00"),
                ("🏃 10 km caminata de recuperación", "01:10:00"),
                ("🏃 11 km caminata de recuperación", "01:17:00"),
                ("🏃 12 km caminata de recuperación", "01:24:00"),
                ("🏃 3 km trote suave", "00:16:30"),
                ("🏃 4 km trote suave", "00:22:00"),
                ("🏃 5 km trote suave", "00:27:30"),
                ("🏃 6 km trote suave", "00:33:00"),
                ("🏃 7 km trote suave", "00:38:30"),
                ("🏃 8 km trote suave", "00:44:00"),
                ("🏃 9 km trote suave", "00:49:30"),
                ("🏃 10 km trote suave", "00:55:00"),
                ("🏃 11 km trote suave", "01:06:00"),
                ("🏃 12 km trote suave", "01:14:00"),
                ("🏃 13 km trote suave", "01:22:00"),
                ("🏃 14 km trote suave", "01:30:00"),
                ("🏃 15 km trote suave", "01:38:00"),
                ("🏃 3 km trote continuo", "00:13:30"),
                ("🏃 4 km trote continuo", "00:18:00"),
                ("🏃 5 km trote continuo", "00:23:00"),
                ("🏃 6 km trote continuo", "00:27:30"),
                ("🏃 7 km trote continuo", "00:32:00"),
                ("🏃 8 km trote continuo", "00:36:30"),
                ("🏃 9 km trote continuo", "00:41:00"),
                ("🏃 10 km trote continuo", "00:45:30"),
                ("🏃 11 km trote continuo", "00:56:00"),
                ("🏃 12 km trote continuo", "01:02:00"),
                ("🏃 13 km trote continuo", "01:08:00"),
                ("🏃 14 km trote continuo", "01:14:00"),
                ("🏃 15 km trote continuo", "01:20:00"),
                ("🏃 3 km trote suave + sprint final", "00:15:00"),
                ("🏃 4 km trote suave + sprint final", "00:19:30"),
                ("🏃 5 km trote suave + sprint final", "00:24:00"),
                ("🏃 6 km trote suave + sprint final", "00:28:30"),
                ("🏃 7 km trote suave + sprint final", "00:33:00"),
                ("🏃 8 km trote suave + sprint final", "00:37:30"),
                ("🏃 9 km trote suave + sprint final", "00:42:00"),
                ("🏃 10 km trote suave + sprint final", "00:46:30"),
                ("🏃 11 km trote suave + sprint final", "01:08:00"),
                ("🏃 12 km trote suave + sprint final", "01:16:00"),
                ("🏃 13 km trote suave + sprint final", "01:24:00"),
                ("🏃 14 km trote suave + sprint final", "01:32:00"),
                ("🏃 5 series de 200 m + 30 seg. descanso", "00:09:00"),
                ("🏃 6 series de 200 m + 30 seg. descanso", "00:10:30"),
                ("🏃 5 series de 300 m + 30 seg. descanso", "00:11:00"),
                ("🏃 4 series de 400 m + 30 seg. descanso", "00:09:30"),
                ("🏃 4 series de 500 m + 30 seg. descanso", "00:11:00"),
                ("🏃 4 series de 600 m + 45 seg. descanso", "00:12:00"),
                ("🏃 3 series de 800 m + 1 min descanso", "00:13:30"),
                ("🏃 3 series de 1000 m + 1 min descanso", "00:15:00"),
                ("🏃 3 km alternando ritmos (continuo/suave)", "00:16:00"),
                ("🏃 4 km alternando ritmos (continuo/suave)", "00:20:00"),
                ("🏃 5 km alternando ritmos (continuo/suave)", "00:23:30"),
                ("🏃 6 km alternando ritmos (continuo/suave)", "00:27:30"),
                ("🏃 7 km alternando ritmos (continuo/suave)", "00:31:30"),
                ("🏃 8 km alternando ritmos (continuo/suave)", "00:35:30"),
                ("🏃 9 km alternando ritmos (continuo/suave)", "00:39:30"),
                ("🏃 10 km alternando ritmos (continuo/suave)", "00:43:30"),
                ("🏃 11 km alternando ritmos (continuo/suave)", "00:52:00"),
                ("🏃 12 km alternando ritmos (continuo/suave)", "00:58:00"),
                ("🏃 13 km alternando ritmos (continuo/suave)", "01:04:00"),
                ("🏃 14 km alternando ritmos (continuo/suave)", "01:10:00"),
            },     
        };

        public static readonly Dictionary<string, List<(string, string)>> ChallengesByLevel = new()
        {
            ["Inicial"] = new()
            {
                ("🏆 7 km caminata", "01:00:00"),
                ("🏆 8 km caminata", "01:10:00"),
                ("🏆 5 km trote suave", "00:40:00"),
                ("🏆 5 km trote continuo", "00:37:00"),
                ("🏆 5 km trote suave + sprint final", "00:35:00"),
                ("🏆 6 km trote suave", "00:48:00"),
                ("🏆 6 km trote continuo", "00:45:00"),
                ("🏆 6 km trote suave + sprint final", "00:43:00"),
                ("🏆 5 km trail running", "00:50:00"),
                ("🏆 7 km trote suave", "00:56:00"),
                ("🏆 7 km trote continuo", "00:52:00"),
                ("🏆 8 km trote suave", "01:04:00"),
                ("🏆 8 km trote continuo", "01:00:00"),
                ("🏆 6 km progresivo (cada km más rápido)", "00:42:00"),
                ("🏆 5 km con cambios de ritmo cada 1 km", "00:38:00"),
                ("🏆 30 min trote continuo sin parar", "00:30:00"),
                ("🏆 2 sesiones de 4 km + 1 sesión de 5 km", "01:15:00"),
                ("🏆 4 km trote suave + 4 x 200 m rápidos", "00:40:00"),
                ("🏆 5 km con sprint final de 500 m", "00:36:00"),
                ("🏆 5 km en circuito urbano con subidas", "00:42:00"),
                ("🏆 3 km suave + 2 km ritmo medio", "00:35:00"),
                ("🏆 4 km suave + 1 km rápido", "00:34:00"),
            },


            ["Aficionado"] = new()
            {
                ("🏆 6 km trote continuo", "00:37:00"),
                ("🏆 7 km trote suave", "00:49:00"),
                ("🏆 7 km trote continuo", "00:46:00"),
                ("🏆 8 km trote suave", "00:56:00"),
                ("🏆 8 km trote continuo", "00:52:00"),
                ("🏆 6 km progresivo (cada km más rápido)", "00:42:00"),
                ("🏆 5 km con cambios de ritmo cada 1 km", "00:36:00"),
                ("🏆 30 min trote continuo sin parar", "00:30:00"),
                ("🏆 5 km con sprint final de 1 km", "00:34:00"),
                ("🏆 6 km en terreno mixto (asfalto/tierra)", "00:44:00"),
                ("🏆 7 km en circuito urbano con subidas", "00:50:00"),
                ("🏆 4 km suave + 2 km ritmo medio", "00:38:00"),
                ("🏆 5 km suave + 1 km rápido", "00:39:00"),
                ("🏆 9 km trote suave", "01:05:00"),
                ("🏆 10 km trote continuo", "01:00:00"),
                ("🏆 5 km trail running con desnivel", "00:50:00"),
            },

            ["Profesional"] = new()
            {
                 ("🏆 8 km trote continuo", "00:40:00"),
                 ("🏆 9 km trote suave", "00:48:00"),
                 ("🏆 10 km trote continuo", "00:50:00"),
                 ("🏆 11 km trote suave", "00:55:00"),
                 ("🏆 12 km trote continuo", "01:00:00"),
                 ("🏆 13 km trote suave", "01:08:00"),
                 ("🏆 14 km trote continuo", "01:10:00"),
                 ("🏆 6 km progresivo (cada km más rápido)", "00:28:00"),
                 ("🏆 7 km progresivo", "00:32:00"),
                 ("🏆 10 km progresivo", "00:45:00"),
                 ("🏆 5 km con cambios de ritmo cada 1 km", "00:26:00"),
                 ("🏆 8 km con cambios de ritmo cada 500 m", "00:42:00"),
                 ("🏆 3 sesiones de 6 km en la semana", "01:30:00"),
                 ("🏆 2 sesiones de 7 km + 1 de 8 km", "02:00:00"),
                 ("🏆 10 km en terreno mixto (asfalto/tierra)", "00:55:00"),
                 ("🏆 8 km con sprint final de 1 km", "00:42:00"),
                 ("🏆 6 km en circuito urbano con subidas", "00:38:00"),
                 ("🏆 4 km suave + 4 km ritmo medio", "00:38:00"),
                 ("🏆 5 km suave + 3 km rápido", "00:36:00"),
                 ("🏆 3 sesiones de 40 min trote continuo", "02:00:00"),
                 ("🏆 12 km trote suave", "01:00:00"),
                 ("🏆 10 km trail running con desnivel", "01:05:00"),
                 ("🏆 5 x 1000 m a ritmo de competencia", "00:25:00")
            },

            ["Senior"] = new()
            {
                ("🏆 11 km caminata de recuperación", "01:17:00"),
                ("🏆 12 km caminata de recuperación", "01:24:00"),
                ("🏆 14 km caminata de recuperación", "01:38:00"),
                ("🏆 16 km caminata de recuperación", "01:52:00"),
                ("🏆 18 km caminata de recuperación", "02:06:00"),
                ("🏆 11 km trote suave", "01:00:00"),
                ("🏆 12 km trote suave", "01:06:00"),
                ("🏆 14 km trote suave", "01:18:00"),
                ("🏆 16 km trote suave", "01:30:00"),
                ("🏆 18 km trote suave", "01:42:00"),
                ("🏆 20 km trote suave", "01:54:00"),
                ("🏆 11 km trote continuo", "00:55:00"),
                ("🏆 12 km trote continuo", "01:00:00"),
                ("🏆 14 km trote continuo", "01:10:00"),
                ("🏆 16 km trote continuo", "01:20:00"),
                ("🏆 18 km trote continuo", "01:30:00"),
                ("🏆 20 km trote continuo", "01:40:00"),
                ("🏆 11 km trote suave + sprint final", "01:02:00"),
                ("🏆 12 km trote suave + sprint final", "01:08:00"),
                ("🏆 14 km trote suave + sprint final", "01:20:00"),
                ("🏆 16 km trote suave + sprint final", "01:32:00"),
                ("🏆 18 km trote suave + sprint final", "01:44:00"),
                ("🏆 20 km trote suave + sprint final", "01:56:00"),
                ("🏆 12 km alternando ritmos (continuo/suave)", "01:04:00"),
                ("🏆 14 km alternando ritmos (continuo/suave)", "01:12:00"),
                ("🏆 16 km alternando ritmos (continuo/suave)", "01:20:00"),
                ("🏆 18 km alternando ritmos (continuo/suave)", "01:28:00"),
                ("🏆 20 km alternando ritmos (continuo/suave)", "01:36:00"),
                ("🏆 12 km progresivo", "01:00:00"),
                ("🏆 14 km progresivo", "01:08:00"),
                ("🏆 16 km progresivo", "01:16:00"),
                ("🏆 18 km progresivo", "01:24:00"),
                ("🏆 20 km progresivo", "01:32:00"),
                ("🏆 21 km MEDIA MARATON", "01:40:00"),

            },
        };


        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}