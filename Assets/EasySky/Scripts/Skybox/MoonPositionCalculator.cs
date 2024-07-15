//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using Unity.Mathematics;
using UnityEngine;

namespace EasySky.Skybox
{
    /// <summary>
    /// This class approximates the position of the moon on the sky. based on date and coordinates
    /// </summary>
    public class MoonPositionCalculator
    {
        #region Constants
        private const double dayMs = 86400000;
        private const double JulianDate1970 = 2440588;
        private const double JulianDate2000 = 2451545;
        private static double Rad = Math.PI / 180;
        private const double e = (Math.PI / 180) * 23.4397;
        #endregion

        #region Public Methods
        public CelestialObjectData.Position Coordinates(DateTime date, double latitude, double longitude, Vector2 offset)
        {
            if (latitude >= 89.99d) latitude = 89.99d;
            if (latitude <= -89.99d) latitude = -89.99d;

            date = date.AddHours(offset.x);
            var d = ToDays(date);
            var L = Rad * (218.316 + 13.176396 * d);
            var M = Rad * (134.963 + 13.064993 * d);
            var F = Rad * (93.272 + 13.229350 * d);
            var l = L + Rad * 6.289 * Math.Sin(M);
            var b = Rad * 5.128 * Math.Sin(F);
            var rightAscension = RightAscension(l, b);
            var declination = Declination(l, b);

            var ut = date.Hour + date.Minute / 60f + date.Second / 3600f - longitude * (24 / 360f);
            var lst = 100.46 + 0.985647f * d + longitude + 15 * ut;

            if (lst < 0)
            {
                lst += 360;
            }
            lst = lst % 360;

            var HA = lst - rightAscension * 180 / math.PI;

            if (HA < 0)
            {
                HA += 360;
            }

            HA = HA % 360;

            var alt = math.asin(math.sin(math.radians(declination)) * math.sin(math.radians(latitude + offset.y)) + math.cos(math.radians(declination)) * math.cos(math.radians(latitude + offset.y)) * math.cos(math.radians(HA)));
            alt = alt * 180 / math.PI;

            var a = math.acos((math.sin(math.radians(declination)) - math.sin(math.radians(alt)) * math.sin(math.radians(latitude + offset.y))) / (math.cos(math.radians(alt)) * math.cos(math.radians(latitude + offset.y))));
            a = a * 180 / math.PI;
            var az = a;
            if (math.sin(math.radians(HA)) > 0)
                az = 360 - a;

            var pos = new CelestialObjectData.Position()
            {
                altitude = alt,
                azimuth = az
            };

            return pos;
        }
        #endregion

        #region Private Methods
        private double RightAscension(double l, double b)
        {
            return Math.Atan2(Math.Sin(l) * Math.Cos(e) - Math.Tan(b) * Math.Sin(e), Math.Cos(l));
        }

        private double Declination(double l, double b)
        {
            return Math.Asin(Math.Sin(b) * Math.Cos(e) + Math.Cos(b) * Math.Sin(e) * Math.Sin(l));
        }

        private double ToJulian(DateTime date)
        {
            return new DateTimeOffset(date, new TimeSpan(0)).ToUnixTimeMilliseconds() / dayMs - 0.5 + JulianDate1970;
        }

        private double ToDays(DateTime date)
        {
            return ToJulian(date) - JulianDate2000;
        }
        #endregion
    }
}