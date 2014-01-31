class Class
{
	void Print()
	{
		Action a = () =>
		{
			System.Console.WriteLine(42);
		};
		Action b = () => {
			System.Console.WriteLine(42);
		};
		A.F(42, () => {
			System.Console.WriteLine(42);
		});
		A.F(42, () => {
						System.Console.WriteLine(42);
					});
	}
}