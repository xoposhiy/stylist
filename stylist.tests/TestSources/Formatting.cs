﻿using System;

namespace A
{
	class Class
	{
		void Print()
		{
			if (true 
				&& false)
				Console.WriteLine();
			else if (false) Console.WriteLine();
			else if (true)
				Console.WriteLine();
			if (true)
				Console.WriteLine();
			else
			{
				Console.WriteLine();
			}
		}
		void Write() { }

		void WriteOk() {
			for (int i = 0; i < 10; i++) {
				Console.WriteLine(i);
			}
		}

		void WrongPrint()
		{
		var used = false;
		}

	void WrongPrint2()
	{
		var used = fasle;
	}

	void WrongPrint3()
		{
			var used = false;
		}
	}
}
//Missing indentation (Line: 32)
//Sibling entities should have same indentation (Line: 35)
//Missing indentation (Line: 35)
//Missing indentation (Line: 40)
//Do not indent blocks (Line: 41)
