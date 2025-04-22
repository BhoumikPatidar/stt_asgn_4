using System;
using System.Threading;

namespace AlarmConsoleApp
{
    // Publisher class: raises the alarm event
    class AlarmClock
    {
        private readonly string _targetTime;  // "HH:mm:ss"
        public event EventHandler? RaiseAlarm;

        public AlarmClock(string targetTime)
        {
            _targetTime = targetTime;
        }

        public void Start()
        {
            while (true)
            {
                // Get current system time as HH:mm:ss
                string now = DateTime.Now.ToString("HH:mm:ss");
                if (now == _targetTime)
                {
                    OnRaiseAlarm();
                    break;
                }
                Thread.Sleep(1000);  // wait 1 second before checking again
            }
        }

        protected virtual void OnRaiseAlarm()
        {
            RaiseAlarm?.Invoke(this, EventArgs.Empty);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter alarm time in HH:mm:ss format:");
            string? input = Console.ReadLine();

            // Validate input
            if (string.IsNullOrWhiteSpace(input)
                || !TimeSpan.TryParse(input, out _))
            {
                Console.WriteLine("Invalid time format. Please use HH:mm:ss (e.g. 14:30:00).");
                return;
            }

            // Instantiate publisher and subscribe
            var alarm = new AlarmClock(input);
            alarm.RaiseAlarm += RingAlarm;

            Console.WriteLine($"Alarm set for {input}. Waiting...");
            alarm.Start();

            // Give user a chance to see the message before console closes
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        // Subscriber method
        private static void RingAlarm(object? sender, EventArgs e)
        {
            Console.WriteLine();
            Console.WriteLine("⏰⏰⏰  Alarm! Time's up!  ⏰⏰⏰");
        }
    }
}