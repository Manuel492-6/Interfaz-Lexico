using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaz_Lexico
{
    public class GeneradorCodigoIntermedio
    {
        // Genera la representación en Notación Postfija (Polaca Inversa / RPN) a partir de un árbol jerárquico
        public static string GenerarNotacionPostfija(NodoJerarquia? nodo)
        {
            if (nodo == null) return "";
            List<string> tokens = new List<string>();
            RecorrerPostfijo(nodo, tokens);
            return string.Join(" ", tokens);
        }

        private static void RecorrerPostfijo(NodoJerarquia? nodo, List<string> tokens)
        {
            if (nodo == null) return;

            // 1. Agrupación por paréntesis: se omite el nodo y se recorre su contenido interno
            if (nodo.TipoNodo == "Agrupacion")
            {
                RecorrerPostfijo(nodo.Izquierdo, tokens);
                return;
            }

            // 2. Operandos: Constantes e Identificadores van directamente a la salida
            if (nodo.TipoNodo == "Constante" || nodo.TipoNodo == "Identificador")
            {
                tokens.Add(nodo.Lexema);
                return;
            }

            // 2.5. Llamadas a función en RPN: argumentos y luego NOMBRE()
            if (nodo.TipoNodo == "Funcion")
            {
                foreach (var arg in nodo.Hijos)
                {
                    RecorrerPostfijo(arg, tokens);
                }
                tokens.Add($"{nodo.Lexema}()");
                return;
            }

            // 3. Asignación: en RPN se recorre destino, expresión y finalmente el operador '='
            if (nodo.TipoNodo == "Asignacion")
            {
                RecorrerPostfijo(nodo.Izquierdo, tokens);
                RecorrerPostfijo(nodo.Derecho, tokens);
                tokens.Add(nodo.Lexema);
                return;
            }

            // 4. Operadores (Unarios o Binarios): recorrido post-orden (Izq, Der, Raíz)
            if (nodo.Izquierdo != null)
            {
                RecorrerPostfijo(nodo.Izquierdo, tokens);
            }
            if (nodo.Derecho != null)
            {
                RecorrerPostfijo(nodo.Derecho, tokens);
            }

            string opLimpio = LimpiarOperador(nodo.Lexema);
            tokens.Add(opLimpio);
        }

        // Genera la lista de cuádruplos (C3D) para un árbol de operación dado
        public static List<Cuadruplo> GenerarCuadruplos(NodoJerarquia? raiz, ref int contadorTemporales, ref int contadorCuadruplos)
        {
            List<Cuadruplo> lista = new List<Cuadruplo>();
            if (raiz == null) return lista;

            GenerarCuadruploRecursivo(raiz, lista, ref contadorTemporales, ref contadorCuadruplos);
            return lista;
        }

        // Recorrido recursivo post-orden que emite los cuádruplos de 3 direcciones
        private static string GenerarCuadruploRecursivo(
            NodoJerarquia? nodo,
            List<Cuadruplo> lista,
            ref int contadorTemporales,
            ref int contadorCuadruplos)
        {
            if (nodo == null) return "";

            // 1. Nodos de agrupación (paréntesis)
            if (nodo.TipoNodo == "Agrupacion")
            {
                return GenerarCuadruploRecursivo(nodo.Izquierdo, lista, ref contadorTemporales, ref contadorCuadruplos);
            }

            // 2. Operandos base (Identificadores y Constantes)
            if (nodo.TipoNodo == "Identificador" || nodo.TipoNodo == "Constante")
            {
                return nodo.Lexema;
            }

            // 2.5. Llamadas a función en Cuádruplos (PARAM y CALL)
            if (nodo.TipoNodo == "Funcion")
            {
                List<string> nombresArgs = new List<string>();
                foreach (var arg in nodo.Hijos)
                {
                    string argNombre = GenerarCuadruploRecursivo(arg, lista, ref contadorTemporales, ref contadorCuadruplos);
                    nombresArgs.Add(argNombre);
                }

                foreach (var a in nombresArgs)
                {
                    lista.Add(new Cuadruplo(contadorCuadruplos++, "PARAM", a, "", ""));
                }

                string tempRetorno = $"T{contadorTemporales++}";
                lista.Add(new Cuadruplo(contadorCuadruplos++, "CALL", nodo.Lexema, nombresArgs.Count.ToString(), tempRetorno));
                return tempRetorno;
            }

            // 3. Asignaciones (ID = Expresión)
            if (nodo.TipoNodo == "Asignacion")
            {
                string variableDestino = nodo.Izquierdo?.Lexema ?? "destino";
                string resultadoExpresion = GenerarCuadruploRecursivo(nodo.Derecho, lista, ref contadorTemporales, ref contadorCuadruplos);

                Cuadruplo cuadAsig = new Cuadruplo(
                    contadorCuadruplos++,
                    "=",
                    resultadoExpresion,
                    "",
                    variableDestino
                );
                lista.Add(cuadAsig);
                return variableDestino;
            }

            // 4. Operadores
            string op = LimpiarOperador(nodo.Lexema);

            // Operador unario (+, -, NOT, !)
            if (nodo.Derecho == null)
            {
                string arg = GenerarCuadruploRecursivo(nodo.Izquierdo, lista, ref contadorTemporales, ref contadorCuadruplos);
                string tempUnario = $"T{contadorTemporales++}";

                Cuadruplo cuadUnario = new Cuadruplo(
                    contadorCuadruplos++,
                    op,
                    arg,
                    "",
                    tempUnario
                );
                lista.Add(cuadUnario);
                return tempUnario;
            }

            // Operador binario (+, -, *, /, ^, %, relacionales, lógicos)
            string arg1 = GenerarCuadruploRecursivo(nodo.Izquierdo, lista, ref contadorTemporales, ref contadorCuadruplos);
            string arg2 = GenerarCuadruploRecursivo(nodo.Derecho, lista, ref contadorTemporales, ref contadorCuadruplos);
            string temporal = $"T{contadorTemporales++}";

            Cuadruplo cuadBinario = new Cuadruplo(
                contadorCuadruplos++,
                op,
                arg1,
                arg2,
                temporal
            );
            lista.Add(cuadBinario);
            return temporal;
        }

        // Remueve sufijos informativos internos como " (Unario)"
        private static string LimpiarOperador(string op)
        {
            if (string.IsNullOrEmpty(op)) return "";
            if (op.EndsWith(" (Unario)"))
            {
                return op.Replace(" (Unario)", "").Trim();
            }
            return op.Trim();
        }

        // Procesa todas las operaciones de un programa y genera los cuádruplos globales continuos
        public static List<Cuadruplo> GenerarCuadruplosPrograma(List<ResultadoSemanticoOperacion> operaciones)
        {
            List<Cuadruplo> todos = new List<Cuadruplo>();
            if (operaciones == null) return todos;

            int contadorTemporalesGlobal = 1;
            int contadorCuadruplosGlobal = 1;

            foreach (var op in operaciones)
            {
                if (op.ArbolRaiz != null)
                {
                    var cuads = GenerarCuadruplos(op.ArbolRaiz, ref contadorTemporalesGlobal, ref contadorCuadruplosGlobal);
                    todos.AddRange(cuads);
                }
            }

            return todos;
        }
    }
}
