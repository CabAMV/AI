
public class PlainSeaMountain extends Automaton {

	private int seaPercentage;
	private int plainPercentage;
	
	public PlainSeaMountain(int X, int Y,int seaPercentage, int plainPercentage) {
		super(X, Y);
		this.seaPercentage=seaPercentage;
		this.plainPercentage=plainPercentage;
	}

	@Override
	public void generateInitialMap() {
		int num;
		for(int i=0;i<height;i++)
		{
			for(int j=0;j<widht;j++)
			{
				num=(int)(Math.random()*100);
				if(num <seaPercentage)
				{
					cells[i][j]="~";
				}
				else if(num >=seaPercentage && num<seaPercentage+plainPercentage)
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

	@Override
	public void generateFinalMap() {
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
				if(aux[i][j]==0)				
					cells[i][j]=" ";
				else if(aux[i][j]==1)
					cells[i][j]="x";
				else if(aux[i][j]==2)
					cells[i][j]="~";
				
			}
		}
		
	}

	@Override
	public int countNeighbours(int x, int y) {
		int count=0;
		int count2=0;
		int count3=0;
		for(int i=x-1;i<=x+1;i++)
		{
			for(int j=y-1;j<=y+1;j++)
			{
				if(i>=0 && j>=0 && i<height && j<widht){
					if(i!=x || j!=y)
					{
						if(cells[i][j]==" ")
						{
							count++;
						}
						if(cells[i][j]=="~")
						{
							count2++;							
						}
						if(cells[i][j]=="x")
						{
							count3++;
						}
						
					}
				}
			}
		}
		if(count>count2 && count>count3)
		{
			return 0;
		}
		else if(count3>count && count3>count2)
		{
			return 1;
		}
		else if(count2>count && count2 >count3)
		{
			return 2;
		}
		else
		{
			return 4;
		}
		
	}

}
