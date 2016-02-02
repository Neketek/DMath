using System;
using System.Collections.Generic;
using DGPE.Math.FixedPoint;
using DGPE.Math.FixedPoint.Geometry2D;
using DGPE.Math.FixedPoint.Geometry3D;
namespace CFixedPoint
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			FixedVertex3D a = new FixedVertex3D (1,-2,0);
			FixedVertex3D b = new FixedVertex3D (2,0,-1);
			FixedVertex3D c = new FixedVertex3D (0,-1,2);
			FixedTriangle3D tria = new FixedTriangle3D (a,b,c);
			tria.RecalculateSurfaceEquation ();
			Console.WriteLine (tria);
			FixedGridXZ3D grid = new FixedGridXZ3D (10,10, (Fixed)1);
		}
	}
}
