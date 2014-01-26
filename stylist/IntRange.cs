using Newtonsoft.Json;

namespace stylist
{
	public class IntRange
	{
		[JsonConstructor]
		public IntRange(int min, int max)
		{
			Min = min;
			Max = max;
		}

		[JsonProperty]
		public int Min { get; private set; }
		[JsonProperty]
		public int Max { get; private set; }

		public IntRange WithMin(int newMin)
		{
			return new IntRange(newMin, Max);
		}

		public override string ToString()
		{
			return string.Format("Min: {0}, Max: {1}", Min, Max);
		}

		protected bool Equals(IntRange other)
		{
			return Min == other.Min && Max == other.Max;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((IntRange) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (Min*397) ^ Max;
			}
		}
	}
}