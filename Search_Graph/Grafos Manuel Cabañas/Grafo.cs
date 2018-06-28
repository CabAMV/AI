using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos_Manuel_Cabañas
{
    public class Grafo<Informacion>
    {
        CjtA<Informacion> Aristas;
        CjtV<Informacion> Vertices;

        public void BorrarArista(Informacion v1, Informacion v2)
        {
            Arista<Informacion> aux = new Arista<Informacion>(v1, v2);
            if (Aristas.Pertenece(aux))
            {
                Aristas.Borrar(aux);
            }

        }

        public  List<Informacion> CaminoMasCorto(Informacion origen, Informacion destino)
        {
            if (destino.Equals(origen)) {
                List<Informacion> path = new List<Informacion>();
                path.Add(origen);
                return path;
            }
                
            Arista<Informacion>[] Ar = Aristas.ObtenerAristas();
            Informacion[] Ver= Vertices.ObtenerVertices();
            Double[,] Pesos = new Double[Vertices.GetNumeroVertices(),Vertices.GetNumeroVertices()];
            Informacion[,] MatrizNodoIntermedio= new Informacion[Vertices.GetNumeroVertices(),Vertices.GetNumeroVertices()];


            for (int i = 0; i < Vertices.GetNumeroVertices(); i++)
                for (int j = 0; j < Vertices.GetNumeroVertices(); j++)
                    Pesos[i, j] = Double.PositiveInfinity;

            for (int k = 0; k < Aristas.GetNumeroAristas(); k++)
                for (int i = 0; i < Vertices.GetNumeroVertices(); i++)
                for (int j = 0; j < Vertices.GetNumeroVertices(); j++)
                {
                 

                    if (Ver[i].Equals(Ar[k].Origen)  && Ver[j].Equals(Ar[k].Destino))
                    {                           
                            Pesos[i, j] = Ar[k].Peso;                       
                    }

                    MatrizNodoIntermedio[i, j] =Ver[j];

                    if (Ver[i].ToString() == Ver[j].ToString())
                    {
                        MatrizNodoIntermedio[i, j] = default(Informacion);
                        Pesos[i,j] = 0;
                    }

                    }


          
           

            Double a=default(Double);
            for (int k = 0; k < Vertices.GetNumeroVertices(); k++)
                for (int i = 0; i < Vertices.GetNumeroVertices(); i++)
                    for (int j = 0; j < Vertices.GetNumeroVertices(); j++)
                    {
                        a = Pesos[i, k] + Pesos[k, j];
                        if (a < Pesos[i, j])
                        {
                            Pesos[i, j] = a;
                            MatrizNodoIntermedio[i, j] = Ver[k];
                            
                        }
                    }

            int or = default(int);
            int des = default(int);
            for (int i = 0; i < Vertices.GetNumeroVertices(); i++)
            {
                if (Ver[i].Equals(origen))
                    or = i;
                if (Ver[i].Equals(destino))
                    des = i;
            }


                List<Informacion> myPath = new List<Informacion>();

            
            Informacion desAux = destino;
            if (MatrizNodoIntermedio[or, des] != null)
            {
                bool finalizado=false;
                while (!finalizado)
                {
     
                    myPath.Add(MatrizNodoIntermedio[or, des]);
                    destino = MatrizNodoIntermedio[or, des];
                    
                    for (int i = 0; i < Vertices.GetNumeroVertices(); i++)
                    {
                        if (Ver[i].Equals(destino))
                            des = i;
                    }
                    for (int j = 0; j < Aristas.GetNumeroAristas(); j++) { 
                        if (origen.Equals(Ar[j].Origen) && destino.Equals(Ar[j].Destino) || Pesos[or,des]== Double.PositiveInfinity)
                            finalizado = true;

                        }
                    
                }
               
                    myPath.Reverse();
                
                    
            }

            myPath.Insert(0,origen);
            myPath.Add(desAux);
            if (Pesos[or, des] == Double.PositiveInfinity)
                myPath = null;
            return myPath;
            

        }

        public CjtA<Informacion> GetAristas()
        {
            return Aristas;
        }

        public CjtV<Informacion> GetVertices()
        {
            return Vertices;
        }

        public Grafo()
        {
            Aristas = new CjtA<Informacion>();
            Vertices = new CjtV<Informacion>();
        }

        public void InsertarArista(Informacion v1, Informacion v2)
        {
            Vertices.Insertar(v1);
            Vertices.Insertar(v2);    
            Aristas.Insertar(new Arista<Informacion>(v1, v2));
                
            
                
        }

        public void InsertarArista(Informacion v1, Informacion v2, double peso)
        {
            InsertarVertice(v1);
            InsertarVertice(v2);
            Aristas.Insertar(new Arista<Informacion>(v1, v2,peso));
        }

        public void InsertarVertice(Informacion vertice)
        {
            Vertices.Insertar(vertice);

        }

        public override string ToString()
        {
            
            
            return "Vertices: " + Vertices.ToString()+ "\n" +"Aristas: "+ Aristas.ToString();
        }




    }

}
