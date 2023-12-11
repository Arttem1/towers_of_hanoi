using System;
using System.Windows.Threading;

namespace towers_of_hanoi
{
    class ClockTimer
    {
        public delegate void SecondTickDelegate(TimeSpan theInterval);
        public delegate void TimeIsOverDelegate();

        public event SecondTickDelegate SecondTickEvent;
        public event TimeIsOverDelegate TimeIsOver;

        public ClockTimer()
        {
        }

        public void Start(TimeSpan theTimeInterval, TimeSpan theTickInterval)
        {
            Stop();

            myStarted = true;
            myTimer.Interval = theTickInterval; // частота обновления
            myTimeInterval = theTimeInterval; // длительность игры
            myStartTime = DateTime.Now; // старт 
            myTimer.Tick += onSecondTick; // добавляем собственный обработчик
            myTimer.Start();   
        }

        public void Stop()
        {
            myTimer.Stop();
            myStarted = false;
            myPaused = false;
            myTimer.Tick -= onSecondTick;
            mySumOfPause = TimeSpan.Zero;
        }

        public void Pause()
        {
            if (!myPaused && myStarted)
            {
                myStartPause = DateTime.Now;
                myTimer.Tick -= onSecondTick;
                myPaused = true;
            }
        }

        public void Continue()
        {
            if (myPaused && myStarted)
            {
                mySumOfPause += DateTime.Now - myStartPause;
                myTimer.Tick += onSecondTick;
                myPaused = false;
            }
        }

        public TimeSpan ElapsedTime
        {
            get { return DateTime.Now - myStartTime - mySumOfPause; } // Количество пройденого времени с начала игры
        }

        private void onSecondTick(object sender, EventArgs e)
        {
            var aTimeLeft = myTimeInterval - ElapsedTime; // Оставшееся время
            if (aTimeLeft <= TimeSpan.Zero)
            { 
                myTimer.Tick -= onSecondTick;
                Stop();
                TimeIsOver();
            }
            else
            {
                SecondTickEvent (aTimeLeft); // Событие со значением оставшегося времени
            }
        }

        private bool myPaused = false;
        private bool myStarted = false;
        private DateTime myStartPause; // Начало времени паузы
        private TimeSpan mySumOfPause = TimeSpan.Zero; // общее количество времене в паузе (-ах)
        private DispatcherTimer myTimer = new DispatcherTimer(); // Таймер для сигнализировании об измененном времени
        private DateTime myStartTime;    //  Точка отсчета времени
        private TimeSpan myTimeInterval; //  Игровое время
    }

}
