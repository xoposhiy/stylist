using System;

namespace A
{
	internal class ReturnBool
	{
		public bool IsOk(bool ok)
		{
			if (ok) return true;
			else return false;
		}
		public bool IsOk2(bool ok)
		{
			if (!ok) return true;
			else return false;
		}
		public bool IsOk3(bool ok)
		{
			if (!ok) return false;
			else return true;
		}
		public bool IsOk4(bool ok)
		{
			if (ok) return false;
			else return true;
		}
		public bool IsOk5(bool ok)
		{
			if (ok) return false;
			return true;
		}
		public bool IsOk6(bool ok)
		{
			if (!ok) return true;
			return false;
		}
		public string IsOk01(bool ok)
		{
			if (ok) return "true";
			else return "false";
		}
		public string IsOk02(bool ok)
		{
			if (ok) return ok && !ok;
			else return false;
		}
		public string IsOk03(bool ok)
		{
			if (ok) return true;
			return ok && !ok;
		}
	}
}
//Use return instead of if statement (Line: 9)
//Use return instead of if statement (Line: 14)
//Use return instead of if statement (Line: 19)
//Use return instead of if statement (Line: 24)
//Use return instead of if statement (Line: 29)
//Use return instead of if statement (Line: 34)