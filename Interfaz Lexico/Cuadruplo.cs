using System;

namespace Interfaz_Lexico
{
    public class Cuadruplo
    {
        public int Indice { get; set; }
        public string Operador { get; set; } = "";
        public string Argumento1 { get; set; } = "";
        public string Argumento2 { get; set; } = "";
        public string Resultado { get; set; } = "";

        public Cuadruplo() { }

        public Cuadruplo(int indice, string operador, string arg1, string arg2, string resultado)
        {
            Indice = indice;
            Operador = operador;
            Argumento1 = arg1;
            Argumento2 = arg2;
            Resultado = resultado;
        }

        public override string ToString()
        {
            string op2 = string.IsNullOrEmpty(Argumento2) ? "_" : Argumento2;
            return $"({Operador}, {Argumento1}, {op2}, {Resultado})";
        }
    }

    public class Triplo
    {
        public int Indice { get; set; }
        public string Referencia => $"({Indice})";
        public string Operador { get; set; } = "";
        public string Argumento1 { get; set; } = "";
        public string Argumento2 { get; set; } = "";

        public Triplo() { }

        public Triplo(int indice, string operador, string arg1, string arg2)
        {
            Indice = indice;
            Operador = operador;
            Argumento1 = arg1;
            Argumento2 = arg2;
        }

        public override string ToString()
        {
            string op2 = string.IsNullOrEmpty(Argumento2) ? "_" : Argumento2;
            return $"({Indice}) ({Operador}, {Argumento1}, {op2})";
        }
    }

    public class InstruccionP
    {
        public int Indice { get; set; }
        public string Instruccion { get; set; } = "";
        public string Operando { get; set; } = "";
        public string Comentario { get; set; } = "";

        public InstruccionP() { }

        public InstruccionP(int indice, string instruccion, string operando = "", string comentario = "")
        {
            Indice = indice;
            Instruccion = instruccion;
            Operando = operando;
            Comentario = comentario;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Operando)
                ? $"{Instruccion,-6}"
                : $"{Instruccion,-6} {Operando}";
        }
    }
}
