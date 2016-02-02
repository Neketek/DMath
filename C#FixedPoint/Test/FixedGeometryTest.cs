using NUnit.Framework;
using System.Collections.Generic;
using DGPE.Math.FixedPoint;
using DGPE.Math.FixedPoint.Geometry2D;
namespace CFixedPoint
{
	[TestFixture()]
	public class FixedGeometryTest
	{
		public static FixedVector2 GetFixedVector2(float x,float y){
			return new FixedVector2 ((Fixed)x, (Fixed)y);
		}
		public static FixedTriangle2D GetFixedTriangle(FixedVertex2D a,FixedVertex2D b,FixedVertex2D c){
			return new FixedTriangle2D (a,b,c);
		}
		[Test()]
		public void FixedTriangleTest ()
		{
			FixedVertex2D a = new FixedVertex2D ((Fixed)1.5,(Fixed)1);
			FixedVertex2D b = new FixedVertex2D (4,3);
			FixedVertex2D c = new FixedVertex2D (1,4);
			List<FixedTriangle2D> triangles = new List<FixedTriangle2D>();
			List<FixedVector2> vertexInTriangle = new List<FixedVector2>();
			List<FixedVector2> vertexNotInTriangle = new List<FixedVector2> ();
			triangles.Add (GetFixedTriangle(a,b,c));
			triangles.Add (GetFixedTriangle(b,c,a));
			triangles.Add (GetFixedTriangle(c,a,b));
			triangles.Add (GetFixedTriangle(a,c,b));
			vertexInTriangle.Add (GetFixedVector2 (1.88f, 2.24f));
			vertexInTriangle.Add (GetFixedVector2 (3.87f, 2.96f));
			vertexInTriangle.Add (GetFixedVector2 (2.69f, 2.95f));
			vertexInTriangle.Add (GetFixedVector2 (1.41f, 3.58f));
			vertexInTriangle.Add (GetFixedVector2 (1.44f, 1.89f));
			vertexInTriangle.Add (a.Coordinates);
			vertexInTriangle.Add (b.Coordinates);
			vertexInTriangle.Add (c.Coordinates);
			vertexNotInTriangle.Add (GetFixedVector2(1,0));
			vertexNotInTriangle.Add (GetFixedVector2(1,10));
			vertexNotInTriangle.Add (GetFixedVector2(32,11));
			vertexNotInTriangle.Add (GetFixedVector2(2.92f,1.69f));
			vertexNotInTriangle.Add (GetFixedVector2(2.67f,4.05f));
			vertexNotInTriangle.Add (GetFixedVector2(0.55f,3.94f));
			vertexNotInTriangle.Add (GetFixedVector2(0.73f,1.73f));
			foreach (FixedVector2 v in vertexInTriangle) {
				foreach (FixedTriangle2D t in triangles) {
					if (!t.IsInTriangle (v))
						throw new AssertionException ((string.Format ("V = {0} in T = {1}:{2}", v, t, t.IsInTriangle (v))));
				}
			}
			foreach (FixedVector2 v in vertexNotInTriangle) {
				foreach (FixedTriangle2D t in triangles) {
					if(t.IsInTriangle(v))
						throw new AssertionException ((string.Format("V = {0} in T = {1}:{2}",v,t,t.IsInTriangle(v))));
				}
			}
		}
	}
}

