using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace MyEshop
{
    public static class PersianConvertor
    {
        public static string ToShamsi(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            string date = $"{pc.GetYear(dateTime).ToString()}/{pc.GetMonth(dateTime).ToString("00")}/{pc.GetDayOfMonth(dateTime).ToString("00")}";
            return date;
        }
    }
}