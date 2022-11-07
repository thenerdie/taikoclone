using osu.Framework.Allocation;
using osu.Framework.Platform;
using NUnit.Framework;

namespace taikoclone.Game.Tests.Visual
{
    [TestFixture]
    public class TestScenetaikocloneGame : taikocloneTestScene
    {
        // Add visual tests to ensure correct behaviour of your game: https://github.com/ppy/osu-framework/wiki/Development-and-Testing
        // You can make changes to classes associated with the tests and they will recompile and update immediately.

        private taikocloneGame game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new taikocloneGame();
            game.SetHost(host);

            AddGame(game);
        }
    }
}
