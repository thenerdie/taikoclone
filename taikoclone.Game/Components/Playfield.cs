using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics;
using System.Linq;
using osu.Framework.Input.Handlers;
using taikoclone.Game;

namespace taikoclone.Game.Components
{
    public class GameplayHitObject
    {
        public TaikoCloneFile.HitObject HitObject { get; set; }
        public DrawableHitObject DrawableHitObject { get; set; }
    }

    public class Playfield : CompositeDrawable
    {
        private Container hitObjectContainer;

        public List<GameplayHitObject> HitObjects;

        public Playfield()
        {
            HitObjects = new List<GameplayHitObject>();
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = new Container
            {
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        X = -500,
                        Anchor = Anchor.TopLeft,
                        Texture = textures.Get("conveyor")
                    },
                    new Sprite
                    {
                        X = -700,
                        Anchor = Anchor.TopLeft,
                        Texture = textures.Get("drumoverlay")
                    },
                    new Sprite
                    {
                        X = -405,
                        Y = 50,
                        Anchor = Anchor.Centre,
                        Texture = textures.Get("judgecircle")
                    },
                    hitObjectContainer = new Container
                    {

                    }
                }
            };
        }

        public void AddHitObject(TaikoCloneFile.HitObject hitObject)
        {
            var gameplayObject = new GameplayHitObject()
            {
                HitObject = hitObject,
                DrawableHitObject = new DrawableHitObject()
            };

            hitObjectContainer.Add(gameplayObject.DrawableHitObject);

            HitObjects.Add(gameplayObject);
        }

        public void RemoveHitObject(GameplayHitObject hitObject)
        {
            hitObjectContainer.Remove(hitObject.DrawableHitObject, true);
        }
    }
}
