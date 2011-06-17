using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bounce
{
    public static class UnitCircle
    {
        static double pi = Math.PI;
        private static Random r = new Random();

        //Top right.
        /// <summary>
        /// 30°
        /// </summary>
        public static double PiOverSix { get { return pi / 6; } }
        /// <summary>
        /// 45°
        /// </summary>
        public static double PiOverFour { get { return pi / 4; } }
        /// <summary>
        /// 60°
        /// </summary>
        public static double PiOverThree { get { return pi / 3; } }
        /// <summary>
        /// 90°
        /// </summary>
        public static double PiOverTwo { get { return pi / 2; } }

        //Top left.
        /// <summary>
        /// 120°
        /// </summary>
        public static double TwoPiOverThree { get { return (2 * pi) / 3; } }
        /// <summary>
        /// 135°
        /// </summary>
        public static double ThreePiOverFour { get { return (3 * pi) / 4; } }
        /// <summary>
        /// 150°
        /// </summary>
        public static double FivePiOverSix { get { return (5 * pi) / 6; } }
        /// <summary>
        /// 180°
        /// </summary>
        public static double PiOverOne { get { return pi; } }

        //Bottom left.
        /// <summary>
        /// 210°
        /// </summary>
        public static double SevenPiOverSix { get { return (7 * pi) / 6; } }
        /// <summary>
        /// 225°
        /// </summary>
        public static double FivePiOverFour { get { return (5 * pi) / 4; } }
        /// <summary>
        /// 240°
        /// </summary>
        public static double FourPiOverThree { get { return (4 * pi) / 3; } }
        /// <summary>
        /// 270°
        /// </summary>
        public static double ThreePiOverTwo { get { return (3 * pi) / 2; } }

        //Bottom right.
        /// <summary>
        /// 300°
        /// </summary>
        public static double FivePiOverThree { get { return (5 * pi) / 3; } }
        /// <summary>
        /// 315°
        /// </summary>
        public static double SevenPiOverFour { get { return (7 * pi) / 4; } }
        /// <summary>
        /// 330°
        /// </summary>
        public static double ElevenPiOverSix { get { return (11 * pi) / 6; } }
        /// <summary>
        /// 360°
        /// </summary>
        public static double TwoPi { get { return 2 * pi; } }

        public static double RandomSegment()
        {
            return Indexed(r.Next(0, 16));
        }

        public static double RandomSign(double segment)
        {
            return r.Next(0, 2) == 0 ? segment : -segment;
        }

        public static double Indexed(int index)
        {
            switch (index)
            {
                case 0:
                    return PiOverSix;
                case 1:
                    return PiOverFour;
                case 2:
                    return PiOverThree;
                case 3:
                    return PiOverTwo;
                case 4:
                    return TwoPiOverThree;
                case 5:
                    return ThreePiOverFour;
                case 6:
                    return FivePiOverSix;
                case 7:
                    return PiOverOne;
                case 8:
                    return SevenPiOverSix;
                case 9:
                    return FivePiOverFour;
                case 10:
                    return FourPiOverThree;
                case 11:
                    return ThreePiOverTwo;
                case 12:
                    return FivePiOverThree;
                case 13:
                    return SevenPiOverFour;
                case 14:
                    return ElevenPiOverSix;
                case 15:
                    return TwoPi;
                default:
                    return 0.00;
            }
        }
    }
}