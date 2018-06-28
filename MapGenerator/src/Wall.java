
public class Wall {
	
	public static enum orientation_type{N,S,E,W};
	
	private Cell cell;
	
	private orientation_type orientation;
	
	private boolean open;
	
	public Wall (Cell cell,orientation_type orientation)
	{
		this.cell=cell;
		
		this.orientation=orientation;
		
		open=false;
		
	}
	public void Open()
	{
		open=true;
		
	}

	public boolean IsOpen()
	{
		return open;		
	}
	
	public orientation_type GetOrientation()
	{
		return orientation;		
	}
	
	public Coordinate NextCell()
	{
		Coordinate next=null;
		switch(orientation)
		{
			case N:
				next= new Coordinate(cell.GetX()-1,cell.GetY());
				break;
			case S:
				next= new Coordinate(cell.GetX()+1,cell.GetY());
				break;
			case W:
				next= new Coordinate(cell.GetX(),cell.GetY()-1);
				break;
			case E:
				next= new Coordinate(cell.GetX(),cell.GetY()+1);
				break;
		}
		return next;
		
	}
}
