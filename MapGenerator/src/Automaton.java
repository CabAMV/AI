
public abstract class Automaton {

	protected String cells[][];
	
	protected int widht;
	protected int height;
	
	
	public Automaton(int X,int Y)
	{
		widht=X;
		height=Y;
		cells=new String[height][widht];
	}
	
	
	public abstract void generateInitialMap();
	
	public abstract void generateFinalMap();
	
	public abstract int countNeighbours(int x,int y);
	
	public void printMap()
	{
		for(int i=0;i<height;i++)
		{
			for(int j=0;j<widht;j++)
			{
				System.out.print(cells[i][j]+" ");
			}
			System.out.println();
		}
		
	}

}
