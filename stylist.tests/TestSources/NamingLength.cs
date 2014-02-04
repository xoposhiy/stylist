using System;

class MyClass
{
	private int i; //one letter field is not OK
	private int x, y; // special names — x and y

	public void X(string s)
	{
		string n = "pavel"; //One letter name is not ok!
		Console.WriteLine("Hello!");
		for(int i=0; i<10; i++)
			Console.WriteLine(n);
	}

	//i — is ok for one line for statement
	void PrintAll(string[] lines)
	{
		for(int i=0; i<lines.Length; i++)
			Console.WriteLine(lines[i]);
	}

	//Short method with short variable name is OK
	public void Print(string s)
	{
		if (s != null)
			Console.WriteLine(s);
	}
}
//Too short name (Line: 5)
//Too short name (Line: 8)
//Too short name (Line: 8)
//Too short name (Line: 10)