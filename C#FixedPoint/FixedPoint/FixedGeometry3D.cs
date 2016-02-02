using System.Collections.Generic;
using DGPE.Math.FixedPoint.Geometry2D;


namespace DGPE.Math.FixedPoint.Geometry3D{
	public class FixedVertex3D{
		public FixedVector3 coordinates;
		public FixedVertex3D(FixedVector3 coordinates){
			this.coordinates = coordinates;
		}
		public FixedVertex3D(Fixed x,Fixed y,Fixed z){
			this.coordinates = new FixedVector3 (x, y, z);
		}
		public FixedVertex3D(int x,int y,int z){
			this.coordinates = new FixedVector3 (x, y, z);
		}
		public override string ToString ()
		{
			return string.Format (coordinates.ToString());
		}
	}
	public class IndexedFixedVertex3D:FixedVertex3D{
		public int index;
		public IndexedFixedVertex3D(Fixed x,Fixed y,Fixed z,int index):base(x,y,z){
			this.index = index;
		}
	}
	public class FixedTriangle3D{
		private FixedVertex3D a,b,c;
		private Fixed mX, mZ, mC;
		public FixedTriangle3D(FixedVertex3D a,FixedVertex3D b,FixedVertex3D c){
			A = a;
			B = b;
			C = c;
		}
		public void RecalculateSurfaceEquation(){
			FixedVector3 v = b.coordinates - a.coordinates;
			FixedVector3 u = c.coordinates - a.coordinates;
			Fixed detY = v.x * u.z - v.z * u.x;
			this.mX = (v.y * u.z - v.z * u.y) / detY;
			this.mZ = (v.x * u.y - v.y * u.x) / detY;
			this.mC = a.coordinates.y - a.coordinates.x * mX - a.coordinates.z * mZ;
		}
		public Fixed GetY(Fixed x,Fixed z){
			return x*mX+z*mZ+mC;
		}
		public FixedVertex3D A {
			get {
				return this.a;
			}
			set {
				CheckArgumentNullException (value);
				a = value;
			}
		}

		public FixedVertex3D B {
			get {
				return this.b;
			}
			set {
				CheckArgumentNullException (value);
				b = value;
			}
		}

		public FixedVertex3D C {
			get {
				return this.c;
			}
			set {
				CheckArgumentNullException (value);
				c = value;
			}
		}

		public override string ToString ()
		{
			return string.Format ("[FixedTriangle3D: a={0}, b={1}, c={2}, y = ({3})*x+({4})*z +({5})]", a, b, c, mX, mZ, mC);
		}
		
		private void CheckArgumentNullException(FixedVertex3D v){
			if (v == null)
				throw new System.ArgumentNullException ();
		}
	}
	public class FixedGridXZ3D{
		public static readonly Fixed OUT_OF_GRID_Y_VALUE = (Fixed)(-1);
		private const  int BOT_LEF = 0;
		private const  int BOT_RIG = 1;
		private const  int TOP_LEF = 2;
		private const  int TOP_RIG = 3;
		private class CellNode{
			public readonly FixedTriangle3D bl_tr_br;
			public readonly FixedTriangle3D bl_tr_tl;
			public readonly Fixed delimLineOffsetX;
			public readonly Fixed delimLineOffsetZ;
			public CellNode(FixedVertex3D bl,FixedVertex3D br,FixedVertex3D tl,FixedVertex3D tr,Fixed offX,Fixed offZ){
				this.bl_tr_br = new FixedTriangle3D(bl,tr,br);
				this.bl_tr_tl = new FixedTriangle3D(bl,tr,tl);
				this.delimLineOffsetX = offX;
				this.delimLineOffsetZ = offZ;
			}
			//TODO:check Get Y 
			public Fixed GetYOf(Fixed x,Fixed z){
				if ((z-delimLineOffsetZ) > (x-delimLineOffsetX))
					return bl_tr_tl.GetY (x, z);
				else
					return bl_tr_br.GetY (x, z);
			}
			public void RecalculateSurfaceEquations(){
				bl_tr_br.RecalculateSurfaceEquation ();
				bl_tr_tl.RecalculateSurfaceEquation ();
			}
		}
		private List<IndexedFixedVertex3D> vertices = null;
		private CellNode[,] cells = null;
		public readonly Fixed cellSize;
		public readonly int width,height;
		public readonly Fixed gridZDimensionMax, gridXDimensionMax;
		public FixedGridXZ3D(int gWidth,int gHeight,Fixed cSize){
			if (gHeight <= 0 || gWidth <= 0 || cSize.IsNegativeOrZero ())
				throw new System.ArgumentOutOfRangeException ("gHeight <= 0 || gWidth <= 0 || cSize.IsNegativeOrZero ()");
			this.cellSize = cSize;
			this.width = gWidth;
			this.height = gHeight;
			this.gridXDimensionMax = cSize * gWidth;
			this.gridZDimensionMax = cSize * gHeight;
			this.CreateIndexedVerticesList ();
			this.CreateCells ();
		}
		public void PutCellTriangleIndexesToArray(int beginPosition,int[]triangleArray,int x,int z){
			CellNode cell = cells [z, x];
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_br.A).index;
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_br.B).index;
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_br.C).index;
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_tl.A).index;
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_tl.B).index;
			triangleArray [beginPosition++] = ((IndexedFixedVertex3D)cell.bl_tr_tl.C).index;
		}
		public Fixed GetYFromCellSafe(Fixed x,Fixed z){
			if (x.IsNegativeOrZero () || z.IsNegativeOrZero () || x > gridXDimensionMax || z > gridZDimensionMax)
				return OUT_OF_GRID_Y_VALUE;
			int cellX = (int)(x / cellSize);
			int cellZ = (int)(z / cellSize);
			return cells [cellZ,cellX].GetYOf (x,z);
		}
		public Fixed GetYFromCellUnsafe(Fixed x,Fixed z){
			int cellX = (int)(x / cellSize);
			int cellZ = (int)(z / cellSize);
			return cells [cellZ,cellX].GetYOf (x,z);
		}
		public Fixed GetYOfVertex(int x,int z){
			return GetIndexedVetexFrom (x,z).coordinates.y;
		}
		public void SetYOfVertex(int x,int z,Fixed y){
			this.GetIndexedVetexFrom (x, z).coordinates.y = y; 
		}
		public void RecalculateSurfaceEquations(){
			for (int z = 0; z<height; z++)
				for (int x = 0; x<width; x++)
					cells [z, x].RecalculateSurfaceEquations ();
		}
		private IndexedFixedVertex3D GetIndexedVetexFrom(int x,int z){
			return vertices [(width+1) * z + x];
		}
		private void CreateIndexedVerticesList(){
			vertices = new List<IndexedFixedVertex3D> ();
			for (int z = 0,index = 0; z<=height; z++) {
				for (int x = 0; x<=width; x++) {
					vertices.Add (new IndexedFixedVertex3D (cellSize*x,FixedConstants.FIXED_ZERO,cellSize*z,index));
					index++;
				}
			}
		}
		private void CreateCells(){
			this.cells = new CellNode[height,width];
			for (int z = 0; z<height; z++)
				for (int x = 0; x<width; x++) {
					cells [z,x] = new CellNode(
					GetVertexForCell(x,z,BOT_LEF),
					GetVertexForCell(x,z,BOT_RIG),
					GetVertexForCell(x,z,TOP_LEF),
					GetVertexForCell(x,z,TOP_RIG),
					cellSize*x,cellSize*z);
				}
		}
		private FixedVertex3D GetVertexForCell(int x,int z,int inCellPosition){
			switch (inCellPosition) {
			case BOT_LEF:
				return GetIndexedVetexFrom(x,z);
			case BOT_RIG:
				return GetIndexedVetexFrom(x+1,z);
			case TOP_LEF:
				return GetIndexedVetexFrom(x,z+1);
			case TOP_RIG:
				return GetIndexedVetexFrom(x+1,z+1);
			}
			throw new System.ArgumentOutOfRangeException ("InCellPosition");
		}
	}
}
