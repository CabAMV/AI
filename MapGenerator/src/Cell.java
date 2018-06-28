import java.util.ArrayList;

public abstract class Cell {
	
	private String cellId;
	
	protected Coordinate coord;
	
	public int pathIndex;
	
	private int fCost;
	
	private Cell parent;

	protected int wallsCount;
	
	protected ArrayList<Wall> walls;
	
	protected int orderVisetedOncreation;
	
	public Cell(int col, int row)
	{
		coord=new Coordinate(col,row);
		cellId=col+"_"+row;
		
		wallsCount=0;		
		walls =new ArrayList<Wall>();
		
		orderVisetedOncreation=-1;

		fCost=Integer.MAX_VALUE;
		pathIndex=-1;
		
	}
	
	public Coordinate GetCoordinate()
	{
		return coord;
		
	}

	public int GetX()
	{
		return coord.getX();
		
	}
	
	public int GetY()
	{
		return coord.getY();
		
	}
	
	public int getOrderVisetedOncreation() {
		return orderVisetedOncreation;
	}

	public void setOrderVisetedOncreation(int orderVisetedOncreation) {
		this.orderVisetedOncreation = orderVisetedOncreation;
	}
	
	public String getCellId() {
		return cellId;
	}

	public void setCellId(String cellId) {
		this.cellId = cellId;
	}
	
	public String toString()
	{
		return cellId;		
	}
	
	public void OpenWall(Wall.orientation_type orientation)
	{
		boolean found=false;
		int i=0;
		
		while(!found && i<wallsCount)
		{
			if(walls.get(i).GetOrientation() == orientation)
			{
				walls.get(i).Open();
				found =true;
			}
			
			else
				i++;
		
			
		}
		
	}
	public void SetNeighbours(ArrayList<Wall> walls)
	{
		wallsCount=walls.size();

		this.walls=walls;		
	}
	
	public void setParent(Cell parent)
	{
		this.parent=parent;
	}
	public Cell getParent()
	{
		return parent;
	}
	
	public void setFcost(int fCost)
	{
		this.fCost=fCost;
	}
	
	public int getFcost()
	{
		return fCost;
	}
	
	public void setPathindex(int value)
	{
		pathIndex=value;
	}
	public int getPathindex()
	{
		return pathIndex;
	}

	public abstract String GetNorthWall();
	public abstract String GetSouthWall();
	public abstract String GetWestWall();
	public abstract String GetEastWall();
	
}
