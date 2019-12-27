using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AVBS.Admin.UI.Helper {
    public static class DateHelper {
         

        public static string GetHourString( int hour ) {

            bool isBeforeMidnight = hour / 12 < 1;

            return (hour%12 == 0 ? 12 : hour%12) + ":00 " + (isBeforeMidnight ? "a.m." : "p.m.");
        }


        public static string[] GetHoursOfDay( ) {
            return new [] {
                "12:00 a.m.",
                "01:00 a.m.",
                "02:00 a.m.",
                "03:00 a.m.",
                "04:00 a.m.",
                "05:00 a.m.",
                "06:00 a.m.",
                "07:00 a.m.",
                "08:00 a.m.",
                "09:00 a.m.",
                "10:00 a.m.",
                "11:00 a.m.",
                "12:00 p.m.",
                "01:00 p.m.",
                "02:00 p.m.",
                "03:00 p.m.",
                "04:00 p.m.",
                "05:00 p.m.",
                "06:00 p.m.",
                "07:00 p.m.",
                "08:00 p.m.",
                "09:00 p.m.",
                "10:00 p.m.",
                "11:00 p.m.",
            };
        }
    }

}