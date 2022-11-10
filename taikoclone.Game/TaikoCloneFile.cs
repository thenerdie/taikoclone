using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework.Constraints;
using osu.Framework.Logging;

namespace taikoclone.Game
{
    public class TaikoCloneFile
    {
        public string AudioFilename;
        public int PreviewTime;

        public int BeatDivisor;

        public string Artist;
        public string ArtistUnicode;
        public int BeatmapID;
        public int BeatmapSetID;
        public string Creator;
        public string Tags;
        public string Title;
        public string TitleUnicode;
        public string Version;

        public float HPDrainRate;
        public float OverallDifficulty;
        public float SliderMultiplier;
        public float SliderTickRate;
        public List<HitObject> HitObjects;

        private enum Section { None, General, Metadata, Editor, HitObjects, TimingPoints, Events, Difficulty }

        public struct HitObject
        {
            public float Time;
            public int Type;
            public int Subtype;
        }

        public TaikoCloneFile(string file)
        {
            HitObjects = new List<HitObject>();

            Section currentSection = Section.None;
            string[] lines = file.Split("\n");

            foreach (string rawline in lines)
            {
                if (string.IsNullOrWhiteSpace(rawline)
                        || rawline.StartsWith("//", StringComparison.Ordinal)
                        || rawline.StartsWith(" ", StringComparison.Ordinal)
                        || rawline.StartsWith("_", StringComparison.Ordinal))
                    continue;

                string line = rawline.Trim();

                switch (line)
                {
                    case "[General]":
                        currentSection = Section.General;
                        break;
                    case "[Editor]":
                        currentSection = Section.Editor;
                        break;
                    case "[Metadata]":
                        currentSection = Section.Metadata;
                        break;
                    case "[Events]":
                        currentSection = Section.Events;
                        break;
                    case "[HitObjects]":
                        currentSection = Section.HitObjects;
                        break;
                    case "[TimingPoints]":
                        currentSection = Section.TimingPoints;
                        break;
                    case "[Difficulty]":
                        currentSection = Section.Difficulty;
                        break;
                }

                if ((currentSection == Section.General
                    || currentSection == Section.Editor
                    || currentSection == Section.Metadata
                    || currentSection == Section.Difficulty) && line.Contains(":"))
                {
                    string key = line[..line.IndexOf(':')].Trim();
                    string value = line.Split(':').Last().Trim();

                    switch (key)
                    {
                        case "AudioFilename":
                            AudioFilename = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "PreviewTime":
                            PreviewTime = int.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "BeatDivisor":
                            BeatDivisor = int.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "Title":
                            Title = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "TitleUnicode":
                            TitleUnicode = value;
                            break;
                        case "Artist":
                            Artist = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "ArtistUnicode":
                            ArtistUnicode = value;
                            break;
                        case "Creator":
                            Creator = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "Version":
                            Version = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "Tags":
                            Tags = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(value));
                            break;
                        case "BeatmapID":
                            BeatmapID = int.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "BeatmapSetID":
                            BeatmapSetID = int.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "HPDrainRate":
                            HPDrainRate = float.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "OverallDifficulty":
                            OverallDifficulty = float.Parse(value, CultureInfo.InvariantCulture);
                            break;
                        case "SliderMultiplier":
                            SliderMultiplier = float.Parse(value, CultureInfo.InvariantCulture);
                            break;
                    }
                }

                if (currentSection == Section.HitObjects && line.Contains(","))
                {
                    var values = line.Split(',');

                    var hitObject = new HitObject()
                    {
                        Time = float.Parse(values[0]),
                        Type = int.Parse(values[1]),
                        Subtype = int.Parse(values[2]),
                    };

                    HitObjects.Add(hitObject);
                }
            }
        }
    }
}
