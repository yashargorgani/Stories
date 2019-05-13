using System;
using System.Globalization;
using System.Reflection;

namespace Stories.Helpers
{
    public static class PersianDateExtensionMethods
    {
        private static CultureInfo _Culture;
        private static CultureInfo GetPersianCulture()
        {
            if (_Culture == null)
            {
                _Culture = new CultureInfo("fa-IR");
                DateTimeFormatInfo formatInfo = _Culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                var monthNames = new[]
                {
                    "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن",
                    "اسفند",
                    ""
                };
                formatInfo.AbbreviatedMonthNames =
                    formatInfo.MonthNames =
                    formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;
                System.Globalization.Calendar cal = new PersianCalendar();

                FieldInfo fieldInfo = _Culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                    fieldInfo.SetValue(_Culture, cal);

                FieldInfo info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);
                if (info != null)
                    info.SetValue(formatInfo, cal);

                _Culture.NumberFormat.NumberDecimalSeparator = "/";
                _Culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _Culture.NumberFormat.NumberNegativePattern = 0;
            }
            return _Culture;
        }


        /// <summary>
        /// تبدیل تاریخ میلادی به شمسی
        /// </summary>
        /// <param name="SampleDate">تاریخ میلادی</param>
        /// <param name="Format">فرمت خروجی</param>
        /// <returns></returns>
        public static string ToPersianDateString(this DateTime SampleDate, string Format = "yyyy/MM/dd")
        {
            return SampleDate.ToString(Format, GetPersianCulture());
        }

        /// <summary>
        /// تبدیل رشته تاریخ شمسی به تاریخ میلادی
        /// </summary>
        /// <param name="SampleDate">رشته تاریخ شمسی</param>
        /// <returns></returns>
        public static DateTime ToMiladiDate(this string SampleDate)
        {
            SampleDate = SampleDate.Trim();
            if (SampleDate.Length == 10)
            {
                PersianCalendar PC = new PersianCalendar();
                int.TryParse(SampleDate.Substring(0, 4), out int Year);
                int.TryParse(SampleDate.Substring(5, 2), out int Month);
                int.TryParse(SampleDate.Substring(8, 2), out int Day);
                return PC.ToDateTime(year: Year, month: Month, day: Day, hour: 0, minute: 0, second: 0, millisecond: 0, era: PersianCalendar.PersianEra);
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// تبدیل رشته تاریخ شمسی به تاریخ و زمان میلادی
        /// </summary>
        /// <param name="SampleDate">رشته تاریخ و زمان شمسی با فرمت YYYY/MM/DD HH:mm:SS</param>
        /// <returns></returns>
        public static DateTime ToMiladiDateTime(this string SampleDate)
        {
            SampleDate = SampleDate.Trim();
            if (SampleDate.Length == 18)
            {
                PersianCalendar PC = new PersianCalendar();
                int.TryParse(SampleDate.Substring(0, 4), out int Year);
                int.TryParse(SampleDate.Substring(5, 2), out int Month);
                int.TryParse(SampleDate.Substring(8, 2), out int Day);
                int.TryParse(SampleDate.Substring(11, 2), out int Hour);
                int.TryParse(SampleDate.Substring(14, 2), out int Minute);
                int.TryParse(SampleDate.Substring(17, 2), out int Second);
                return PC.ToDateTime(year: Year, month: Month, day: Day, hour: Hour, minute: Minute, second: Second, millisecond: 0, era: PersianCalendar.PersianEra);
            }
            return DateTime.MinValue;
        }
    }
}
