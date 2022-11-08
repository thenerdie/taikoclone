using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using taikoclone.Game.Components;
using osu.Framework.Graphics.Containers;
using System;

namespace taikoclone.Game
{
    public class MainScreen : Screen
    {
        private Playfield playfield;
        private double time = 0;
        private int index = 0;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                },
                new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                playfield = new Playfield
                {
                    Anchor = Anchor.Centre
                }
            };
        }

        protected override void Update()
        {
            base.Update();

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
