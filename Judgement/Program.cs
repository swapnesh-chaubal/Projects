using System;

namespace Judgement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Judgement game = new Judgement())
            {
                game.Run();
            }
        }
    }
}

