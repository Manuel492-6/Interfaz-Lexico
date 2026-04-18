using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaz_Lexico
{
    public class ClaseListaSimpleOrdenada<Tipo> where Tipo : IEquatable<Tipo>, IComparable<Tipo>
    {
        private ClaseNodo<Tipo> _nodoInicial;

        private ClaseNodo<Tipo> NodoInicial
        {
            get { return _nodoInicial; }
            set { _nodoInicial = value; }
        }

        public ClaseListaSimpleOrdenada()
        {
            NodoInicial = null;
        }

        public bool Vacia
        {
            get { return (NodoInicial == null) ? true : false; }
        }

        public IEnumerator<Tipo> GetEnumerator()
        {

            if (Vacia == true)
            {
                yield break;
            }
            else
            {

                ClaseNodo<Tipo> NodoActual = NodoInicial;

                do
                {

                    yield return (NodoActual.ObjetoConDatos);
                    NodoActual = NodoActual.Siguiente;

                }
                while (NodoActual != null);
                yield break;

            }

        }

        public void Insertar(Tipo Objeto)
        {
            if (Vacia)
            {
                ClaseNodo<Tipo> nodoNuevo = new ClaseNodo<Tipo>();
                nodoNuevo.ObjetoConDatos = Objeto;
                nodoNuevo.Siguiente = null;
                NodoInicial = nodoNuevo;

            }
            else
            {
                ClaseNodo<Tipo> NodoActual = new ClaseNodo<Tipo>();
                ClaseNodo<Tipo> NodoPrevio = new ClaseNodo<Tipo>();
                NodoActual = NodoInicial;

                do
                {
                    if (Objeto.Equals(NodoActual.ObjetoConDatos))
                    {
                        throw new Exception("No se aceptan duplicados en la lista");
                    }
                    else
                    {
                        if (Objeto.CompareTo(NodoActual.ObjetoConDatos) < 0)
                        {
                            if (NodoActual == NodoInicial)
                            {
                                ClaseNodo<Tipo> NodoNuevo = new ClaseNodo<Tipo>();
                                NodoNuevo.ObjetoConDatos = Objeto;
                                NodoNuevo.Siguiente = NodoActual;
                                NodoInicial = NodoNuevo;
                                return;
                            }
                            else
                            {
                                ClaseNodo<Tipo> NodoNuevo = new ClaseNodo<Tipo>();
                                NodoNuevo.ObjetoConDatos = Objeto;
                                NodoPrevio.Siguiente = NodoNuevo;
                                NodoNuevo.Siguiente = NodoActual;
                                return;
                            }
                        }
                        else
                        {
                            NodoPrevio = NodoActual;
                            NodoActual = NodoActual.Siguiente;
                        }

                    }
                }
                while (NodoActual != null);
                {
                    ClaseNodo<Tipo> NodoNuevo = new ClaseNodo<Tipo>();
                    NodoNuevo.ObjetoConDatos = Objeto;
                    NodoPrevio.Siguiente = NodoNuevo;
                    NodoNuevo.Siguiente = NodoActual;
                    return;
                }


            }
        }

        public Tipo Eliminar(Tipo Objeto)
        {
            if (Vacia)
            {
                throw new Exception("La lista esta vacia");
            }
            else
            {
                ClaseNodo<Tipo> NodoActual = new ClaseNodo<Tipo>();
                ClaseNodo<Tipo> NodoPrevio = new ClaseNodo<Tipo>();
                Tipo Auxiliar;
                NodoActual = NodoInicial;

                do
                {
                    if (Objeto.Equals(NodoInicial.ObjetoConDatos))
                    {
                        Auxiliar = NodoActual.ObjetoConDatos;
                        NodoInicial = NodoActual.Siguiente;
                        NodoActual = null;
                        return Auxiliar;

                    }
                    else
                    {
                        if (Objeto.Equals(NodoActual.ObjetoConDatos))
                        {
                            NodoPrevio.Siguiente = NodoActual.Siguiente;
                            Auxiliar = NodoActual.ObjetoConDatos;
                            NodoActual = null;
                            return Auxiliar;
                        }
                        else
                        {
                            if (Objeto.CompareTo(NodoActual.ObjetoConDatos) < 0)
                            {
                                throw new Exception("No existe dentro de la lista");
                            }
                            else
                            {
                                NodoPrevio = NodoActual;
                                NodoActual = NodoActual.Siguiente;

                            }

                        }
                    }
                }
                while (NodoActual != null);
                {
                    throw new Exception("No Existe dentro de la lista");
                }
            }
        }

        public bool Buscar(Tipo Objeto)
        {
            if (Vacia)
            {
                throw new Exception("La lista esta vacia");
            }
            else
            {
                ClaseNodo<Tipo> NodoActual = new ClaseNodo<Tipo>();
                ClaseNodo<Tipo> NodoPrevio = new ClaseNodo<Tipo>();
                Tipo Auxiliar;
                NodoActual = NodoInicial;

                do
                {
                    if (Objeto.Equals(NodoInicial.ObjetoConDatos))
                    {
                        Auxiliar = NodoActual.ObjetoConDatos;
                        return true;
                    }
                    else
                    {
                        if (Objeto.Equals(NodoActual.ObjetoConDatos))
                        {
                            Auxiliar = NodoActual.ObjetoConDatos;
                            return true;
                        }
                        else
                        {
                            if (Objeto.CompareTo(NodoActual.ObjetoConDatos) < 0)
                            {
                                return false;
                            }
                            else
                            {
                                NodoPrevio = NodoActual;
                                NodoActual = NodoActual.Siguiente;

                            }

                        }
                    }
                }
                while (NodoActual != null);
                {
                    return false;
                }
            }
        }
        ~ClaseListaSimpleOrdenada()
        {
            Vaciar();
        }
        public void Vaciar()
        {
            if (Vacia)
            {
                throw new Exception("La lista esta vacia");
            }
            else
            {
                ClaseNodo<Tipo> NodoPrevio = new ClaseNodo<Tipo>();
                ClaseNodo<Tipo> NodoActual = new ClaseNodo<Tipo>();
                NodoActual = NodoInicial;

                do
                {
                    NodoPrevio = NodoActual;
                    NodoActual = NodoActual.Siguiente;
                    NodoPrevio = null;
                }
                while (NodoActual != null);
                {
                    NodoInicial = null;
                    return;
                }
            }
        }

        public int Contar
        {
            get
            {
                if (!Vacia)
                {
                    ClaseNodo<Tipo> NodoActual = new ClaseNodo<Tipo>();
                    NodoActual = NodoInicial;
                    int Contador = 0;
                    do
                    {
                        Contador++;
                        NodoActual = NodoActual.Siguiente;
                    }
                    while (NodoActual != null);
                    {
                        return Contador;
                    }
                }
                else
                {
                    return 0;
                }
            }

        }


    }
}
