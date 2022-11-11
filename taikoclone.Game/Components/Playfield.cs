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
using osuTK;
using osu.Framework.Logging;

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

        private TextureStore textureStore;

        private double currentTime;
        private int hitObjectIndex = 0;
        private double initialTime;

        private double getHitWindow(JudgementType judgementType, double od)
        {
            switch (judgementType)
            {
                case JudgementType.Great:
                    return 65 - 3 * od;
                case JudgementType.Okay:
                    return 130 - 6 * od;
                case JudgementType.Miss:
                    return 195 - 9 * od;
            }

            return -1;
        }

        private double od = 7;
        private double scrollSpeedMilliseconds = 1000;
        private double missMilliseconds;
        private double okayMilliseconds;
        private double greatMilliseconds;

        private Sprite leftRim;
        private Sprite leftCenter;
        private Sprite rightRim;
        private Sprite rightCenter;

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

        private enum JudgementType
        {
            Great,
            Okay,
            Miss
        }

        private Container hitObjectContainer;
        private Container judgeContainer;

        public List<GameplayHitObject> HitObjects;

        private Key[] don = { Key.N, Key.Slash };
        private Key[] kat = { Key.B, Key.ShiftRight };

        private List<Key> currentlyPressed;

        public Playfield()
        {
            HitObjects = new List<GameplayHitObject>();
            initialTime = getCurrentTime();
            currentlyPressed = new List<Key>();

            greatMilliseconds = getHitWindow(JudgementType.Great, od);
            okayMilliseconds = getHitWindow(JudgementType.Okay, od);
            missMilliseconds = getHitWindow(JudgementType.Miss, od);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            textureStore = textures;

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
                    leftRim = new Sprite
                    {
                        X = -658,
                        Y = 128,
                        Origin = Anchor.CentreLeft,
                        Texture = textures.Get("leftrim"),
                        Alpha = 0,
                    },
                    leftCenter = new Sprite
                    {
                        X = -641,
                        Y = 128,
                        Origin = Anchor.CentreLeft,
                        Texture = textures.Get("leftcenter"),
                        Alpha = 0,
                    },
                    rightRim = new Sprite
                    {
                        X = -489,
                        Y = 128,
                        Origin = Anchor.CentreRight,
                        Texture = textures.Get("rightrim"),
                        Alpha = 0,
                    },
                    rightCenter = new Sprite
                    {
                        X = -506,
                        Y = 128,
                        Origin = Anchor.CentreRight,
                        Texture = textures.Get("rightcenter"),
                        Alpha = 0,
                    },
                    hitObjectContainer = new Container(),
                    judgeContainer = new Container()
                    {
                        X = -330,
                        Y = 120,
                        Scale = new Vector2(0.95f, 0.95f),
                        Anchor = Anchor.Centre
                    }
                }
            };
        }

        private void addHitObject(HitObject hitObject)
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

        private void removeHitObject(GameplayHitObject hitObject)
        {
            hitObjectContainer.Remove(hitObject.DrawableHitObject, true);

            HitObjects.Remove(hitObject);
        }

        private void addJudgement(JudgementType judgementType)
        {
            var judgement = new Sprite()
            {
                Texture = judgementType == JudgementType.Great ? textureStore.Get("hitgood") : textureStore.Get("hitmiss"),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
            };

            judgeContainer.Add(judgement);

            judgement.ScaleTo(1.15f, 150, Easing.Out).Then().ScaleTo(0.75f, 30, Easing.In).FadeOut(100).OnComplete((_) => {
                judgeContainer.Remove(judgement, true);
            });
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (currentlyPressed.Contains(e.Key))
            {
                return false;
            }

            currentlyPressed.Add(e.Key);

            var isKat = Array.FindIndex(kat, el => el == e.Key);
            var isDon = Array.FindIndex(don, el => el == e.Key);

            Sprite sprite = null;

            switch (isKat)
            {
                case 0:
                    sprite = leftRim;
                    break;
                case 1:
                    sprite = rightRim;
                    break;
            }

            switch (isDon)
            {
                case 0:
                    sprite = leftCenter;
                    break;
                case 1:
                    sprite = rightCenter;
                    break;
            }

            sprite?.FadeTo(1).Delay(100).Then().FadeTo(0, 80, Easing.Out);

            if (HitObjects.Count == 0)
                return false;

            var hitObject = HitObjects[0];

            if (hitObject.HitObject.Time - currentTime > missMilliseconds)
                return false;

            var check = hitObject.HitObject.Type == 0 && (hitObject.HitObject.Subtype == 1 || hitObject.HitObject.Subtype == 3) ? don : kat;

            removeHitObject(hitObject);

            addJudgement(check.Contains(e.Key) ? JudgementType.Great : JudgementType.Miss);

            return false;
        }

        protected override void OnKeyUp(KeyUpEvent e)
        {
            if (currentlyPressed.Contains(e.Key))
                currentlyPressed.Remove(e.Key);
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
                    addHitObject(hitObject);

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
                removeHitObject(remove);
                addJudgement(JudgementType.Miss);
            }
        }
    }
}
