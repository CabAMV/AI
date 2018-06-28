
public class PlainMountain extends Automaton {

	private int separatorPercentage;

	
	public PlainMountain(int X, int Y, int percentage) {
		super(X, Y);
		separatorPercentage=percentage;

	}

	/*Genera un mapa de ruido inicial a partir del cual aplicar el automata celular
	 * */
	@Override
	public void generateInitialMap() {  
		int num;
		for(int i=0;i<height;i++)
		{
			for(int j=0;j<widht;j++)
			{
				num=(int)(Math.random()*100);
				if(num <separatorPercentage)
				{
					cells[i][j]=" ";
				}
				else
				{
					cells[i][j]="x";
				}
				
			}
		}		
	}

	/*Aplica el automata al mapa que ha debido ser creado previamente
	 * 
	 * */
	@Override
	public void generateFinalMap() 
	{
		int aux[][]=new int[height][widht];
		for(int i=0;i<height;i++)
		{
			for(int j=0;j<widht;j++)
			{
				aux[i][j]=countNeighbours(i,j);
				
			}
		}
		
		
		
		for(int i=0;i<height;i++)
		{
			for(int j=0;j<widht;j++)
			{
				if(aux[i][j]<=3)				
					cells[i][j]=" ";
				else if(aux[i][j]>3)
					cells[i][j]="x";
				
			}
		}
			
	}
	
	public int countNeighbours(int x,int y)
	{
		int count=0;
		for(int i=x-1;i<=x+1;i++)
		{
			for(int j=y-1;j<=y+1;j++)
			{
				if(i>=0 && j>=0 && i<height && j<widht){
					if(i!=x || j!=y)
					{
						if(cells[i][j]=="x")
						{
							count++;
						}
						
					}
				}
			}
		}
		return count;
	}
	

}
