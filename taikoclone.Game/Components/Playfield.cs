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
using osu.Framework.Input.Events;
using osuTK.Input;
using osu.Framework.Extensions.ObjectExtensions;

namespace taikoclone.Game.Components
{
    public class GameplayHitObject
    {
        public HitObject HitObject { get; set; }
        public DrawableHitObject DrawableHitObject { get; set; }
    }

    public class Playfield : CompositeDrawable
    {
        public TaikoCloneFile File;

        private double currentTime;
        private int hitObjectIndex = 0;
        private double initialTime;

        private double scrollSpeedMilliseconds = 1000;
        private double missMilliseconds = 135;
        //private double okayMilliseconds = 80;

        private double getCurrentTime()
        {
            return DateTime.Now.Ticks / 10000;
        }

        private double lerp(double min, double max, double alpha)
        {
            return min + ((max - min) * alpha);
        }

        private double inverseLerp(double min, double max, double num)
        {
            return (num - min) / (max - min);
        }

        private Container hitObjectContainer;

        public List<GameplayHitObject> HitObjects;

        private Key[] don = { Key.Slash, Key.N };
        private Key[] kat = { Key.B, Key.ShiftRight };

        public Playfield()
        {
            HitObjects = new List<GameplayHitObject>();
            initialTime = getCurrentTime();
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

        public void AddHitObject(HitObject hitObject)
        {
            var gameplayObject = new GameplayHitObject()
            {
                HitObject = hitObject,
                DrawableHitObject = new DrawableHitObject()
                {
                    NoteType = hitObject.Type,
                    NoteSubtype = hitObject.Subtype
                }
            };

            hitObjectContainer.Add(gameplayObject.DrawableHitObject);

            HitObjects.Add(gameplayObject);
        }

        public void RemoveHitObject(GameplayHitObject hitObject)
        {
            hitObjectContainer.Remove(hitObject.DrawableHitObject, true);

            HitObjects.Remove(hitObject);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (HitObjects.Count == 0)
                return false;

            var hitObject = HitObjects[0];

            if (hitObject.HitObject.Time - currentTime > missMilliseconds)
                return false;

            var check = hitObject.HitObject.Type == 0 && (hitObject.HitObject.Subtype == 1 || hitObject.HitObject.Subtype == 3) ? don : kat;

            if (check.Contains(e.Key))
                RemoveHitObject(hitObject);

            return false;
        }

        protected override void Update()
        {
            // logic based timer

            base.Update();

            currentTime = getCurrentTime() - initialTime;

            for (int i = hitObjectIndex; i < File.HitObjects.Count; i++)
            {
                var hitObject = File.HitObjects[i];

                if (hitObject.Time - scrollSpeedMilliseconds <= currentTime)
                {
                    AddHitObject(hitObject);

                    hitObjectIndex++;
                }
                else
                {
                    break;
                }
            }

            var toRemove = new List<GameplayHitObject>();

            foreach (var gameplayObject in HitObjects)
            {
                if (currentTime - gameplayObject.HitObject.Time > missMilliseconds)
                {
                    toRemove.Add(gameplayObject);
                }

                var alpha = 1 - inverseLerp(gameplayObject.HitObject.Time - scrollSpeedMilliseconds, gameplayObject.HitObject.Time, currentTime);

                gameplayObject.DrawableHitObject.X = (float)lerp(800, -405, 1 - alpha);
            }

            foreach (var remove in toRemove)
            {
                RemoveHitObject(remove);
            }
        }
    }
}
