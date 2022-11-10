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
using NUnit.Framework;
using System.Collections.Generic;

namespace taikoclone.Game
{
    public class MainScreen : Screen
    {
        private TaikoCloneFile taikoCloneFile;

        [BackgroundDependencyLoader]
        private void load()
        {
            SpriteText spriteText;

            StreamReader sr = new StreamReader(@"test.tc");
            taikoCloneFile = new TaikoCloneFile(sr.ReadToEnd());

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
                new SpriteText
                {
                    Y = 70,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40),
                    Colour = Color4.White
                },
                new Playfield
                {
                    Anchor = Anchor.Centre,
                    File = taikoCloneFile,
                }
            };

            spriteText.Text = $"The song is {taikoCloneFile.Title} by {taikoCloneFile.Artist} made by {taikoCloneFile.Creator}!!";
        }
    }
}
