using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bounce
{
    public class UnitCircle
    {
        public UnitCircle()
        {
            pi = Math.PI;
            r = new Random();
            RadiansDictionary = new Dictionary<CircleRadians, double>();
            //DegreesDictionary = new Dictionary<CircleDegrees, double>();
            Initialize();
        }
        double pi;
        private Random r;
        public Dictionary<CircleRadians, double> RadiansDictionary;
        //public Dictionary<CircleDegrees, double> DegreesDictionary;

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
        //public enum CircleDegrees
        //{
        //    Thirty, // 30°
        //    FourtyFive, // 45°
        //    Sixty, // 60°
        //    Ninety, // 90°

        //    OneTwenty, // 120°
        //    OneThirtyFive, // 135°
        //    OneFifty, // 150°
        //    OneEighty, // 180°

        //    TwoTen, // 210°
        //    TwoTentyFive, // 225°
        //    TwoFourty, // 240°
        //    TwoSeventy, //270

        //    ThreeHundred, // 300°
        //    ThreeFifteen, // 315°
        //    ThreeThirty, // 330°
        //    ThreeSixty // 360°
        //}

        public double RandomSegment()
        {
            try
            {
                return IndexedRadianDictionary(r.Next(16));
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

        private Enum indexedEnumValue(int index, Type enumType)
        {
            var values = Enum.GetValues(enumType);
            return (Enum)values.GetValue(index);
        }

        public double IndexedRadianDictionary(int index)
        {
            return RadiansDictionary[(CircleRadians)indexedEnumValue(index, typeof(CircleRadians))];
        }

        private void Initialize()
        {
            //Radians dictionary.
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

            //Degrees dictionary, purely for convenience. Not in use right now.
            //DegreesDictionary.Add(CircleDegrees.Thirty, RadiansDictionary[CircleRadians.PiOverSix]);
            //DegreesDictionary.Add(CircleDegrees.FourtyFive, RadiansDictionary[CircleRadians.PiOverFour]);
            //DegreesDictionary.Add(CircleDegrees.Sixty, RadiansDictionary[CircleRadians.PiOverThree]);
            //DegreesDictionary.Add(CircleDegrees.Ninety, RadiansDictionary[CircleRadians.PiOverTwo]);

            //DegreesDictionary.Add(CircleDegrees.OneTwenty, RadiansDictionary[CircleRadians.TwoPiOverThree]);
            //DegreesDictionary.Add(CircleDegrees.OneThirtyFive, RadiansDictionary[CircleRadians.ThreePiOverFour]);
            //DegreesDictionary.Add(CircleDegrees.OneFifty, RadiansDictionary[CircleRadians.FivePiOverSix]);
            //DegreesDictionary.Add(CircleDegrees.OneEighty, RadiansDictionary[CircleRadians.PiOverOne]);

            //DegreesDictionary.Add(CircleDegrees.TwoTen, RadiansDictionary[CircleRadians.SevenPiOverSix]);
            //DegreesDictionary.Add(CircleDegrees.TwoTentyFive, RadiansDictionary[CircleRadians.FivePiOverFour]);
            //DegreesDictionary.Add(CircleDegrees.TwoFourty, RadiansDictionary[CircleRadians.FourPiOverThree]);
            //DegreesDictionary.Add(CircleDegrees.TwoSeventy, RadiansDictionary[CircleRadians.ThreePiOverTwo]);

            //DegreesDictionary.Add(CircleDegrees.ThreeHundred, RadiansDictionary[CircleRadians.FivePiOverThree]);
            //DegreesDictionary.Add(CircleDegrees.ThreeFifteen, RadiansDictionary[CircleRadians.SevenPiOverFour]);
            //DegreesDictionary.Add(CircleDegrees.ThreeThirty, RadiansDictionary[CircleRadians.ElevenPiOverSix]);
            //DegreesDictionary.Add(CircleDegrees.ThreeSixty, RadiansDictionary[CircleRadians.TwoPi]);
        }
    }
}