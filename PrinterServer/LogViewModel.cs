using PropertyChanged;

namespace PrinterServer
{
    [AddINotifyPropertyChangedInterface]
    public class LogViewModel
    {
        static LogViewModel instance = new LogViewModel();
        public static LogViewModel GetInstance() => instance;
        public static LogViewModel Instance => instance;

        public void L(string s)
        {
            if (Log.Length >= 10000)
            {
                Log = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(Log))
            {
                Log += s;
            }
            else
            {
                Log += Environment.NewLine + s;
            }
        }

        public string Log { get; set; } = string.Empty;
    }
}
