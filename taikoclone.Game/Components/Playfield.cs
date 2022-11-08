using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;

namespace taikoclone.Game.Components
{
    public class Playfield : CompositeDrawable
    {
        private Container playfield;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = playfield = new Container
            {
                Child = new Sprite
                {
                    Texture = textures.Get("conveyor")
                }
            };
        }

        public void AddHitObject(HitObject hitObject)
        {

        }
    }
}
