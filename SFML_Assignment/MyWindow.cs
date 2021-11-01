using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Text;
using TGUI;
using System.Threading;
using System.Diagnostics;

namespace SFML_Assignment
{
    //Class that is used to change color of the ball using thread
    class ChangeColor
    {
        public static void changeOutlineColor(Ball ball)
        {
            Thread.Sleep(100);
            ball.removeHightlight();
        }
    }

    //Constructor of MyWindow class
    class MyWindow
    {
        RenderWindow window;
        Clock clock;

        Time delta;

        Deque<Ball> balls;

        float circleSize;

        Random random;
        float speed;

        Font font;
        Text text;

        Text dequeLabel;
        Text operationLabel;
        Text currentOperation;

        StringBuilder printDeque;
        StringBuilder operation;

        Gui gui;

        TextBox textBox;

        Button clearButton;
        Button sortButton;
        Button searchButton;


        RectangleShape ballBorder;

        bool onWidget = false;
        bool bMouseDown = false;
        Ball draggedBall;

        int[] rgbArray;

        TextBox intToSearch;

        //MyWindow constructor
        public MyWindow()
        {
            VideoMode mode = new VideoMode(1800, 800);
            window = new RenderWindow(mode, "DEQUE Data Structures", Styles.Titlebar);

            window.Closed += this.Window_close;
            window.KeyPressed += this.Key_press;
            window.MouseButtonPressed += this.Mouse_press;
            window.MouseMoved += this.Mouse_moved;
            window.MouseButtonReleased += this.Mouse_release;
            

            gui = new Gui(window);

            clock = new Clock();
            delta = Time.Zero;

            balls = new Deque<Ball>();
            circleSize = 20f;

            random = new Random();
            speed = 200f;

            font = new Font(@"C:\\Windows\Fonts\Arial.ttf");
            text = new Text("Left-click to push front. Right-click to push rear. Left-Arrow to pop front. Right-Arrow to pop rear." +
                "\r\nGreen Ball is deque top. Blue Ball is deque Rear. Hold scroll click to select a ball and drag it around.\r\nEscape to quit.", font, 25);
            text.Position = new Vector2f(0, window.Size.Y * 0.85f);

            dequeLabel = new Text("Deque :", font, 25);
            dequeLabel.Position = new Vector2f(window.Size.X * 0.7f, 10.0f);

            printDeque = new StringBuilder("");
            operation = new StringBuilder("");

            textBox = new TextBox();
            textBox.Position = new Vector2f(window.Size.X * 0.7f, 50.0f);
            textBox.Text = printDeque.ToString();
            textBox.VerticalScrollbarPolicy = Scrollbar.Policy.Automatic;
            textBox.ReadOnly = true;
            textBox.Size = new Vector2f(300.0f, 400.0f);

            operationLabel = new Text("Operation Performed : ", font, 25);
            operationLabel.Position = new Vector2f(window.Size.X * 0.7f, 670.0f);

            currentOperation = new Text("", font, 25);
            currentOperation.Position = new Vector2f(window.Size.X * 0.7f, 720.0f);
            gui.Add(textBox);

            rgbArray = new int[42];

            int initialRgb = 50;
            for (int i = 0; i < rgbArray.Length; i++)
            {
                rgbArray[i] = initialRgb;
                initialRgb += 5;
            }

            ballBorder = new RectangleShape(new Vector2f(window.Size.X * 0.65f, window.Size.Y * 0.8f));
            ballBorder.OutlineThickness = 1.0f;
            ballBorder.OutlineColor = Color.White;
            ballBorder.FillColor = Color.Transparent;

            clearButton = new Button("Clear");
            sortButton = new Button("Sort");
            searchButton = new Button("Search");

            clearButton.SetSize(new Layout2d(125f, 50f));
            clearButton.Position = new Vector2f(window.Size.X * 0.7f, 470.0f);
            clearButton.TextSize = 25;

            sortButton.SetSize(new Layout2d(125f, 50f));
            sortButton.Position = new Vector2f(window.Size.X * 0.8f, 470.0f);
            sortButton.TextSize = 25;

            searchButton.SetSize(new Layout2d(125f, 50f));
            searchButton.Position = new Vector2f(window.Size.X * 0.8f, 550.0f);
            searchButton.TextSize = 25;

            intToSearch = new TextBox();
            intToSearch.Position = new Vector2f(window.Size.X * 0.7f, 550.0f);
            intToSearch.Size = new Vector2f(150.0f, 50.0f);
            intToSearch.TextSize = 30;

            gui.Add(clearButton);
            gui.Add(sortButton);
            gui.Add(searchButton);
            gui.Add(intToSearch);
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }

        ////////////// Game Window Critical Methods ///////////////
        
        //Method to keep window running and updated
        public void Run()
        {
            while (window.IsOpen)
            {
                this.Update();
                this.Display();
            }
        }

        //Method to update the window
        public void Update()
        {
            window.DispatchEvents();

            delta = clock.Restart();

            this.UpdateCircles();

            this.UpdateOperation();

            if (!onWidget)
            {
                this.UpdateDequeList();
            }
        }

        //Method to update currentOperation text that is displayed on the window
        public void UpdateOperation()
        {
            currentOperation = new Text(operation.ToString(), font, 25);
            currentOperation.Position = new Vector2f(window.Size.X * 0.7f, 720.0f);
        }

        //Method to update the deque list
        public void UpdateDequeList()
        {
            printDeque.Clear();

            for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
            {
                printDeque.Append(front.data.getData() + "\r\n");
            }
            textBox.Text = printDeque.ToString();
        }

        public void Display()
        {
            /// clear the window
            window.Clear();

            /// draw shapes to the window
            this.DrawCircles();

            //Draw the GUI
            gui.Draw();

            //Draw other widget on the window
            window.Draw(dequeLabel);
            window.Draw(operationLabel);
            window.Draw(currentOperation);
            window.Draw(ballBorder);

            /// draw the help text
            window.Draw(text);

            /// display the window to the screen
            window.Display();
        }


        ////////////// Game Window Event Methods ///////////////

        //Method to remove highlight when ball is no longer dragged
        public void Mouse_release(object sender, MouseButtonEventArgs e)
        {
            bMouseDown = false;
            if(draggedBall != null)
            {
                draggedBall.removeHightlight();
            }
            draggedBall = null;
        }

        //Method to capture keyboard event
        public void Key_press(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;

            if (!onWidget)
            {
                try
                {
                    Ball ball;
                    switch (e.Code)
                    {
                        case Keyboard.Key.Escape:
                            window.Close();
                            break;
                        case Keyboard.Key.Left:
                            ball = this.balls.popFront();
                            operation.Clear();
                            operation.Append("Pop front\r\nRemoved value : " + ball.getData());
                            break;
                        case Keyboard.Key.Right:
                            ball = this.balls.popRear();
                            operation.Clear();
                            operation.Append("Pop Rear\r\nRemoved value : " + ball.getData());
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    operation.Clear();
                    operation.Append(ex.Message);
                }
            }

        }

        //Method to move the ball while scroll-middle button is clicked
        public void Mouse_moved(object sender, MouseMoveEventArgs e)
        {
            Window window = (Window)sender;

            if(draggedBall != null && bMouseDown)
            {
                draggedBall.Position = new Vector2f(e.X, e.Y);
            }
        }

        //Method to capture mouse press event
        public void Mouse_press(object sender, MouseButtonEventArgs e)
        {
            bMouseDown = true;

            if (Mouse.IsButtonPressed(Mouse.Button.Middle))
            {
                for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
                {
                    float dx = e.X - front.data.Position.X;
                    float dy = e.Y - front.data.Position.Y;
                    if (dx < 0)
                    {
                        dx = dx - (dx * 2);
                    }

                    if (dy < 0)
                    {
                        dy = dy - (dy * 2);
                    }

                    float d1 = dx * dx;
                    float d2 = dy * dy;
                    float d3 = d1 + d2;
                    double d = Math.Sqrt(Convert.ToDouble(d3));

                    if (d <= front.data.getRadius())
                    {
                        draggedBall = front.data;
                        front.data.highLight();
                    }
                    
                }
            }else if (!textBox.MouseOnWidget(new Vector2f(e.X, e.Y)) && !intToSearch.MouseOnWidget(new Vector2f(e.X,e.Y)))
            {
                try
                {
                    if(clearButton.MouseOnWidget(new Vector2f(e.X, e.Y)))
                    {
                        this.balls.clear();
                        operation.Clear();
                        operation.Append("Deque is cleared");
                    }
                    else
                    {
                        if(sortButton.MouseOnWidget(new Vector2f(e.X, e.Y)))
                        {
                            sortDeque();
                            operation.Clear();
                            operation.Append("Deque is sorted");
                        }
                        else
                        {
                            if(searchButton.MouseOnWidget(new Vector2f(e.X, e.Y)))
                            {
                                
                                Ball temp = searchHighLight(Int32.Parse(intToSearch.Text));

                                temp.highLight();

                                Thread changeBallColor = new Thread(() => ChangeColor.changeOutlineColor(temp));
                                changeBallColor.Start();

                            }
                            else
                            {
                                if (balls.size == 42)
                                {
                                    throw new Exception("Limit capacity reached");
                                }

                                switch (e.Button)
                                {
                                    case Mouse.Button.Left:
                                        this.AddBallFront(e.X, e.Y);
                                        break;
                                    case Mouse.Button.Right:
                                        this.AddBallRear(e.X, e.Y);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    operation.Clear();
                    operation.Append(ex.Message);
                }

                onWidget = false;
            }
            else
            {
                onWidget = true;
            }

        }

        public void Window_close(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
        }

        //Method to add the balls to the front of the deque
        public void AddBallFront(float x, float y)
        {
            for (int i = 0; i < 1; i++)
            {
                float velocityX = (float)(random.NextDouble() - 0.5) * speed;
                float velocityY = (float)(random.NextDouble() - 0.5) * speed;

                int data = random.Next(1, 100);

                Ball ball = new Ball(Color.Red, Color.White, data, circleSize);
                ball.setPosition(x, y);
                ball.setVelocity(velocityX, velocityY);

                balls.pushFront(ball);

                operation.Clear();
                operation.Append("Push front\r\nAdded value : " + ball.getData());
            }
        }

        //Method to add the balls to the rear of the deque
        public void AddBallRear(float x, float y)
        {
            for (int i = 0; i < 1; i++)
            {
                float velocityX = (float)(random.NextDouble() - 0.5) * speed;
                float velocityY = (float)(random.NextDouble() - 0.5) * speed;

                int data = random.Next(1, 100);

                Ball ball = new Ball(Color.Red, Color.White, data, circleSize);
                ball.setPosition(x, y);
                ball.setVelocity(velocityX, velocityY);

                balls.pushRear(ball);

                operation.Clear();
                operation.Append("Push Rear\r\nAdded value : " + ball.getData());
            }
        }

        //Method to draw all the circles
        public void DrawCircles()
        {
            if (this.balls.size == 0) return;

            // draw all the ball links first so they don't draw over the balls
            for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
            {
                front.data.DrawLink(window, RenderStates.Default);
            }

            for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
            {
                front.data.Draw(window, RenderStates.Default);
                Text ballValueText = new Text(front.data.getData() + "", font, 25);
                ballValueText.Position = new Vector2f(front.data.Position.X - 10f, front.data.Position.Y + 25f);
                this.window.Draw(ballValueText);
            }
        }

        //Method to uodateCircle
        //Deque front will have a color of green
        //Deque rear will have a clor of blue
        //Ball inside the ball will have its color changing from dark to high depending of its position in deque
        public void UpdateCircles()
        {
            if (this.balls.size == 0) return;

            int count = 0;
            bool isFront = true;

            for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
            {
                Vector2f linktarget = front.nextLink != null ? front.nextLink.data.Position : front.data.Position;
                front.data.Update(this.delta.AsSeconds(), linktarget, 0, 0, window.Size.X * 0.65f, window.Size.Y * 0.8f);
                
                if (isFront)
                {
                    //Green color
                    front.data.setColor(new Color(85,107,47));
                    isFront = false;
                }
                else
                {
                    if (front.nextLink == null)
                    {
                        //Blue color
                        front.data.setColor(new Color(25,25,112));
                    }
                    else
                    {
                        front.data.setColor(ballColor(count++));
                    }
                }
            }
        }

        //Returning color for specified ball based on its position
        public Color ballColor(int currentBall)
        {
            return new Color((byte)rgbArray[currentBall], 0, 0);
        }

        //Method to sort the deque using bubble sort technique
        public void sortDeque()
        {
            if (balls.isEmpty())
            {
                Console.WriteLine("Deque is empty");
                throw new Exception("Deque is empty");
            }
            else
            {  for(int i=0; i < this.balls.size; i++)
                {
                    for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
                    {
                        if (front.nextLink != null)
                        {
                            if (front.data.getData() > front.nextLink.data.getData())
                            {
                                Node<Ball> temp = front;
                                Node<Ball> temp1 = front.nextLink;

                                if (front.prevLink != null)
                                {
                                    front.prevLink.nextLink = temp1;
                                    temp1.prevLink = front.prevLink;
                                }
                                else
                                {
                                    this.balls.front = temp1;
                                    temp1.prevLink = null;
                                }

                                if (temp1.nextLink != null)
                                {
                                    temp1.nextLink.prevLink = temp;
                                    temp.nextLink = temp1.nextLink;
                                }
                                else
                                {
                                    this.balls.rear = temp;
                                    temp.nextLink = null;
                                }

                                temp1.nextLink = temp;
                                temp.prevLink = temp1;
                            }
                        }
                    }
                }    
            }
        }

        //Return ball object that have the specified data
        public Ball searchHighLight(int data)
        {
            for (Node<Ball> front = this.balls.front; front != null; front = front.nextLink)
            {
                if(front.data.getData() == data)
                {
                    return front.data;
                }
            }
            throw new Exception("Ball not found");
        }
    }
}
