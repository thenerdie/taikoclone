using osu.Framework.Platform;
using osu.Framework;
using taikoclone.Game;

namespace taikoclone.Desktop
{
    public static class Program
    {
        public static void Main()
        {
            using (GameHost host = Host.GetSuitableDesktopHost(@"taikoclone"))
            using (osu.Framework.Game game = new taikocloneGame())
                host.Run(game);
        }
    }
}
