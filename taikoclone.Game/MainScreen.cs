using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osu.Framework.Logging;
using osuTK.Graphics;
using taikoclone.Game.Components;
using System;
using System.IO;
using NuGet.Protocol;

namespace taikoclone.Game
{
    public class MainScreen : Screen
    {
        private Playfield playfield;
        private SpriteText debugTime;
        private double currentTime;
        private int hitObjectIndex = 0;
        private double initialTime;

        private double scrollSpeedMilliseconds = 1000;
        private double missMilliseconds = 135;

        TaikoCloneFile taikoCloneFile;

        private double getCurrentTime()
        {
            return DateTime.Now.Ticks / 10000;
        }

        public MainScreen()
        {
            initialTime = getCurrentTime();
        }

        private double lerp(double min, double max, double alpha)
        {
            return min + ((max - min) * alpha);
        }

        private double inverseLerp(double min, double max, double num)
        {
            return (num - min) / (max - min);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            SpriteText spriteText;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.DarkGray,
                    RelativeSizeAxes = Axes.Both,
                },
                spriteText = new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40),
                    Colour = Color4.White
                },
                debugTime = new SpriteText
                {
                    Y = 70,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40),
                    Colour = Color4.White
                },
                playfield = new Playfield
                {
                    Anchor = Anchor.Centre
                }
            };

            StreamReader sr = new StreamReader(@"test.tc");
            taikoCloneFile = new TaikoCloneFile(sr.ReadToEnd());

            spriteText.Text = $"The song is {taikoCloneFile.Title} by {taikoCloneFile.Artist} made by {taikoCloneFile.Creator}!!";
        }

        protected override void Update()
        {
            // logic based timer

            base.Update();

            currentTime = getCurrentTime() - initialTime;

            debugTime.Text = currentTime.ToString();

            for (int i = hitObjectIndex; i < taikoCloneFile.HitObjects.Count; i++)
            {
                var hitObject = taikoCloneFile.HitObjects[i];

                if (hitObject.Time - scrollSpeedMilliseconds <= currentTime)
                {
                    playfield.AddHitObject(hitObject);

                    hitObjectIndex++;
                }
                else
                {
                    break;
                }
            }

            foreach (var gameplayObject in playfield.HitObjects)
            {
                if (currentTime - gameplayObject.HitObject.Time > missMilliseconds)
                {
                    playfield.RemoveHitObject(gameplayObject);
                }

                var alpha = 1 - inverseLerp(gameplayObject.HitObject.Time - scrollSpeedMilliseconds, gameplayObject.HitObject.Time, currentTime);

                gameplayObject.DrawableHitObject.X = (float)lerp(800, -405, 1 - alpha);
            }

            //time++;

            //if (time > 50)
            //{
            //    index++;

            //    playfield.Add(new HitObject()
            //    {
            //        X = 10 * index
            //    });

            //    time = 0;
            //}
        }
    }
}
