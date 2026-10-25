using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    public class TokenSemantico
    {
        public string Lexema { get; set; } = "";
        public string Tipo { get; set; } = ""; // "NUM", "ID", "STR", "OP_ARIT", "OP_REL", "OP_LOG", "ASIG", "PAR_ABRE", "PAR_CIERRA"
        public int Linea { get; set; } = 1;
        public int Posicion { get; set; }
    }

    public class ResultadoSemanticoOperacion
    {
        public string ExpresionOriginal { get; set; } = "";
        public NodoJerarquia? ArbolRaiz { get; set; }
        public List<string> PasosEvaluacion { get; set; } = new List<string>();
        public List<string> ErroresSemanticos { get; set; } = new List<string>();
        public string TipoDatoResultante { get; set; } = "desconocido";
        public string? ValorCalculado { get; set; }
        public string NotacionPostfija { get; set; } = "";
        public List<Cuadruplo> Cuadruplos { get; set; } = new List<Cuadruplo>();
        public bool EsValida => ErroresSemanticos.Count == 0;
    }

    public class DefinicionFuncion
    {
        public string Nombre { get; set; } = "";
        public List<string> Parametros { get; set; } = new List<string>();
        public List<string> Instrucciones { get; set; } = new List<string>();
        public string TipoRetorno { get; set; } = "int";
        public string? ExpresionRetorno { get; set; }
    }

    public class AnalizadorSemanticoJerarquia
    {
        private List<TokenSemantico> tokens = new List<TokenSemantico>();
        private int pos = 0;
        private List<string> errores = new List<string>();
        private List<string> pasos = new List<string>();
        private int pasoContador = 1;
        private int lineaActual = 1;
        private Dictionary<string, string> tablaTipos = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> tablaValores = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> tablaRetornoFunciones = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, string> tablaValoresRetornoFunciones = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, DefinicionFuncion> definicionesFunciones = new Dictionary<string, DefinicionFuncion>(StringComparer.OrdinalIgnoreCase);

        // Inicializa una nueva instancia del analizador semántico con la tabla de tipos AnalizadorSemanticoJerarquia()
        public AnalizadorSemanticoJerarquia(Dictionary<string, string>? variablesConTipo = null)
        {
            if (variablesConTipo != null)
            {
                foreach (var kvp in variablesConTipo)
                {
                    tablaTipos[kvp.Key] = kvp.Value;
                }
            }
        }

        // Registra o actualiza el tipo de dato y valor de una variable en las tablas de símbolos RegistrarVariable()
        public void RegistrarVariable(string nombre, string tipoDato, string? valor = null)
        {
            if (!string.IsNullOrEmpty(tipoDato))
            {
                tablaTipos[nombre] = tipoDato;
            }
            if (valor != null)
            {
                tablaValores[nombre] = valor;
            }
        }

        // Registra el tipo de dato dinámico que retorna una función y opcionalmente su valor calculado
        public void RegistrarRetornoFuncion(string nombreFuncion, string tipoRetorno, string? valorRetorno = null)
        {
            if (!string.IsNullOrEmpty(tipoRetorno) && tipoRetorno != "desconocido")
            {
                tablaRetornoFunciones[nombreFuncion] = tipoRetorno;
                tablaTipos[nombreFuncion] = tipoRetorno;
            }
            if (valorRetorno != null)
            {
                tablaValoresRetornoFunciones[nombreFuncion] = valorRetorno;
                tablaValores[nombreFuncion] = valorRetorno;
            }
        }

        // Obtiene el tipo de dato de retorno configurado o inferido para una función
        public string ObtenerRetornoFuncion(string nombreFuncion)
        {
            if (tablaRetornoFunciones.TryGetValue(nombreFuncion, out string? tipo) && !string.IsNullOrEmpty(tipo) && tipo != "desconocido")
            {
                return tipo;
            }
            if (tablaTipos.TryGetValue(nombreFuncion, out string? t) && !string.IsNullOrEmpty(t) && t != "desconocido" && t != "Null")
            {
                return t;
            }
            return "int";
        }

        // Obtiene el valor calculado de retorno si la función retorna una constante conocida
        public string? ObtenerValorRetornoFuncion(string nombreFuncion)
        {
            if (tablaValoresRetornoFunciones.TryGetValue(nombreFuncion, out string? val) && !string.IsNullOrEmpty(val))
            {
                return val;
            }
            if (tablaValores.TryGetValue(nombreFuncion, out string? v) && !string.IsNullOrEmpty(v))
            {
                return v;
            }
            return null;
        }

        // Registra la definición completa de una función para ejecución dinámica
        public void RegistrarDefinicionFuncion(DefinicionFuncion def)
        {
            if (def != null && !string.IsNullOrWhiteSpace(def.Nombre))
            {
                definicionesFunciones[def.Nombre] = def;
                if (!string.IsNullOrEmpty(def.TipoRetorno) && def.TipoRetorno != "desconocido")
                {
                    RegistrarRetornoFuncion(def.Nombre, def.TipoRetorno);
                }
            }
        }

        // Obtiene la definición de una función si está registrada
        public DefinicionFuncion? ObtenerDefinicionFuncion(string nombre)
        {
            if (definicionesFunciones.TryGetValue(nombre, out var def))
            {
                return def;
            }
            return null;
        }

        // Crea un sub-ámbito aislado para ejecutar funciones con sus argumentos
        public AnalizadorSemanticoJerarquia CrearSubAmbito()
        {
            var sub = new AnalizadorSemanticoJerarquia();
            foreach (var kvp in this.tablaTipos) sub.tablaTipos[kvp.Key] = kvp.Value;
            foreach (var kvp in this.tablaValores) sub.tablaValores[kvp.Key] = kvp.Value;
            foreach (var kvp in this.tablaRetornoFunciones) sub.tablaRetornoFunciones[kvp.Key] = kvp.Value;
            foreach (var kvp in this.tablaValoresRetornoFunciones) sub.tablaValoresRetornoFunciones[kvp.Key] = kvp.Value;
            foreach (var kvp in this.definicionesFunciones) sub.definicionesFunciones[kvp.Key] = kvp.Value;
            return sub;
        }

        // Ejecuta dinámicamente el cuerpo de una función con los argumentos pasados
        public (string TipoRetorno, string? ValorRetorno) EjecutarFuncion(string nombreFunc, List<NodoJerarquia> argumentos)
        {
            if (!definicionesFunciones.TryGetValue(nombreFunc, out var def) || def == null)
            {
                string tRet = ObtenerRetornoFuncion(nombreFunc);
                string? vRet = ObtenerValorRetornoFuncion(nombreFunc);
                if (string.IsNullOrEmpty(vRet))
                {
                    vRet = tRet switch
                    {
                        "string" => "\"\"",
                        "bool" => "TRUE",
                        "float" => "+0.0",
                        _ => "0"
                    };
                }
                return (tRet, vRet);
            }

            var subAmbito = CrearSubAmbito();

            // 1. Enlazar los valores de los argumentos con los parámetros formales
            for (int i = 0; i < def.Parametros.Count; i++)
            {
                string pNombre = def.Parametros[i];
                string tipoArg = "int";
                string valArg = "0";

                if (i < argumentos.Count)
                {
                    var arg = argumentos[i];
                    tipoArg = !string.IsNullOrEmpty(arg.TipoDato) && arg.TipoDato != "desconocido" ? arg.TipoDato : "int";
                    valArg = arg.ValorCalculado ?? (tipoArg == "string" ? "\"\"" : "0");
                }

                subAmbito.RegistrarVariable(pNombre, tipoArg, valArg);
                this.RegistrarVariable(pNombre, tipoArg, valArg);
            }

            // 2. Ejecutar instrucciones del cuerpo en el sub-ámbito
            foreach (var instr in def.Instrucciones)
            {
                if (string.IsNullOrWhiteSpace(instr)) continue;
                subAmbito.AnalizarExpresion(instr);
            }

            // Sincronizar variables calculadas en el cuerpo con el ámbito padre
            foreach (var kvp in subAmbito.tablaTipos)
            {
                this.tablaTipos[kvp.Key] = kvp.Value;
            }
            foreach (var kvp in subAmbito.tablaValores)
            {
                this.tablaValores[kvp.Key] = kvp.Value;
            }

            // 3. Evaluar la expresión de retorno
            string tipoFinal = def.TipoRetorno;
            string? valFinal = null;

            if (!string.IsNullOrWhiteSpace(def.ExpresionRetorno))
            {
                var resRet = subAmbito.AnalizarExpresion(def.ExpresionRetorno);
                if (resRet.ArbolRaiz != null)
                {
                    tipoFinal = resRet.TipoDatoResultante;
                    valFinal = resRet.ValorCalculado;
                }
            }

            if (string.IsNullOrEmpty(valFinal))
            {
                valFinal = ObtenerValorRetornoFuncion(nombreFunc);
            }
            if (string.IsNullOrEmpty(valFinal))
            {
                valFinal = tipoFinal switch
                {
                    "string" => "\"\"",
                    "bool" => "TRUE",
                    "float" => "+0.0",
                    _ => "0"
                };
            }

            RegistrarRetornoFuncion(nombreFunc, tipoFinal, valFinal);
            RegistrarVariable(nombreFunc, tipoFinal, valFinal);

            return (tipoFinal, valFinal);
        }

        // Verifica si una variable se encuentra registrada con un tipo válido en la tabla TieneVariable()
        public bool TieneVariable(string nombre)
        {
            return tablaTipos.ContainsKey(nombre) && !string.IsNullOrEmpty(tablaTipos[nombre]) && tablaTipos[nombre] != "Null";
        }

        // Obtiene el tipo de dato registrado correspondiente al nombre de una variable ObtenerTipo()
        public string ObtenerTipo(string nombre)
        {
            return ObtenerTipoVariable(nombre);
        }

        // Obtiene el valor actual calculado o asignado a una variable ObtenerValor()
        public string? ObtenerValor(string nombre)
        {
            return ObtenerValorVariable(nombre);
        }

        // Retorna una copia de la tabla de tipos de datos de las variables ObtenerTablaTipos()
        public Dictionary<string, string> ObtenerTablaTipos()
        {
            return new Dictionary<string, string>(tablaTipos, StringComparer.OrdinalIgnoreCase);
        }

        // Retorna una copia de la tabla de valores de las variables ObtenerTablaValores()
        public Dictionary<string, string> ObtenerTablaValores()
        {
            return new Dictionary<string, string>(tablaValores, StringComparer.OrdinalIgnoreCase);
        }

        // Analiza semánticamente una expresión según la jerarquía de operadores y genera su árbol y pasos AnalizarExpresion()
        public ResultadoSemanticoOperacion AnalizarExpresion(string expresionTexto, int linea = 1)
        {
            lineaActual = linea;
            errores.Clear();
            pasos.Clear();
            pasoContador = 1;
            pos = 0;

            tokens = Tokenizar(expresionTexto, linea);

            ResultadoSemanticoOperacion resultado = new ResultadoSemanticoOperacion
            {
                ExpresionOriginal = expresionTexto
            };

            if (tokens.Count == 0)
            {
                resultado.ErroresSemanticos.Add("La operación ingresada está vacía.");
                return resultado;
            }

            // Verificar balance de paréntesis previo
            VerificarBalanceParentesis();

            NodoJerarquia? raiz = null;
            try
            {
                raiz = ParsearAsignacion();

                if (pos < tokens.Count && errores.Count == 0)
                {
                    ReportarError($"Token inesperado al final de la expresión: '{tokens[pos].Lexema}'", tokens[pos].Linea);
                }
            }
            catch (Exception ex)
            {
                ReportarError($"Error al procesar la jerarquía de la operación: {ex.Message}", linea);
            }

            if (raiz != null)
            {
                // Inferir tipos y evaluar valores
                InferirTiposYValores(raiz);
                resultado.TipoDatoResultante = raiz.TipoDato;
                resultado.ValorCalculado = raiz.ValorCalculado;

                // Generar los pasos de resolución por jerarquía
                GenerarPasosJerarquia(raiz);

                // Generar Notación Postfija (RPN) y Cuádruplos (C3D)
                resultado.NotacionPostfija = GeneradorCodigoIntermedio.GenerarNotacionPostfija(raiz);
                int tempCont = 1;
                int cuadCont = 1;
                resultado.Cuadruplos = GeneradorCodigoIntermedio.GenerarCuadruplos(raiz, ref tempCont, ref cuadCont);
            }

            resultado.ArbolRaiz = raiz;
            resultado.ErroresSemanticos = new List<string>(errores);
            resultado.PasosEvaluacion = new List<string>(pasos);

            return resultado;
        }

        // Convierte una lista de tokens sintácticos a texto y los analiza semánticamente AnalizarTokensCompilador()
        public ResultadoSemanticoOperacion AnalizarTokensCompilador(List<TokenSintactico> tokensSintacticos)
        {
            StringBuilder sb = new StringBuilder();
            int linea = tokensSintacticos.FirstOrDefault()?.Linea ?? 1;

            foreach (var t in tokensSintacticos)
            {
                string lex = !string.IsNullOrEmpty(t.Lexema) ? t.Lexema : t.Tipo;
                // Si el tipo es ALO= pero el lexema vino vacío o con tipo
                if (t.Tipo.StartsWith("ALO=")) lex = "=";
                else if (t.Tipo.StartsWith("AO+")) lex = "+";
                else if (t.Tipo.StartsWith("AO-")) lex = "-";
                else if (t.Tipo.StartsWith("AO*")) lex = "*";
                else if (t.Tipo.StartsWith("AO/")) lex = "/";
                else if (t.Tipo.StartsWith("AO^")) lex = "^";
                else if (t.Tipo.StartsWith("SC(")) lex = "(";
                else if (t.Tipo.StartsWith("SC)")) lex = ")";
                else if (t.Tipo.StartsWith("SC,") || t.Lexema == ",") lex = ",";
                else if (t.Tipo.StartsWith("RW26") || t.Lexema.Equals("TRUE", StringComparison.OrdinalIgnoreCase)) lex = "TRUE";
                else if (t.Tipo.StartsWith("RW08") || t.Lexema.Equals("FALSE", StringComparison.OrdinalIgnoreCase)) lex = "FALSE";
                else if (t.Tipo.StartsWith("LO1") || t.Lexema.Equals("AND", StringComparison.OrdinalIgnoreCase) || t.Lexema == "&&") lex = "AND";
                else if (t.Tipo.StartsWith("LO2") || t.Lexema.Equals("OR", StringComparison.OrdinalIgnoreCase) || t.Lexema == "||") lex = "OR";
                else if (t.Tipo.StartsWith("LO3") || t.Lexema.Equals("NOT", StringComparison.OrdinalIgnoreCase) || t.Lexema == "!") lex = "NOT";
                else if (t.Tipo.StartsWith("RO"))
                {
                    if (!string.IsNullOrEmpty(t.Lexema) && (t.Lexema == ">=" || t.Lexema == "<=" || t.Lexema == "==" || t.Lexema == "!=" || t.Lexema == ">" || t.Lexema == "<"))
                        lex = t.Lexema;
                    else if (t.Tipo.Length > 2)
                        lex = t.Tipo.Substring(2);
                }

                sb.Append(lex).Append(" ");
            }

            return AnalizarExpresion(sb.ToString().Trim(), linea);
        }

        #region Tokenizador de Expresiones

        // Convierte la cadena de entrada en una lista de tokens semánticos clasificados Tokenizar()
        private List<TokenSemantico> Tokenizar(string entrada, int linea)
        {
            List<TokenSemantico> list = new List<TokenSemantico>();
            int i = 0;

            while (i < entrada.Length)
            {
                char c = entrada[i];

                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }

                // Paréntesis
                if (c == '(')
                {
                    list.Add(new TokenSemantico { Lexema = "(", Tipo = "PAR_ABRE", Linea = linea, Posicion = i });
                    i++;
                    continue;
                }
                else if (c == ')')
                {
                    list.Add(new TokenSemantico { Lexema = ")", Tipo = "PAR_CIERRA", Linea = linea, Posicion = i });
                    i++;
                    continue;
                }
                // Coma (para argumentos de funciones o listas)
                else if (c == ',')
                {
                    list.Add(new TokenSemantico { Lexema = ",", Tipo = "COMA", Linea = linea, Posicion = i });
                    i++;
                    continue;
                }
                // Asignación o Relacional (==, <=, >=, !=, <, >)
                else if (c == '=')
                {
                    if (i + 1 < entrada.Length && entrada[i + 1] == '=')
                    {
                        list.Add(new TokenSemantico { Lexema = "==", Tipo = "OP_REL", Linea = linea, Posicion = i });
                        i += 2;
                    }
                    else
                    {
                        list.Add(new TokenSemantico { Lexema = "=", Tipo = "ASIG", Linea = linea, Posicion = i });
                        i++;
                    }
                    continue;
                }
                else if (c == '<' || c == '>')
                {
                    if (i + 1 < entrada.Length && entrada[i + 1] == '=')
                    {
                        list.Add(new TokenSemantico { Lexema = entrada.Substring(i, 2), Tipo = "OP_REL", Linea = linea, Posicion = i });
                        i += 2;
                    }
                    else
                    {
                        list.Add(new TokenSemantico { Lexema = c.ToString(), Tipo = "OP_REL", Linea = linea, Posicion = i });
                        i++;
                    }
                    continue;
                }
                else if (c == '!')
                {
                    if (i + 1 < entrada.Length && entrada[i + 1] == '=')
                    {
                        list.Add(new TokenSemantico { Lexema = "!=", Tipo = "OP_REL", Linea = linea, Posicion = i });
                        i += 2;
                    }
                    else
                    {
                        list.Add(new TokenSemantico { Lexema = "!", Tipo = "OP_LOG", Linea = linea, Posicion = i });
                        i++;
                    }
                    continue;
                }
                // Operadores Lógicos (&&, ||)
                else if (c == '&' && i + 1 < entrada.Length && entrada[i + 1] == '&')
                {
                    list.Add(new TokenSemantico { Lexema = "&&", Tipo = "OP_LOG", Linea = linea, Posicion = i });
                    i += 2;
                    continue;
                }
                else if (c == '|' && i + 1 < entrada.Length && entrada[i + 1] == '|')
                {
                    list.Add(new TokenSemantico { Lexema = "||", Tipo = "OP_LOG", Linea = linea, Posicion = i });
                    i += 2;
                    continue;
                }
                // Números con signo obligatorio o explícito (+5, -9, +10.5, etc.)
                // Permite opcionalmente espacios entre signo y número: ej. "+5", "+ 5", "-9", "- 9"
                int k = i + 1;
                while (k < entrada.Length && char.IsWhiteSpace(entrada[k])) k++;
                bool seguidoDeDigito = k < entrada.Length && (char.IsDigit(entrada[k]) || (entrada[k] == '.' && k + 1 < entrada.Length && char.IsDigit(entrada[k + 1])));

                bool esSignoNumero = (c == '+' || c == '-') && seguidoDeDigito &&
                    (list.Count == 0 ||
                     list.Last().Tipo == "OP_ARIT" ||
                     list.Last().Tipo == "OP_REL" ||
                     list.Last().Tipo == "OP_LOG" ||
                     list.Last().Tipo == "ASIG" ||
                     list.Last().Tipo == "PAR_ABRE");

                if (esSignoNumero)
                {
                    char signo = c;
                    i = k; // Avanzar hasta el inicio del número
                    int inicioDigitos = i;
                    bool tienePunto = false;
                    while (i < entrada.Length && (char.IsDigit(entrada[i]) || (entrada[i] == '.' && !tienePunto)))
                    {
                        if (entrada[i] == '.') tienePunto = true;
                        i++;
                    }
                    string num = signo + entrada.Substring(inicioDigitos, i - inicioDigitos);
                    list.Add(new TokenSemantico { Lexema = num, Tipo = "NUM", Linea = linea, Posicion = inicioDigitos });
                }
                // Números sin signo explícito (en este lenguaje los números positivos llevan '+')
                else if (char.IsDigit(c))
                {
                    int inicio = i;
                    bool tienePunto = false;
                    while (i < entrada.Length && (char.IsDigit(entrada[i]) || (entrada[i] == '.' && !tienePunto)))
                    {
                        if (entrada[i] == '.') tienePunto = true;
                        i++;
                    }
                    string num = "+" + entrada.Substring(inicio, i - inicio);
                    list.Add(new TokenSemantico { Lexema = num, Tipo = "NUM", Linea = linea, Posicion = inicio });
                }
                // Operadores Aritméticos (^, *, /, %, +, -)
                else if (c == '^' || c == '*' || c == '/' || c == '%' || c == '+' || c == '-')
                {
                    list.Add(new TokenSemantico { Lexema = c.ToString(), Tipo = "OP_ARIT", Linea = linea, Posicion = i });
                    i++;
                }
                // Cadenas literales "..."
                else if (c == '"' || c == '\'')
                {
                    char comilla = c;
                    int inicio = i;
                    i++;
                    while (i < entrada.Length && entrada[i] != comilla)
                    {
                        i++;
                    }
                    if (i < entrada.Length) i++; // saltar comilla de cierre
                    string str = entrada.Substring(inicio, i - inicio);
                    list.Add(new TokenSemantico { Lexema = str, Tipo = "STR", Linea = linea, Posicion = inicio });
                }
                // Identificadores o palabras reservadas (AND, OR, NOT, etc.)
                else if (char.IsLetter(c) || c == '_')
                {
                    int inicio = i;
                    while (i < entrada.Length && (char.IsLetterOrDigit(entrada[i]) || entrada[i] == '_'))
                    {
                        i++;
                    }
                    string palabra = entrada.Substring(inicio, i - inicio);
                    if (palabra.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
                        palabra.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                        palabra.Equals("NOT", StringComparison.OrdinalIgnoreCase) ||
                        palabra.Equals("LO1", StringComparison.OrdinalIgnoreCase) ||
                        palabra.Equals("LO2", StringComparison.OrdinalIgnoreCase) ||
                        palabra.Equals("LO3", StringComparison.OrdinalIgnoreCase))
                    {
                        string opLogLex = palabra.ToUpper() switch
                        {
                            "LO1" => "AND",
                            "LO2" => "OR",
                            "LO3" => "NOT",
                            _ => palabra.ToUpper()
                        };
                        list.Add(new TokenSemantico { Lexema = opLogLex, Tipo = "OP_LOG", Linea = linea, Posicion = inicio });
                    }
                    else if (palabra.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                             palabra.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
                    {
                        list.Add(new TokenSemantico { Lexema = palabra.ToUpper(), Tipo = "BOOL", Linea = linea, Posicion = inicio });
                    }
                    else
                    {
                        list.Add(new TokenSemantico { Lexema = palabra, Tipo = "ID", Linea = linea, Posicion = inicio });
                    }
                }
                else
                {
                    // Carácter no reconocido
                    i++;
                }
            }

            return list;
        }

        // Comprueba que los paréntesis de apertura y cierre se encuentren balanceados VerificarBalanceParentesis()
        private void VerificarBalanceParentesis()
        {
            int balance = 0;
            foreach (var tok in tokens)
            {
                if (tok.Tipo == "PAR_ABRE") balance++;
                else if (tok.Tipo == "PAR_CIERRA")
                {
                    balance--;
                    if (balance < 0)
                    {
                        ReportarError("Paréntesis de cierre ')' sin su correspondiente paréntesis de apertura '('.", tok.Linea);
                        return;
                    }
                }
            }
            if (balance > 0)
            {
                ReportarError("Paréntesis de apertura '(' no cerrado.", tokens.Last().Linea);
            }
        }

        #endregion

        #region Parser de Jerarquía de Operaciones

        // Parsea una asignación de variable en el nivel 7 de jerarquía ParsearAsignacion()
        private NodoJerarquia ParsearAsignacion()
        {
            // Mirar si es una asignación: ID = Expresion
            if (pos + 1 < tokens.Count && tokens[pos].Tipo == "ID" && tokens[pos + 1].Tipo == "ASIG")
            {
                TokenSemantico idTok = tokens[pos];
                TokenSemantico asigTok = tokens[pos + 1];
                pos += 2;

                NodoJerarquia nodoDestino = new NodoJerarquia(idTok.Lexema, "Identificador", 0, "Variable Destino", ObtenerTipoVariable(idTok.Lexema));
                NodoJerarquia nodoExpresion = ParsearLogico();

                NodoJerarquia nodoAsig = new NodoJerarquia(asigTok.Lexema, "Asignacion", 7, "Nivel 7: Asignación (=)", nodoDestino.TipoDato)
                {
                    Izquierdo = nodoDestino,
                    Derecho = nodoExpresion
                };

                return nodoAsig;
            }

            return ParsearLogico();
        }

        // Parsea expresiones con operadores lógicos en el nivel 6 de jerarquía ParsearLogico()
        private NodoJerarquia ParsearLogico()
        {
            NodoJerarquia izquierdo = ParsearRelacional();

            while (pos < tokens.Count && (tokens[pos].Lexema == "||" || tokens[pos].Lexema.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                                          tokens[pos].Lexema == "&&" || tokens[pos].Lexema.Equals("AND", StringComparison.OrdinalIgnoreCase)))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia derecho = ParsearRelacional();

                string opLex = opTok.Lexema.ToUpper();
                NodoJerarquia nuevo = new NodoJerarquia(opLex, "Operador", 6, "Nivel 6: Lógico (" + opLex + ")", "bool")
                {
                    Izquierdo = izquierdo,
                    Derecho = derecho
                };
                izquierdo = nuevo;
            }

            return izquierdo;
        }

        // Parsea comparaciones con operadores relacionales en el nivel 5 de jerarquía ParsearRelacional()
        private NodoJerarquia ParsearRelacional()
        {
            NodoJerarquia izquierdo = ParsearSumaResta();

            while (pos < tokens.Count && (tokens[pos].Tipo == "OP_REL" ||
                                          tokens[pos].Lexema == "==" || tokens[pos].Lexema == "!=" ||
                                          tokens[pos].Lexema == "<" || tokens[pos].Lexema == "<=" ||
                                          tokens[pos].Lexema == ">" || tokens[pos].Lexema == ">="))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia derecho = ParsearSumaResta();

                NodoJerarquia nuevo = new NodoJerarquia(opTok.Lexema, "Operador", 5, "Nivel 5: Relacional (" + opTok.Lexema + ")", "bool")
                {
                    Izquierdo = izquierdo,
                    Derecho = derecho
                };
                izquierdo = nuevo;
            }

            return izquierdo;
        }

        // Parsea sumas y restas en el nivel 4 de la jerarquía de operadores ParsearSumaResta()
        private NodoJerarquia ParsearSumaResta()
        {
            NodoJerarquia izquierdo = ParsearMultiplicacionDivision();

            while (pos < tokens.Count && (tokens[pos].Lexema == "+" || tokens[pos].Lexema == "-"))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia derecho = ParsearMultiplicacionDivision();

                string desc = opTok.Lexema == "+" ? "Nivel 4: Suma (+)" : "Nivel 4: Resta (-)";
                NodoJerarquia nuevo = new NodoJerarquia(opTok.Lexema, "Operador", 4, desc)
                {
                    Izquierdo = izquierdo,
                    Derecho = derecho
                };
                izquierdo = nuevo;
            }

            return izquierdo;
        }

        // Parsea multiplicaciones, divisiones y módulos en el nivel 3 de jerarquía ParsearMultiplicacionDivision()
        private NodoJerarquia ParsearMultiplicacionDivision()
        {
            NodoJerarquia izquierdo = ParsearPotencia();

            while (pos < tokens.Count && (tokens[pos].Lexema == "*" || tokens[pos].Lexema == "/" || tokens[pos].Lexema == "%"))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia derecho = ParsearPotencia();

                // Verificación semántica inmediata de división entre cero con constantes
                if ((opTok.Lexema == "/" || opTok.Lexema == "%") && derecho != null)
                {
                    if (derecho.TipoNodo == "Constante" &&
                        (derecho.Lexema == "0" || derecho.Lexema == "+0" || derecho.Lexema == "-0" ||
                         derecho.Lexema == "0.0" || derecho.Lexema == "+0.0" || derecho.Lexema == "-0.0"))
                    {
                        derecho.TieneError = true;
                        derecho.MensajeError = "División entre cero no permitida";
                        ReportarError("Error Semántico [Lógica]: División entre cero detectada.", opTok.Linea);
                    }
                }

                string desc = opTok.Lexema switch
                {
                    "*" => "Nivel 3: Multiplicación (*)",
                    "/" => "Nivel 3: División (/)",
                    _ => "Nivel 3: Módulo (%)"
                };

                NodoJerarquia nuevo = new NodoJerarquia(opTok.Lexema, "Operador", 3, desc)
                {
                    Izquierdo = izquierdo,
                    Derecho = derecho
                };
                izquierdo = nuevo;
            }

            return izquierdo;
        }

        // Parsea potenciación y operadores unarios en el nivel 2 de jerarquía ParsearPotencia()
        private NodoJerarquia ParsearPotencia()
        {
            // Operadores lógicos unarios (! o NOT)
            if (pos < tokens.Count && (tokens[pos].Lexema == "!" || tokens[pos].Lexema.Equals("NOT", StringComparison.OrdinalIgnoreCase)))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia operando = ParsearPotencia();
                string desc = $"Nivel 2: Negación Lógica ({opTok.Lexema})";
                string? valCalc = null;
                string valOperando = operando.ValorCalculado?.Trim() ?? "";
                if (valOperando.Equals("TRUE", StringComparison.OrdinalIgnoreCase)) valCalc = "FALSE";
                else if (valOperando.Equals("FALSE", StringComparison.OrdinalIgnoreCase)) valCalc = "TRUE";
                else if (bool.TryParse(valOperando, out bool bVal)) valCalc = bVal ? "FALSE" : "TRUE";

                return new NodoJerarquia(opTok.Lexema + " (Unario)", "Operador", 2, desc, "bool")
                {
                    Izquierdo = operando,
                    ValorCalculado = valCalc
                };
            }

            // Operadores unarios (+ o -): ej. -(a + b), +(a + b), -x, +x, -9, +5
            if (pos < tokens.Count && (tokens[pos].Lexema == "-" || tokens[pos].Lexema == "+") &&
                (pos == 0 || tokens[pos - 1].Tipo == "OP_ARIT" || tokens[pos - 1].Tipo == "ASIG" || tokens[pos - 1].Tipo == "PAR_ABRE" || tokens[pos - 1].Tipo == "OP_REL" || tokens[pos - 1].Tipo == "OP_LOG"))
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                NodoJerarquia operando = ParsearPotencia();
                string desc = opTok.Lexema == "-" ? "Nivel 2: Menos Unario (-)" : "Nivel 2: Más Unario (+)";
                string? valCalc = null;
                if (double.TryParse(operando.ValorCalculado, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                {
                    double resVal = opTok.Lexema == "-" ? -v : v;
                    valCalc = FormatearNumeroConSigno(resVal, operando.TipoDato == "int");
                }

                return new NodoJerarquia(opTok.Lexema + " (Unario)", "Operador", 2, desc, operando.TipoDato)
                {
                    Izquierdo = operando,
                    ValorCalculado = valCalc
                };
            }

            NodoJerarquia primario = ParsearPrimario();

            if (pos < tokens.Count && tokens[pos].Lexema == "^")
            {
                TokenSemantico opTok = tokens[pos];
                pos++;
                // La potencia es asociativa por la derecha
                NodoJerarquia exponente = ParsearPotencia();

                return new NodoJerarquia("^", "Operador", 2, "Nivel 2: Potencia (^)")
                {
                    Izquierdo = primario,
                    Derecho = exponente
                };
            }

            return primario;
        }

        // Parsea operandos primarios, constantes, variables o subexpresiones entre paréntesis ParsearPrimario()
        private NodoJerarquia ParsearPrimario()
        {
            if (pos >= tokens.Count)
            {
                ReportarError("Se esperaba un operando o subexpresión pero se llegó al final de la línea.");
                return new NodoJerarquia("?", "Error", 0, "Operando faltante") { TieneError = true, MensajeError = "Operando faltante" };
            }

            TokenSemantico tok = tokens[pos];

            // 1. NIVEL MÁXIMO DE PRIORIDAD: PARÉNTESIS '( ... )'
            if (tok.Tipo == "PAR_ABRE")
            {
                pos++; // consumir '('

                if (pos < tokens.Count && tokens[pos].Tipo == "PAR_CIERRA")
                {
                    ReportarError("Expresión vacía dentro de paréntesis '()'.", tok.Linea);
                    pos++;
                    return new NodoJerarquia("()", "Error", 1, "Paréntesis vacíos") { TieneError = true, MensajeError = "Paréntesis vacíos" };
                }

                // Analizar la expresión completa contenida dentro del paréntesis
                NodoJerarquia contenidoParentesis = ParsearLogico();

                if (pos < tokens.Count && tokens[pos].Tipo == "PAR_CIERRA")
                {
                    pos++; // consumir ')'
                }
                else
                {
                    ReportarError("Falta cerrar paréntesis ')' correspondiente.", tok.Linea);
                }

                // Crear el nodo de agrupación que representa explícitamente el Nivel 1 de Jerarquía
                NodoJerarquia nodoAgrupacion = new NodoJerarquia("( )", "Agrupacion", 1, "Nivel 1: Paréntesis (Máxima Prioridad)", contenidoParentesis.TipoDato)
                {
                    Izquierdo = contenidoParentesis,
                    ValorCalculado = contenidoParentesis.ValorCalculado
                };

                return nodoAgrupacion;
            }

            // 2. CONSTANTES BOOLEANAS (TRUE, FALSE)
            if (tok.Tipo == "BOOL" ||
                tok.Lexema.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                tok.Lexema.Equals("FALSE", StringComparison.OrdinalIgnoreCase))
            {
                pos++;
                string valBool = tok.Lexema.ToUpper();
                return new NodoJerarquia(valBool, "Constante", 0, "Operando Constante Booleana", "bool")
                {
                    ValorCalculado = valBool
                };
            }

            // 3. CONSTANTES NUMÉRICAS
            if (tok.Tipo == "NUM")
            {
                pos++;
                string tipoNum = tok.Lexema.Contains('.') ? "float" : "int";
                return new NodoJerarquia(tok.Lexema, "Constante", 0, "Operando Constante Numérica", tipoNum)
                {
                    ValorCalculado = tok.Lexema
                };
            }

            // 4. CADENAS
            if (tok.Tipo == "STR")
            {
                pos++;
                return new NodoJerarquia(tok.Lexema, "Constante", 0, "Operando Constante Texto", "string")
                {
                    ValorCalculado = tok.Lexema
                };
            }

            // 5. IDENTIFICADORES O LLAMADAS A FUNCIÓN: SUMA(X, Y) o SUMA()
            if (tok.Tipo == "ID")
            {
                string idNombre = tok.Lexema;
                pos++;

                // Si viene '(' inmediatamente después del ID -> Llamada a Función
                if (pos < tokens.Count && tokens[pos].Tipo == "PAR_ABRE")
                {
                    pos++; // consumir '('
                    List<NodoJerarquia> argumentos = new List<NodoJerarquia>();

                    if (pos < tokens.Count && tokens[pos].Tipo == "PAR_CIERRA")
                    {
                        pos++; // consumir ')'
                    }
                    else
                    {
                        argumentos.Add(ParsearLogico());
                        while (pos < tokens.Count && (tokens[pos].Tipo == "COMA" || tokens[pos].Lexema == ","))
                        {
                            pos++; // consumir ','
                            argumentos.Add(ParsearLogico());
                        }

                        if (pos < tokens.Count && tokens[pos].Tipo == "PAR_CIERRA")
                        {
                            pos++; // consumir ')'
                        }
                        else
                        {
                            ReportarError($"Se esperaba ')' al cerrar los argumentos de la función '{idNombre}'.", tok.Linea);
                        }
                    }

                    // Se crea el nodo función con retorno dinámico funcional (string, bool, float, int)
                    string tipoRet = ObtenerRetornoFuncion(idNombre);
                    string? valRet = ObtenerValorRetornoFuncion(idNombre);
                    if (string.IsNullOrEmpty(valRet))
                    {
                        valRet = tipoRet switch
                        {
                            "string" => "\"\"",
                            "bool" => "TRUE",
                            "float" => "+0.0",
                            _ => "0"
                        };
                    }

                    NodoJerarquia nodoFunc = new NodoJerarquia(idNombre, "Funcion", 0, $"Llamada Función {idNombre}()", tipoRet)
                    {
                        Hijos = argumentos,
                        ValorCalculado = valRet // Valor representativo o inferido para operar en expresiones
                    };
                    return nodoFunc;
                }

                string tipoVar = ObtenerTipoVariable(idNombre);
                string? valVar = ObtenerValorVariable(idNombre);
                return new NodoJerarquia(idNombre, "Identificador", 0, "Operando Variable", tipoVar)
                {
                    ValorCalculado = valVar
                };
            }

            // Si hay un operador o carácter inesperado
            ReportarError($"Operando no válido o inesperado: '{tok.Lexema}'", tok.Linea);
            pos++;
            return new NodoJerarquia(tok.Lexema, "Error", 0, "No válido") { TieneError = true, MensajeError = $"Carácter no válido '{tok.Lexema}'" };
        }

        #endregion

        #region Verificación Semántica de Tipos y Pasos de Reducción

        // Consulta el tipo de dato asignado a una variable en la tabla interna ObtenerTipoVariable()
        private string ObtenerTipoVariable(string nombreVar)
        {
            if (tablaTipos.TryGetValue(nombreVar, out string? tipo) && !string.IsNullOrEmpty(tipo) && tipo != "Null")
            {
                return tipo;
            }
            return "desconocido";
        }

        // Consulta el valor almacenado de una variable en la tabla interna ObtenerValorVariable()
        private string? ObtenerValorVariable(string nombreVar)
        {
            if (tablaValores.TryGetValue(nombreVar, out string? val) && !string.IsNullOrEmpty(val))
            {
                return val;
            }
            return null;
        }

        // Realiza inferencia de tipos y calcula el valor resultante en cada nodo del árbol InferirTiposYValores()
        private void InferirTiposYValores(NodoJerarquia nodo)
        {
            if (nodo == null) return;

            // Manejo especial de Asignación: el lado izquierdo es la variable de destino
            if (nodo.TipoNodo == "Asignacion")
            {
                if (nodo.Derecho != null)
                {
                    InferirTiposYValores(nodo.Derecho);
                    nodo.TipoDato = nodo.Derecho.TipoDato;
                    nodo.ValorCalculado = nodo.Derecho.ValorCalculado;
                    nodo.TieneError = nodo.Derecho.TieneError;

                    if (nodo.Izquierdo != null)
                    {
                        nodo.Izquierdo.TipoDato = nodo.Derecho.TipoDato;
                        nodo.Izquierdo.ValorCalculado = nodo.Derecho.ValorCalculado;
                        nodo.Izquierdo.TieneError = nodo.Derecho.TieneError;

                        if (!nodo.Derecho.TieneError && nodo.Derecho.TipoDato != "desconocido")
                        {
                            RegistrarVariable(nodo.Izquierdo.Lexema, nodo.Derecho.TipoDato, nodo.Derecho.ValorCalculado);
                        }
                        else
                        {
                            RegistrarVariable(nodo.Izquierdo.Lexema, "desconocido", null);
                        }
                    }
                }
                return;
            }

            // Para los demás nodos, evaluar recursivamente los operandos
            if (nodo.Izquierdo != null) InferirTiposYValores(nodo.Izquierdo);
            if (nodo.Derecho != null) InferirTiposYValores(nodo.Derecho);

            if (nodo.TipoNodo == "Agrupacion")
            {
                if (nodo.Izquierdo != null)
                {
                    nodo.TipoDato = nodo.Izquierdo.TipoDato;
                    nodo.ValorCalculado = nodo.Izquierdo.ValorCalculado;
                    nodo.TieneError = nodo.Izquierdo.TieneError;
                    nodo.MensajeError = nodo.Izquierdo.MensajeError;
                }
                return;
            }

            // Las llamadas a funciones ejecutan su cuerpo dinámicamente con los argumentos
            if (nodo.TipoNodo == "Funcion")
            {
                foreach (var h in nodo.Hijos)
                {
                    InferirTiposYValores(h);
                }

                var (tipoRet, valRet) = EjecutarFuncion(nodo.Lexema, nodo.Hijos);
                nodo.TipoDato = tipoRet;
                nodo.ValorCalculado = valRet;

                RegistrarRetornoFuncion(nodo.Lexema, tipoRet, valRet);
                RegistrarVariable(nodo.Lexema, tipoRet, valRet);
                return;
            }

            // Verificación semántica de variables cuando se leen como operando
            if (nodo.TipoNodo == "Identificador")
            {
                string nombre = nodo.Lexema;
                bool registrada = TieneVariable(nombre);
                string? val = ObtenerValorVariable(nombre);
                string tipo = ObtenerTipoVariable(nombre);

                if (!registrada)
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    nodo.MensajeError = $"Error Semántico [Alcance]: La variable '{nombre}' no está declarada ni inicializada en el ámbito.";
                    ReportarError(nodo.MensajeError, lineaActual);
                }
                else if (tipo == "desconocido" || string.IsNullOrEmpty(val) || val == "Null")
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    nodo.MensajeError = $"Error Semántico [Inicialización]: La variable '{nombre}' se utiliza sin haber sido inicializada o contiene un error previo.";
                    ReportarError(nodo.MensajeError, lineaActual);
                }
                else
                {
                    nodo.TipoDato = tipo;
                    nodo.ValorCalculado = val;
                }
                return;
            }

            if (nodo.TipoNodo == "Operador")
            {
                // Si algún operando tiene error, propagar el error y detener la operación
                if (nodo.Izquierdo != null && nodo.Izquierdo.TieneError)
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    return;
                }
                if (nodo.Derecho != null && nodo.Derecho.TieneError)
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    return;
                }

                // Operadores unarios (+, -, !, NOT)
                if (nodo.Derecho == null)
                {
                    if (nodo.Lexema.Contains("!") || nodo.Lexema.Contains("NOT"))
                    {
                        nodo.TipoDato = "bool";
                        string valIzq = nodo.Izquierdo?.ValorCalculado?.Trim() ?? "";
                        if (valIzq.Equals("TRUE", StringComparison.OrdinalIgnoreCase) || valIzq.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            nodo.ValorCalculado = "FALSE";
                        }
                        else if (valIzq.Equals("FALSE", StringComparison.OrdinalIgnoreCase) || valIzq.Equals("false", StringComparison.OrdinalIgnoreCase))
                        {
                            nodo.ValorCalculado = "TRUE";
                        }
                        else if (bool.TryParse(valIzq, out bool bVal))
                        {
                            nodo.ValorCalculado = bVal ? "FALSE" : "TRUE";
                        }
                    }
                    else
                    {
                        nodo.TipoDato = nodo.Izquierdo?.TipoDato ?? "int";
                        if (double.TryParse(nodo.Izquierdo?.ValorCalculado, NumberStyles.Any, CultureInfo.InvariantCulture, out double v))
                        {
                            double resVal = nodo.Lexema.StartsWith("-") ? -v : v;
                            nodo.ValorCalculado = FormatearNumeroConSigno(resVal, nodo.TipoDato == "int");
                        }
                    }
                    return;
                }

                string tipoIzq = nodo.Izquierdo?.TipoDato ?? "desconocido";
                string tipoDer = nodo.Derecho?.TipoDato ?? "desconocido";

                // Verificación de tipos en cadenas
                if (tipoIzq == "string" || tipoDer == "string")
                {
                    if (nodo.Lexema != "+")
                    {
                        nodo.TieneError = true;
                        nodo.TipoDato = "desconocido";
                        nodo.MensajeError = $"Error Semántico [Tipo de Datos]: Incompatibilidad de tipos: no se puede aplicar el operador '{nodo.Lexema}' a cadenas de texto.";
                        ReportarError(nodo.MensajeError, lineaActual);
                        return;
                    }
                    else
                    {
                        nodo.TipoDato = "string";
                    }
                }
                else if (tipoIzq == "bool" || tipoDer == "bool")
                {
                    if (nodo.NivelJerarquia == 5 || nodo.NivelJerarquia == 6)
                    {
                        nodo.TipoDato = "bool";
                    }
                    else
                    {
                        nodo.TieneError = true;
                        nodo.TipoDato = "desconocido";
                        nodo.MensajeError = $"Error Semántico [Tipo de Datos]: Incompatibilidad de tipos: no se puede aplicar el operador aritmético '{nodo.Lexema}' a valores booleanos ('{tipoIzq}' {nodo.Lexema} '{tipoDer}').";
                        ReportarError(nodo.MensajeError, lineaActual);
                        return;
                    }
                }
                else if (tipoIzq == "desconocido" || tipoDer == "desconocido")
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    nodo.MensajeError = $"Error Semántico [Tipo de Datos]: Incompatibilidad o tipo desconocido en la operación '{nodo.Lexema}'.";
                    ReportarError(nodo.MensajeError, lineaActual);
                    return;
                }
                else
                {
                    // Ambos son numéricos
                    if (nodo.NivelJerarquia == 5 || nodo.NivelJerarquia == 6)
                    {
                        nodo.TipoDato = "bool";
                    }
                    else
                    {
                        // Operación aritmética (+, -, *, /, ^)
                        if (tipoIzq == "float" || tipoDer == "float" || nodo.Lexema == "/")
                        {
                            nodo.TipoDato = "float";
                        }
                        else
                        {
                            nodo.TipoDato = "int";
                        }
                    }
                }

                // Cálculo constante si ambos lados tienen valor
                string valIzqStr = nodo.Izquierdo?.ValorCalculado ?? "";
                string valDerStr = nodo.Derecho?.ValorCalculado ?? "";

                // Validación semántica de División entre cero (Error de Lógica)
                if ((nodo.Lexema == "/" || nodo.Lexema == "%") && double.TryParse(valDerStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double dCero) && dCero == 0)
                {
                    nodo.TieneError = true;
                    nodo.TipoDato = "desconocido";
                    nodo.MensajeError = "Error Semántico [Lógica]: División entre cero detectada.";
                    ReportarError(nodo.MensajeError, lineaActual);
                    return;
                }

                bool esBoolIzq = bool.TryParse(valIzqStr, out bool bIzq);
                bool esBoolDer = bool.TryParse(valDerStr, out bool bDer);

                if (nodo.TipoDato == "string" && nodo.Lexema == "+")
                {
                    try
                    {
                        string sIzq = LimpiarValorCadena(valIzqStr);
                        string sDer = LimpiarValorCadena(valDerStr);
                        nodo.ValorCalculado = $"\"{sIzq}{sDer}\"";
                    }
                    catch { }
                }
                else if (esBoolIzq && esBoolDer)
                {
                    try
                    {
                        if (nodo.NivelJerarquia == 5)
                        {
                            bool resBool = nodo.Lexema switch
                            {
                                "==" => bIzq == bDer,
                                "!=" => bIzq != bDer,
                                _ => false
                            };
                            nodo.ValorCalculado = resBool ? "TRUE" : "FALSE";
                        }
                        else if (nodo.NivelJerarquia == 6)
                        {
                            bool resBool = (nodo.Lexema == "&&" || nodo.Lexema == "AND") ? (bIzq && bDer) : (bIzq || bDer);
                            nodo.ValorCalculado = resBool ? "TRUE" : "FALSE";
                        }
                    }
                    catch { }
                }
                else if (double.TryParse(valIzqStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double valIzq) &&
                         double.TryParse(valDerStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double valDer))
                {
                    try
                    {
                        if (nodo.NivelJerarquia == 5)
                        {
                            bool resBool = nodo.Lexema switch
                            {
                                "<" => valIzq < valDer,
                                "<=" => valIzq <= valDer,
                                ">" => valIzq > valDer,
                                ">=" => valIzq >= valDer,
                                "==" => Math.Abs(valIzq - valDer) < 0.000001,
                                "!=" => Math.Abs(valIzq - valDer) >= 0.000001,
                                _ => false
                            };
                            nodo.ValorCalculado = resBool ? "TRUE" : "FALSE";
                        }
                        else if (nodo.NivelJerarquia == 6)
                        {
                            bool bIzqNum = valIzq != 0;
                            bool bDerNum = valDer != 0;
                            bool resBool = (nodo.Lexema == "&&" || nodo.Lexema == "AND") ? (bIzqNum && bDerNum) : (bIzqNum || bDerNum);
                            nodo.ValorCalculado = resBool ? "TRUE" : "FALSE";
                        }
                        else
                        {
                            double resultadoCalc = nodo.Lexema switch
                            {
                                "+" => valIzq + valDer,
                                "-" => valIzq - valDer,
                                "*" => valIzq * valDer,
                                "/" => valDer != 0 ? valIzq / valDer : 0,
                                "%" => valDer != 0 ? valIzq % valDer : 0,
                                "^" => Math.Pow(valIzq, valDer),
                                _ => 0
                            };

                            // Comprobación de desbordamiento de memoria / recurso numérico (Punto 1.7)
                            if (double.IsInfinity(resultadoCalc) || double.IsNaN(resultadoCalc) || (nodo.TipoDato == "int" && Math.Abs(resultadoCalc) > int.MaxValue))
                            {
                                nodo.TieneError = true;
                                nodo.TipoDato = "desconocido";
                                nodo.MensajeError = "Error Semántico [Recurso]: Desbordamiento de memoria o valor numérico fuera del rango soportado.";
                                ReportarError(nodo.MensajeError, lineaActual);
                                return;
                            }

                            nodo.ValorCalculado = FormatearNumeroConSigno(resultadoCalc, nodo.TipoDato == "int");
                        }
                    }
                    catch { }
                }
            }
        }

        // Formatea un número numérico incluyendo explícitamente su signo (+/-) FormatearNumeroConSigno()
        private string FormatearNumeroConSigno(double valor, bool esEntero)
        {
            if (esEntero)
            {
                long vLong = (long)Math.Round(valor);
                return vLong >= 0 ? "+" + vLong : vLong.ToString();
            }
            else
            {
                string s = valor.ToString("0.##", CultureInfo.InvariantCulture);
                return (valor >= 0 && !s.StartsWith("+")) ? "+" + s : s;
            }
        }

        // Limpia comillas envolventes o signos sobrantes de valores para concatenación de cadenas
        private string LimpiarValorCadena(string? val)
        {
            if (string.IsNullOrEmpty(val) || val == "Null") return "";
            string s = val.Trim();
            if (s.StartsWith("\"") && s.EndsWith("\"") && s.Length >= 2)
            {
                return s.Substring(1, s.Length - 2);
            }
            if (s.StartsWith("'") && s.EndsWith("'") && s.Length >= 2)
            {
                return s.Substring(1, s.Length - 2);
            }
            if (s.StartsWith("+") && double.TryParse(s.Substring(1), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return s.Substring(1);
            }
            return s;
        }

        // Recorre el árbol en post-orden generando los pasos secuenciales de resolución GenerarPasosJerarquia()
        private void GenerarPasosJerarquia(NodoJerarquia nodo)
        {
            if (nodo == null) return;

            // Recorrido en post-orden: primero los subárboles de mayor prioridad (dentro de paréntesis o ramas inferiores)
            if (nodo.Izquierdo != null) GenerarPasosJerarquia(nodo.Izquierdo);
            if (nodo.Derecho != null) GenerarPasosJerarquia(nodo.Derecho);
            foreach (var h in nodo.Hijos) GenerarPasosJerarquia(h);

            if (nodo.TipoNodo == "Agrupacion")
            {
                string valor = !string.IsNullOrEmpty(nodo.ValorCalculado) ? $" -> Resuelto: {nodo.ValorCalculado}" : "";
                pasos.Add($"Paso {pasoContador++} [NIVEL 1 - PARÉNTESIS PRIORITARIO]: Evaluar subexpresión agrupada ( ) {valor}");
            }
            else if (nodo.TipoNodo == "Operador")
            {
                string izqStr = nodo.Izquierdo?.ValorCalculado ?? nodo.Izquierdo?.Lexema ?? "?";
                string resStr = !string.IsNullOrEmpty(nodo.ValorCalculado) ? $" = {nodo.ValorCalculado}" : "";

                if (nodo.Derecho == null)
                {
                    // Operador unario (+ o -)
                    pasos.Add($"Paso {pasoContador++} [{nodo.DescripcionJerarquia.ToUpper()}]: Operar {nodo.Lexema} {izqStr}{resStr} (Tipo: {nodo.TipoDato})");
                }
                else
                {
                    string derStr = nodo.Derecho.ValorCalculado ?? nodo.Derecho.Lexema ?? "?";
                    pasos.Add($"Paso {pasoContador++} [{nodo.DescripcionJerarquia.ToUpper()}]: Operar {izqStr} {nodo.Lexema} {derStr}{resStr} (Tipo: {nodo.TipoDato})");
                }
            }
            else if (nodo.TipoNodo == "Asignacion")
            {
                string dest = nodo.Izquierdo?.Lexema ?? "variable";
                string val = nodo.Derecho?.ValorCalculado ?? "expresión";
                pasos.Add($"Paso {pasoContador++} [NIVEL 7 - ASIGNACIÓN]: Guardar resultado '{val}' en variable destino '{dest}'");
            }
            else if (nodo.TipoNodo == "Funcion")
            {
                string argsStr = string.Join(", ", nodo.Hijos.Select(h => h.ValorCalculado ?? h.Lexema));
                string resStr = !string.IsNullOrEmpty(nodo.ValorCalculado) ? $" = {nodo.ValorCalculado}" : "";
                pasos.Add($"Paso {pasoContador++} [LLAMADA A FUNCIÓN]: Invocar {nodo.Lexema}({argsStr}){resStr} (Tipo: {nodo.TipoDato})");
            }
        }

        // Registra un error semántico con su línea correspondiente en la lista de errores ReportarError()
        private void ReportarError(string mensaje, int? linea = null)
        {
            int lin = linea ?? lineaActual;
            string msgConPrefijo = mensaje.StartsWith("Error Semántico") ? mensaje : $"Error Semántico: {mensaje}";
            errores.Add($"Línea {lin}: {msgConPrefijo}");
        }

        #endregion

        // Carga y despliega visualmente el árbol jerárquico de una operación en el TreeView CargarArbolEnTreeView()
        public void CargarArbolEnTreeView(TreeView tv, NodoJerarquia? raiz, string tituloOperacion)
        {
            tv.BeginUpdate();
            tv.Nodes.Clear();

            TreeNode nodoRaizPrincipal = new TreeNode($"🌳 Operación: {tituloOperacion}")
            {
                ForeColor = System.Drawing.Color.DarkSlateBlue
            };

            if (raiz != null)
            {
                nodoRaizPrincipal.Nodes.Add(raiz.ToTreeNode());
            }
            else
            {
                TreeNode errorNode = new TreeNode("❌ No se pudo generar el árbol jerárquico.")
                {
                    ForeColor = System.Drawing.Color.Red
                };
                nodoRaizPrincipal.Nodes.Add(errorNode);
            }

            tv.Nodes.Add(nodoRaizPrincipal);
            tv.ExpandAll();
            tv.EndUpdate();
        }

        // Escanea el código fuente completo y extrae todas las operaciones a evaluar ExtraerOperacionesDeCodigoFuente()
        public List<ResultadoSemanticoOperacion> ExtraerOperacionesDeCodigoFuente(string codigoFuente)
        {
            List<ResultadoSemanticoOperacion> lista = new List<ResultadoSemanticoOperacion>();
            if (string.IsNullOrWhiteSpace(codigoFuente)) return lista;

            string[] lineas = codigoFuente.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            bool dentroDeFuncion = false;
            DefinicionFuncion? defActual = null;

            for (int i = 0; i < lineas.Length; i++)
            {
                string lineaTexto = lineas[i].Trim();
                if (string.IsNullOrEmpty(lineaTexto)) continue;

                // Ignorar comentarios
                if (lineaTexto.StartsWith("//") || lineaTexto.StartsWith("/*")) continue;

                // Eliminar comillas envolventes de la línea completa si existen, y punto y coma final
                string lineaLimpia = lineaTexto.Trim();
                if (lineaLimpia.StartsWith("\"") && lineaLimpia.EndsWith("\"") && lineaLimpia.Length >= 2)
                {
                    lineaLimpia = lineaLimpia.Substring(1, lineaLimpia.Length - 2).Trim();
                }
                string lineaSinPuntoComa = lineaLimpia.TrimEnd(';').Trim();
                if (string.IsNullOrEmpty(lineaSinPuntoComa)) continue;

                // Detección de inicio de función: EXECUTE SUMA(X, Y), FUNCTION SALUDAR(NOM), DEF ...
                var matchFunc = System.Text.RegularExpressions.Regex.Match(lineaSinPuntoComa, @"^\s*(EXECUTE|FUNCTION|DEF)\s+([A-Za-z0-9_]+)\s*(?:\(([^)]*)\))?", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (matchFunc.Success)
                {
                    dentroDeFuncion = true;
                    string nombreFunc = matchFunc.Groups[2].Value;
                    string rawParams = matchFunc.Groups[3].Value;
                    List<string> parms = new List<string>();
                    if (!string.IsNullOrWhiteSpace(rawParams))
                    {
                        parms = rawParams.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(p => p.Trim().Split(' ').Last())
                                         .Where(p => !string.IsNullOrEmpty(p))
                                         .ToList();
                    }
                    defActual = new DefinicionFuncion
                    {
                        Nombre = nombreFunc,
                        Parametros = parms
                    };
                    continue;
                }

                if (dentroDeFuncion)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*(ENDEXECUTE|ENDFUNCTION|ENDDEF|ENDDO)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        dentroDeFuncion = false;
                        if (defActual != null)
                        {
                            RegistrarDefinicionFuncion(defActual);
                            defActual = null;
                        }
                        continue;
                    }

                    if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*RETURN\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        string exprRet = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*RETURN\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                        if (defActual != null)
                        {
                            defActual.ExpresionRetorno = exprRet;
                        }
                        continue;
                    }

                    if (defActual != null && !string.IsNullOrWhiteSpace(lineaSinPuntoComa))
                    {
                        defActual.Instrucciones.Add(lineaSinPuntoComa);
                    }
                    continue;
                }

                // 1. Asignación: ID = Expresion (asegurar que no sea IF, WHILE, FOR, etc.)
                bool esEstructuraControl = System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*\b(IF|WHILE|FOR|PRINT|READ)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                int idxIgual = lineaSinPuntoComa.IndexOf('=');
                if (idxIgual > 0 && !lineaSinPuntoComa.Substring(0, idxIgual).Contains("==") && !esEstructuraControl)
                {
                    // En un FOR: FOR contador = FROM +1 UNTIL contador <= +10 INTERVAL +1
                    if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"\bFOR\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase) &&
                        System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"\bFROM\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                    {
                        int idxFrom = lineaSinPuntoComa.ToUpper().IndexOf("FROM");
                        int idxUntil = lineaSinPuntoComa.ToUpper().IndexOf("UNTIL");
                        if (idxFrom > 0)
                        {
                            string asigFor = lineaSinPuntoComa.Substring(0, idxFrom).Replace("FOR", "", StringComparison.OrdinalIgnoreCase).Trim();
                            if (idxUntil > idxFrom)
                            {
                                string valFrom = lineaSinPuntoComa.Substring(idxFrom + 4, idxUntil - (idxFrom + 4)).Trim();
                                var resFor = AnalizarExpresion(asigFor + " " + valFrom, i + 1);
                                lista.Add(resFor);
                            }
                        }
                        if (idxUntil > 0)
                        {
                            int idxInterval = lineaSinPuntoComa.ToUpper().IndexOf("INTERVAL");
                            string condUntil = idxInterval > idxUntil
                                ? lineaSinPuntoComa.Substring(idxUntil + 5, idxInterval - (idxUntil + 5)).Trim()
                                : lineaSinPuntoComa.Substring(idxUntil + 5).Trim();
                            var resCond = AnalizarExpresion(condUntil, i + 1);
                            lista.Add(resCond);
                        }
                        continue;
                    }
                    else
                    {
                        var resAsig = AnalizarExpresion(lineaSinPuntoComa, i + 1);
                        lista.Add(resAsig);
                        continue;
                    }
                }

                // 2. PRINT Expresion
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*PRINT\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string exprPrint = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*PRINT\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrEmpty(exprPrint))
                    {
                        var resPrint = AnalizarExpresion(exprPrint, i + 1);
                        lista.Add(resPrint);
                    }
                    continue;
                }

                // 3. IF Condicion THEN
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*IF\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string cond = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*IF\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    int idxThen = cond.ToUpper().IndexOf("THEN");
                    if (idxThen > 0) cond = cond.Substring(0, idxThen).Trim();

                    if (!string.IsNullOrEmpty(cond))
                    {
                        var resIf = AnalizarExpresion(cond, i + 1);
                        lista.Add(resIf);
                    }
                    continue;
                }

                // 4. WHILE Condicion DO
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*WHILE\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string cond = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*WHILE\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    int idxDo = cond.ToUpper().IndexOf("DO");
                    if (idxDo > 0) cond = cond.Substring(0, idxDo).Trim();

                    if (!string.IsNullOrEmpty(cond))
                    {
                        var resWhile = AnalizarExpresion(cond, i + 1);
                        lista.Add(resWhile);
                    }
                    continue;
                }

                // 4b. DO WHILE Condicion
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*DO\s+WHILE\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string cond = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*DO\s+WHILE\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrEmpty(cond))
                    {
                        var resDoWhile = AnalizarExpresion(cond, i + 1);
                        lista.Add(resDoWhile);
                    }
                    continue;
                }

                // 4c. UNTIL Condicion (en EXECUTE)
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*UNTIL\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string cond = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*UNTIL\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrEmpty(cond))
                    {
                        var resUntil = AnalizarExpresion(cond, i + 1);
                        lista.Add(resUntil);
                    }
                    continue;
                }

                // 4d. RETURN Expresion (en funciones)
                if (System.Text.RegularExpressions.Regex.IsMatch(lineaSinPuntoComa, @"^\s*RETURN\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    string exprRet = System.Text.RegularExpressions.Regex.Replace(lineaSinPuntoComa, @"^\s*RETURN\b", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Trim();
                    if (!string.IsNullOrEmpty(exprRet))
                    {
                        var resRet = AnalizarExpresion(exprRet, i + 1);
                        lista.Add(resRet);
                    }
                    continue;
                }

                // 5. Palabras reservadas que no son operaciones
                string primeraPalabra = lineaSinPuntoComa.Split(' ')[0].ToUpper();
                if (primeraPalabra == "START" || primeraPalabra == "END" || primeraPalabra == "ELSE" ||
                    primeraPalabra == "ENDIF" || primeraPalabra == "ENDWHILE" ||
                    primeraPalabra == "ENDFOR" || primeraPalabra == "READ" || primeraPalabra == "DO" ||
                    primeraPalabra == "ENDDO" || primeraPalabra == "EXECUTE" || primeraPalabra == "ENDEXECUTE" ||
                    primeraPalabra == "FUNCTION" || primeraPalabra == "ENDFUNCTION" || primeraPalabra == "DEF" || primeraPalabra == "ENDDEF" ||
                    primeraPalabra == "DEL" || primeraPalabra == "PERHAPS" || primeraPalabra == "CASE" ||
                    primeraPalabra == "NONE" || primeraPalabra == "ENDCASE" || primeraPalabra == "FOR" ||
                    primeraPalabra == "FROM" || primeraPalabra == "SET" || primeraPalabra == "UNTIL" ||
                    primeraPalabra == "INTERVAL")
                {
                    continue;
                }

                // 6. Operación o expresión aritmética suelta que el usuario haya escrito en el editor
                // Ejemplo: ( +5 + +3 ) * -2  o  -9 + +4  o  +10 / +2  o  cualquier problema matemático
                if (lineaSinPuntoComa.Length > 0)
                {
                    var resSuelto = AnalizarExpresion(lineaSinPuntoComa, i + 1);
                    if (resSuelto.ArbolRaiz != null)
                    {
                        lista.Add(resSuelto);
                    }
                }
            }

            return lista;
        }

        // Despliega todas las operaciones analizadas de forma sincronizada en el TreeView CargarArbolSincronizado()
        public void CargarArbolSincronizado(TreeView tv, List<ResultadoSemanticoOperacion> operaciones)
        {
            tv.BeginUpdate();
            tv.Nodes.Clear();

            if (operaciones.Count == 0)
            {
                TreeNode nodoVacio = new TreeNode("📝 Escribe una operación o programa en el Programa Fuente...")
                {
                    ForeColor = System.Drawing.Color.Gray
                };
                tv.Nodes.Add(nodoVacio);
                tv.EndUpdate();
                return;
            }

            if (operaciones.Count == 1)
            {
                var op = operaciones[0];
                TreeNode raizOperacion = new TreeNode($"🌳 Operación: {op.ExpresionOriginal}")
                {
                    ForeColor = op.EsValida ? System.Drawing.Color.DarkSlateBlue : System.Drawing.Color.Red,
                    Tag = op
                };

                if (op.ArbolRaiz != null)
                {
                    raizOperacion.Nodes.Add(op.ArbolRaiz.ToTreeNode());
                }
                tv.Nodes.Add(raizOperacion);
            }
            else
            {
                TreeNode nodoPrograma = new TreeNode($"🌳 Programa Fuente ({operaciones.Count} operaciones)")
                {
                    ForeColor = System.Drawing.Color.DarkSlateBlue
                };

                for (int i = 0; i < operaciones.Count; i++)
                {
                    var op = operaciones[i];
                    string estado = op.EsValida ? $"[Tipo: {op.TipoDatoResultante}]" : "[❌ Error Semántico]";
                    TreeNode nodoOp = new TreeNode($"📄 Línea: {op.ExpresionOriginal}  {estado}")
                    {
                        ForeColor = op.EsValida ? System.Drawing.Color.Navy : System.Drawing.Color.Red,
                        Tag = op
                    };

                    if (op.ArbolRaiz != null)
                    {
                        nodoOp.Nodes.Add(op.ArbolRaiz.ToTreeNode());
                    }

                    nodoPrograma.Nodes.Add(nodoOp);
                }

                tv.Nodes.Add(nodoPrograma);
            }

            tv.ExpandAll();
            tv.EndUpdate();
        }
    }
}
