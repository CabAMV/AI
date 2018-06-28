import java.util.ArrayList;

public class MapSquare extends Map 
{
	public MapSquare(int nRow,int nCol)
	{
		super(nRow,nCol);
		
		cells=new CellSquare[nRow][nCol];
		for(int i=0;i<nRow;i++)
		{
			for(int j=0;j<nCol;j++)
			{
				cells[i][j]=new CellSquare(i,j);			
			}
			
		}
			
	}

	@Override
	protected void SetWalls() 

	{

		for(int i=0;i<nRow;i++)
		{
			for(int j=0;j<nCol;j++)
			{
				ArrayList<Wall> wallList =new ArrayList<Wall>();
								
				if(i==0) //Primera fila
				{
					if(j==0)//Primera Columna
					{						
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
						
					}
					else if(j==nCol-1)//ultima columna
					{
						
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
					}
					else // fila superior , 3 vecinos 
					{
						
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
					}
				}
				
				else if(i== nRow-1)//ultimafila
				{
					if(j==0)//Primera Columna
					{
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
						
					}
					else if(j==nCol-1)//ultima columna
					{
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
					}
					else // fila inferior , 3 vecinos 
					{
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
						wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
						
					}				
				}
				else if(j==0)//Primera columna
				{
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
					
				}
				
				else if(j==nCol-1)//ultima columna
				{
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
				}
				else // caso 4 paredes por celda
				{
					
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.N));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.S));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.E));
					wallList.add(new Wall(cells[i][j],Wall.orientation_type.W));
					
				}
				cells[i][j].SetNeighbours(wallList);
				
			}
			
		}
		
		
	}
	
	public void ShowInitMap()
	{

		for (int row =0;row<nRow;row++)
		{
			for (int col =0;col<nCol;col++)//primerafila
			{
				System.out.print("+" + cells[row][col].GetNorthWall()+ "+");
			}
			System.out.println();
			for (int col =0;col<nCol;col++)//relleno celda
			{
				System.out.print(cells[row][col].GetWestWall() + "  "+" " + cells[row][col].GetEastWall());
			}
			System.out.println();

			
			for (int col =0;col<nCol;col++)//paredsur
			{
				System.out.print("+" + cells[row][col].GetSouthWall() + "+");
			}
			System.out.println();
		}
		
		
	}
	
	public void ShowMap()
	{

		for (int row =0;row<nRow;row++)
		{
			for (int col =0;col<nCol;col++)//primerafila
			{
				System.out.print("+" + cells[row][col].GetNorthWall() + "+");
			}
			System.out.println();
			for (int col =0;col<nCol;col++)//relleno celda
			{
				if(cells[row][col].getOrderVisetedOncreation()<10)
					System.out.print(cells[row][col].GetWestWall()+"  " +cells[row][col].getOrderVisetedOncreation() + cells[row][col].GetEastWall());
				else
					System.out.print(cells[row][col].GetWestWall()+" " +cells[row][col].getOrderVisetedOncreation() + cells[row][col].GetEastWall());
			}
			System.out.println();

			
			for (int col =0;col<nCol;col++)//paredsur
			{
				System.out.print("+" + cells[row][col].GetSouthWall() + "+");
			}
			System.out.println();
		}
		
		
	}
		
	
	
	@Override
	public void ShowSearchedMap() {
		for (int row =0;row<nRow;row++)
		{
			for (int col =0;col<nCol;col++)//primerafila
			{
				System.out.print("+ " + cells[row][col].GetNorthWall() + " +");
			}
			System.out.println();
			for (int col =0;col<nCol;col++)//relleno celda
			{
				if(cells[row][col].getPathindex()>-1){
					if(cells[row][col].getPathindex()<10)
						System.out.print(cells[row][col].GetWestWall()+"    " +cells[row][col].getPathindex() + cells[row][col].GetEastWall());
					else
						System.out.print(cells[row][col].GetWestWall()+"   " +cells[row][col].getPathindex() + cells[row][col].GetEastWall());
				
				}
				else
					System.out.print(cells[row][col].GetWestWall()+"     " + cells[row][col].GetEastWall());
				/*if(cells[row][col].getOrderVisetedOncreation()<10 && cells[row][col].getPathindex()<10)
					System.out.print(cells[row][col].GetWestWall() +cells[row][col].getPathindex()+" " +cells[row][col].getOrderVisetedOncreation() + cells[row][col].GetEastWall());
				else if ((cells[row][col].getOrderVisetedOncreation()>10 && cells[row][col].getPathindex()<10)||(cells[row][col].getOrderVisetedOncreation()<10 && cells[row][col].getPathindex()>10) )
					System.out.print(cells[row][col].GetWestWall() +cells[row][col].getPathindex()+" " +cells[row][col].getOrderVisetedOncreation() + cells[row][col].GetEastWall());
				*/
			}
			System.out.println();

			
			for (int col =0;col<nCol;col++)//paredsur
			{
				System.out.print("+ " + cells[row][col].GetSouthWall() + " +");
			}
			System.out.println();
		}		
	}
	
	
	protected void OpenConexion(Cell A, Cell V)
	{
		if(A.GetX()==V.GetX() && A.GetY() < V.GetY())
		{
			A.OpenWall(Wall.orientation_type.E);
			V.OpenWall(Wall.orientation_type.W);
		}
		
		if(A.GetX()==V.GetX() && A.GetY() > V.GetY())
		{
			A.OpenWall(Wall.orientation_type.W);
			V.OpenWall(Wall.orientation_type.E);
		}
		
		if(A.GetY()==V.GetY() && A.GetX() < V.GetX())
		{
			A.OpenWall(Wall.orientation_type.S);
			V.OpenWall(Wall.orientation_type.N);
		}
		
		if(A.GetY()==V.GetY() && A.GetX() > V.GetX())
		{
			A.OpenWall(Wall.orientation_type.N);
			V.OpenWall(Wall.orientation_type.S);
		}
		
	}

}



