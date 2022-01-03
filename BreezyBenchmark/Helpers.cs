using System;
using System.Collections.Generic;
using System.Text;

namespace BreezyBenchmark
{
    public static class Helpers
    {
		private static List<string> EmptyList = new List<string>() { "", "N/A", "NA", "TBD", "UNKNOWN", "NULL" };

		public static bool HasValue(this string Value)
        {
            return !string.IsNullOrEmpty(Value.SafeTrim())
                && !EmptyList.Contains(Value.SafeTrim().ToUpper());
        }

        public static string SafeTrim(this object Value)
        {
            return ToSafeString(Value, string.Empty).Trim();
        }

		public static string ToSafeString(this object Value)
		{
			return ToSafeString(Value, string.Empty);
		}

		public static string ToSafeString(this object Value, string defaultValue)
		{
			try
			{
				if (Value == null)
				{
					return defaultValue;
				}
				else
				{
					return Value.ToString();
				}
			}
			catch
			{
				return defaultValue;
			}
		}

		public static Int32 ToInt32(this object Value)
		{
			return ToInt32(Value, default(Int32));
		}

		public static Int32 ToInt32(object Value, Int32 defaultValue)
		{
			try
			{
				if (Value == null)
				{
					return defaultValue;
				}
				else
				{
					return Convert.ToInt32(Value);
				}
			}
			catch
			{
				return defaultValue;
			}
		}

		public static string ToDisplayString(this TimeSpan ts)
		{
			string display = string.Empty;

			if (ts != null)
			{
				display += (ts.Days > 0 ? ChooseTense(ts.Days, "Day ", "Days ") : string.Empty);
				display += (ts.Hours > 0 ? ChooseTense(ts.Hours, "Hour ", "Hours ") : string.Empty);
				display += (ts.Minutes > 0 ? ChooseTense(ts.Minutes, "Minute ", "Minutes ") : string.Empty);
				display += (ts.Seconds > 0 ? ChooseTense(ts.Seconds, "Second ", "Seconds ") : string.Empty);
				display += (ts.Milliseconds > 0 ? ChooseTense(ts.Milliseconds, "Millisecond ", "Milliseconds ") : string.Empty);
			}

			return display;
		}

		public static string ChooseTense(int Value, string singular, string plural)
		{
			if (Value == 1)
			{
				return Value.ToString() + " " + singular;
			}
			else
			{
				return Value.ToString() + " " + plural;
			}
		}

		public static string ToFullDateTimeString(this DateTime dt)
		{
			return dt.ToString("dd/MM/yyyy HH:mm:ss");
		}
	}
}
