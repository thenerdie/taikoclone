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
    public class HitObject : CompositeDrawable
    {
        private Container hitObject;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = hitObject = new Container
            {
                AutoSizeAxes = Axes.Both,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Texture = textures.Get("smalldon")
                    },
                }
            };
        }

        public void SetPosition(float alpha)
        {
            hitObject.X = 500 - (alpha * 500);
        }
    }
}
