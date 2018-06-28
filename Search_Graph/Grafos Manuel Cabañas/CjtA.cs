using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos_Manuel_Cabañas
{
    public class CjtA<Informacion>
    {
        private Nodo<Arista<Informacion>> conjunto;


        public void Borrar(Arista<Informacion> e)
        {
            Nodo<Arista<Informacion>> aux = null;
            Nodo<Arista<Informacion>> nodo = conjunto;
            while (nodo.darSiguiente() != null)
            {

                if (e.Equals(nodo.darDato()))
                {
                    aux.fijarSiguiente(nodo.darSiguiente());

                }
                aux = nodo;
                nodo = nodo.darSiguiente();
            }
            if (nodo.darSiguiente() == null && e.Equals(nodo.darDato()))
                aux.fijarSiguiente(null);
        }

        public CjtA()
        {
            conjunto = null;
        }

        public CjtA(CjtA<Informacion> otroConjunto)
        {
            this.conjunto = otroConjunto.conjunto;
        }

        public bool EsVacio()
        {
            return conjunto == null;
            
        }

        public int GetNumeroAristas()
        {
            Nodo<Arista<Informacion>> nodo = conjunto;
            int count = 0;
            while (nodo != null && !EsVacio())
            {
                if (nodo != null)
                {
                    count++;
                    nodo = nodo.darSiguiente();
                }

            }
            
            return count;
        }

        public void Insertar(Arista<Informacion> e)
        {

            
            if (e != null)
            {
                Nodo<Arista<Informacion>> nodo = new Nodo<Arista<Informacion>>(e);
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
        public Arista<Informacion>[] ObtenerAristas()
        {
            Arista<Informacion> [] array = new Arista<Informacion>[GetNumeroAristas()];
            Nodo<Arista<Informacion>> nodo = conjunto;
            for (int i = 0; i < GetNumeroAristas(); i++)
            {
                array[i] = nodo.darDato();
                nodo = nodo.darSiguiente();
            }
            return array;
        }

        public bool Pertenece(Arista<Informacion> e)
        {
            
            if (!EsVacio() && e != null)
            {
                
                Nodo<Arista<Informacion>> nodo = conjunto;
                while (nodo != null)
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
            

            
                devolucion = "{";

            for (int i = 0; i < ObtenerAristas().Length; i++) {


                if (i != ObtenerAristas().Length - 1)
                {
                    devolucion = devolucion + ObtenerAristas()[i] + ", ";
                }
                else
                {
                    devolucion = devolucion + ObtenerAristas()[i];
                }

            }
            
            


            return devolucion+"}";
        }


    }
}
