
namespace DGPE.Math.FixedPoint{
	public static class FixedConstants{
		public static int FRACTIONAL_BITS_COUNT = 16;
		public static int VALUE_BITS_COUNT = 31;
		public static int NATURAL_BITS_COUNT = VALUE_BITS_COUNT-FRACTIONAL_BITS_COUNT;
		public static int SHIFT_MULTIPLIER = 1<<FRACTIONAL_BITS_COUNT;
		public static int MAX_INT_VALUE = (1<<NATURAL_BITS_COUNT)-2;// 2^15-1 because of negative representaion
		public static int MIN_INT_VALUE = -MAX_INT_VALUE;
		public static int MAX_FIXED_VALUE = MAX_INT_VALUE * SHIFT_MULTIPLIER;
		public static int MIN_FIXED_VALUE = -MAX_FIXED_VALUE;
		public static int FIXED_ONE_VALUE = SHIFT_MULTIPLIER;
		public static int MIN_POSITIVE_VALUE = 1;
		public static int MAX_NEGATIVE_VALUE = -MIN_POSITIVE_VALUE;
		public static float MAX_FLOAT_VALUE = MAX_INT_VALUE;
		public static float MIN_FLOAT_VALUE = -MAX_FLOAT_VALUE;
		public static Fixed FIXED_ZERO = (Fixed)0;
	}
}
