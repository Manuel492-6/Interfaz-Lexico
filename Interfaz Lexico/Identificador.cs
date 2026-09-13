using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaz_Lexico
{
    internal class Identificador: IEquatable<Identificador>, IComparable<Identificador>
    {
		private int _NumeroDeIdentificador;

		public int NumeroDeIdentificador
		{
			get { return _NumeroDeIdentificador; }
			set { _NumeroDeIdentificador = value; }
		}

		private string _Nombre;

		public string Nombre
		{	
			get { return _Nombre; }
			set { _Nombre = value; }
		}

		private string _TipoDeDato;

		public string TipoDeDato
		{
			get { return _TipoDeDato; }
			set { _TipoDeDato = value; }
		}

		private string _Valor;

		public string Valor
		{
			get { return _Valor; }
			set { _Valor = value; }
		}


		// Compara si dos identificadores son iguales según su nombre Equals()
		public bool Equals(Identificador other)
		{
			return this.Nombre.Equals(other.Nombre);
        }

		// Compara dos identificadores en base a su número de identificador CompareTo()
		public int CompareTo(Identificador other)
		{
			return this.NumeroDeIdentificador.CompareTo(other.NumeroDeIdentificador);
        }



    }
}
