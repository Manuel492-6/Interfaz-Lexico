using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    public class TokenSintactico
    {
        public string Tipo { get; set; }
        public string Lexema { get; set; }
        public int Linea { get; set; }
    }

    public class AnalizadorSintactico
    {
        private List<TokenSintactico> tokens;
        private int pos;
        private DataGridView dgtErrores;
        private AnalizadorSemanticoJerarquia semantico;
        public List<ResultadoSemanticoOperacion> OperacionesDetectadas { get; } = new List<ResultadoSemanticoOperacion>();
        public List<Cuadruplo> CuadruplosPrograma { get; } = new List<Cuadruplo>();
        private int contadorTemporalesGlobal = 1;
        private int contadorCuadruplosGlobal = 1;
        private int contadorLabels = 1;
        private string? funcionActual = null;

        private string NuevaEtiqueta(string prefijo = "L")
        {
            return $"{prefijo}{contadorLabels++}";
        }

        private void EmitirCuadruplo(string op, string arg1 = "", string arg2 = "", string res = "")
        {
            // Saltos de control (JMP, JFALSE, LABEL, RECEIVE, RETURN) deshabilitados por solicitud de diseño limpio
        }

        private string AgregarCuadruplosDeResultado(ResultadoSemanticoOperacion? resultado)
        {
            if (resultado == null) return "";
            string ultimoResultado = "";

            if (resultado.Cuadruplos != null && resultado.Cuadruplos.Count > 0)
            {
                foreach (var c in resultado.Cuadruplos)
                {
                    if (string.IsNullOrEmpty(funcionActual))
                    {
                        Cuadruplo nuevo = new Cuadruplo(contadorCuadruplosGlobal++, c.Operador, c.Argumento1, c.Argumento2, c.Resultado);
                        CuadruplosPrograma.Add(nuevo);
                    }
                    ultimoResultado = c.Resultado;
                }
            }
            else if (resultado.ArbolRaiz != null)
            {
                ultimoResultado = resultado.ArbolRaiz.Lexema;
            }

            return ultimoResultado;
        }

        // Inicializa una nueva instancia del analizador sintáctico con los tokens y la tabla de errores AnalizadorSintactico()
        public AnalizadorSintactico(List<TokenSintactico> tokens, DataGridView dgtErrores, AnalizadorSemanticoJerarquia? semantico = null)
        {
            this.tokens = tokens;
            this.pos = 0;
            this.dgtErrores = dgtErrores;
            this.semantico = semantico ?? new AnalizadorSemanticoJerarquia();
        }

        // Helper para comprobar si un token es un delimitador (DEL o SC;)
        private bool EsDelimitador(TokenSintactico tok)
        {
            if (tok == null) return false;
            return tok.Tipo.StartsWith("DEL") || tok.Tipo.StartsWith("SC;") || tok.Lexema == ";";
        }

        // Consume el delimitador si está presente (y cualquier delimitador consecutivo adicional)
        private bool MatchDelimitador()
        {
            if (pos >= tokens.Count) return false;
            if (EsDelimitador(tokens[pos]))
            {
                while (pos < tokens.Count && EsDelimitador(tokens[pos]))
                {
                    pos++;
                }
                return true;
            }
            return false;
        }

        // Exige la presencia obligatoria de un delimitador ';' y reporta error si falta
        private void ExigirDelimitador(string contexto = "")
        {
            if (!MatchDelimitador())
            {
                string msg = string.IsNullOrEmpty(contexto)
                    ? "Se esperaba delimitador ';' (DEL) al final de la instrucción."
                    : $"Se esperaba delimitador ';' (DEL) al final de {contexto}.";
                ReportarError(msg);
            }
        }

        // Modo pánico: descarta tokens hasta el siguiente delimitador o cierre de bloque para evitar cascada de errores
        private void SincronizarHastaDelimitador()
        {
            while (pos < tokens.Count && !EsDelimitador(tokens[pos]) && !EsFinDeBloque())
            {
                pos++;
            }
            if (pos < tokens.Count && EsDelimitador(tokens[pos]))
            {
                pos++;
            }
        }

        // Normaliza nombres de tipos de datos a 'string', 'bool', 'float' o 'int'
        private string NormalizarTipoDato(string tipo)
        {
            string t = tipo.ToLower();
            if (t.Contains("str") || t.Contains("cad") || t.Contains("text")) return "string";
            if (t.Contains("bool")) return "bool";
            if (t.Contains("float") || t.Contains("real") || t.Contains("dec") || t.Contains("double")) return "float";
            return "int";
        }

        // Deduce el tipo probable de un parámetro por su convención de nombre si no se especificó tipo explícito
        private string DeducirTipoPorNombre(string nombre)
        {
            string n = nombre.ToLower();
            if (n.StartsWith("str") || n.StartsWith("nom") || n.StartsWith("txt") || n.StartsWith("msg") || n.StartsWith("mensaje") || n.StartsWith("texto"))
                return "string";
            if (n.StartsWith("bool") || n.StartsWith("flag") || n.StartsWith("es_") || n.StartsWith("es"))
                return "bool";
            if (n.StartsWith("flt") || n.StartsWith("prom") || n.StartsWith("promedio") || n.StartsWith("precio") || n.StartsWith("total") || n.StartsWith("area"))
                return "float";
            return "int";
        }

        // Realiza un pre-escaneo de las funciones definidas en el programa para inferir y registrar sus tipos de retorno dinámicos
        private void PreEscanearDefinicionesDeFunciones()
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                string t = tokens[i].Tipo;
                string l = tokens[i].Lexema;

                bool esInicioFunc = false;
                string nombreFunc = "";

                if ((t.StartsWith("RW22") || l.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase)) &&
                    i + 1 < tokens.Count && tokens[i + 1].Tipo.StartsWith("IDV"))
                {
                    esInicioFunc = true;
                    nombreFunc = tokens[i + 1].Lexema;
                    i++;
                }
                else if ((l.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) || l.Equals("DEF", StringComparison.OrdinalIgnoreCase)) &&
                         i + 1 < tokens.Count && tokens[i + 1].Tipo.StartsWith("IDV"))
                {
                    esInicioFunc = true;
                    nombreFunc = tokens[i + 1].Lexema;
                    i++;
                }

                if (esInicioFunc)
                {
                    semantico.RegistrarRetornoFuncion(nombreFunc, "int", "0");

                    List<string> parametros = new List<string>();
                    int kScan = i + 1;
                    if (kScan < tokens.Count && (tokens[kScan].Tipo.StartsWith("SC(") || tokens[kScan].Lexema == "("))
                    {
                        kScan++;
                        while (kScan < tokens.Count && !tokens[kScan].Tipo.StartsWith("SC)") && tokens[kScan].Lexema != ")")
                        {
                            if (tokens[kScan].Tipo.StartsWith("IDV"))
                            {
                                string p = tokens[kScan].Lexema;
                                if (kScan + 1 < tokens.Count && tokens[kScan + 1].Tipo.StartsWith("IDV"))
                                {
                                    kScan++;
                                    p = tokens[kScan].Lexema;
                                }
                                parametros.Add(p);
                            }
                            kScan++;
                        }
                        if (kScan < tokens.Count && (tokens[kScan].Tipo.StartsWith("SC)") || tokens[kScan].Lexema == ")"))
                        {
                            kScan++;
                        }
                    }

                    int inicioCuerpo = kScan;
                    while (inicioCuerpo < tokens.Count && EsDelimitador(tokens[inicioCuerpo])) inicioCuerpo++;

                    int nivel = 1;
                    int j = inicioCuerpo;
                    int finCuerpo = j;
                    while (j < tokens.Count && nivel > 0)
                    {
                        string tj = tokens[j].Tipo;
                        string lj = tokens[j].Lexema;

                        if (lj.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase) ||
                            lj.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) ||
                            lj.Equals("DEF", StringComparison.OrdinalIgnoreCase))
                        {
                            nivel++;
                        }
                        else if (lj.Equals("ENDEXECUTE", StringComparison.OrdinalIgnoreCase) ||
                                 lj.Equals("ENDFUNCTION", StringComparison.OrdinalIgnoreCase) ||
                                 lj.Equals("ENDDEF", StringComparison.OrdinalIgnoreCase) ||
                                 tj.StartsWith("RW25") || tj.StartsWith("RW23"))
                        {
                            nivel--;
                            if (nivel == 0)
                            {
                                finCuerpo = j;
                                break;
                            }
                        }
                        j++;
                    }

                    if (finCuerpo >= inicioCuerpo)
                    {
                        var tokensCuerpo = tokens.GetRange(inicioCuerpo, finCuerpo - inicioCuerpo);
                        var (instrs, exprRet, tRet) = ExtraerInstruccionesYCuerpoFuncion(tokensCuerpo);
                        var def = new DefinicionFuncion
                        {
                            Nombre = nombreFunc,
                            Parametros = parametros,
                            Instrucciones = instrs,
                            ExpresionRetorno = exprRet,
                            TipoRetorno = tRet
                        };
                        semantico.RegistrarDefinicionFuncion(def);
                    }
                }
            }
        }

        // Inicia el análisis sintáctico del programa verificando inicio, cuerpo y fin ParsearPrograma()
        public void ParsearPrograma()
        {
            // Pre-escanear funciones para conocer sus tipos de retorno dinámicos antes de que sean llamadas
            PreEscanearDefinicionesDeFunciones();

            // Ignorar delimitadores previos si los hubiera al inicio
            while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;

            if (pos >= tokens.Count)
            {
                ReportarError("El programa está vacío. Se esperaba el inicio del programa (START / RW01).");
                return;
            }

            // Permitir definiciones de funciones antes de START
            while (pos < tokens.Count &&
                   (tokens[pos].Tipo.StartsWith("RW22") || tokens[pos].Lexema.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase) ||
                    tokens[pos].Lexema.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) || tokens[pos].Lexema.Equals("DEF", StringComparison.OrdinalIgnoreCase)))
            {
                ParsearInstruccion();
                while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;
            }

            if (!Match("RW01", "START"))
            {
                ReportarError("Se esperaba el inicio del programa (START / RW01).");
            }
            // Consumir delimitador opcional tras START (Diagrama IN01: START DEL)
            MatchDelimitador();

            ParsearInstrucciones();

            if (!Match("RW02", "END"))
            {
                ReportarError("Se esperaba el fin del programa (END / RW02).");
            }
            // Consumir delimitador opcional tras END (Diagrama IN02: END DEL)
            MatchDelimitador();

            // Permitir definiciones de funciones después de END
            while (pos < tokens.Count &&
                   (tokens[pos].Tipo.StartsWith("RW22") || tokens[pos].Lexema.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase) ||
                    tokens[pos].Lexema.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) || tokens[pos].Lexema.Equals("DEF", StringComparison.OrdinalIgnoreCase)))
            {
                ParsearInstruccion();
                while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;
            }

            // Ignorar delimitadores finales si los hubiera
            while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;

            // Verificar si hay tokens no válidos después del fin del programa
            if (pos < tokens.Count)
            {
                ReportarError($"Se encontraron tokens no válidos fuera de la estructura principal del programa (después de END): '{tokens[pos].Lexema}'.");
                pos = tokens.Count;
            }

            // Re-indexar los cuádruplos de todo el programa de forma secuencial 1..N
            for (int i = 0; i < CuadruplosPrograma.Count; i++)
            {
                CuadruplosPrograma[i].Indice = i + 1;
            }
        }

        // Procesa secuencialmente las instrucciones del bloque hasta encontrar un token de fin ParsearInstrucciones()
        private void ParsearInstrucciones(params string[] stopTokensAdicionales)
        {
            while (pos < tokens.Count && !EsFinDeBloque(stopTokensAdicionales))
            {
                ParsearInstruccion();
            }
        }

        // Determina si el token actual representa el cierre de un bloque de código EsFinDeBloque()
        private bool EsFinDeBloque(string[]? stopTokensAdicionales = null)
        {
            if (pos >= tokens.Count) return true;
            string t = tokens[pos].Tipo;
            string l = tokens[pos].Lexema;

            // Si le mandamos un token de parada (como el WHILE de un DO WHILE o UNTIL de EXECUTE), detiene el bloque de instrucciones
            if (stopTokensAdicionales != null)
            {
                foreach (string stop in stopTokensAdicionales)
                {
                    if (t.StartsWith(stop) || l.Equals(stop, StringComparison.OrdinalIgnoreCase)) return true;
                }
            }

            // Palabras de cierre de bloque válidas en NovaNyx 1.4.2
            return t.StartsWith("RW02") || l.Equals("END", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW07") || l.Equals("ELSE", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW09") || l.Equals("ENDIF", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW11") || l.Equals("CASE", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW12") || l.Equals("NONE", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW13") || l.Equals("ENDCASE", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW19") || l.Equals("ENDFOR", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW23") || l.Equals("ENDDO", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW24") || l.Equals("ENDWHILE", StringComparison.OrdinalIgnoreCase) ||
                   t.StartsWith("RW25") || l.Equals("ENDEXECUTE", StringComparison.OrdinalIgnoreCase) ||
                   l.Equals("ENDFUNCTION", StringComparison.OrdinalIgnoreCase) ||
                   l.Equals("ENDDEF", StringComparison.OrdinalIgnoreCase);
        }

        // Identifica y analiza la sintaxis de una instrucción específica según su token ParsearInstruccion()
        private void ParsearInstruccion()
        {
            if (pos >= tokens.Count) return;

            // Delimitador suelto o repetido (ej. ;) se consume sin romper la sintaxis
            if (EsDelimitador(tokens[pos]))
            {
                pos++;
                return;
            }

            string t = tokens[pos].Tipo;
            string l = tokens[pos].Lexema;

            // 0. Instrucción RETURN (RETURN expr; o RETURN;) - Prioridad antes de IDV
            if (l.Equals("RETURN", StringComparison.OrdinalIgnoreCase) || t.StartsWith("RW27"))
            {
                pos++;
                int inicioOp = pos;
                string resVal = "";
                if (pos < tokens.Count && !EsDelimitador(tokens[pos]))
                {
                    ParsearExpresion();
                    int finOp = pos;
                    if (finOp > inicioOp)
                    {
                        var res = VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                        resVal = AgregarCuadruplosDeResultado(res);
                        if (!string.IsNullOrEmpty(funcionActual) && res != null && !string.IsNullOrEmpty(res.TipoDatoResultante) && res.TipoDatoResultante != "desconocido")
                        {
                            semantico.RegistrarRetornoFuncion(funcionActual, res.TipoDatoResultante, res.ValorCalculado);
                        }
                    }
                }
                EmitirCuadruplo("RETURN", resVal, "", "");
                ExigirDelimitador("la instrucción RETURN");
                return;
            }

            // 1. Llamada a función o Asignación (IDV ...)
            if (t.StartsWith("IDV"))
            {
                // Comprobar si es llamada a función directa: SUMA(X, Y); o SUMA();
                if (pos + 1 < tokens.Count && (tokens[pos + 1].Tipo.StartsWith("SC(") || tokens[pos + 1].Lexema == "("))
                {
                    int inicioOp = pos;
                    Match("IDV");
                    pos++; // consumir '('
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                    {
                        pos++; // consumir ')'
                    }
                    else
                    {
                        ParsearExpresion();
                        while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                        {
                            pos++;
                            ParsearExpresion();
                        }
                        if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                        {
                            pos++;
                        }
                        else
                        {
                            ReportarError("Se esperaba cierre de paréntesis ')' en la llamada a función.");
                        }
                    }
                    int finOp = pos;
                    if (finOp > inicioOp)
                    {
                        var res = VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                        AgregarCuadruplosDeResultado(res);
                    }
                    ExigirDelimitador("la llamada a función");
                }
                else
                {
                    // Asignación: ID = Expresión;
                    int inicioOp = pos;
                    Match("IDV");
                    if (!Match("ALO=", "=")) ReportarError("Se esperaba operador de asignación (ALO= o =).");
                    ParsearExpresion();
                    int finOp = pos;
                    if (finOp > inicioOp)
                    {
                        var res = VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                        AgregarCuadruplosDeResultado(res);
                    }
                    ExigirDelimitador("la asignación");
                }
            }
            // 2. Lectura (READ) - RW03 / Diagrama IN04: READ ARG4 (, ARG4)* DEL
            else if (t.StartsWith("RW03") || l.Equals("READ", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW03", "READ");
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("IDV") || tokens[pos].Tipo.StartsWith("NUC") || tokens[pos].Tipo.StartsWith("INC") || tokens[pos].Tipo.StartsWith("RNC") || tokens[pos].Tipo.StartsWith("NCR") || tokens[pos].Tipo.StartsWith("STR") || tokens[pos].Tipo.StartsWith("RW26") || tokens[pos].Tipo.StartsWith("RW08")))
                {
                    EmitirCuadruplo("READ", "", "", tokens[pos].Lexema);
                    pos++;
                }
                else
                {
                    ReportarError("Se esperaba un argumento válido para leer (IDV, constantes).");
                }

                // Múltiples argumentos separados por coma según diagrama IN04: (, ARG4)*
                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                {
                    pos++;
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("IDV") || tokens[pos].Tipo.StartsWith("NUC") || tokens[pos].Tipo.StartsWith("INC") || tokens[pos].Tipo.StartsWith("RNC") || tokens[pos].Tipo.StartsWith("NCR") || tokens[pos].Tipo.StartsWith("STR") || tokens[pos].Tipo.StartsWith("RW26") || tokens[pos].Tipo.StartsWith("RW08")))
                    {
                        EmitirCuadruplo("READ", "", "", tokens[pos].Lexema);
                        pos++;
                    }
                    else
                    {
                        ReportarError("Se esperaba un argumento válido después de la coma en READ.");
                    }
                }
                ExigirDelimitador("la instrucción READ");
            }
            // 3. Escritura (PRINT) - RW04 / Diagrama IN03: PRINT ARG3 (, ARG3)* DEL
            else if (t.StartsWith("RW04") || l.Equals("PRINT", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW04", "PRINT");
                int inicioOp = pos;
                ParsearExpresion();
                int finOp = pos;
                if (finOp > inicioOp)
                {
                    var res = VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                    string resVal = AgregarCuadruplosDeResultado(res);
                    EmitirCuadruplo("PRINT", resVal, "", "");
                }

                // Múltiples argumentos separados por coma según diagrama IN03: (, ARG3)*
                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                {
                    pos++;
                    int inicioArg = pos;
                    ParsearExpresion();
                    int finArg = pos;
                    if (finArg > inicioArg)
                    {
                        var res = VerificarSemanticaOperacion(tokens.GetRange(inicioArg, finArg - inicioArg));
                        string resVal = AgregarCuadruplosDeResultado(res);
                        EmitirCuadruplo("PRINT", resVal, "", "");
                    }
                }
                ExigirDelimitador("la instrucción PRINT");
            }
            // 4. Estructura IF (RW05) / Diagrama IN05
            else if (t.StartsWith("RW05") || l.Equals("IF", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW05", "IF");
                string condVar = ParsearCondicion();
                if (!Match("RW06", "THEN")) ReportarError("Se esperaba instrucción THEN (RW06).");
                MatchDelimitador();

                string lElse = NuevaEtiqueta("L_ELSE");
                string lFinIf = NuevaEtiqueta("L_ENDIF");

                EmitirCuadruplo("JFALSE", condVar, "", lElse);

                ParsearInstrucciones();

                if (Match("RW07", "ELSE"))
                {
                    EmitirCuadruplo("JMP", "", "", lFinIf);
                    EmitirCuadruplo("LABEL", "", "", lElse);
                    MatchDelimitador(); // Diagrama IN05_2: ELSE DEL
                    ParsearInstrucciones();
                    EmitirCuadruplo("LABEL", "", "", lFinIf);
                }
                else
                {
                    EmitirCuadruplo("LABEL", "", "", lElse);
                }

                if (!Match("RW09", "ENDIF")) ReportarError("Se esperaba cierre ENDIF (RW09).");
                MatchDelimitador(); // Diagrama IN05_3: ENDIF DEL
            }
            // 5. Estructura PERHAPS (RW10) / Diagrama IN06
            else if (t.StartsWith("RW10") || l.Equals("PERHAPS", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW10", "PERHAPS");
                int inicioP = pos;
                string exprVal = "";
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("IDV") || tokens[pos].Tipo.StartsWith("NUC")))
                {
                    exprVal = tokens[pos].Lexema;
                    pos++;
                }
                else
                {
                    ParsearExpresion();
                    int finP = pos;
                    if (finP > inicioP)
                    {
                        var resP = VerificarSemanticaOperacion(tokens.GetRange(inicioP, finP - inicioP));
                        exprVal = AgregarCuadruplosDeResultado(resP);
                    }
                }
                MatchDelimitador();

                string lFinCase = NuevaEtiqueta("L_ENDCASE");

                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW11") || tokens[pos].Lexema.Equals("CASE", StringComparison.OrdinalIgnoreCase)))
                {
                    Match("RW11", "CASE");
                    string testVal = ParsearCondicionOExpresion();
                    string lNextCase = NuevaEtiqueta("L_CASE_NEXT");
                    string tComp = $"T{contadorTemporalesGlobal++}";
                    EmitirCuadruplo("==", exprVal, testVal, tComp);
                    EmitirCuadruplo("JFALSE", tComp, "", lNextCase);

                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC:") || tokens[pos].Lexema == ":"))
                    {
                        pos++;
                    }
                    MatchDelimitador();
                    ParsearInstrucciones();
                    EmitirCuadruplo("JMP", "", "", lFinCase);
                    EmitirCuadruplo("LABEL", "", "", lNextCase);
                }

                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW12") || tokens[pos].Lexema.Equals("NONE", StringComparison.OrdinalIgnoreCase)))
                {
                    Match("RW12", "NONE");
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC:") || tokens[pos].Lexema == ":"))
                    {
                        pos++;
                    }
                    else if (pos < tokens.Count && !EsFinDeBloque() && !EsDelimitador(tokens[pos]) && !tokens[pos].Tipo.StartsWith("IDV") && !tokens[pos].Tipo.StartsWith("RW"))
                    {
                        ParsearCondicionOExpresion();
                    }
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC:") || tokens[pos].Lexema == ":"))
                    {
                        pos++;
                    }
                    MatchDelimitador();
                    ParsearInstrucciones();
                }

                EmitirCuadruplo("LABEL", "", "", lFinCase);

                if (!Match("RW13", "ENDCASE")) ReportarError("Se esperaba cierre ENDCASE (RW13).");
                MatchDelimitador(); // Diagrama IN06_3: ENDCASE DEL
            }
            // 6. Ciclo WHILE (RW20) / Diagrama IN10 (WHILE CONDIC ... ENDWHILE DEL)
            else if (t.StartsWith("RW20") || l.Equals("WHILE", StringComparison.OrdinalIgnoreCase))
            {
                string lInicio = NuevaEtiqueta("L_WHILE_INI");
                string lFin = NuevaEtiqueta("L_WHILE_FIN");

                EmitirCuadruplo("LABEL", "", "", lInicio);

                Match("RW20", "WHILE");
                string condVar = ParsearCondicion();

                // Permitir DO opcionalmente si se incluye, pero sin forzarlo
                if (Match("RW21", "DO"))
                {
                    MatchDelimitador();
                }
                else
                {
                    MatchDelimitador();
                }

                EmitirCuadruplo("JFALSE", condVar, "", lFin);

                ParsearInstrucciones();

                EmitirCuadruplo("JMP", "", "", lInicio);
                EmitirCuadruplo("LABEL", "", "", lFin);

                if (!Match("RW24", "ENDWHILE")) ReportarError("Se esperaba cierre ENDWHILE (RW24).");
                MatchDelimitador(); // Diagrama IN10_1: ENDWHILE DEL
            }
            // 7. Ciclo FOR (RW14) / Diagrama IN07
            else if (t.StartsWith("RW14") || l.Equals("FOR", StringComparison.OrdinalIgnoreCase))
            {
                int lineaFor = pos < tokens.Count ? tokens[pos].Linea : 1;
                Match("RW14", "FOR");

                TokenSintactico? tokId = null;
                TokenSintactico? tokAsig = null;

                // Forma 1: FOR FROM id = inicio ... (Diagrama IN07)
                // Forma 2: FOR id [SET] = FROM inicio ... (Código actual / Archivo.txt)
                if (Match("RW15", "FROM"))
                {
                    if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                    {
                        tokId = tokens[pos];
                        pos++;
                    }
                    else ReportarError("Se esperaba un identificador después de FROM en el FOR.");

                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("ALO=") || tokens[pos].Lexema == "="))
                    {
                        tokAsig = tokens[pos];
                        pos++;
                    }
                    else ReportarError("Se esperaba operador de asignación '=' en el FOR.");
                }
                else
                {
                    if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                    {
                        tokId = tokens[pos];
                        pos++;
                    }
                    else ReportarError("Se esperaba un identificador para el FOR.");

                    Match("RW16", "SET"); // Opcional SET (RW16)

                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("ALO=") || tokens[pos].Lexema == "="))
                    {
                        tokAsig = tokens[pos];
                        pos++;
                    }
                    else ReportarError("Se esperaba operador de asignación '=' en el FOR.");

                    if (!Match("RW15", "FROM")) ReportarError("Se esperaba la palabra reservada FROM.");
                }

                int inicioOp = pos;
                ParsearExpresion();
                int finOp = pos;

                string idVar = tokId?.Lexema ?? "i";
                if (tokId != null && finOp > inicioOp)
                {
                    List<TokenSintactico> tokensAsigFor = new List<TokenSintactico>
                    {
                        tokId,
                        tokAsig ?? new TokenSintactico { Tipo = "ALO=", Lexema = "=", Linea = lineaFor }
                    };
                    tokensAsigFor.AddRange(tokens.GetRange(inicioOp, finOp - inicioOp));
                    var resAsig = VerificarSemanticaOperacion(tokensAsigFor);
                    AgregarCuadruplosDeResultado(resAsig);
                }

                string lInicio = NuevaEtiqueta("L_FOR_INI");
                string lFin = NuevaEtiqueta("L_FOR_FIN");
                EmitirCuadruplo("LABEL", "", "", lInicio);

                if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL.");
                string limiteVal = ParsearCondicionOExpresion();
                string tCond = $"T{contadorTemporalesGlobal++}";
                EmitirCuadruplo("<=", idVar, limiteVal, tCond);
                EmitirCuadruplo("JFALSE", tCond, "", lFin);

                string pasoVal = "+1";
                if (!Match("RW18", "INTERVAL")) ReportarError("Se esperaba la palabra reservada INTERVAL.");
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("NUC") || tokens[pos].Tipo.StartsWith("INC") || tokens[pos].Tipo.StartsWith("RNC") || tokens[pos].Tipo.StartsWith("NCR") || tokens[pos].Tipo.StartsWith("IDV")))
                {
                    pasoVal = tokens[pos].Lexema;
                    pos++;
                }
                else
                {
                    ReportarError("Se esperaba constante o variable para el intervalo.");
                }
                MatchDelimitador();

                ParsearInstrucciones();

                // Incremento al final del bloque
                string tPaso = $"T{contadorTemporalesGlobal++}";
                EmitirCuadruplo("+", idVar, pasoVal, tPaso);
                EmitirCuadruplo("=", tPaso, "", idVar);
                EmitirCuadruplo("JMP", "", "", lInicio);
                EmitirCuadruplo("LABEL", "", "", lFin);

                if (!Match("RW19", "ENDFOR")) ReportarError("Se esperaba cierre ENDFOR (RW19).");
                MatchDelimitador(); // Diagrama IN07_1: ENDFOR DEL
            }
            // 8. Ciclo DO WHILE (RW21) / Diagrama IN08
            else if (t.StartsWith("RW21") || l.Equals("DO", StringComparison.OrdinalIgnoreCase))
            {
                string lInicio = NuevaEtiqueta("L_DO_INI");
                EmitirCuadruplo("LABEL", "", "", lInicio);

                Match("RW21", "DO");

                // Diagrama IN08: DO WHILE CONDIC ... ENDDO DEL
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW20") || tokens[pos].Lexema.Equals("WHILE", StringComparison.OrdinalIgnoreCase)))
                {
                    Match("RW20", "WHILE");
                    string condVar = ParsearCondicion();
                    MatchDelimitador();

                    string lFin = NuevaEtiqueta("L_DO_FIN");
                    EmitirCuadruplo("JFALSE", condVar, "", lFin);

                    ParsearInstrucciones();

                    EmitirCuadruplo("JMP", "", "", lInicio);
                    EmitirCuadruplo("LABEL", "", "", lFin);

                    if (!Match("RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDDO (RW23).");
                    MatchDelimitador(); // Diagrama IN08_1: ENDDO DEL
                }
                else
                {
                    // Variante de bloque: DO ... WHILE CONDIC ENDDO;
                    MatchDelimitador();
                    ParsearInstrucciones("RW20", "WHILE");

                    if (!Match("RW20", "WHILE")) ReportarError("Se esperaba la palabra reservada WHILE al final del DO.");
                    string condVar = ParsearCondicion();

                    EmitirCuadruplo("JTRUE", condVar, "", lInicio);

                    if (!Match("RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDDO (RW23).");
                    MatchDelimitador();
                }
            }
            // 9. Ciclo EXECUTE o Definición de Función EXECUTE (RW22)
            else if (t.StartsWith("RW22") || l.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW22", "EXECUTE");

                // Si le sigue un identificador, es la definición de una función: EXECUTE SUMA(X, Y) ... ENDEXECUTE
                if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                {
                    string nombreFunc = tokens[pos].Lexema;
                    pos++;

                    List<string> parametros = new List<string>();
                    Dictionary<string, string> tiposParametros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "("))
                    {
                        pos++;
                        while (pos < tokens.Count && !tokens[pos].Tipo.StartsWith("SC)") && tokens[pos].Lexema != ")")
                        {
                            if (tokens[pos].Tipo.StartsWith("IDV"))
                            {
                                string pNombre = tokens[pos].Lexema;
                                pos++;
                                string tipoParam = "int";
                                if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                                {
                                    tipoParam = NormalizarTipoDato(pNombre);
                                    pNombre = tokens[pos].Lexema;
                                    pos++;
                                }
                                else
                                {
                                    tipoParam = DeducirTipoPorNombre(pNombre);
                                }
                                parametros.Add(pNombre);
                                tiposParametros[pNombre] = tipoParam;
                            }
                            else
                            {
                                pos++;
                            }

                            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                            {
                                pos++;
                            }
                        }

                        if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                        {
                            pos++;
                        }
                        else
                        {
                            ReportarError("Se esperaba ')' al cerrar los parámetros de la función.");
                        }
                    }
                    MatchDelimitador();

                    funcionActual = nombreFunc;

                    // Registrar formalmente los parámetros en el analizador semántico con su tipo correspondiente
                    foreach (var p in parametros)
                    {
                        string tP = tiposParametros.ContainsKey(p) ? tiposParametros[p] : "int";
                        string valP = tP switch
                        {
                            "string" => "\"\"",
                            "bool" => "TRUE",
                            "float" => "+0.0",
                            _ => "0"
                        };
                        semantico.RegistrarVariable(p, tP, valP);
                    }

                    string lFinFunc = NuevaEtiqueta($"END_{nombreFunc}");
                    EmitirCuadruplo("JMP", "", "", lFinFunc);
                    EmitirCuadruplo("LABEL", "", "", $"FUNC_{nombreFunc}");

                    foreach (var p in parametros)
                    {
                        EmitirCuadruplo("RECEIVE", "", "", p);
                    }

                    int inicioCuerpoTokens = pos;
                    ParsearInstrucciones("ENDEXECUTE", "RW23", "ENDDO", "RW25");
                    int finCuerpoTokens = pos;

                    if (pos < tokens.Count && (tokens[pos].Lexema.Equals("ENDEXECUTE", StringComparison.OrdinalIgnoreCase) ||
                                              tokens[pos].Lexema.Equals("ENDDO", StringComparison.OrdinalIgnoreCase) ||
                                              tokens[pos].Tipo.StartsWith("RW23") ||
                                              tokens[pos].Tipo.StartsWith("RW25")))
                    {
                        finCuerpoTokens = pos;
                        pos++;
                    }
                    else
                    {
                        ReportarError("Se esperaba cierre ENDEXECUTE o ENDDO al final de la función.");
                    }

                    if (finCuerpoTokens >= inicioCuerpoTokens)
                    {
                        var tokensCuerpo = tokens.GetRange(inicioCuerpoTokens, finCuerpoTokens - inicioCuerpoTokens);
                        var (instrs, exprRet, tRet) = ExtraerInstruccionesYCuerpoFuncion(tokensCuerpo);
                        var def = new DefinicionFuncion
                        {
                            Nombre = nombreFunc,
                            Parametros = parametros,
                            Instrucciones = instrs,
                            ExpresionRetorno = exprRet,
                            TipoRetorno = tRet
                        };
                        semantico.RegistrarDefinicionFuncion(def);
                    }

                    funcionActual = null;

                    EmitirCuadruplo("RETURN", "", "", "");
                    EmitirCuadruplo("LABEL", "", "", lFinFunc);
                    MatchDelimitador();
                }
                else
                {
                    // Ciclo tradicional EXECUTE ... UNTIL CONDIC
                    string lInicio = NuevaEtiqueta("L_EXEC_INI");
                    EmitirCuadruplo("LABEL", "", "", lInicio);

                    MatchDelimitador();

                    ParsearInstrucciones("RW17", "UNTIL");

                    if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL al final del EXECUTE.");
                    string condVar = ParsearCondicion();

                    EmitirCuadruplo("JFALSE", condVar, "", lInicio);

                    // Consumo tolerante si incluyeron ENDDO
                    Match("RW23", "ENDDO");
                    MatchDelimitador();
                }
            }
            // 11. Definición alternativa de función: FUNCTION IDV(...) ... ENDFUNCTION
            else if (l.Equals("FUNCTION", StringComparison.OrdinalIgnoreCase) || l.Equals("DEF", StringComparison.OrdinalIgnoreCase))
            {
                pos++;
                if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                {
                    string nombreFunc = tokens[pos].Lexema;
                    pos++;

                    List<string> parametros = new List<string>();
                    Dictionary<string, string> tiposParametros = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "("))
                    {
                        pos++;
                        while (pos < tokens.Count && !tokens[pos].Tipo.StartsWith("SC)") && tokens[pos].Lexema != ")")
                        {
                            if (tokens[pos].Tipo.StartsWith("IDV"))
                            {
                                string pNombre = tokens[pos].Lexema;
                                pos++;
                                string tipoParam = "int";
                                if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
                                {
                                    tipoParam = NormalizarTipoDato(pNombre);
                                    pNombre = tokens[pos].Lexema;
                                    pos++;
                                }
                                else
                                {
                                    tipoParam = DeducirTipoPorNombre(pNombre);
                                }
                                parametros.Add(pNombre);
                                tiposParametros[pNombre] = tipoParam;
                            }
                            else
                            {
                                pos++;
                            }

                            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                            {
                                pos++;
                            }
                        }

                        if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                        {
                            pos++;
                        }
                        else
                        {
                            ReportarError("Se esperaba ')' al cerrar los parámetros de la función.");
                        }
                    }
                    MatchDelimitador();

                    funcionActual = nombreFunc;

                    // Registrar formalmente los parámetros en el analizador semántico con su tipo correspondiente
                    foreach (var p in parametros)
                    {
                        string tP = tiposParametros.ContainsKey(p) ? tiposParametros[p] : "int";
                        string valP = tP switch
                        {
                            "string" => "\"\"",
                            "bool" => "TRUE",
                            "float" => "+0.0",
                            _ => "0"
                        };
                        semantico.RegistrarVariable(p, tP, valP);
                    }

                    string lFinFunc = NuevaEtiqueta($"END_{nombreFunc}");
                    EmitirCuadruplo("JMP", "", "", lFinFunc);
                    EmitirCuadruplo("LABEL", "", "", $"FUNC_{nombreFunc}");

                    foreach (var p in parametros)
                    {
                        EmitirCuadruplo("RECEIVE", "", "", p);
                    }

                    int inicioCuerpoTokens = pos;
                    ParsearInstrucciones("ENDFUNCTION", "ENDDEF", "RW02", "END");
                    int finCuerpoTokens = pos;

                    if (pos < tokens.Count && (tokens[pos].Lexema.Equals("ENDFUNCTION", StringComparison.OrdinalIgnoreCase) ||
                                              tokens[pos].Lexema.Equals("ENDDEF", StringComparison.OrdinalIgnoreCase) ||
                                              tokens[pos].Lexema.Equals("END", StringComparison.OrdinalIgnoreCase)))
                    {
                        finCuerpoTokens = pos;
                        pos++;
                    }
                    else
                    {
                        ReportarError("Se esperaba cierre ENDFUNCTION al final de la función.");
                    }

                    if (finCuerpoTokens >= inicioCuerpoTokens)
                    {
                        var tokensCuerpo = tokens.GetRange(inicioCuerpoTokens, finCuerpoTokens - inicioCuerpoTokens);
                        var (instrs, exprRet, tRet) = ExtraerInstruccionesYCuerpoFuncion(tokensCuerpo);
                        var def = new DefinicionFuncion
                        {
                            Nombre = nombreFunc,
                            Parametros = parametros,
                            Instrucciones = instrs,
                            ExpresionRetorno = exprRet,
                            TipoRetorno = tRet
                        };
                        semantico.RegistrarDefinicionFuncion(def);
                    }

                    funcionActual = null;

                    EmitirCuadruplo("RETURN", "", "", "");
                    EmitirCuadruplo("LABEL", "", "", lFinFunc);
                    MatchDelimitador();
                }
            }
            else
            {
                ReportarError($"Instrucción no válida o no esperada: '{tokens[pos].Lexema}' ({tokens[pos].Tipo}).");
                SincronizarHastaDelimitador();
            }
        }

        // Comprueba si un token representa un operador lógico binario (AND, OR, &&, ||, LO1, LO2)
        private bool EsOperadorLogicoBinario(TokenSintactico tok)
        {
            if (tok == null) return false;
            string t = tok.Tipo;
            string l = tok.Lexema;
            return t.StartsWith("LO1") || t.StartsWith("LO2") || t.StartsWith("LOP") || t.StartsWith("OLOG") ||
                   l.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
                   l.Equals("OR", StringComparison.OrdinalIgnoreCase) ||
                   l == "&&" || l == "||" ||
                   (t.StartsWith("LO") && !EsOperadorLogicoUnario(tok));
        }

        // Comprueba si un token representa un operador lógico unario (NOT, !, LO3)
        private bool EsOperadorLogicoUnario(TokenSintactico tok)
        {
            if (tok == null) return false;
            string t = tok.Tipo;
            string l = tok.Lexema;
            return t.StartsWith("LO3") || t.StartsWith("LONOT") ||
                   l.Equals("NOT", StringComparison.OrdinalIgnoreCase) ||
                   l == "!" ||
                   (t == "LO2" && l.Equals("NOT", StringComparison.OrdinalIgnoreCase));
        }

        // Comprueba si un token representa un operador relacional (>, <, ==, !=, >=, <=, RO, REO)
        private bool EsOperadorRelacional(TokenSintactico tok)
        {
            if (tok == null) return false;
            string t = tok.Tipo;
            string l = tok.Lexema;
            return t.StartsWith("RO") || t.StartsWith("REO") ||
                   l == ">" || l == "<" || l == "==" || l == "!=" || l == ">=" || l == "<=";
        }

        // Analiza y valida una condición lógica o relacional compuesta ParsearCondicion()
        private string ParsearCondicion()
        {
            int inicioCond = pos;
            ParsearCondicionLogica();
            int finCond = pos;

            if (finCond > inicioCond)
            {
                var res = VerificarSemanticaOperacion(tokens.GetRange(inicioCond, finCond - inicioCond));
                return AgregarCuadruplosDeResultado(res);
            }
            return "";
        }

        // Parsea condiciones encadenadas por operadores lógicos opcionales (AND, OR)
        private void ParsearCondicionLogica()
        {
            ParsearSubCondicion();

            // Los operadores lógicos (AND, OR) son OPCIONALES: si están presentes, encadenan subcondiciones
            while (pos < tokens.Count && EsOperadorLogicoBinario(tokens[pos]))
            {
                pos++; // Consumir operador lógico AND / OR
                ParsearSubCondicion();
            }
        }

        // Parsea una subcondición unitaria (relacional, entre paréntesis, con negación NOT o valor booleano)
        private void ParsearSubCondicion()
        {
            int inicioSub = pos;
            bool tieneNot = false;

            // Soporte para operador lógico unario NOT
            if (pos < tokens.Count && EsOperadorLogicoUnario(tokens[pos]))
            {
                pos++;
                tieneNot = true;
            }

            // Soporte para subcondición agrupada entre paréntesis (ej. (nota >= 70) o (a > b AND c < d))
            bool tieneParentesis = false;
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "("))
            {
                pos++;
                tieneParentesis = true;
                ParsearCondicionLogica();

                if (!Match("SC)") && (pos < tokens.Count && tokens[pos].Lexema != ")"))
                {
                    ReportarError("Se esperaba cierre de paréntesis ')' en la condición.");
                }
                else if (pos < tokens.Count && tokens[pos].Lexema == ")")
                {
                    pos++;
                }
            }
            else
            {
                ParsearExpresion();
            }

            bool tieneRelacional = false;
            if (pos < tokens.Count && EsOperadorRelacional(tokens[pos]))
            {
                tieneRelacional = true;
                pos++;
                ParsearExpresion();
            }

            // Si vino un grupo con paréntesis y luego un operador relacional (ej. '(a + b) > 10')
            if (tieneParentesis && !tieneRelacional && pos < tokens.Count && EsOperadorRelacional(tokens[pos]))
            {
                tieneRelacional = true;
                pos++;
                ParsearExpresion();
            }

            bool esBooleanoDirecto = (inicioSub < pos && tokens.GetRange(inicioSub, pos - inicioSub).Any(tok =>
                tok.Tipo.StartsWith("RW26") || tok.Tipo.StartsWith("RW08") ||
                tok.Lexema.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                tok.Lexema.Equals("FALSE", StringComparison.OrdinalIgnoreCase) ||
                semantico.ObtenerTipo(tok.Lexema) == "bool" ||
                semantico.ObtenerRetornoFuncion(tok.Lexema) == "bool"));

            if (!tieneRelacional && !tieneNot && !tieneParentesis && !esBooleanoDirecto)
            {
                ReportarError("Se esperaba un operador relacional (>, <, ==, !=, >=, <=) o lógico en la condición.");
            }
        }

        // Analiza argumentos mixtos que pueden ser condiciones o expresiones (ej. ARG6 en CASE / NONE / UNTIL)
        private string ParsearCondicionOExpresion()
        {
            int inicio = pos;
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "(" || EsOperadorLogicoUnario(tokens[pos])))
            {
                ParsearCondicionLogica();
            }
            else
            {
                ParsearExpresion();
                if (pos < tokens.Count && EsOperadorRelacional(tokens[pos]))
                {
                    pos++;
                    ParsearExpresion();
                }
                while (pos < tokens.Count && EsOperadorLogicoBinario(tokens[pos]))
                {
                    pos++;
                    ParsearSubCondicion();
                }
            }
            int fin = pos;
            if (fin > inicio)
            {
                var res = VerificarSemanticaOperacion(tokens.GetRange(inicio, fin - inicio));
                return AgregarCuadruplosDeResultado(res);
            }
            return "";
        }

        // Analiza expresiones compuestas por términos separados por suma o resta ParsearExpresion()
        private void ParsearExpresion()
        {
            ParsearTermino();
            while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO+") || tokens[pos].Tipo.StartsWith("AO-") ||
                                         tokens[pos].Lexema == "+" || tokens[pos].Lexema == "-"))
            {
                pos++;
                ParsearTermino();
            }
        }

        // Analiza términos compuestos por factores separados por multiplicación, división o módulo ParsearTermino()
        private void ParsearTermino()
        {
            ParsearPotencia();
            while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO*") || tokens[pos].Tipo.StartsWith("AO/") ||
                                         tokens[pos].Tipo.StartsWith("AO%") || tokens[pos].Tipo.StartsWith("SC%") ||
                                         tokens[pos].Lexema == "*" || tokens[pos].Lexema == "/" || tokens[pos].Lexema == "%"))
            {
                pos++;
                ParsearPotencia();
            }
        }

        // Analiza operaciones de potencia respetando la jerarquía de operadores ParsearPotencia()
        private void ParsearPotencia()
        {
            ParsearFactor();
            while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO^") || tokens[pos].Tipo.StartsWith("SC^") || tokens[pos].Lexema == "^"))
            {
                pos++;
                ParsearFactor();
            }
        }

        // Analiza factores primarios como constantes, variables, signos unarios o subexpresiones ParsearFactor()
        private void ParsearFactor()
        {
            if (pos >= tokens.Count) return;
            // Soporte para operador lógico unario NOT (LO3, NOT, !)
            if (EsOperadorLogicoUnario(tokens[pos]))
            {
                pos++;
            }

            // Permitir signo unario (+ o -) en números con signo como +5 o -9
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO+") || tokens[pos].Tipo.StartsWith("AO-") || tokens[pos].Lexema == "+" || tokens[pos].Lexema == "-"))
            {
                pos++;
            }

            if (pos >= tokens.Count)
            {
                ReportarError("Se esperaba un operando después del signo u operador.");
                return;
            }

            if (pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV"))
            {
                pos++; // Consumir IDV (variable o función)
                // Si es llamada a función con argumentos: ID(arg1, arg2, ...) o ID()
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "("))
                {
                    pos++; // consumir '('
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                    {
                        pos++; // consumir ')'
                    }
                    else
                    {
                        ParsearExpresion();
                        while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                        {
                            pos++;
                            ParsearExpresion();
                        }
                        if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC)") || tokens[pos].Lexema == ")"))
                        {
                            pos++;
                        }
                        else
                        {
                            ReportarError("Se esperaba cierre de paréntesis ')' en los argumentos de la función.");
                        }
                    }
                }
            }
            else if (Match("INC") || Match("RNC") || Match("NCR") || Match("NUC") || Match("STR") ||
                     Match("RW26", "TRUE") || Match("RW08", "FALSE") || Match("BOOL"))
            {
                // Operando constante válido
            }
            else if (Match("SC(") || (pos < tokens.Count && tokens[pos].Lexema == "("))
            {
                if (pos < tokens.Count && tokens[pos].Lexema == "(") pos++;
                ParsearExpresion();
                if (!Match("SC)") && (pos < tokens.Count && tokens[pos].Lexema != ")"))
                {
                    ReportarError("Se esperaba cierre de paréntesis 'SC)'.");
                }
                else if (pos < tokens.Count && tokens[pos].Lexema == ")")
                {
                    pos++;
                }
            }
            else
            {
                ReportarError($"Se esperaba un valor, variable o expresión aritmética válida en vez de '{tokens[pos].Lexema}'.");
                pos++;
            }
        }

        // Envía una lista de tokens al analizador semántico y registra los errores detectados VerificarSemanticaOperacion()
        private ResultadoSemanticoOperacion? VerificarSemanticaOperacion(List<TokenSintactico> tokensOp)
        {
            if (tokensOp == null || tokensOp.Count == 0) return null;

            var resultado = semantico.AnalizarTokensCompilador(tokensOp);
            if (string.IsNullOrEmpty(funcionActual))
            {
                OperacionesDetectadas.Add(resultado);
            }

            foreach (var err in resultado.ErroresSemanticos)
            {
                int linea = tokensOp.FirstOrDefault()?.Linea ?? 0;
                ReportarErrorSemantico(err, linea);
            }
            return resultado;
        }

        // Reconstruye una expresión textual limpia a partir de una lista de tokens sintácticos
        public static string ReconstruirTextoTokens(IEnumerable<TokenSintactico> toks)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var t in toks)
            {
                string lex = !string.IsNullOrEmpty(t.Lexema) ? t.Lexema : t.Tipo;
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
            return sb.ToString().Trim();
        }

        // Extrae las sentencias internas de una función y su expresión de retorno
        private (List<string> Instrucciones, string? ExpresionRetorno, string TipoRetorno) ExtraerInstruccionesYCuerpoFuncion(List<TokenSintactico> tokensCuerpo)
        {
            List<string> instrucciones = new List<string>();
            string? expresionRetorno = null;
            string tipoRetorno = "int";

            int idx = 0;
            while (idx < tokensCuerpo.Count)
            {
                while (idx < tokensCuerpo.Count && EsDelimitador(tokensCuerpo[idx])) idx++;
                if (idx >= tokensCuerpo.Count) break;

                int inicioSentencia = idx;
                while (idx < tokensCuerpo.Count && !EsDelimitador(tokensCuerpo[idx]) &&
                       !tokensCuerpo[idx].Lexema.Equals("ENDEXECUTE", StringComparison.OrdinalIgnoreCase) &&
                       !tokensCuerpo[idx].Lexema.Equals("ENDFUNCTION", StringComparison.OrdinalIgnoreCase) &&
                       !tokensCuerpo[idx].Lexema.Equals("ENDDEF", StringComparison.OrdinalIgnoreCase))
                {
                    idx++;
                }

                if (idx > inicioSentencia)
                {
                    var tokensSentencia = tokensCuerpo.GetRange(inicioSentencia, idx - inicioSentencia);
                    string primerLexema = tokensSentencia[0].Lexema;
                    string primerTipo = tokensSentencia[0].Tipo;

                    if (primerLexema.Equals("RETURN", StringComparison.OrdinalIgnoreCase) || primerTipo.StartsWith("RW27"))
                    {
                        if (tokensSentencia.Count > 1)
                        {
                            var toksRet = tokensSentencia.GetRange(1, tokensSentencia.Count - 1);
                            expresionRetorno = ReconstruirTextoTokens(toksRet);

                            var resRet = semantico.AnalizarTokensCompilador(toksRet);
                            if (resRet != null && !string.IsNullOrEmpty(resRet.TipoDatoResultante) && resRet.TipoDatoResultante != "desconocido")
                            {
                                tipoRetorno = resRet.TipoDatoResultante;
                            }
                        }
                    }
                    else
                    {
                        string textoSentencia = ReconstruirTextoTokens(tokensSentencia);
                        if (!string.IsNullOrWhiteSpace(textoSentencia))
                        {
                            instrucciones.Add(textoSentencia);
                        }
                    }
                }
            }

            return (instrucciones, expresionRetorno, tipoRetorno);
        }

        // Comprueba si el tipo o lexema del token actual coincide con alguno de los esperados y avanza Match()
        private bool Match(params string[] esperados)
        {
            if (pos >= tokens.Count) return false;
            string tipoActual = tokens[pos].Tipo;
            string lexemaActual = tokens[pos].Lexema;
            foreach (string exp in esperados)
            {
                if (tipoActual.StartsWith(exp) || lexemaActual.Equals(exp, StringComparison.OrdinalIgnoreCase))
                {
                    pos++;
                    return true;
                }
            }
            return false;
        }

        // Registra un error sintáctico con su línea en la tabla visual de errores ReportarError()
        private void ReportarError(string mensaje)
        {
            int lineaError = pos < tokens.Count ? tokens[pos].Linea : (tokens.LastOrDefault()?.Linea ?? 0);
            dgtErrores.Rows.Add(lineaError, "Error Sintáctico: " + mensaje);

            int lastRow = dgtErrores.Rows.Count - 1;
            if (lastRow >= 0)
                dgtErrores.Rows[lastRow].DefaultCellStyle.BackColor = System.Drawing.Color.Orange;
        }

        // Registra un error semántico con su línea en la tabla visual de errores ReportarErrorSemantico()
        private void ReportarErrorSemantico(string mensaje, int linea)
        {
            string mensajeLimpio = mensaje;
            if (mensajeLimpio.StartsWith("Línea "))
            {
                int idxDosPuntos = mensajeLimpio.IndexOf(':');
                if (idxDosPuntos != -1 && idxDosPuntos + 1 < mensajeLimpio.Length)
                {
                    mensajeLimpio = mensajeLimpio.Substring(idxDosPuntos + 1).Trim();
                }
            }

            dgtErrores.Rows.Add(linea, mensajeLimpio);

            int lastRow = dgtErrores.Rows.Count - 1;
            if (lastRow >= 0)
                dgtErrores.Rows[lastRow].DefaultCellStyle.BackColor = System.Drawing.Color.MediumPurple;
        }
    }
}