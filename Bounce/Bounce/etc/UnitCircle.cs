using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Bounce
{
    public class UnitCircle
    {
        public UnitCircle()
        {
            RadiansList = new SortedList<CircleRadians, double>();
            Initialize();
        }

        private const double tau = Math.PI * 2;
        private static Random r = new Random();
        public SortedList<CircleRadians, double> RadiansList;

        public enum CircleRadians
        {
            Twelfth, // 30°
            Eighth, // 45°
            Sixth, // 60°
            Quarter, // 90°

            Third, // 120°
            ThreeEighths, // 135°
            FiveTwelfths, // 150°
            Half, // 180°

            SevenTwelfths, // 210°
            FiveEighths, // 225°
            TwoThirds, // 240°
            ThreeQuarters, //270

            FiveSixths, // 300°
            SevenEighths, // 315°
            ElevenTwefths, // 330°
            One // 360°
        }

        public Vector2 RandomSegment(Vector2 axis)
        {
            return new Vector2(
                (float)(RadiansList.Values[r.Next(RadiansList.Count)] * axis.X),
                (float)(RadiansList.Values[r.Next(RadiansList.Count)] * axis.Y));
        }

        public double RandomSign(double segment)
        {
            return r.Next(0, 2) == 0 ? segment : -segment;
        }

        private void Initialize()
        {
            RadiansList.Add(CircleRadians.Twelfth, tau / 12);
            RadiansList.Add(CircleRadians.Eighth, tau / 8);
            RadiansList.Add(CircleRadians.Sixth, tau / 6);
            RadiansList.Add(CircleRadians.Quarter, tau / 4);

            RadiansList.Add(CircleRadians.Third, tau / 3);
            RadiansList.Add(CircleRadians.ThreeEighths, (3 * tau) / 8);
            RadiansList.Add(CircleRadians.FiveTwelfths, (5 * tau) / 12);
            RadiansList.Add(CircleRadians.Half, tau / 2);

            RadiansList.Add(CircleRadians.SevenTwelfths, (7 * tau) / 12);
            RadiansList.Add(CircleRadians.FiveEighths, (5 * tau) / 8);
            RadiansList.Add(CircleRadians.TwoThirds, (2 * tau) / 3);
            RadiansList.Add(CircleRadians.ThreeQuarters, (3 * tau) / 4);

            RadiansList.Add(CircleRadians.FiveSixths, (5 * tau) / 6);
            RadiansList.Add(CircleRadians.SevenEighths, (7 * tau) / 8);
            RadiansList.Add(CircleRadians.ElevenTwefths, (11 * tau) / 12);
            RadiansList.Add(CircleRadians.One, tau);
        }
    }
}