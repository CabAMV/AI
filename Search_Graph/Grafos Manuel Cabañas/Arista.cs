using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grafos_Manuel_Cabañas
{
    public class Arista<Informacion>
    {
        private Informacion destino;
        private Informacion origen;
        private double peso;

        public Informacion Destino { get { return destino; } }
        public Informacion Origen { get { return origen; } }
        public Double Peso { get { return peso; } }

        public Arista(Informacion origen, Informacion destino)
        {
            this.origen = origen;
            this.destino = destino;
            peso = 0;
        }

        public Arista(Informacion origen, Informacion destino, double peso)
        {
            this.origen = origen;
            this.destino = destino;
            this.peso = peso;
        }

        public override bool Equals(object obj)
        {

            
            Arista <Informacion> aux = (Arista<Informacion>)obj ;
            
            
            return (aux.origen.Equals(this.origen) && aux.destino.Equals(this.destino) && aux.peso.Equals(this.peso));
        }

        public override string ToString()
        {
            return "(" + Origen.ToString() +", " + Destino.ToString() +", " + Peso.ToString() + ")";
        }
    }
}
