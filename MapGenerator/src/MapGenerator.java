import java.util.Scanner;


public class MapGenerator {
	 static Scanner sc;
	public static void main (String[] args)
	{
	    sc = new Scanner(System.in);
	    int opc;
	    System.out.println("¿Que quiere hacer?");
	    System.out.println("[0] Mapa mazmorra");
	    System.out.println("[1] Mapa de mundo");
	    opc=sc.nextInt();
	    if(opc==0)
	    	generateSquareMap();
	    else if(opc==1)
	    {
		    System.out.println("¿Con mar o sin mar?");
		    System.out.println("[0] Sin mar");
		    System.out.println("[1] Con mar");
		    opc=sc.nextInt();
	    	automatonMap(opc);
	    }
	    	    
		else
		{
			System.out.println("Opcion incorrecta");
			System.exit(0);
			
		}

	}
	
	public static void generateSquareMap()
	{
		System.out.println("Numero de filas: ");
	    int rows = sc.nextInt();
		System.out.println("Numero de columnas: ");
	    int cols= sc.nextInt();
	    
	    System.out.print("Celda de inicio: ");
	    int x= sc.nextInt();
	    int y= sc.nextInt();

		Map map=new MapSquare(rows,cols);
		map.SetWalls();
		map.SetOutCell(x, y);
		map.ShowInitMap();
		System.out.println();
		map.GenerateMapDFS();
		System.out.println();
		map.ShowMap();
		
	    System.out.print("Celda de inicio: ");
	    int x1= sc.nextInt();
	    int y1= sc.nextInt();
	    System.out.print("Celda objetivo: "); 
	    int x2= sc.nextInt();
	    int y2= sc.nextInt();
		map.searchPath(x1,y1,x2,y2);
		map.ShowSearchedMap();
		System.out.println();

		
		
	}
	
	
	public static void automatonMap(int opc)
	{
		Automaton map=null;
		if(opc==0)
		{
			System.out.println("Introduzca columnas, filas, y porcentage de llanura deseado");
			int x=sc.nextInt();
			int y=sc.nextInt();
			int per=sc.nextInt();
			map=new PlainMountain(x,y,per);
		}
		else if(opc==1)
		{
			System.out.println("Introduzca columnas, filas,porcentage de mar y de llanura deseado (recomendado 20 20 50 20)");
			int x=sc.nextInt();
			int y=sc.nextInt();
			int per=sc.nextInt();
			int per2=sc.nextInt();
			map=new PlainSeaMountain(x,y,per,per2);
		}
		else
		{
			System.out.println("Opcion incorrecta");
			System.exit(0);
			
		}
		map.generateInitialMap();
		map.printMap();
		System.out.println();
		map.generateFinalMap();
		map.printMap();
	}
	
	
}
