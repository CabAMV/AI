
public class CellSquare extends Cell {
	
	public CellSquare (int col, int row)
	{
		super (col,row);
		
	}

	@Override
	public String GetNorthWall() {

		String aux = "---";
		
		boolean found =false;
		int i=0;
		while(!found && i<wallsCount)
		{
			if(walls.get(i).GetOrientation()==Wall.orientation_type.N && walls.get(i).IsOpen())
			{
				aux="   ";
				found=true;				
			}
			
			else i++;
		}
		
		return aux;
	}

	@Override
	public String GetSouthWall() {

		String aux = "---";
		
		boolean found =false;
		int i=0;
		while(!found && i<wallsCount)
		{
			if(walls.get(i).GetOrientation()==Wall.orientation_type.S && walls.get(i).IsOpen())
			{
				aux="   ";
				found=true;				
			}
			
			else i++;
		}
		
		return aux;
	}

	@Override
	public String GetWestWall() {

		String aux = "|";
		
		boolean found =false;
		int i=0;
		while(!found && i<wallsCount)
		{
			if(walls.get(i).GetOrientation()==Wall.orientation_type.W && walls.get(i).IsOpen())
			{
				aux=" ";
				found=true;				
			}
			
			else i++;
		}
		
		return aux;
	}

	@Override
	public String GetEastWall() {

		String aux = "|";
		
		boolean found =false;
		int i=0;
		while(!found && i<wallsCount)
		{
			if(walls.get(i).GetOrientation()==Wall.orientation_type.E && walls.get(i).IsOpen())
			{
				aux=" ";
				found=true;				
			}
			
			else i++;
		}
		
		return aux;
	}

}
