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
            RadiansDictionary = new Dictionary<CircleRadians, double>();
            Initialize();
        }

        private double pi;
        private Random r;
        public Dictionary<CircleRadians, double> RadiansDictionary;

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
            try
            {
                int maxEnumIndex = Enum.GetNames(typeof(CircleRadians)).Length;
                return IndexedRadianDictionary(r.Next(maxEnumIndex));
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Dictionary not loaded: {0}", e);
                return 0.00;
            }
        }

        public double RandomSign(double segment)
        {
            return r.Next(0, 2) == 0 ? segment : -segment;
        }

        private T indexToEnum<T>(int index)
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(index);
        }

        public double IndexedRadianDictionary(int index)
        {
            if (Enum.IsDefined(typeof(CircleRadians), index))
                return RadiansDictionary[indexToEnum<CircleRadians>(index)];
            else
                throw new ArgumentOutOfRangeException("index", index, "The enum has no value at this index.");
        }

        private void Initialize()
        {
            //Radians dictionary. Consider storing the doubles in plaintext, as this method is too verbose.
            RadiansDictionary.Add(CircleRadians.PiOverSix, pi / 6);
            RadiansDictionary.Add(CircleRadians.PiOverFour, pi / 4);
            RadiansDictionary.Add(CircleRadians.PiOverThree, pi / 3);
            RadiansDictionary.Add(CircleRadians.PiOverTwo, pi / 2);

            RadiansDictionary.Add(CircleRadians.TwoPiOverThree, (2 * pi) / 3);
            RadiansDictionary.Add(CircleRadians.ThreePiOverFour, (3 * pi) / 4);
            RadiansDictionary.Add(CircleRadians.FivePiOverSix, (5 * pi) / 6);
            RadiansDictionary.Add(CircleRadians.PiOverOne, pi);

            RadiansDictionary.Add(CircleRadians.SevenPiOverSix, (7 * pi) / 6);
            RadiansDictionary.Add(CircleRadians.FivePiOverFour, (5 * pi) / 4);
            RadiansDictionary.Add(CircleRadians.FourPiOverThree, (4 * pi) / 3);
            RadiansDictionary.Add(CircleRadians.ThreePiOverTwo, (3 * pi) / 2);

            RadiansDictionary.Add(CircleRadians.FivePiOverThree, (5 * pi) / 3);
            RadiansDictionary.Add(CircleRadians.SevenPiOverFour, (7 * pi) / 4);
            RadiansDictionary.Add(CircleRadians.ElevenPiOverSix, (11 * pi) / 6);
            RadiansDictionary.Add(CircleRadians.TwoPi, 2 * pi);
        }
    }
}