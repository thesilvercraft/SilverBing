using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverBingConfigUi
{
    internal class DateUtils
    {
        /// <summary>
        /// Validates if a specified date is valid
        /// </summary>
        /// <param name="day">The day as <see cref="Int32"/></param>
        /// <param name="month">The month as <see cref="Int32"/></param>
        /// <param name="year">The year as <see cref="Int32"/></param>
        /// <returns>An <see cref="IsValidDate"/></returns>
        public static IsValidDate Is_Valid_Date(int day, int month, int year)
        {
            if (year > 9999)
            {
                //uhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
                //bruh moment
                //welp
                return IsValidDate.YearLargerThanAllowed;
            }
            if (month > 12)
            {
                //imagine
                return IsValidDate.MonthLargerThanAllowed;
            }
            if (day > 31)
            {
                //can you even tie your shoes
                return IsValidDate.DayLargerThanPossible;
            }
            if (DateTime.DaysInMonth(year, month) < day)
            {
                //WeirdChamp
                return IsValidDate.DayLargerThanPossible;
            }
            //Pog
            return IsValidDate.True;
        }

        /// <summary>
        /// Validates if a specified date is valid
        /// </summary>
        /// <param name="day">The day as <see cref="Int32"/></param>
        /// <param name="month">The month as <see cref="Int32"/></param>
        /// <param name="year">The year as <see cref="Int32"/></param>
        /// <returns>An <see cref="bool"/></returns>
        public static bool Is_Valid_Date_Bool(int day, int month, int year) => Is_Valid_Date(day, month, year) == IsValidDate.True;

        public enum IsValidDate
        {
            True = 1,
            YearLargerThanAllowed = 9999,
            MonthLargerThanAllowed = 12,
            DayLargerThanPossible = 69
        }

        internal static bool Is_Valid_Date_Bool(int day, int month, int? year)
        {
            throw new NotImplementedException();
        }
    }
}