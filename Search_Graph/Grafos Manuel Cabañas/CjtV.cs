using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos_Manuel_Cabañas
{
    public class CjtV<Informacion>
    {
        private Nodo<Informacion> conjunto;


        public void Borrar(Informacion e)
        {
            Nodo<Informacion> aux = null;
            Nodo<Informacion> nodo=conjunto;

            if (!EsVacio())
            {
                if (e.Equals(conjunto.darDato()))
                {
                    conjunto = conjunto.darSiguiente();

                }
                while (nodo.darSiguiente() != null && !e.Equals(nodo.darDato()))
                {


                    aux = nodo;
                    nodo = nodo.darSiguiente();
                }
                if (nodo.darSiguiente() != null)
                {
                    aux.fijarSiguiente(nodo.darSiguiente());

                }

                /* if (nodo.darSiguiente() == null && e.Equals(nodo.darDato()))
                     aux.fijarSiguiente(null);*/

            }
        }

        public CjtV()
        {
            conjunto = null; 

        }

        public CjtV(CjtV<Informacion> otroConjunto)
        {
            this.conjunto = otroConjunto.conjunto;
        }

        

        public bool EsVacio()
        {
            return conjunto == null;
            
        }

        public int GetNumeroVertices()
        {
            Nodo<Informacion> nodo = conjunto;
            int count = 0;
            while (nodo != null && !EsVacio())
            {
                if (nodo != null) {
                    count++;
                    nodo = nodo.darSiguiente();
                }

            }
            

            return count;
        }

        public void Insertar(Informacion e)
        {
            
           
            if (e != null)
            {
                Nodo<Informacion> nodo = new Nodo<Informacion>(e);
                if (EsVacio())
                {
                    conjunto = nodo;
                }

                if (!Pertenece(nodo.darDato()))
                {
                    nodo.fijarSiguiente(conjunto);
                    conjunto = nodo;
                }
            }
        }

        public Informacion[] ObtenerVertices()
        {
            Informacion  [] array  = new Informacion[GetNumeroVertices()];
            Nodo<Informacion> nodo = conjunto;
            for (int i=0; i<GetNumeroVertices() ;i++)
            {
                array[i] = nodo.darDato();
                nodo = nodo.darSiguiente();
            }
            return array;

        }

        public bool Pertenece(Informacion e)
        {

            Nodo<Informacion> nodo = conjunto;
            if (!EsVacio() && e != null)
            {
                while (nodo != null && !EsVacio())
                {
                    if (e.Equals(nodo.darDato()))
                    {
                        return true;
                    }
                    nodo = nodo.darSiguiente();


                }
            }     
            return false;
        }

        public override string ToString()
        {
            string devolucion;
            devolucion = null;
            Nodo<Informacion> nodo = conjunto;

            if (!EsVacio())
                devolucion = "{"+ Convert.ToString(conjunto.darDato());

            while (nodo != null )
            {
                devolucion = devolucion + ", " + Convert.ToString(nodo.darDato());
                nodo = nodo.darSiguiente();


            }



            return devolucion+"}";
        }
    }
}
