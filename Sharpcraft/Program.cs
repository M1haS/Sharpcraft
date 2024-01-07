namespace Sharpcraft_Mineecraft_Clone_OpenTK
{
    class Program
    {
        static void Main(string[] args)
        {
            using Game game = new Game(500, 500);
            game.Run();
        }
    }
}