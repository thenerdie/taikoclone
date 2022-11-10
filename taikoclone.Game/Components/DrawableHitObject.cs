using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace taikoclone.Game.Components
{
    public class DrawableHitObject : CompositeDrawable
    {
        public int NoteType;
        public int NoteSubtype;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            Texture t = null;

            switch (NoteSubtype)
            {
                case 1:
                    t = textures.Get("smalldon");
                    break;
                case 2:
                    t = textures.Get("smallkat");
                    break;
                case 3:
                    t = textures.Get("bigdon");
                    break;
                case 4:
                    t = textures.Get("bigkat");
                    break;
            }

            InternalChild = new Container
            {
                X = 0, //800
                Y = 125,
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = t
                    },
                }
            };
        }
    }
}
