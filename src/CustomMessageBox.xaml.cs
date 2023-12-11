using System;
using System.Windows;
using System.Windows.Controls;

namespace towers_of_hanoi
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public enum ButtonsType
        {
            YesButton = 1,
            NoButton = 2,
            OkButton = 4
        };

        public delegate void ButtonClickedDelegate(ButtonsType theButton); // вид требуемого обработчика

        public CustomMessageBox (String theMessage, ButtonsType theButtonType, ButtonClickedDelegate theDelegate)
        {
            InitializeComponent();

            myCallback = theDelegate;

            Message.Text = theMessage;

            if ((theButtonType & ButtonsType.YesButton) == ButtonsType.YesButton)
            {
                PrepareButton (Yes_Button);
            }

            if ((theButtonType & ButtonsType.NoButton) == ButtonsType.NoButton)
            {
                PrepareButton(No_Button);
            }

            if ((theButtonType & ButtonsType.OkButton) == ButtonsType.OkButton)
            {
                PrepareButton(Ok_Button);
            }
        }

        public void PrepareButton(Button theButton)
        {
            theButton.Visibility = Visibility.Visible;
            theButton.Click += onButtonClicked;
        }

        public void onButtonClicked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;

            ButtonsType aType = ButtonsType.NoButton;

            if (button.Name == "Yes_Button") {
                aType = ButtonsType.YesButton;
            }
            else if (button.Name == "No_Button") {
                aType = ButtonsType.NoButton;
            }
            else if (button.Name == "Ok_Button")
            {
                aType = ButtonsType.OkButton;
            }

            Hide();
            myCallback(aType);
        }

        // Статический метод принимающий сообщение, тип кнопок и обработчик
        static public void ShowMessage(String theMessage, ButtonsType theButtonType, ButtonClickedDelegate theDelegate)
        {
            CustomMessageBox messageBox = new CustomMessageBox (theMessage, theButtonType, theDelegate);
            messageBox.ShowDialog();
        }

        private ButtonClickedDelegate myCallback; // Храним обработчик, чтобы вызвать его когда пользователь нажмет одну из кнопок
    }
}
