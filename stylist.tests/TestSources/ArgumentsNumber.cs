using System;

namespace A
{
	internal class Arguments
	{
		public Arguments(int a, int b, int c, int d)
		{

		}

		public Arguments(int a, int b, int c, int d, int e)
		{

		}

		private void Print(int a, int b, int c, int d)
		{
		}

		private void Print(int a, int b, int c, int d, int e)
		{
			Action<int, int, int, int, int> f = (q, w, z, r, t) => { };

		}

		private static void Print(int a, int b, int c, int d, int e)
		{
			Action<int, int, int, int, int> f = (q, w, z, r, t) => { };
		}
		
		private 
			static 
			void 
			Print(
			int a, 
			int b, 
			int c, 
			int d, 
			int e)
		{
			Action<int, int, int, int, int> 
				f = 
				(q, 
					w, 
					z, 
					r, 
					t,
					v) => { };
		}
	}
}
//Too many arguments (Line: 12)
//Too many arguments (Line: 21)
//Too many arguments (Line: 23)
//Too many arguments (Line: 27)
//Too many arguments (Line: 29)
//Too many arguments (Line: 40)
//Too many arguments (Line: 48)
//Too many arguments (Line: 49)
