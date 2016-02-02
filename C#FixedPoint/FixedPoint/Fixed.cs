using System;
namespace DGPE.Math.FixedPoint{
	public struct Fixed{
		//TODO: may be i should add code which would throw an error when Fixed
		//is reached one of the limitations(MAX or Min)
		private int fixedValue;// yep this is all of the data in this stuct :)
		//"to Fixed" operators
		public static Fixed CreateFixedByFixedValue(int fixedValue){
			CropFixedValue (ref fixedValue);
			return new Fixed (fixedValue);
		}
		public static Fixed RoundByBits(Fixed f,ushort bits){
			if (bits > FixedConstants.FRACTIONAL_BITS_COUNT)
				throw new System.ArgumentOutOfRangeException ("bits > FixedConstants.FRACTIONAL_BITS_COUNT");
			return new Fixed ((f.fixedValue >> bits) << bits);
		}
		public static Fixed CropToFraction(Fixed f){
			return new Fixed(f.fixedValue&(FixedConstants.SHIFT_MULTIPLIER-1));
		}
		public static Fixed Abs(Fixed v){
			if (v.fixedValue < 0)
				return new Fixed (-v.fixedValue);
			return new Fixed (v.fixedValue);
		}
		public static explicit operator Fixed(float value){
			CropFloatValue (ref value);
			return new Fixed ((int)(value * FixedConstants.SHIFT_MULTIPLIER));
		}
		public static explicit operator Fixed(int value){
			CropIntValue (ref value);
			return new Fixed (value<<FixedConstants.FRACTIONAL_BITS_COUNT);
		}
		//"Fixed to" operators
		public static explicit operator int(Fixed value){
			return value.fixedValue>>FixedConstants.FRACTIONAL_BITS_COUNT;
		}
		public static explicit operator float(Fixed value){
			return ((float)value.fixedValue) / FixedConstants.SHIFT_MULTIPLIER;
		}
		//Fixed with Fixed arithmetic operators
		public static Fixed operator +(Fixed a,Fixed b){
			int newFixedValue = a.fixedValue + b.fixedValue;
			CropFixedValue (ref newFixedValue);
			return new Fixed(newFixedValue);
		}
		public static Fixed operator-(Fixed a,Fixed b){
			int newFixedValue = a.fixedValue - b.fixedValue;
			CropFixedValue (ref newFixedValue);
			return new Fixed(newFixedValue);
		}
		public static Fixed operator*(Fixed a,Fixed b){
			long newFixedValue = (((long)a.fixedValue) * b.fixedValue) >> FixedConstants.FRACTIONAL_BITS_COUNT;
			if (TryToFixMulDivSign (ref a, ref b, ref newFixedValue)) 
				return new Fixed ((int)(newFixedValue));
			return new Fixed (ref newFixedValue);
		}
		public static Fixed operator/(Fixed a,Fixed b){
			long newFixedValue = (((long)a.fixedValue) << FixedConstants.FRACTIONAL_BITS_COUNT)/b.fixedValue;
			if (TryToFixMulDivSign (ref a, ref b, ref newFixedValue)) 
				return new Fixed ((int)(newFixedValue));
			return new Fixed (ref newFixedValue);
		}
		public static Fixed operator-(Fixed a){
			return new Fixed (-a.fixedValue);
		}
		//Fixed with int mul and div
		public static Fixed operator/(Fixed a,int b){
			long newFixedValue = a.fixedValue / b;
			TryToFixDivMulSign (ref a, ref b, ref newFixedValue);
			return new Fixed ((int)newFixedValue);
		}
		public static Fixed operator/(int a,Fixed b){
			return (Fixed)a / b;
		}
		public static Fixed operator*(Fixed a,int b){
			long newFixedValue = a.fixedValue * b;
			if (TryToFixDivMulSign (ref a, ref b, ref newFixedValue))
				return new Fixed ((int)newFixedValue);
			return new Fixed (ref newFixedValue);
		}
		public static Fixed operator*(int a,Fixed b){
			return b * a;
		}
		//increment operators
		public static Fixed operator++(Fixed a){
			a.fixedValue = a.fixedValue+FixedConstants.FIXED_ONE_VALUE;
			CropFixedValue (ref a.fixedValue);
			//Console.WriteLine ("INCREMENT");
			return a;
		}
		public static Fixed operator--(Fixed a){
			a.fixedValue -= FixedConstants.FIXED_ONE_VALUE;
			CropFixedValue (ref a.fixedValue);
			return a;
		}
		//comparsion operators
		public static bool operator==(Fixed a,Fixed b){
			return a.fixedValue == b.fixedValue;
		}
		public static bool operator!=(Fixed a,Fixed b){
			return a.fixedValue != b.fixedValue;
		}
		public static bool operator>(Fixed a,Fixed b){
			return a.fixedValue > b.fixedValue;
		}
		public static bool operator>=(Fixed a,Fixed b){
			return a.fixedValue >= b.fixedValue;
		}
		public static bool operator<(Fixed a,Fixed b){
			return a.fixedValue < b.fixedValue;
		}
		public static bool operator<=(Fixed a,Fixed b){
			return a.fixedValue <= b.fixedValue;
		}
		//SQRT and SQUARE
		//TODO: check 0.xxxx square roots calculation
		//OPTIMIZATION_REQUIRED:SQRT 
		public static Fixed Sqrt(Fixed a){
			if(a.fixedValue<0)
				throw new System.ArgumentOutOfRangeException("Can not calculate sqrt of negative number");
			if(a.fixedValue==0)
				return new Fixed(0);
			long fixedValue = a.fixedValue>>FixedConstants.FRACTIONAL_BITS_COUNT;
			long bit = 1<<(FixedConstants.VALUE_BITS_COUNT-1);
			long fixedRoot = 0;
			while(bit>fixedValue)
				bit>>=2;
			while(bit!=0){
				if(fixedValue>=fixedRoot+bit){
					fixedValue-=fixedRoot+bit;
					fixedRoot = (fixedRoot>>1)+bit;
				}else{
					fixedRoot>>=1;
				}
				bit>>=2;
			}
			fixedValue = a.fixedValue;
			fixedRoot<<=FixedConstants.FRACTIONAL_BITS_COUNT;
			if(fixedRoot==0)
				fixedRoot = FixedConstants.FIXED_ONE_VALUE;
			fixedRoot = (fixedRoot+(fixedValue<<FixedConstants.FRACTIONAL_BITS_COUNT)/fixedRoot)>>1;
			fixedRoot = (fixedRoot+(fixedValue<<FixedConstants.FRACTIONAL_BITS_COUNT)/fixedRoot)>>1;
			fixedRoot = (fixedRoot+(fixedValue<<FixedConstants.FRACTIONAL_BITS_COUNT)/fixedRoot)>>1;
			fixedRoot = (fixedRoot+(fixedValue<<FixedConstants.FRACTIONAL_BITS_COUNT)/fixedRoot)>>1;
			Fixed root = new Fixed((int)fixedRoot);
			return root;
		}
		public static Fixed Square(Fixed a){
			long newFixedValue = (((long)a.fixedValue) * a.fixedValue) >> FixedConstants.FRACTIONAL_BITS_COUNT;
			if (newFixedValue == 0 && a.fixedValue != 0)
				return new Fixed (FixedConstants.MIN_POSITIVE_VALUE);
			return new Fixed(ref newFixedValue);
		}
		//overridings
		public override int GetHashCode ()
		{
			return fixedValue;
		}
		public override bool Equals (object obj)
		{
			return obj!= null && obj.GetType()==this.GetType()&&(Fixed)obj==this;
		}
		public override string ToString (){
			return string.Format("{0}",((float)this.fixedValue/FixedConstants.SHIFT_MULTIPLIER));
		}
		public bool IsZero(){
			return this.fixedValue == 0;
		}
		//TODO:unchecked method
		public bool IsZero(Fixed EPS){
			int eps;
			if (EPS.fixedValue < 0)
				eps = -EPS.fixedValue;
			else
				eps = EPS.fixedValue;
			return fixedValue>=0&&fixedValue<=eps;
		}
		public bool IsPositiveOrZero(){
			return this.fixedValue >= 0;
		}
		public bool IsPositive(){
			return this.fixedValue > 0;
		}
		public bool IsNegative(){
			return this.fixedValue < 0;
		}
		public bool IsNegativeOrZero(){
			return this.fixedValue <= 0;
		}
		private Fixed(int fixedValue){
			this.fixedValue = fixedValue;
		}
		private Fixed(ref long mulOrDivResultFixedValue){
			CropFixedValue (ref mulOrDivResultFixedValue);
			fixedValue = (int)(mulOrDivResultFixedValue);
		}
		private static void CropFloatValue(ref float value){
			if (value < FixedConstants.MIN_FLOAT_VALUE) {
				value = FixedConstants.MIN_FLOAT_VALUE;
				return;
			}
			if (value > FixedConstants.MAX_FLOAT_VALUE) {
				value = FixedConstants.MAX_FLOAT_VALUE;
				return;
			}
		}
		private static void CropIntValue(ref int value){
			if (value < FixedConstants.MIN_INT_VALUE) {
				value = FixedConstants.MIN_INT_VALUE;
				return;
			}
			if (value > FixedConstants.MAX_INT_VALUE) {
				value = FixedConstants.MAX_INT_VALUE;
				return;
			}
		}
		private static void CropFixedValue(ref int value){
			if (value <= FixedConstants.MIN_FIXED_VALUE) {
				value = FixedConstants.MIN_FIXED_VALUE;
				return;
			}
			if (value >= FixedConstants.MAX_FIXED_VALUE) {
				value = FixedConstants.MAX_FIXED_VALUE;
				return;
			}
		}
		private static void CropFixedValue(ref long value){
			if (value < FixedConstants.MIN_FIXED_VALUE) {
				value = FixedConstants.MIN_FIXED_VALUE;
				return;
			}
			if (value > FixedConstants.MAX_FIXED_VALUE) {
				value = FixedConstants.MAX_FIXED_VALUE;
				return;
			}
		}
		//TODO: decide the fate of min positive value limit 2) check this shit
		private static bool TryToFixMulDivSign(ref Fixed a,ref Fixed b,ref long newFixedValue){
			if (newFixedValue == 0) {
				if ((a.fixedValue == 0)||(b.fixedValue == 0))
					return false;
				else {
					if ((a.fixedValue > 0) == (b.fixedValue > 0)) {
						newFixedValue = FixedConstants.MIN_POSITIVE_VALUE;
					} else {
						newFixedValue = FixedConstants.MAX_NEGATIVE_VALUE;
					}
					return true; 
				}
			}
			return false;
		}
		private static bool TryToFixDivMulSign(ref Fixed a,ref int b,ref long newFixedValue){
			if (newFixedValue == 0) {
				if ((a.fixedValue == 0)||(b == 0))
					return false;
				else {
					if ((a.fixedValue > 0) == (b > 0)) {
						newFixedValue = FixedConstants.MIN_POSITIVE_VALUE;
					} else {
						newFixedValue = FixedConstants.MAX_NEGATIVE_VALUE;
					}
					return true; 
				}
			}
			return false;
		}
	}
}
