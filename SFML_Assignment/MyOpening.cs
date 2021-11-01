using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Text;
using TGUI;

namespace SFML_Assignment
{
    class MyOpening
    {
        RenderWindow window;
        Font font;
        Text text;
        Gui gui;
        Button okButton;
        StringBuilder openingText;

        //Constructor for MyOpening Class
        public MyOpening()
        {
            VideoMode mode = new VideoMode(815, 600);
            window = new RenderWindow(mode, "DEQUE Opening", Styles.Titlebar);

            gui = new Gui(window);

            openingText = new StringBuilder();
            openingText.Append("ISCG6426 Data Structures and Algorithms Final Assignment\r\n\r\n");
            openingText.Append("Deque Data structure\r\n\r\n\r\n");
            openingText.Append("Name : Sabrian Jufenlindo (1548517)\r\n\r\n");
            openingText.Append("This deque data structure has a limit of 42 element.\r\nThe limit is placed so that user is able to see clearly the color changing\r\nfrom" +
                " dark (top) to lighter color as it goes toward the bottom of the deque.");


            font = new Font(@"C:\\Windows\Fonts\Arial.ttf");
            text = new Text(openingText.ToString(), font, 25);

            text.Position = new Vector2f(10f, 50.0f);

            okButton = new Button("OK");
            okButton.SetSize(new Layout2d(100f, 50f));
            okButton.Position = new Vector2f(window.Size.X * 0.45f, 500.0f);

            gui.Add(okButton);

            window.Closed += this.Window_close;
            window.MouseButtonPressed += this.Mouse_press;
        }

        //Event handler to close the window
        public void Window_close(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Close();
        }

        //Event handler to handle mouse click, when "OK" widget is clicked, close window
        public void Mouse_press(object sender, MouseButtonEventArgs e)
        {
            if (okButton.MouseOnWidget(new Vector2f(e.X, e.Y)))
            {
                switch (e.Button)
                {
                    case Mouse.Button.Left:
                        window.Close();
                        break;
                    default:
                        break;
                }
            }

        }

        //Method to keep the window open and running
        public void Run()
        {
            while (window.IsOpen)
            {
                this.Update();
                this.Display();
            }
        }

        public void Update()
        {
            window.DispatchEvents();
        }

        //Redraw the window
        public void Display()
        {
            window.Clear();
            window.Draw(text);
            gui.Draw();
            window.Display();
        }
    }
}
