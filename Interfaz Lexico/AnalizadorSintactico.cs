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

        // Inicia el análisis sintáctico del programa verificando inicio, cuerpo y fin ParsearPrograma()
        public void ParsearPrograma()
        {
            if (pos >= tokens.Count) return;

            if (!Match("RW01", "START")) ReportarError("Se esperaba el inicio del programa (START / RW01).");

            ParsearInstrucciones();

            if (pos < tokens.Count && !Match("RW02", "END")) ReportarError("Se esperaba el fin del programa (END / RW02).");
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
        private bool EsFinDeBloque(string[] stopTokensAdicionales)
        {
            if (pos >= tokens.Count) return true;
            string t = tokens[pos].Tipo;

            // Si le mandamos un token de parada (como el WHILE de un DO WHILE), detiene el bloque de instrucciones
            if (stopTokensAdicionales != null)
            {
                foreach (string stop in stopTokensAdicionales)
                {
                    if (t.StartsWith(stop)) return true;
                }
            }

            return t.StartsWith("RW02") || t.StartsWith("RW07") || t.StartsWith("RW09") || t.StartsWith("RW13") ||
                   t.StartsWith("RW24") || t.StartsWith("RW25") || t.StartsWith("RW32") ||
                   t.StartsWith("END") ||
                   t.StartsWith("ENDFOR") || /*t.StartsWith("RW18") ||*/
                   t.StartsWith("ENDDO") || t.StartsWith("RW19") ||
                   t.StartsWith("ENDEXECUTE") || t.StartsWith("RW23") ||
                   t.StartsWith("CASE") || t.StartsWith("RW11") ||
                   t.StartsWith("NONE") || t.StartsWith("RW12") ||
                   t.StartsWith("ENDCASE");
        }

        // Identifica y analiza la sintaxis de una instrucción específica según su token ParsearInstruccion()
        private void ParsearInstruccion()
        {
            if (pos >= tokens.Count) return;
            string t = tokens[pos].Tipo;

            // 1. Asignación
            if (t.StartsWith("IDV"))
            {
                int inicioOp = pos;
                Match("IDV");
                if (!Match("ALO=", "=")) ReportarError("Se esperaba operador de asignación (ALO= o =).");
                ParsearExpresion();
                int finOp = pos;
                VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
            }
            // 2. Lectura (READ) - RW03
            else if (t.StartsWith("RW03") || t.StartsWith("READ"))
            {
                Match("RW03", "READ");
                if (!Match("IDV") && !Match("NUC") && !Match("INC") && !Match("RNC") && !Match("STR"))
                    ReportarError("Se esperaba un argumento válido para leer (IDV, NUC, STR).");
            }
            // 3. Escritura (PRINT) - RW04
            else if (t.StartsWith("RW04") || t.StartsWith("PRINT"))
            {
                Match("RW04", "PRINT");
                int inicioOp = pos;
                ParsearExpresion();
                int finOp = pos;
                VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
            }
            // 4. Estructura IF
            else if (t.StartsWith("RW05") || t.StartsWith("IF"))
            {
                Match("RW05", "IF");
                ParsearCondicion();
                if (!Match("RW06", "THEN")) ReportarError("Se esperaba instrucción THEN (RW06).");

                ParsearInstrucciones();

                if (Match("RW07", "ELSE")) ParsearInstrucciones();

                if (!Match("RW09", "ENDIF")) ReportarError("Se esperaba cierre ENDIF (RW09).");
            }
            // 5. Estructura PERHAPS
            else if (t.StartsWith("RW10") || t.StartsWith("PERHAPS"))
            {
                Match("RW10", "PERHAPS");
                ParsearExpresion();
                Match("DEL");

                while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW11") || tokens[pos].Tipo.StartsWith("CASE")))
                {
                    Match("RW11", "CASE");
                    ParsearExpresion();
                    Match("DEL");
                    ParsearInstrucciones();
                }

                if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RW12") || tokens[pos].Tipo.StartsWith("NONE")))
                {
                    Match("RW12", "NONE");
                    Match("DEL");
                    ParsearInstrucciones();
                }

                if (!Match("RW13", "ENDCASE")) ReportarError("Se esperaba cierre ENDCASE (RW13).");
            }
            // 6. Ciclo WHILE NORMAL
            else if (t.StartsWith("RW20") || t.StartsWith("WHILE"))
            {
                Match("RW20", "WHILE");
                ParsearCondicion();

                if (!Match("RW21", "DO")) ReportarError("Se esperaba la palabra reservada DO.");
                Match("DEL");
                ParsearInstrucciones();
                if (!Match("RW24", "ENDWHILE")) ReportarError("Se esperaba cierre ENDWHILE (RW24).");
            }
            // 7. Ciclo FOR (Revisa que los RW14, RW15, etc., sean los correctos de tu imagen)
            else if (t.StartsWith("RW14") || t.StartsWith("FOR"))
            {
                int lineaFor = pos < tokens.Count ? tokens[pos].Linea : 1;
                Match("RW14", "FOR");
                TokenSintactico? tokId = pos < tokens.Count && tokens[pos].Tipo.StartsWith("IDV") ? tokens[pos] : null;
                if (!Match("IDV")) ReportarError("Se esperaba un identificador para el FOR.");
                TokenSintactico? tokAsig = pos < tokens.Count && (tokens[pos].Tipo.StartsWith("ALO=") || tokens[pos].Lexema == "=") ? tokens[pos] : null;
                if (!Match("ALO=", "=")) ReportarError("Se esperaba operador de asignación '='.");

                if (!Match("RW15", "FROM")) ReportarError("Se esperaba la palabra reservada FROM.");
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
                else
                {
                    VerificarSemanticaOperacion(tokens.GetRange(inicioOp, finOp - inicioOp));
                }

                if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL.");
                ParsearCondicion();

                if (!Match("RW18", "INTERVAL")) ReportarError("Se esperaba la palabra reservada INTERVAL.");
                if (!Match("NUC") && !Match("INC") && !Match("RNC")) ReportarError("Se esperaba constante para el intervalo.");
                Match("DEL");

                ParsearInstrucciones();

                if (!Match("RW19", "ENDFOR")) ReportarError("Se esperaba cierre ENDFOR.");
            }
            // 8. NUEVO: Ciclo DO WHILE (Ej. DO ... instrucciones ... WHILE condicion ENDDO)
            else if (t.StartsWith("RW21") || t.StartsWith("DO"))
            {
                Match("RW21", "DO");
                Match("DEL");

                // Lee las instrucciones y se DETIENE automáticamente al ver un WHILE
                ParsearInstrucciones("RW20", "WHILE");

                if (!Match("RW20", "WHILE")) ReportarError("Se esperaba la palabra reservada WHILE al final del DO.");
                ParsearCondicion();

                if (!Match("RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDDO (RW23).");
            }
            // 9. NUEVO: Ciclo EXECUTE (Ej. EXECUTE ... instrucciones ... UNTIL condicion ENDEXECUTE)
            else if (t.StartsWith("RW22") || t.StartsWith("EXECUTE"))
            {
                Match("RW22", "EXECUTE");
                Match("DEL");

                // Lee las instrucciones y se DETIENE automáticamente al ver un UNTIL
                ParsearInstrucciones("RW17", "UNTIL");

                if (!Match("RW17", "UNTIL")) ReportarError("Se esperaba la palabra reservada UNTIL al final del EXECUTE.");
                ParsearCondicion();

                // Cierre: Ajusta si en tu imagen dice ENDEXECUTE o ENDDO u otro
                if (!Match(/*"RW23", "ENDEXECUTE",*/ "RW23", "ENDDO")) ReportarError("Se esperaba cierre ENDEXECUTE.");
            }
            else
            {
                ReportarError($"Instrucción no válida o no esperada.");
                pos++;
            }

            Match("DEL", "SC;");
        }

        // Analiza y valida una condición lógica o relacional compuesta ParsearCondicion()
        private void ParsearCondicion()
        {
            int inicioCond = pos;
            ParsearExpresion();
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("RO") || tokens[pos].Tipo.StartsWith("REO")))
            {
                pos++;
                ParsearExpresion();
            }
            if (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("LO") || tokens[pos].Tipo.StartsWith("OLOG")))
            {
                pos++;
                ParsearCondicion();
            }
            int finCond = pos;
            if (finCond > inicioCond)
            {
                VerificarSemanticaOperacion(tokens.GetRange(inicioCond, finCond - inicioCond));
            }
        }

        // Analiza expresiones compuestas por términos separados por suma o resta ParsearExpresion()
        private void ParsearExpresion()
        {
            ParsearTermino();
            while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO+") || tokens[pos].Tipo.StartsWith("AO-")))
            {
                pos++;
                ParsearTermino();
            }
        }

        // Analiza términos compuestos por factores separados por multiplicación, división o módulo ParsearTermino()
        private void ParsearTermino()
        {
            ParsearPotencia();
            while (pos < tokens.Count && (tokens[pos].Tipo.StartsWith("AO*") || tokens[pos].Tipo.StartsWith("AO/") || tokens[pos].Tipo.StartsWith("AO%")))
            {
                pos++;
                ParsearPotencia();
            }
        }

        // Analiza operaciones de potencia respetando la jerarquía de operadores ParsearPotencia()
        private void ParsearPotencia()
        {
            ParsearFactor();
            while (pos < tokens.Count && tokens[pos].Tipo.StartsWith("AO^"))
            {
                pos++;
                ParsearPotencia();
            }
        }

        // Analiza factores primarios como constantes, variables, signos unarios o subexpresiones ParsearFactor()
        private void ParsearFactor()
        {
            if (pos >= tokens.Count) return;
            // Permitir signo unario (+ o -) en números con signo como +5 o -9
            if (tokens[pos].Tipo.StartsWith("AO+") || tokens[pos].Tipo.StartsWith("AO-"))
            {
                pos++;
            }
            if (Match("IDV") || Match("INC") || Match("RNC") || Match("NUC") || Match("STR")) { }
            else if (Match("SC("))
            {
                ParsearExpresion();
                if (!Match("SC)")) ReportarError("Se esperaba cierre de paréntesis 'SC)'.");
            }
            else
            {
                ReportarError("Se esperaba un valor, variable o expresión aritmética válida.");
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

        // Comprueba si el tipo del token actual coincide con alguno de los esperados y avanza Match()
        private bool Match(params string[] esperados)
        {
            if (pos >= tokens.Count) return false;
            string tipoActual = tokens[pos].Tipo;
            foreach (string exp in esperados)
            {
                if (tipoActual.StartsWith(exp))
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