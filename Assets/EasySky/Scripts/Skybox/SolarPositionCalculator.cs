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
    /// This class approximates the positon of the sun on the sky
    /// </summary>
    public class SolarPositionCalculator
    {
        public CelestialObjectData.Position GetSunPosition(DateTime date, double latitude, double longitude, float utc_offset, Vector2 offset)
        {
            if (latitude >= 89.99d) latitude = 89.99d;
            if (latitude <= -89.99d) latitude = -89.99d;
            var hour_minute = (date.Hour + offset.x) % 23 + date.Minute / 60f + date.Second / 3600f - utc_offset;
            var day_of_year = date.DayOfYear;
            var g = 360 / 365.25 * (day_of_year + hour_minute / 24);
            var g_radians = math.radians(g);
            var declination = 0.396372 - 22.91327 * math.cos(g_radians) + 4.02543 * math.sin(g_radians) - 0.387205 * math.cos(2 * g_radians) + 0.051967 * math.sin(2 * g_radians) - 0.154527 * math.cos(3 * g_radians) + 0.084798 * math.sin(3 * g_radians);
            var time_correction = 0.004297 + 0.107029 * math.cos(g_radians) - 1.837877 * math.sin(g_radians) - 0.837378 * math.cos(2 * g_radians) - 2.340475 * math.sin(2 * g_radians);
            var SHA = (hour_minute - 12) * 15 + longitude + time_correction;
            var lat_radians = math.radians(latitude + offset.y);
            var d_radians = math.radians(declination);
            var SHA_radians = math.radians(SHA);
            var SZA_radians = math.acos(math.sin(lat_radians) * math.sin(d_radians) + math.cos(lat_radians) * math.cos(d_radians) * math.cos(SHA_radians));
            var SZA = math.degrees(SZA_radians);
            var SEA = 90 - SZA;
            var cos_AZ = (math.sin(d_radians) - math.sin(lat_radians) * math.cos(SZA_radians)) / (math.cos(lat_radians) * math.sin(SZA_radians));
            var AZ_rad = math.acos(cos_AZ);
            var AZ = math.degrees(AZ_rad);
            if ((date.Hour + offset.x) % 23 + date.Minute / 60f + date.Second / 3600f > 12)
            {
                AZ = 360 - AZ;
            }

            return new CelestialObjectData.Position() { altitude = SEA, azimuth = AZ };
        }
    }
}