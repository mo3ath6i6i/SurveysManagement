using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveysManagement.Web.Helpers
{
    public static class DateHelpers
    {
        //public static DateTime FormatDate(DateTime? value)
        //{
        //    string dateformat = "dd/MM/yyyy";
        //    if (value != null)
        //        return DateTime.TryParseExact(value, dateformat, System.Globalization.CultureInfo.InvariantCulture);
        //    return DateTime.MinValue;
        //}

        public static DateTime ConvertStringToDate(string value, string format)
        {
            string dateformat = "dd/MM/yyyy";
            if (!string.IsNullOrEmpty(value))
                return DateTime.ParseExact(value, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            return DateTime.MinValue;
        }

        public static DateTime ConvertStringToDate(string value)
        {
            DateTime result = DateTime.MinValue;
            if (!string.IsNullOrEmpty(value) && DateTime.TryParse(value, out result))
                return result;
            return DateTime.MinValue;
        }

        public static string ConvertDateToString(DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            else
                return string.Empty;
        }

        public static string FormatDateTime(DateTime? date)
        {
            string FormatedDate = string.Empty;
            if (date != null)
                FormatedDate = date.Value.ToString("dd/MM/yyyy H:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            return FormatedDate;
        }

        public static string FormatDateOnly(DateTime? date)
        {
            string FormatedDate = string.Empty;
            if (date != null)
                FormatedDate = date.Value.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            return FormatedDate;
        }
    }
}