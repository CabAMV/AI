using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos_Manuel_Cabañas
{
    class Program
    {
        static void Main(string[] args)
        {
            Grafo<string> grafo = new Grafo<string>();
            CrearAristas(grafo);

            Console.WriteLine(grafo.ToString());
            Console.WriteLine();
            Console.WriteLine("Camino valido entre dos nodos (Anoth ,Chandrila):");
                List<string> camino = grafo.CaminoMasCorto("Anoth", "Chandrila");
                mostrarEnPantalla(camino);

            Console.WriteLine();
            Console.WriteLine("Camino de un nodo a si mismo(Anoth):");
                camino= grafo.CaminoMasCorto("Anoth", "Anoth");
                mostrarEnPantalla(camino);

            Console.WriteLine();
            Console.WriteLine("Camino no valido (Chandrila, Anoth):");
                camino = grafo.CaminoMasCorto("Chandrila", "Anoth");
                mostrarEnPantalla(camino);
            



        }

        static void CrearAristas(Grafo<string> grafo)
        {
            grafo.InsertarArista("Anoth", "Drall", 3);
            grafo.InsertarArista("Drall", "Cerea", 8);
            grafo.InsertarArista("Cerea", "Chandrila", 8);
            grafo.InsertarArista("Bespin","Chandrila", 2);
            grafo.InsertarArista("Arkania","Bespin", 6);
            grafo.InsertarArista("Bespin", "Drall", 3);
            grafo.InsertarArista("Arkania", "Drall", 1);
            grafo.InsertarArista("Anoth", "Arkania", 7);
            grafo.InsertarArista("Cerea", "Bespin", 2);
          
        }
        static void mostrarEnPantalla(List<string> camino)
        {
            Console.WriteLine();
            if(camino==null )
                Console.WriteLine("No hay camino");
            else
            for (int i = 0; i < camino.Count; i++)
                Console.Write("-"+camino[i].ToString() +"-");
            
            Console.WriteLine();
        }

    }
}
