using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace towers_of_hanoi
{ 
    class Game
    {
        static int StepsCount = 100;
        static int MinutesCount = 5;

        public Game(MainWindow theWindow)
        {
            myWindow = theWindow;

            myTowers.Add(new Tower(myWindow.FirstTower.Children));
            myTowers.Add(new Tower(myWindow.SecondTower.Children));
            myTowers.Add(new Tower(myWindow.ThirdTower.Children));

            myPopButtons.Add(myWindow.Pop_1);
            myPopButtons.Add(myWindow.Pop_2);
            myPopButtons.Add(myWindow.Pop_3);

            myPushButtons.Add(myWindow.Push_1);
            myPushButtons.Add(myWindow.Push_2);
            myPushButtons.Add(myWindow.Push_3);

            myTowers[0].Append();

            foreach (var aButton in myPopButtons) {
                aButton.Click += OnButtonPopClicked;
                aButton.IsEnabled = false;
            }

            foreach (var aButton in myPushButtons)
            {
                aButton.Click += OnButtonPushClicked;
                aButton.IsEnabled = false;
            }

            myWindow.Start.Click += OnStart;
            myWindow.Exit.Click += OnExit;

            myClock.SecondTickEvent += OnTimeIsUpdated;
            myClock.TimeIsOver += OnTimeIsOver;

            Reset();
        }

        void Push(int index)
        {
            if (myCurrentRing != null)
            {
                bool aResult = myTowers[index].Push(myCurrentRing);

                if (!aResult) {
                    return;
                }

                foreach (var aTower in myTowers) {
                    aTower.Unhighlight();
                }
                myCurrentRing = null;
                OnStep();
            }
        }

        void Pop(int index)
        {
            if (myCurrentRing == null)
            {
                myCurrentRing = myTowers[index].Pop();
                if (myCurrentRing != null) {
                    myTowers[index].Highlight();
                }
            }
        }

        void OnButtonPopClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;
                var aNames = button.Name.Split('_');
                var anIndex = Int32.Parse(aNames[1]);
                Pop(anIndex - 1);
            }
        }

        void OnButtonPushClicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
            {
                Button button = sender as Button;
                var aNames = button.Name.Split('_');
                var anIndex = Int32.Parse(aNames[1]);
                Push(anIndex - 1);
            }
        }

        void OnStep()
        {
            myStepsCount--;
            myWindow.steps.Content = myStepsCount.ToString();

            if (myTowers[0].Count == 7) {
                OnGameWin();
                return;
            }

            if (myStepsCount <= 0) {
                OnStepsIsOver();
            }
        }

        void OnGameWin()
        {
            Pause();
            CustomMessageBox.ShowMessage("Готово!\n" +
                                         $"Ходы: {StepsCount - myStepsCount}\n" +
                                         $"Время: {myClock.ElapsedTime.ToString(@"mm\:ss")}\n",
                                         CustomMessageBox.ButtonsType.OkButton,
                                         DoSomethingIsOver);
        }

        void OnStepsIsOver()
        {
            Pause();
            CustomMessageBox.ShowMessage("Вы проиграли!\nХоды закончились",
                                         CustomMessageBox.ButtonsType.OkButton,
                                         DoSomethingIsOver);
        }

        void OnStart(object sender, RoutedEventArgs e)
        {
            Pause();
            CustomMessageBox.ShowMessage("Начать новую игру?",
                                         CustomMessageBox.ButtonsType.YesButton | CustomMessageBox.ButtonsType.NoButton,
                                         DoStart);
        }

        void DoStart(CustomMessageBox.ButtonsType theButton)
        {
            if (theButton == CustomMessageBox.ButtonsType.YesButton)
            {
                Reset();
                SetControlsEnabled(true);
                myClock.Start(myGameTime, myUpdateTime);
                myGameStarted = true;
                myCurrentRing = null;

                foreach (var aTower in myTowers) {
                    aTower.Reset();
                }

                myTowers[0].Append();
            }
            else {
                Continue();
            }
        }

        void OnExit(object sender, RoutedEventArgs e)
        {
            Pause();
            CustomMessageBox.ShowMessage("Выйти?", 
                                         CustomMessageBox.ButtonsType.YesButton | CustomMessageBox.ButtonsType.NoButton, 
                                         DoExit);
        }

        void DoExit(CustomMessageBox.ButtonsType theButton)
        {
            if (theButton == CustomMessageBox.ButtonsType.YesButton)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else {
                Continue();
            }
        }

        void OnTimeIsUpdated(TimeSpan theTime)
        {
            myWindow.time.Content = theTime.ToString(@"mm\:ss");
        }

        void OnTimeIsOver()
        {
            Pause();
            CustomMessageBox.ShowMessage("Вы проиграли!\nЗакончилось время",
                                         CustomMessageBox.ButtonsType.OkButton,
                                         DoSomethingIsOver);
        }

        void DoSomethingIsOver(CustomMessageBox.ButtonsType theButton)
        {
            myGameStarted = false;
            //Continue();
        }

        void Reset()
        {
            myStepsCount = StepsCount;
            myGameTime = new TimeSpan(0, MinutesCount, 0);
            myWindow.time.Content = myGameTime.ToString(@"mm\:ss");
            myWindow.steps.Content = myStepsCount.ToString();
        }

        // Отключает/Включает кнопки "Взять" и "Положить"
        void SetControlsEnabled (bool theEnabled)
        {
            foreach (var aButton in myPopButtons)
            {
                aButton.IsEnabled = theEnabled;
            }

            foreach (var aButton in myPushButtons)
            {
                aButton.IsEnabled = theEnabled;
            }
        }

        void Continue()
        { 
            myClock.Continue();
            if (myGameStarted) {
                SetControlsEnabled(true);
            }
        }

        void Pause()
        {
            SetControlsEnabled(false);
            myClock.Pause();
        }


        bool myGameStarted = false;

        MainWindow myWindow;
        List<Button> myPopButtons = new List<Button>();
        List<Button> myPushButtons = new List<Button>();

        Ring myCurrentRing;
        List<Tower> myTowers = new List<Tower>();
        
        int myStepsCount;
        TimeSpan myGameTime;
        TimeSpan myUpdateTime = new TimeSpan(0, 0, 1);
        ClockTimer myClock = new ClockTimer();
    }
}
