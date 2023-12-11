
using System.Windows;
using System.Windows.Controls;

namespace towers_of_hanoi
{
    /// <summary>
    /// Interaction logic for Ring.xaml
    /// </summary>
    public partial class Ring : UserControl
    {
        public Ring()
        {
            InitializeComponent();
            Visibility = Visibility.Visible;
        }

        public int Size
        {
            get { return mySize; }
            set
            {
                mySize = value;
                UpdateProperties();
            }
        }

        public int Position
        {
            set
            {
                myPosition = value;
                UpdateProperties();
            }
        }

        private void UpdateProperties()
        {
            Width = 108 + (mySize-1) * 18;
            Height = 36;
            Margin = new Thickness(0, 435 + (7 - myPosition * 37), 0, 0);
            Visibility = Visibility.Visible;
            VerticalAlignment = VerticalAlignment.Top;
        }

        static Ring()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(Ring), new FrameworkPropertyMetadata(typeof(Ring)));
        }

        int mySize = 1;
        int myPosition = 0;
    }
}
