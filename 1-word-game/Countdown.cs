using System.Timers;
using Timer = System.Timers.Timer;

namespace _1_word_game
{
    internal class Countdown
    {
        private const int CountdownInterval = 1000;

        private byte _countdown = 0;
        private Timer _countdownTimer = new Timer();

        public Countdown()
        {
            _countdownTimer.Interval = CountdownInterval;
            _countdownTimer.Elapsed += CountdownTimerElapsed;
        }

        public void Start()
        {
            if (!_countdownTimer.Enabled)
            {
                _countdown = 30;
                _countdownTimer.Stop();
                _countdownTimer.Start();
            }
        }

        public void Stop()
        {
            _countdown = 0;
            _countdownTimer.Stop();
        }

        private void CountdownTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _countdown--;
            InputOutput.PrintUnderCursorAndGoBack(messages: [$"Time left: {_countdown} seconds"], cleanOldLine: true);

            if (_countdown <= 0)
            {
                Stop();
                InputOutput.PrintMessages(messages: ["Time is up! Please press Enter."]);
            }
        }

        public bool IsRunning()
        {
            return _countdownTimer.Enabled;
        }
    }
}
