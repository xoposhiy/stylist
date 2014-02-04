class Class
{
	public void Do()
	{
		var a = 1;
		a = 2;
		a = 3;
		a = 4;
		a = 5;
		a = 6;
		a = 7;
		a = 8;
		a = 9;
		a = 10;
		a = 11;
		a = 12;
		a = 13;
		a = 14;
		a = 15;
		a = 16;
		a = 17;
		a = 18;
		a = 19;
		a = 20;
		a = 21;
	}

	public void DoIf()
	{
		if (true)
		{
			var a = 1;
			a = 2;
			a = 3;
			a = 4;
			a = 5;
			a = 6;
			a = 7;
			a = 8;
			a = 9;
			a = 10;
			a = 11;
			a = 12;
			a = 13;
			a = 14;
			a = 15;
			a = 16;
			a = 17;
			a = 18;
			a = 19;
			a = 20;
			a = 21;
		}
	}
	public void DoFor()
	{
		for (int i=0; i<10; i++)
		{
			var a = 1;
			a = 2;
			a = 3;
			a = 4;
			a = 5;
			a = 6;
			a = 7;
			a = 8;
			a = 9;
			a = 10;
			a = 11;
			a = 12;
			a = 13;
			a = 14;
			a = 15;
			a = 16;
			a = 17;
			a = 18;
			a = 19;
			a = 20;
			a = 21;
		}
	}

	public void DoNested()
	{
		if (true)
			if (true)
				if (true) return;
				else return;
			else
			{
				if (true) return;
				else return;
			}
		else
		{
			if (true)
				if (true) return;
				else return;
			else
			{
				if (true) return;
				else return;
			}
		}
		if(true) return;
	}

	public void DoShort()
	{
		var a = 1;
		a = 2;
		a = 3;
		a = 4;
		a = 5;
		a = 6;
		a = 7;
		a = 8;
		a = 9;
		a = 10;
		a = 11;
		a = 12;
		a = 13;
		a = 14;
		a = 15;
		a = 16;
		a = 17;
		a = 18;
		a = 19;
	}
	public void DoVeryShort()
	{
	}
}

//Too long method. Try to divide it into smaller parts. (Line: 3)
//Too long method. Try to divide it into smaller parts. (Line: 28)
//Too long method. Try to divide it into smaller parts. (Line: 55)
//Too long method. Try to divide it into smaller parts. (Line: 83)