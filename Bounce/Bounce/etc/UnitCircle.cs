using System;
using System.Collections.Generic;

namespace Bounce
{
    public class UnitCircle
    {
        public UnitCircle()
        {
            pi = Math.PI;
            r = new Random();
            RadiansList = new SortedList<CircleRadians, double>();
            Initialize();
        }

        private double pi;
        private Random r;
        public SortedList<CircleRadians, double> RadiansList;

        public enum CircleRadians
        {
            PiOverSix, // 30°
            PiOverFour, // 45°
            PiOverThree, // 60°
            PiOverTwo, // 90°

            TwoPiOverThree, // 120°
            ThreePiOverFour, // 135°
            FivePiOverSix, // 150°
            PiOverOne, // 180°

            SevenPiOverSix, // 210°
            FivePiOverFour, // 225°
            FourPiOverThree, // 240°
            ThreePiOverTwo, //270

            FivePiOverThree, // 300°
            SevenPiOverFour, // 315°
            ElevenPiOverSix, // 330°
            TwoPi // 360°
        }

        public double RandomSegment()
        {
            return RadiansList.Values[r.Next(RadiansList.Count)];
        }

        public double RandomSign(double segment)
        {
            return r.Next(0, 2) == 0 ? segment : -segment;
        }

        private void Initialize()
        {
            RadiansList.Add(CircleRadians.PiOverSix, pi / 6);
            RadiansList.Add(CircleRadians.PiOverFour, pi / 4);
            RadiansList.Add(CircleRadians.PiOverThree, pi / 3);
            RadiansList.Add(CircleRadians.PiOverTwo, pi / 2);

            RadiansList.Add(CircleRadians.TwoPiOverThree, (2 * pi) / 3);
            RadiansList.Add(CircleRadians.ThreePiOverFour, (3 * pi) / 4);
            RadiansList.Add(CircleRadians.FivePiOverSix, (5 * pi) / 6);
            RadiansList.Add(CircleRadians.PiOverOne, pi);

            RadiansList.Add(CircleRadians.SevenPiOverSix, (7 * pi) / 6);
            RadiansList.Add(CircleRadians.FivePiOverFour, (5 * pi) / 4);
            RadiansList.Add(CircleRadians.FourPiOverThree, (4 * pi) / 3);
            RadiansList.Add(CircleRadians.ThreePiOverTwo, (3 * pi) / 2);

            RadiansList.Add(CircleRadians.FivePiOverThree, (5 * pi) / 3);
            RadiansList.Add(CircleRadians.SevenPiOverFour, (7 * pi) / 4);
            RadiansList.Add(CircleRadians.ElevenPiOverSix, (11 * pi) / 6);
            RadiansList.Add(CircleRadians.TwoPi, 2 * pi);
        }
    }
}