using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Text;
using TGUI;

namespace SFML_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press ESC key to close window");
            MyOpening opening = new MyOpening();
            opening.Run();
            MyWindow window = new MyWindow();
            window.Run();
            Console.WriteLine("All done");
        }
    }
}
