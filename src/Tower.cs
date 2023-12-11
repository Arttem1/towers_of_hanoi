using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace towers_of_hanoi
{
    class Tower
    {
        public Tower(UIElementCollection theColection)
        {
            myGrid = new Grid();
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Top;
            myGrid.Height = 496;

            // Rectangle properties
            Rectangle aRectangle = new Rectangle();

            // Position
            aRectangle.HorizontalAlignment = HorizontalAlignment.Center;
            aRectangle.VerticalAlignment = VerticalAlignment.Bottom;
            aRectangle.Margin = new Thickness(0, 0, 0, 20);

            // Size
            aRectangle.Height = 380;
            aRectangle.Width = 30;

            // Border
            aRectangle.Stroke = System.Windows.Media.Brushes.Black;
            aRectangle.StrokeThickness = 2;

            // Circle properties
            myEllipse = new Ellipse();

            // Position
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Top;
            myEllipse.Margin = new Thickness(0, 50, 0, 0);

            // Size
            myEllipse.Height = 40;
            myEllipse.Width = 40;

            // Border
            myEllipse.Stroke = System.Windows.Media.Brushes.Black;
            myEllipse.StrokeThickness = 2;


            myGrid.Children.Add(aRectangle);
            myGrid.Children.Add(myEllipse);
            theColection.Add(myGrid);
        }

        public bool Push(Ring theRing)
        {
            if (theRing == null || myStack.Count >= 7)
            {
                return false;
            }
            if (myStack.Count > 0 && myStack.Peek().Size < theRing.Size)
            {
                return false;
            }
            theRing.Position = myStack.Count;
            myStack.Push(theRing);
            theRing.Visibility = Visibility.Visible;

            myGrid.Children.Add(theRing);
            return true;
        }

        public Ring Pop()
        {
            if (myStack.Count > 0)
            {
                var aRing = myStack.Peek();
                aRing.Visibility = Visibility.Hidden;
                myGrid.Children.Remove(aRing);
                return myStack.Pop();
            }
            return null;
        }

        public int Count
        {
            get { return myStack.Count; }
        }

        // Удаляет все кольца из башни
        public void Reset()
        {
            Unhighlight();

            while (myStack.Count > 0) {
                Pop();
            }
        }

        // Заполняет башню семью кольцами
        public void Append()
        {
            for (int i = 7; i > 0; i--)
            {
                Ring aRing = new Ring();
                aRing.Size = i;
                Push(aRing);
            }
        }

        // Подсвечивает кружок
        public void Highlight()
        {
            myEllipse.Fill = new SolidColorBrush(Color.FromRgb(252, 174, 200));
        }

        // Убирает подствеку кружка
        public void Unhighlight()
        {
            myEllipse.Fill = System.Windows.Media.Brushes.Transparent;
        }

        private Stack<Ring> myStack = new Stack<Ring>();
        private Grid myGrid;
        private Ellipse myEllipse;
    }
}
