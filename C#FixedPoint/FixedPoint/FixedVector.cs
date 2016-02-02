namespace DGPE.Math.FixedPoint{
	public struct FixedVector2{
		public static FixedVector2 CreateZeroVector(){
			return new FixedVector2 (FixedConstants.FIXED_ZERO, FixedConstants.FIXED_ZERO);
		}
		// operators 
		public static FixedVector2 operator+(FixedVector2 v1,FixedVector2 v2){
			return new FixedVector2(v1.x +v2.x,v1.y+v2.y);
		}
		public static FixedVector2 operator-(FixedVector2 v1,FixedVector2 v2){
			return new FixedVector2(v1.x-v2.x,v1.y-v2.y);
		}
		public static FixedVector2 operator*(FixedVector2 v,Fixed scale){
			return new FixedVector2(v.x*scale,v.y*scale);
		}
		public static FixedVector2 operator/(FixedVector2 v,Fixed scale){
			return new FixedVector2(v.x/scale,v.y/scale);
		}
		public static FixedVector2 operator-(FixedVector2 v){
			return new FixedVector2 (-v.x,-v.y);
		}
		public static bool operator==(FixedVector2 v1,FixedVector2 v2){
			return v1.x == v2.x && v1.y == v2.y;
		}
		public static bool operator!=(FixedVector2 v1,FixedVector2 v2){
			return !(v1 == v2);
		}
		//remember! this shit is using in the Fixed Geometry class 
		public static Fixed PseudoscalarMultiplication(FixedVector2 v1,FixedVector2 v2){
			return v1.x * v2.y - v1.y * v2.x;
		}
		public static Fixed ScalarMultiplication(FixedVector2 v1,FixedVector2 v2){
			return v1.x*v2.x + v1.y*v2.y;
		}
		//TODO:add cast to UnityVectors
		public Fixed x;
		public Fixed y;
		public FixedVector2(Fixed x,Fixed y){
			this.x = x;
			this.y = y;
		}
		public FixedVector2 RotateZAxe(int deg,FixedVector2 pivotPoint){
			return RotateZAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg),pivotPoint);
		}
		public FixedVector2 RotateZAxe(Fixed sin,Fixed cos,FixedVector2 pivotPoint){
			Fixed nx = (x - pivotPoint.x) * cos - (y - pivotPoint.y) * sin+pivotPoint.x;
			Fixed ny = (x - pivotPoint.x) * sin + (y - pivotPoint.y) * cos+pivotPoint.y;
			x = nx;
			y = ny;
			return this;
		}
		public FixedVector2 RotateGlobalZAxe(int deg){
			return RotateGlobalZAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg));
		}
		public FixedVector2 RotateGlobalZAxe(Fixed sin,Fixed cos){
			Fixed nx = x*cos-y*sin;
			Fixed ny = x*sin+y*cos;
			x = nx;
			y = ny;
			return this;
		}
		public Fixed CalculateLength(){
			return Fixed.Sqrt(CalculateSquareLength());
		}
		public Fixed CalculateSquareLength(){
			return Fixed.Square(x)+Fixed.Square(y);
		}
		public Fixed CalculateSquareDistanceTo(FixedVector2 v2){
			return (this-v2).CalculateSquareLength();
		}
		public Fixed CalculateDistanceTo(FixedVector2 v2){
			return Fixed.Sqrt(CalculateSquareDistanceTo(v2));
		}
		public override string ToString ()
		{
			return string.Format ("({0},{1})", x, y);
		}
		public override bool Equals (object obj)
		{
			return obj != null && obj.GetType () == this.GetType () && this == (FixedVector2)obj;
		}
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

	}		
	public struct FixedVector3{
		//operators
		//FixedVector3 and FixedVector3
		public static FixedVector3 operator -(FixedVector3 v){
			return new FixedVector3 (-v.x,-v.y,-v.z);
		}
		public static FixedVector3 operator +(FixedVector3 v1,FixedVector3 v2){
			return new FixedVector3(v1.x+v2.x,v1.y+v2.y,v1.z+v2.z);
		}
		public static FixedVector3 operator -(FixedVector3 v1,FixedVector3 v2){
			return new FixedVector3(v1.x-v2.x,v1.y-v2.y,v1.z-v2.z);
		}
		//FixedVector3 and Fixed
		public static FixedVector3 operator *(FixedVector3 v1,Fixed scale){
			return new FixedVector3(v1.x*scale,v1.y*scale,v1.z*scale);
		}
		public static FixedVector3 operator /(FixedVector3 v1,Fixed scale){
			return new FixedVector3(v1.x/scale,v1.y/scale,v1.z/scale);
		}
		//FixedVector3 and FixedVector2
		public static FixedVector3 operator +(FixedVector3 v1,FixedVector2 v2){
			return new FixedVector3(v1.x+v2.x,v1.y+v2.y,v1.z);
		}
		public static FixedVector3 operator -(FixedVector3 v1,FixedVector2 v2){
			return new FixedVector3(v1.x-v2.x,v1.y-v2.y,v1.z);
		}
		public static bool operator==(FixedVector3 v1,FixedVector3 v2){
			return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
		}
		public static bool operator!=(FixedVector3 v1,FixedVector3 v2){
			return !(v1 == v2);
		}
		//TODO: add cast to Unity Vectors
		//TODO: add rotation code
		public FixedVector3 RotateGlobalZAxe(int deg){
			return RotateGlobalZAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg));
		}
		public FixedVector3 RotateGlobalXAxe(int deg){
			return RotateGlobalXAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg));
		}
		public FixedVector3 RotateGlobalYAxe(int deg){
			return RotateGlobalYAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg));
		}
		public FixedVector3 RotateZAxe(int deg,FixedVector3 pivotPoint){
			return RotateZAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg),pivotPoint);
		}
		public FixedVector3 RotateYAxe(int deg,FixedVector3 pivotPoint){
			return RotateYAxe (FixedTrig.Sin(deg),FixedTrig.Cos(deg),pivotPoint);
		}
		public FixedVector3 RotateXAxe(int deg,FixedVector3 pivotPoint){
			return RotateXAxe (FixedTrig.Sin (deg), FixedTrig.Cos (deg), pivotPoint);
		}
		public FixedVector3 RotateZAxe(Fixed sin,Fixed cos,FixedVector3 pivotPoint){
			Fixed nx = (x-pivotPoint.x)*cos-(y-pivotPoint.y)*sin+pivotPoint.x;
			Fixed ny = (x-pivotPoint.x)*sin+(y-pivotPoint.y)*cos+pivotPoint.y;
			x = nx;
			y = ny;
			return this;
		}
		public FixedVector3 RotateXAxe(Fixed sin,Fixed cos,FixedVector3 pivotPoint){
			Fixed ny = (y-pivotPoint.y) * cos - (z-pivotPoint.z) * sin + pivotPoint.y;
			Fixed nz = (y-pivotPoint.y) * sin + (z-pivotPoint.z) * cos + pivotPoint.z;
			y = ny;
			z = nz;
			return this;
		}
		public FixedVector3 RotateYAxe(Fixed sin,Fixed cos,FixedVector3 pivotPoint){
			Fixed nx = (x-pivotPoint.x) * cos + (z-pivotPoint.z) * sin+pivotPoint.x;
			Fixed nz = (z-pivotPoint.z) * cos - (x-pivotPoint.x) * sin+pivotPoint.z;
			x = nx;
			z = nz;
			return this;
		}
		public FixedVector3 RotateGlobalZAxe(Fixed sin,Fixed cos){
			Fixed nx = x*cos-y*sin;
			Fixed ny = x*sin+y*cos;
			x = nx;
			y = ny;
			return this;
		}
		public FixedVector3 RotateGlobalXAxe(Fixed sin,Fixed cos){
			Fixed ny = y * cos - z * sin;
			Fixed nz = y * sin + z * cos;
			y = ny;
			z = nz;
			return this;
		}
		public FixedVector3 RotateGlobalYAxe(Fixed sin,Fixed cos){
			Fixed nx = x * cos + z * sin;
			Fixed nz = z * cos - x * sin;
			x = nx;
			z = nz;
			return this;
		}
		public Fixed x,y,z;
		public FixedVector3(Fixed x,Fixed y,Fixed z){
			this.x = x;
			this.y = y;
			this.z = z;
		}
		public FixedVector3(int x,int y,int z){
			this.x = (Fixed)x;
			this.y = (Fixed)y;
			this.z = (Fixed)z;
		}
		public Fixed CalculateSquareLength(){
			return Fixed.Square(x)+Fixed.Square(y)+Fixed.Square(z);
		}
		public Fixed CalculateLength(){
			return Fixed.Sqrt(CalculateSquareLength());
		}
		public Fixed CalculateSquareDistanceTo(FixedVector3 v2){
			return (this-v2).CalculateSquareLength();
		}
		public Fixed CalculateDistanceTo(FixedVector3 v2){
			return Fixed.Sqrt(CalculateSquareDistanceTo(v2));
		}
		public override string ToString ()
		{
			return string.Format ("({0},{1},{2})", x, y,z);
		}
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(FixedVector3))
				return false;
			FixedVector3 other = (FixedVector3)obj;
			return x == other.x && y == other.y && z == other.z;
		}
		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}
	}
}

