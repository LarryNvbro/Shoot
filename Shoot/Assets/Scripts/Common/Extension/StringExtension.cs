using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class StringExtension
{
	public static string SubstringFirst(this string s, int length)
	{
		if (string.IsNullOrEmpty(s))
			return string.Empty;
		string value = s.Trim();
		return length >= value.Length ? value : value.Substring(0, length);
	}

	public static string SubstringLast(this string s, int length)
	{
		if (string.IsNullOrEmpty(s))
			return string.Empty;
		string value = s.Trim();
		return length >= value.Length ? value : value.Substring(value.Length - length);
	}

	public static int ToInt(this string s, bool throwException = false)
	{
		int result = 0;
		if (!int.TryParse(s, out result)) {
			if (throwException) {
				throw new System.FormatException(string.Format("'{0}' cannot be converted as int", s));
			}
		}
		return result;
	}

	public static float ToFloat(this string s, bool throwException = false)
	{
		float result = 0.0f;
		if (!float.TryParse(s, out result)) {
			if (throwException) {
				throw new System.FormatException(string.Format("'{0}' cannot be converted as float", s));
			}
		}
		return result;
	}

	public static double ToDouble(this string s, bool throwException = false)
	{
		double result = 0.0;
		if (!double.TryParse(s, out result)) {
			if (throwException) {
				throw new System.FormatException(string.Format("'{0}' cannot be converted as double", s));
			}
		}
		return result;
	}
}
