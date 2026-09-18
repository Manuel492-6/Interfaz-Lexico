using System;
using System.Collections.Generic;
using System.Linq;
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

        // Inicia el análisis sintáctico del programa verificando inicio, cuerpo y fin ParsearPrograma()
        public void ParsearPrograma()
        {
            // Ignorar delimitadores previos si los hubiera al inicio
            while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;

            if (pos >= tokens.Count)
            {
                ReportarError("El programa está vacío. Se esperaba el inicio del programa (START / RW01).");
                return;
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

            // Ignorar delimitadores finales si los hubiera
            while (pos < tokens.Count && EsDelimitador(tokens[pos])) pos++;

            // Verificar si hay tokens no válidos después del fin del programa
            if (pos < tokens.Count)
            {
                ReportarError($"Se encontraron tokens no válidos fuera de la estructura principal del programa (después de END): '{tokens[pos].Lexema}'.");
                pos = tokens.Count;
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
                   t.StartsWith("RW24") || l.Equals("ENDWHILE", StringComparison.OrdinalIgnoreCase);
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

            // 1. Asignación (Diagrama ASIGN: IDV ALO= ARG4 DEL)
            if (t.StartsWith("IDV"))
            {
                int inicioOp = pos;
                Match("IDV");
                if (!Match("ALO=", "=")) ReportarError("Se esperaba operador de asignación (ALO= o =).");
                ParsearExpresion();
                int finOp = pos;
                if (finOp > inicioOp)
                {
                    VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                }
                ExigirDelimitador("la asignación");
            }
            // 2. Lectura (READ) - RW03 / Diagrama IN04: READ ARG4 (, ARG4)* DEL
            else if (t.StartsWith("RW03") || l.Equals("READ", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW03", "READ");
                if (!Match("IDV") && !Match("NUC") && !Match("INC") && !Match("RNC") && !Match("NCR") && !Match("STR") && !Match("RW26", "TRUE") && !Match("RW08", "FALSE"))
                {
                    ReportarError("Se esperaba un argumento válido para leer (IDV, NUC, NCR, constantes).");
                }

                // Múltiples argumentos separados por coma según diagrama IN04: (, ARG4)*
                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC,") || tokens[pos].Lexema == ","))
                {
                    pos++;
                    if (!Match("IDV") && !Match("NUC") && !Match("INC") && !Match("RNC") && !Match("NCR") && !Match("STR") && !Match("RW26", "TRUE") && !Match("RW08", "FALSE"))
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
                    VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
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
                        VerificarSemanticaOperacion(tokens.GetRange(inicioArg, finArg - inicioArg));
                    }
                }
                ExigirDelimitador("la instrucción PRINT");
            }
            // 4. Estructura IF (RW05) / Diagrama IN05
            else if (t.StartsWith("RW05") || l.Equals("IF", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW05", "IF");
                ParsearCondicion();
                if (!Match("RW06", "THEN")) ReportarError("Se esperaba instrucción THEN (RW06).");
                MatchDelimitador();

                ParsearInstrucciones();

                if (Match("RW07", "ELSE"))
                {
                    MatchDelimitador(); // Diagrama IN05_2: ELSE DEL
                    ParsearInstrucciones();
                }

                if (!Match("RW09", "ENDIF")) ReportarError("Se esperaba cierre ENDIF (RW09).");
                MatchDelimitador(); // Diagrama IN05_3: ENDIF DEL
            }
            // 5. Estructura PERHAPS (RW10) / Diagrama IN06
            else if (t.StartsWith("RW10") || l.Equals("PERHAPS", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW10", "PERHAPS");
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("IDV") || tokens[pos].Tipo.StartsWith("NUC")))
                {
                    pos++;
                }
                else
                {
                    ParsearExpresion();
                }
                MatchDelimitador();

                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW11") || tokens[pos].Lexema.Equals("CASE", StringComparison.OrdinalIgnoreCase)))
                {
                    Match("RW11", "CASE");
                    ParsearCondicionOExpresion();
                    if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC:") || tokens[pos].Lexema == ":"))
                    {
                        pos++;
                    }
                    MatchDelimitador();
                    ParsearInstrucciones();
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

                if (!Match("RW13", "ENDCASE")) ReportarError("Se esperaba cierre ENDCASE (RW13).");
                MatchDelimitador(); // Diagrama IN06_3: ENDCASE DEL
            }
            // 6. Ciclo WHILE (RW20) / Diagrama IN10 (WHILE CONDIC ... ENDWHILE DEL)
            else if (t.StartsWith("RW20") || l.Equals("WHILE", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW20", "WHILE");
                ParsearCondicion();

                // Permitir DO opcionalmente si se incluye, pero sin forzarlo
                if (Match("RW21", "DO"))
                {
                    MatchDelimitador();
                }
                else
                {
                    MatchDelimitador();
                }

                ParsearInstrucciones();
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

                if (tokId != null && finOp > inicioOp)
                {
                    List<TokenSintactico> tokensAsigFor = new List<TokenSintactico>
                    {
                        tokId,
                        tokAsig ?? new TokenSintactico { Tipo = "ALO=", Lexema = "=", Linea = lineaFor }
                    };
                    tokensAsigFor.AddRange(tokens.GetRange(inicioOp, finOp - inicioOp));
                    VerificarSemanticaOperacion(tokensAsigFor);
                }

                if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL.");
                ParsearCondicionOExpresion();

                if (!Match("RW18", "INTERVAL")) ReportarError("Se esperaba la palabra reservada INTERVAL.");
                if (!Match("NUC") && !Match("INC") && !Match("RNC") && !Match("NCR")) ReportarError("Se esperaba constante para el intervalo.");
                MatchDelimitador();

                ParsearInstrucciones();

                if (!Match("RW19", "ENDFOR")) ReportarError("Se esperaba cierre ENDFOR (RW19).");
                MatchDelimitador(); // Diagrama IN07_1: ENDFOR DEL
            }
            // 8. Ciclo DO WHILE (RW21) / Diagrama IN08
            else if (t.StartsWith("RW21") || l.Equals("DO", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW21", "DO");

                // Diagrama IN08: DO WHILE CONDIC ... ENDDO DEL
                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW20") || tokens[pos].Lexema.Equals("WHILE", StringComparison.OrdinalIgnoreCase)))
                {
                    Match("RW20", "WHILE");
                    ParsearCondicion();
                    MatchDelimitador();

                    ParsearInstrucciones();

                    if (!Match("RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDDO (RW23).");
                    MatchDelimitador(); // Diagrama IN08_1: ENDDO DEL
                }
                else
                {
                    // Variante de bloque: DO ... WHILE CONDIC ENDDO;
                    MatchDelimitador();
                    ParsearInstrucciones("RW20", "WHILE");

                    if (!Match("RW20", "WHILE")) ReportarError("Se esperaba la palabra reservada WHILE al final del DO.");
                    ParsearCondicion();

                    if (!Match("RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDDO (RW23).");
                    MatchDelimitador();
                }
            }
            // 9. Ciclo EXECUTE (RW22) / Diagrama IN09 (EXECUTE INST UNTIL CONDIC)
            else if (t.StartsWith("RW22") || l.Equals("EXECUTE", StringComparison.OrdinalIgnoreCase))
            {
                Match("RW22", "EXECUTE");
                MatchDelimitador();

                ParsearInstrucciones("RW17", "UNTIL");

                if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL al final del EXECUTE.");
                ParsearCondicion();

                // Consumo tolerante si incluyeron ENDDO
                Match("RW23", "ENDDO");
                MatchDelimitador();
            }
            else
            {
                ReportarError($"Instrucción no válida o no esperada: '{tokens[pos].Lexema}' ({tokens[pos].Tipo}).");
                SincronizarHastaDelimitador();
            }
        }

        // Analiza y valida una condición lógica o relacional compuesta ParsearCondicion()
        private void ParsearCondicion()
        {
            int inicioCond = pos;
            bool tieneNot = false;

            // Soporte para operador lógico unario NOT (LO2)
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("LO2") || tokens[pos].Lexema.Equals("NOT", StringComparison.OrdinalIgnoreCase)))
            {
                pos++;
                tieneNot = true;
            }

            // Soporte para condición agrupada entre paréntesis (ej. (a > b))
            bool tieneParentesis = false;
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("SC(") || tokens[pos].Lexema == "("))
            {
                pos++;
                tieneParentesis = true;
                ParsearCondicion();
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
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RO") || tokens[pos].Tipo.StartsWith("REO")))
            {
                tieneRelacional = true;
                pos++;
                ParsearExpresion();
            }

            bool tieneLogico = false;
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("LO") || tokens[pos].Tipo.StartsWith("LOP") || tokens[pos].Tipo.StartsWith("OLOG")))
            {
                tieneLogico = true;
                pos++;
                ParsearCondicion();
            }

            bool esBooleanoDirecto = (inicioCond < pos && tokens.GetRange(inicioCond, pos - inicioCond).Any(tok =>
                tok.Tipo.StartsWith("RW26") || tok.Tipo.StartsWith("RW08") ||
                tok.Lexema.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                tok.Lexema.Equals("FALSE", StringComparison.OrdinalIgnoreCase) ||
                semantico.ObtenerTipo(tok.Lexema) == "bool"));

            if (!tieneRelacional && !tieneLogico && !tieneNot && !tieneParentesis && !esBooleanoDirecto)
            {
                ReportarError("Se esperaba un operador relacional (>, <, ==, !=, >=, <=) o lógico en la condición.");
            }

            int finCond = pos;
            if (finCond > inicioCond)
            {
                VerificarSemanticaOperacion(tokens.GetRange(inicioCond, finCond - inicioCond));
            }
        }

        // Analiza argumentos mixtos que pueden ser condiciones o expresiones (ej. ARG6 en CASE / NONE)
        private void ParsearCondicionOExpresion()
        {
            int inicio = pos;
            ParsearExpresion();
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RO") || tokens[pos].Tipo.StartsWith("REO")))
            {
                pos++;
                ParsearExpresion();
            }
            int fin = pos;
            if (fin > inicio)
            {
                VerificarSemanticaOperacion(tokens.GetRange(inicio, fin - inicio));
            }
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
            // Soporte para operador lógico unario NOT (LO2, NOT, !)
            if (tokens[pos].Tipo.StartsWith("LO2") || tokens[pos].Lexema.Equals("NOT", StringComparison.OrdinalIgnoreCase) || tokens[pos].Lexema == "!")
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

            if (Match("IDV") || Match("INC") || Match("RNC") || Match("NCR") || Match("NUC") || Match("STR") ||
                Match("RW26", "TRUE") || Match("RW08", "FALSE") || Match("BOOL"))
            {
                // Operando válido
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
        private void VerificarSemanticaOperacion(List<TokenSintactico> tokensOp)
        {
            if (tokensOp == null || tokensOp.Count == 0) return;

            var resultado = semantico.AnalizarTokensCompilador(tokensOp);
            OperacionesDetectadas.Add(resultado);

            foreach (var err in resultado.ErroresSemanticos)
            {
                int linea = tokensOp.FirstOrDefault()?.Linea ?? 0;
                ReportarErrorSemantico(err, linea);
            }
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
            dgtErrores.Rows.Add(linea, mensaje);

            int lastRow = dgtErrores.Rows.Count - 1;
            if (lastRow >= 0)
                dgtErrores.Rows[lastRow].DefaultCellStyle.BackColor = System.Drawing.Color.MediumPurple;
        }
    }
}