using osu.Framework.Testing;

namespace taikoclone.Game.Tests.Visual
{
    public class taikocloneTestScene : TestScene
    {
        protected override ITestSceneTestRunner CreateRunner() => new taikocloneTestSceneTestRunner();

        private class taikocloneTestSceneTestRunner : taikocloneGameBase, ITestSceneTestRunner
        {
            private TestSceneTestRunner.TestRunner runner;

            protected override void LoadAsyncComplete()
            {
                base.LoadAsyncComplete();
                Add(runner = new TestSceneTestRunner.TestRunner());
            }

            public void RunTestBlocking(TestScene test) => runner.RunTestBlocking(test);
        }
    }
}
