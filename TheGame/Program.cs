namespace TheGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var game = new MainGame())
            {
                game.Run();
            }
        }
    }
}
