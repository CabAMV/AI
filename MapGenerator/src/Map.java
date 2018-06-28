import java.util.ArrayList;
import java.util.Collections;
import java.util.PriorityQueue;

public abstract class Map {
	protected int nRow;
	protected int nCol;
	protected Cell[][] cells;
	
	private Cell outCell;
	private int order;
	
	public Map(int nRow, int nCol)
	{
		this.nRow=nRow;
		this.nCol=nCol;
	}
	
	
	public void SetOutCell(int nRow, int nCol)
	{
		outCell=cells[nRow][nCol];
		
	}

	
	public void GenerateMapDFS()
	{
		order=0;
		GenerateMapDFSRecursive(outCell);
	}
	
	public void GenerateMapDFSRecursive(Cell actual)
	{
		ArrayList<Cell> sucessors=new ArrayList<Cell>();
		actual.setOrderVisetedOncreation(order);

		sucessors=generateSuccesors(actual.walls);
		Collections.shuffle(sucessors);
		for(int i=0;i<sucessors.size();i++)	{	
			
			if(sucessors.get(i).getOrderVisetedOncreation()==-1)
			{
				OpenConexion(actual,sucessors.get(i));
				order++;
				GenerateMapDFSRecursive(sucessors.get(i));
			}
		}	
	}
	
	public void searchPath(int a,int b,int c,int d)
	{
		int count=0;
		Cell start=cells[a][b];
		Cell goal=cells[c][d];
		
		ArrayList<Cell> opened = new ArrayList<Cell>();
        ArrayList<Cell> closed = new ArrayList<Cell>();
        ArrayList<Cell> successors=new ArrayList<Cell>();
        Cell actual=null;

        opened.add(start);
        while (!opened.isEmpty())
        {              	
        	for(int i=0;i<opened.size();i++)
        	{
        		for(int j=0;j<opened.size();j++)
        		{
        			if(opened.get(i).getFcost()<opened.get(j).getFcost())
        			{
        				actual=opened.get(i);
        			}
        			else
        			{
        				actual=opened.get(j);
        			}
        		}
        	}
        	
        	opened.remove(actual);
        	closed.add(actual);
        	
	        if (actual==goal)
	        {
	        	backTrack(actual,start);
	        	return;
	        }
	        else
	        {
	        	successors=generatePathSuccesors(actual.walls,actual);
	        	for(int i=0;i<successors.size();i++)
	        	{
	        		count++;
	        		if(closed.contains(successors.get(i)))
	        			continue;
	        		if(!opened.contains(successors.get(i)))
	        		{
	        			calculateHcost(successors.get(i),goal);
	        			successors.get(i).setParent(actual);
	        			//successors.get(i).setPathindex(count);
	        			
	        			opened.add(successors.get(i));	        			
	        		}
	        		
	        	}
	        	
	        }
     
        }
	}
	
	public void backTrack(Cell goal, Cell start) 
	{
		
		
		ArrayList<Cell> list=new ArrayList<Cell>();
		while(goal!=start)
		{
			list.add(goal);
			goal=goal.getParent();			
		}
		start.setPathindex(0);
		int aux=1;
		for(int i=list.size()-1;i>=0;i--)
		{
			list.get(i).setPathindex(aux);
			aux++;
			
		}
		
		
	}
	protected abstract void SetWalls();
	
	public abstract void ShowInitMap();
	
	public abstract void ShowMap();
	
	public abstract void ShowSearchedMap();
	
	public ArrayList<Cell> generateSuccesors(ArrayList<Wall> walls)
	{
		ArrayList<Cell> aux=new ArrayList<Cell>();						
		for (int i=0;i<walls.size();i++)
		{	
			for (int j=0;j<nRow;j++)
			{	
				for(int k=0;k<nCol;k++)
				{	
					if(cells[j][k].GetX() == walls.get(i).NextCell().getX() && 
						cells[j][k].GetY() == walls.get(i).NextCell().getY())
					{	
						aux.add(cells[j][k]); 
					}
				}
			}
		}
		return aux;
		
	}
	
	public ArrayList<Cell> generatePathSuccesors(ArrayList<Wall> walls, Cell actual)
	{
		ArrayList<Cell> aux=new ArrayList<Cell>();						

		for (int i=0;i<walls.size();i++)
		{	
			if(walls.get(i).IsOpen()) //comprobamos si hay muro
			{
				for (int j=0;j<nRow;j++)
				{	
					for(int k=0;k<nCol;k++)
					{	
						if(cells[j][k].GetX() == walls.get(i).NextCell().getX() && 
							cells[j][k].GetY() == walls.get(i).NextCell().getY())
						{	
							aux.add(cells[j][k]);
						}
					}
				}			
				
			}
			
		}
		
		
		return aux;
	}
	
	public void calculateHcost(Cell evaluating, Cell goal)
	{
		int result=0;
		result = Math.abs( evaluating.GetX()-goal.GetX()) + Math.abs( evaluating.GetY()-goal.GetY());
		evaluating.setFcost(result);
		
	}
	
	protected abstract void OpenConexion(Cell A, Cell V);


	
}
