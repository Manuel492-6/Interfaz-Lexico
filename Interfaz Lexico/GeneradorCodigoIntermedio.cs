using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaz_Lexico
{
    public class GeneradorCodigoIntermedio
    {
        // 1. NOTACIÓN INFIJA: Recorrido In-Orden (Izquierda, Raíz, Derecha)
        public static string GenerarNotacionInfija(NodoJerarquia? nodo)
        {
            if (nodo == null) return "";
            List<string> tokens = new List<string>();
            RecorrerInfijo(nodo, tokens);
            return string.Join(" ", tokens);
        }

        private static void RecorrerInfijo(NodoJerarquia? nodo, List<string> tokens)
        {
            if (nodo == null) return;

            if (nodo.TipoNodo == "Agrupacion")
            {
                tokens.Add("(");
                RecorrerInfijo(nodo.Izquierdo, tokens);
                tokens.Add(")");
                return;
            }

            if (nodo.TipoNodo == "Constante" || nodo.TipoNodo == "Identificador")
            {
                tokens.Add(nodo.Lexema);
                return;
            }

            if (nodo.TipoNodo == "Funcion")
            {
                List<string> argsTxt = new List<string>();
                foreach (var arg in nodo.Hijos)
                {
                    argsTxt.Add(GenerarNotacionInfija(arg));
                }
                tokens.Add($"{nodo.Lexema}({string.Join(", ", argsTxt)})");
                return;
            }

            if (nodo.TipoNodo == "Asignacion")
            {
                RecorrerInfijo(nodo.Izquierdo, tokens);
                tokens.Add("=");
                RecorrerInfijo(nodo.Derecho, tokens);
                return;
            }

            string opLimpio = LimpiarOperador(nodo.Lexema);

            // Operador unario
            if (nodo.Derecho == null)
            {
                tokens.Add(opLimpio);
                RecorrerInfijo(nodo.Izquierdo, tokens);
                return;
            }

            // Operador binario
            RecorrerInfijo(nodo.Izquierdo, tokens);
            tokens.Add(opLimpio);
            RecorrerInfijo(nodo.Derecho, tokens);
        }

        // 2. NOTACIÓN PREFIJA (Polaca): Recorrido Pre-Orden (Raíz, Izquierda, Derecha)
        public static string GenerarNotacionPrefija(NodoJerarquia? nodo)
        {
            if (nodo == null) return "";
            List<string> tokens = new List<string>();
            RecorrerPrefijo(nodo, tokens);
            return string.Join(" ", tokens);
        }

        private static void RecorrerPrefijo(NodoJerarquia? nodo, List<string> tokens)
        {
            if (nodo == null) return;

            // Agrupación por paréntesis: se omite el paréntesis y se recorre el interior en pre-orden
            if (nodo.TipoNodo == "Agrupacion")
            {
                RecorrerPrefijo(nodo.Izquierdo, tokens);
                return;
            }

            if (nodo.TipoNodo == "Constante" || nodo.TipoNodo == "Identificador")
            {
                tokens.Add(nodo.Lexema);
                return;
            }

            if (nodo.TipoNodo == "Funcion")
            {
                tokens.Add($"{nodo.Lexema}()");
                foreach (var arg in nodo.Hijos)
                {
                    RecorrerPrefijo(arg, tokens);
                }
                return;
            }

            if (nodo.TipoNodo == "Asignacion")
            {
                tokens.Add(nodo.Lexema);
                RecorrerPrefijo(nodo.Izquierdo, tokens);
                RecorrerPrefijo(nodo.Derecho, tokens);
                return;
            }

            string opLimpio = LimpiarOperador(nodo.Lexema);
            tokens.Add(opLimpio);

            if (nodo.Izquierdo != null)
            {
                RecorrerPrefijo(nodo.Izquierdo, tokens);
            }
            if (nodo.Derecho != null)
            {
                RecorrerPrefijo(nodo.Derecho, tokens);
            }
        }

        // 3. NOTACIÓN POSTFIJA (Polaca Inversa / RPN): Recorrido Post-Orden (Izquierda, Derecha, Raíz)
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

        // 4. CUÁDRUPLOS (C3D)
        public static List<Cuadruplo> GenerarCuadruplos(NodoJerarquia? raiz, ref int contadorTemporales, ref int contadorCuadruplos)
        {
            List<Cuadruplo> lista = new List<Cuadruplo>();
            if (raiz == null) return lista;

            GenerarCuadruploRecursivo(raiz, lista, ref contadorTemporales, ref contadorCuadruplos);
            return lista;
        }

        private static string GenerarCuadruploRecursivo(
            NodoJerarquia? nodo,
            List<Cuadruplo> lista,
            ref int contadorTemporales,
            ref int contadorCuadruplos)
        {
            if (nodo == null) return "";

            if (nodo.TipoNodo == "Agrupacion")
            {
                return GenerarCuadruploRecursivo(nodo.Izquierdo, lista, ref contadorTemporales, ref contadorCuadruplos);
            }

            if (nodo.TipoNodo == "Identificador" || nodo.TipoNodo == "Constante")
            {
                return nodo.Lexema;
            }

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

            string op = LimpiarOperador(nodo.Lexema);

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

        // 5. TRIPLOS (Referencias por índice sin variables temporales explícitas)
        public static List<Triplo> GenerarTriplos(NodoJerarquia? raiz, ref int contadorTriplos)
        {
            List<Triplo> lista = new List<Triplo>();
            if (raiz == null) return lista;

            GenerarTriploRecursivo(raiz, lista, ref contadorTriplos);
            return lista;
        }

        private static string GenerarTriploRecursivo(
            NodoJerarquia? nodo,
            List<Triplo> lista,
            ref int contadorTriplos)
        {
            if (nodo == null) return "";

            if (nodo.TipoNodo == "Agrupacion")
            {
                return GenerarTriploRecursivo(nodo.Izquierdo, lista, ref contadorTriplos);
            }

            if (nodo.TipoNodo == "Identificador" || nodo.TipoNodo == "Constante")
            {
                return nodo.Lexema;
            }

            if (nodo.TipoNodo == "Funcion")
            {
                List<string> refsArgs = new List<string>();
                foreach (var arg in nodo.Hijos)
                {
                    string argRef = GenerarTriploRecursivo(arg, lista, ref contadorTriplos);
                    refsArgs.Add(argRef);
                }

                foreach (var a in refsArgs)
                {
                    lista.Add(new Triplo(contadorTriplos++, "PARAM", a, ""));
                }

                int idxCall = contadorTriplos++;
                lista.Add(new Triplo(idxCall, "CALL", nodo.Lexema, refsArgs.Count.ToString()));
                return $"({idxCall})";
            }

            if (nodo.TipoNodo == "Asignacion")
            {
                string variableDestino = nodo.Izquierdo?.Lexema ?? "destino";
                string resultadoExpresion = GenerarTriploRecursivo(nodo.Derecho, lista, ref contadorTriplos);

                int idxAsig = contadorTriplos++;
                lista.Add(new Triplo(idxAsig, "=", variableDestino, resultadoExpresion));
                return $"({idxAsig})";
            }

            string op = LimpiarOperador(nodo.Lexema);

            if (nodo.Derecho == null)
            {
                string arg = GenerarTriploRecursivo(nodo.Izquierdo, lista, ref contadorTriplos);
                int idxUnario = contadorTriplos++;
                lista.Add(new Triplo(idxUnario, op, arg, ""));
                return $"({idxUnario})";
            }

            string arg1 = GenerarTriploRecursivo(nodo.Izquierdo, lista, ref contadorTriplos);
            string arg2 = GenerarTriploRecursivo(nodo.Derecho, lista, ref contadorTriplos);
            int idxBinario = contadorTriplos++;
            lista.Add(new Triplo(idxBinario, op, arg1, arg2));
            return $"({idxBinario})";
        }

        // 6. CÓDIGO P (P-Code / Máquina Abstracta de Pila)
        public static List<InstruccionP> GenerarCodigoP(NodoJerarquia? raiz, ref int contadorP)
        {
            List<InstruccionP> lista = new List<InstruccionP>();
            if (raiz == null) return lista;

            GenerarCodigoPRecursivo(raiz, lista, ref contadorP);
            return lista;
        }

        private static void GenerarCodigoPRecursivo(
            NodoJerarquia? nodo,
            List<InstruccionP> lista,
            ref int contadorP)
        {
            if (nodo == null) return;

            if (nodo.TipoNodo == "Agrupacion")
            {
                GenerarCodigoPRecursivo(nodo.Izquierdo, lista, ref contadorP);
                return;
            }

            if (nodo.TipoNodo == "Constante")
            {
                lista.Add(new InstruccionP(contadorP++, "LDC", nodo.Lexema, $"Cargar constante {nodo.Lexema} en la pila"));
                return;
            }

            if (nodo.TipoNodo == "Identificador")
            {
                lista.Add(new InstruccionP(contadorP++, "LOD", nodo.Lexema, $"Cargar valor de variable '{nodo.Lexema}'"));
                return;
            }

            if (nodo.TipoNodo == "Funcion")
            {
                lista.Add(new InstruccionP(contadorP++, "MST", "0", $"Preparar activación para función '{nodo.Lexema}'"));
                foreach (var arg in nodo.Hijos)
                {
                    GenerarCodigoPRecursivo(arg, lista, ref contadorP);
                }
                lista.Add(new InstruccionP(contadorP++, "CUP", $"{nodo.Hijos.Count} {nodo.Lexema}", $"Invocar función '{nodo.Lexema}' ({nodo.Hijos.Count} args)"));
                return;
            }

            if (nodo.TipoNodo == "Asignacion")
            {
                string variableDestino = nodo.Izquierdo?.Lexema ?? "destino";
                lista.Add(new InstruccionP(contadorP++, "LDA", variableDestino, $"Cargar dirección de variable '{variableDestino}'"));
                GenerarCodigoPRecursivo(nodo.Derecho, lista, ref contadorP);
                lista.Add(new InstruccionP(contadorP++, "STO", variableDestino, $"Almacenar tope de la pila en '{variableDestino}'"));
                return;
            }

            string op = LimpiarOperador(nodo.Lexema);

            // Operador unario
            if (nodo.Derecho == null)
            {
                GenerarCodigoPRecursivo(nodo.Izquierdo, lista, ref contadorP);
                if (op == "-")
                {
                    lista.Add(new InstruccionP(contadorP++, "NGI", "", "Inversión de signo (Negación entera/real)"));
                }
                else if (op.Equals("NOT", StringComparison.OrdinalIgnoreCase) || op == "!")
                {
                    lista.Add(new InstruccionP(contadorP++, "NOT", "", "Negación lógica booleana"));
                }
                return;
            }

            // Operador binario
            GenerarCodigoPRecursivo(nodo.Izquierdo, lista, ref contadorP);
            GenerarCodigoPRecursivo(nodo.Derecho, lista, ref contadorP);

            var (mnemonico, descripcion) = op.ToUpperInvariant() switch
            {
                "+" => ("ADI", "Suma / Concatenación en el tope de la pila"),
                "-" => ("SBI", "Resta del sub-tope menos el tope de la pila"),
                "*" => ("MPI", "Multiplicación de los dos valores del tope"),
                "/" => ("DVI", "División del sub-tope entre el tope de la pila"),
                "^" => ("POW", "Potenciación del sub-tope elevado al tope"),
                "%" => ("MOD", "Residuo de división entera"),
                ">" => ("GRT", "Comparación relacional Mayor que (>)"),
                "<" => ("LES", "Comparación relacional Menor que (<)"),
                ">=" => ("GEQ", "Comparación relacional Mayor o igual (>=)"),
                "<=" => ("LEQ", "Comparación relacional Menor o igual (<=)"),
                "==" => ("EQU", "Comparación relacional Igualdad (==)"),
                "!=" => ("NEQ", "Comparación relacional Diferente (!=)"),
                "AND" or "&&" => ("AND", "Conjunción lógica booleana (AND)"),
                "OR" or "||" => ("IOR", "Disyunción lógica booleana (OR)"),
                _ => ("OPR", $"Operación '{op}'")
            };

            lista.Add(new InstruccionP(contadorP++, mnemonico, "", descripcion));
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

        // Procesa todas las operaciones de un programa y genera los cuádruplos globales continuos (sin saltos)
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

        // Procesa todas las operaciones de un programa y genera los triplos globales continuos
        public static List<Triplo> GenerarTriplosPrograma(List<ResultadoSemanticoOperacion> operaciones)
        {
            List<Triplo> todos = new List<Triplo>();
            if (operaciones == null) return todos;

            int contadorTriplosGlobal = 1;

            foreach (var op in operaciones)
            {
                if (op.ArbolRaiz != null)
                {
                    var trips = GenerarTriplos(op.ArbolRaiz, ref contadorTriplosGlobal);
                    todos.AddRange(trips);
                }
            }

            return todos;
        }

        // Procesa todas las operaciones de un programa y genera las instrucciones de Código P globales
        public static List<InstruccionP> GenerarCodigoPPrograma(List<ResultadoSemanticoOperacion> operaciones)
        {
            List<InstruccionP> todos = new List<InstruccionP>();
            if (operaciones == null) return todos;

            int contadorPGlobal = 1;

            foreach (var op in operaciones)
            {
                if (op.ArbolRaiz != null)
                {
                    var insts = GenerarCodigoP(op.ArbolRaiz, ref contadorPGlobal);
                    todos.AddRange(insts);
                }
            }

            return todos;
        }
    }
}
