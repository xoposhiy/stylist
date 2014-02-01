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
//CanBeSimplified: Use return instead of if statement (Line: 9)
//CanBeSimplified: Use return instead of if statement (Line: 14)
//CanBeSimplified: Use return instead of if statement (Line: 19)
//CanBeSimplified: Use return instead of if statement (Line: 24)
//CanBeSimplified: Use return instead of if statement (Line: 29)
//CanBeSimplified: Use return instead of if statement (Line: 34)