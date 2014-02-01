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
//ArgumentsNumber: Too many arguments (Line: 12)
//ArgumentsNumber: Too many arguments (Line: 21)
//ArgumentsNumber: Too many arguments (Line: 23)
//ArgumentsNumber: Too many arguments (Line: 27)
//ArgumentsNumber: Too many arguments (Line: 29)
//ArgumentsNumber: Too many arguments (Line: 40)
//ArgumentsNumber: Too many arguments (Line: 48)
//ArgumentsNumber: Too many arguments (Line: 49)
