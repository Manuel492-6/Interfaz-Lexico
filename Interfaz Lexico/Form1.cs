using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    public partial class Form1 : Form
    {
        string NombreArchivo = "..\\..\\..\\..\\ArchivosTexto\\Archivo.txt";
        string NombreArchivo2 = "..\\..\\..\\..\\ArchivosTexto\\Archivo2.txt";
        List<Identificador> ListaDeIdentificadores = new List<Identificador>();

        // Cambia aquí los datos a los reales de tu gestor
        private string ConexionBD = @"Server=DESKTOP-3G6AMVL\SQLEXPRESS; Database=NovaNyx; Integrated Security=True; TrustServerCertificate=True;";

        ClaseListaSimpleOrdenada<Identificador> ListaDeIdentificadoresOrdenada = new ClaseListaSimpleOrdenada<Identificador>();
        private AnalizadorSemanticoJerarquia analizadorSemantico = new AnalizadorSemanticoJerarquia();
        private List<ResultadoSemanticoOperacion> operacionesDetectadas = new List<ResultadoSemanticoOperacion>();
        private string[,] matrizCompleta;
        private List<string> alfabetoTemporal = new List<string>();

        Dictionary<string, string> Errores = new Dictionary<string, string>()
        {
            {"Error identificador no valido","__EIDNV__" },
            {"Error operador aritmetico no valido","__EARONV__" },
            {"Error operador logico no valido","__ELOPNV__" },
            {"Error operador relacional no valido","__EREONV__"},
            {"Error operador de asignacion no valido","__EALONV__" },
            {"Error constante numerica no valida","__ENUCNV__"},
            {"Error cadena no valida","__ESTRNV__"},
            {"Error comentario no valido","__ECOMNV__"},
            {"Error caracter especial no valido","__ESPCNV__"},
            {"Error palabra reservada no valida","__ERWNV__"},
            {"No valido","__ERROR__" }
        };

        Dictionary<string, string> Errores2 = new Dictionary<string, string>()
        {
            {"__EIDNV__", "Error identificador no valido"},
            {"__EARONV__", "Error operador aritmetico no valido"},
            {"__ELOPNV__", "Error operador logico no valido"},
            {"__EREONV__", "Error operador relacional no valido"},
            {"__EALONV__", "Error operador de asignacion no valido"},
            {"__ENUCNV__", "Error constante numerica no valida"},
            {"__ESTRNV__", "Error cadena no valida"},
            {"__ECOMNV__", "Error comentario no valido"},
            {"__ESPCNV__", "Error caracter especial no valido"},
            {"__ERWNV__", "Error palabra reservada no valida"},
            {"__ERROR__", "Error no valido por caracteres no validos"}
         };


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                CargarEstructuraYDatosDesdeSQL();
                ConfigurarDataGridView();
                richArchivoDeTokens.ReadOnly = true;

                picLineas.Paint += picLineas_Paint;
                picLinea2.Paint += picLinea2_Paint;

                // Solo para redibujar visualmente cuando se hace scroll
                richProgramaFuente.VScroll += (s, ev) => picLineas.Invalidate();
                richProgramaFuente.HScroll += (s, ev) => picLineas.Invalidate();

                richArchivoDeTokens.VScroll += (s, ev) => picLinea2.Invalidate();
                richArchivoDeTokens.HScroll += (s, ev) => picLinea2.Invalidate();

                // Estado de espera inicial hasta presionar 'Analizar Todo'
                InicializarArbolEnEspera();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar o procesar la tabla:\n\n" + ex.Message);
            }
        }

        private void VerificarToken(int i, List<string> Tokens)
        {
            string NuevoToken = "";
            string[] palabras = richProgramaFuente.Lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            bool Error = false;

            for (int j = 0; j < palabras.Length; j++)
            {
                int EstadoActual = 1;
                int SiguienteEstado = 1;
                int columna = 0;
                int contadorChar = 0;

                foreach (char simbolo in palabras[j])
                {
                    contadorChar++;
                    columna = alfabetoTemporal.IndexOf(simbolo.ToString());

                    if (columna == -1)
                    {
                        NuevoToken += "__ERROR__ " + " ";
                        AgregarErrores("__ERROR__", i);
                        Error = true;
                        continue;
                    }

                    SiguienteEstado = matrizCompleta[EstadoActual, columna + 1] == "Error" ? -1 : int.Parse(matrizCompleta[EstadoActual, columna + 1]);

                    if (SiguienteEstado == -1)
                    {
                        string eKey = matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1];
                        NuevoToken += Errores.ContainsKey(eKey) ? Errores[eKey] : "__ERROR__";
                        AgregarErrores(Errores.ContainsKey(eKey) ? Errores[eKey] : "__ERROR__", i);
                        Error = true;
                        continue;
                    }
                    EstadoActual = SiguienteEstado;
                }

                if (!Error)
                {
                    columna = alfabetoTemporal.IndexOf("EOC");
                    SiguienteEstado = int.TryParse(matrizCompleta[EstadoActual, columna + 1], out int resultado) ? resultado : -1;

                    if (SiguienteEstado == -1)
                    {
                        string eKey = matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1];
                        NuevoToken += Errores.ContainsKey(eKey) ? Errores[eKey] : "__ERROR__";
                        AgregarErrores(Errores.ContainsKey(eKey) ? Errores[eKey] : "__ERROR__", i);
                        continue;
                    }
                    else
                    {
                        if (matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] == "IDV")
                        {
                            if (contadorChar == palabras[j].Length)
                            {
                                Identificador nuevoIdentificador = new Identificador();
                                nuevoIdentificador.Nombre = palabras[j];
                                nuevoIdentificador.Valor = "Null";
                                nuevoIdentificador.TipoDeDato = "Null";

                                Identificador existente = null;

                                if (!ListaDeIdentificadoresOrdenada.Vacia)
                                {
                                    foreach (var item in ListaDeIdentificadoresOrdenada)
                                    {
                                        if (item.Nombre == nuevoIdentificador.Nombre)
                                        {
                                            existente = item;
                                            break;
                                        }
                                    }
                                }

                                int idAUsar;
                                if (existente == null)
                                {
                                    idAUsar = ListaDeIdentificadoresOrdenada.Contar + 1;
                                    nuevoIdentificador.NumeroDeIdentificador = idAUsar;
                                    ListaDeIdentificadoresOrdenada.Insertar(nuevoIdentificador);
                                }
                                else
                                {
                                    idAUsar = existente.NumeroDeIdentificador;
                                }

                                NuevoToken += matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] + idAUsar.ToString() + " ";
                            }
                            else
                            {
                                NuevoToken += matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] + " ";
                            }
                        }
                        else
                        {
                            NuevoToken += matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] + " ";
                        }
                    }
                }
                Error = false;
            }

            dgtTablaDeSimbolos.Rows.Clear();
            foreach (var identificador in ListaDeIdentificadoresOrdenada)
            {
                dgtTablaDeSimbolos.Rows.Add(identificador.NumeroDeIdentificador, identificador.Nombre, identificador.TipoDeDato, identificador.Valor);
            }

            Tokens.Add(NuevoToken.TrimEnd());
            palabras = null;
        }

        private void CargarEstructuraYDatosDesdeSQL()
        {
            using (SqlConnection conn = new SqlConnection(ConexionBD))
            {
                conn.Open();

                string query = "SELECT * FROM MatrizTransicion";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tablaDB = new DataTable();
                adapter.Fill(tablaDB);

                if (tablaDB.Rows.Count == 0)
                    throw new Exception("La tabla MatrizTransicion está vacía.");

                Dictionary<string, string> mapaColumnas = new Dictionary<string, string>();

                foreach (DataColumn col in tablaDB.Columns)
                {
                    string nombreReal = col.ColumnName;
                    if (nombreReal != tablaDB.Columns[0].ColumnName && nombreReal.ToUpper() != "ACEPTA")
                    {
                        string nombreLimpio = nombreReal;
                        if (nombreReal.Length == 2 && nombreReal.EndsWith("1"))
                        {
                            nombreLimpio = nombreReal.Substring(0, 1);
                        }
                        alfabetoTemporal.Add(nombreLimpio);
                        mapaColumnas[nombreLimpio] = nombreReal;
                    }
                }

                int maxEstado = 0;
                foreach (DataRow row in tablaDB.Rows)
                {
                    if (int.TryParse(row[0].ToString(), out int id) && id > maxEstado)
                    {
                        maxEstado = id;
                    }
                }

                int totalFilas = maxEstado + 1;
                int totalColumnas = alfabetoTemporal.Count + 2;

                matrizCompleta = new string[totalFilas, totalColumnas];

                matrizCompleta[0, 0] = "Estado";
                for (int c = 0; c < alfabetoTemporal.Count; c++)
                {
                    matrizCompleta[0, c + 1] = alfabetoTemporal[c];
                }
                matrizCompleta[0, totalColumnas - 1] = "ACEPTA";

                for (int f = 1; f < totalFilas; f++)
                {
                    matrizCompleta[f, 0] = f.ToString();
                    for (int c = 1; c < totalColumnas - 1; c++)
                    {
                        matrizCompleta[f, c] = "Error";
                    }
                    matrizCompleta[f, totalColumnas - 1] = "No valido";
                }

                foreach (DataRow row in tablaDB.Rows)
                {
                    if (!int.TryParse(row[0].ToString(), out int idEstadoActual))
                    {
                        continue;
                    }

                    int filaMatriz = idEstadoActual;

                    for (int c = 0; c < alfabetoTemporal.Count; c++)
                    {
                        string simbolo = alfabetoTemporal[c];
                        string nombreColDB = mapaColumnas[simbolo];

                        object valorCelda = row[nombreColDB];

                        if (valorCelda != DBNull.Value && !string.IsNullOrWhiteSpace(valorCelda.ToString()))
                        {
                            string valorString = valorCelda.ToString().ToLower();
                            if (valorString != "error")
                            {
                                matrizCompleta[filaMatriz, c + 1] = valorCelda.ToString();
                            }
                        }
                    }

                    matrizCompleta[filaMatriz, totalColumnas - 1] = row["ACEPTA"]?.ToString() ?? "No valido";
                }
            }
        }

        // Evento de cambio de texto en el Programa Fuente
        private void richProgramaFuente_TextChanged(object sender, EventArgs e)
        {
            picLineas.Invalidate(); // Se actualizan las líneas del editor visualmente
        }

        // =====================================================================
        // BOTÓN PRINCIPAL DE ANÁLISIS (LÉXICO Y SINTÁCTICO)
        // =====================================================================
        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            // 1. Limpieza General
            richArchivoDeTokens.Clear();
            dgtErrores.Rows.Clear();
            if (!ListaDeIdentificadoresOrdenada.Vacia) ListaDeIdentificadoresOrdenada.Vaciar();

            List<string> Tokens = new List<string>();
            int NumeroDeLineas = richProgramaFuente.Lines.Length;

            // 2. Ejecutar Analizador LÉXICO Línea por Línea
            for (int i = 0; i < NumeroDeLineas; i++)
            {
                VerificarToken(i, Tokens);
            }
            // Agregar tokens al UI
            richArchivoDeTokens.Lines = Tokens.ToArray();

            // 3. Preparar los tokens generados para el Analizador SINTÁCTICO y SEMÁNTICO
            List<TokenSintactico> listaTokensSintacticos = new List<TokenSintactico>();
            for (int i = 0; i < richArchivoDeTokens.Lines.Length; i++)
            {
                string lineaTokens = richArchivoDeTokens.Lines[i];
                string[] partes = lineaTokens.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string[] palabras = i < richProgramaFuente.Lines.Length
                    ? richProgramaFuente.Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    : new string[0];

                for (int p = 0; p < partes.Length; p++)
                {
                    string parte = partes[p];
                    // Evitamos mandar los errores léxicos al sintáctico para no crear cascadas de fallos
                    if (parte == "__ERROR__" || parte.StartsWith("Error")) continue;

                    string lexema = p < palabras.Length ? palabras[p] : parte;
                    listaTokensSintacticos.Add(new TokenSintactico() { Tipo = parte, Lexema = lexema, Linea = i + 1 });
                }
            }

            // Registrar identificadores existentes en el analizador semántico
            analizadorSemantico = new AnalizadorSemanticoJerarquia();
            if (!ListaDeIdentificadoresOrdenada.Vacia)
            {
                foreach (var id in ListaDeIdentificadoresOrdenada)
                {
                    string tipoInicial = (!string.IsNullOrEmpty(id.TipoDeDato) && id.TipoDeDato != "Null") ? id.TipoDeDato : "int";
                    string? valInicial = (!string.IsNullOrEmpty(id.Valor) && id.Valor != "Null") ? id.Valor : null;
                    analizadorSemantico.RegistrarVariable(id.Nombre, tipoInicial, valInicial);
                }
            }

            // 4. Ejecutar Analizador SINTÁCTICO y SEMÁNTICO
            AnalizadorSintactico sintactico = new AnalizadorSintactico(listaTokensSintacticos, dgtErrores, analizadorSemantico);
            sintactico.ParsearPrograma();

            // 5. Compilar simultáneamente el Árbol de Jerarquía junto con los tokens
            CompilarArbolJerarquia(sintactico);

            // 6. Actualizar la tabla de símbolos en el DataGridView con los tipos y valores del análisis semántico / árbol
            ActualizarTablaDeSimbolos();

            // 7. Totalizador de Errores al final de la tabla
            int conteoErrores = dgtErrores.Rows.Count;
            dgtErrores.Rows.Add("Total de Errores", conteoErrores);
        }

        private void AgregarErrores(string error, int linea)
        {
            error = Errores2.ContainsKey(error) ? Errores2[error] : "Error desconocido";
            dgtErrores.Rows.Add(linea + 1, error);

            int lastIndex = dgtErrores.Rows.Count - 1;
            if (lastIndex >= 0)
            {
                dgtErrores.Rows[lastIndex].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
            }
        }

        private void ConfigurarDataGridView()
        {
            dgtErrores.AllowUserToDeleteRows = false;
            dgtErrores.AllowUserToAddRows = false;
            dgtErrores.ReadOnly = true;
            dgtErrores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgtTablaDeSimbolos.AllowUserToAddRows = false;
            dgtTablaDeSimbolos.AllowUserToDeleteRows = false;
            dgtTablaDeSimbolos.ReadOnly = true;
            dgtTablaDeSimbolos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void btnGuardarPrograma_Click(object sender, EventArgs e)
        {
            Archivo<string> archivoTexto = new Archivo<string>(NombreArchivo);

            if (File.Exists(archivoTexto.NombreArchivo))
            {
                archivoTexto.EliminarArchivo();
            }

            archivoTexto.HacerModoEscritura();

            foreach (string linea in richProgramaFuente.Lines)
            {
                archivoTexto.AgregarObjeto(linea);
            }

            archivoTexto.CerrarArchivo();
            MessageBox.Show("Archivo guardado correctamente.");
        }

        private void btnCargarPrograma_Click(object sender, EventArgs e)
        {
            Archivo<string> archivoTexto = new Archivo<string>(NombreArchivo);
            archivoTexto.HacerModoLectura();

            richProgramaFuente.Clear();
            dgtErrores.Rows.Clear();
            dgtTablaDeSimbolos.Rows.Clear();
            richArchivoDeTokens.Clear();

            while (!archivoTexto.FinArchivo)
            {
                string lineaLeida = archivoTexto.LeerObjeto();
                if (lineaLeida != null)
                {
                    richProgramaFuente.AppendText(lineaLeida + Environment.NewLine);
                }
            }

            archivoTexto.CerrarArchivo();
            richProgramaFuente.ReadOnly = true;
            richProgramaFuente.Enabled = false;
            InicializarArbolEnEspera();
        }

        private void btnEditarPrograma_Click(object sender, EventArgs e)
        {
            richProgramaFuente.ReadOnly = false;
            richProgramaFuente.Enabled = true;
            MessageBox.Show("El programa fuente ahora es editable.");
        }

        private void btnGuardarArchivo_Click(object sender, EventArgs e)
        {
            Archivo<string> archivoTexto = new Archivo<string>(NombreArchivo2);

            if (File.Exists(archivoTexto.NombreArchivo))
            {
                archivoTexto.EliminarArchivo();
            }

            archivoTexto.HacerModoEscritura();

            foreach (string linea in richArchivoDeTokens.Lines)
            {
                archivoTexto.AgregarObjeto(linea);
            }

            archivoTexto.CerrarArchivo();
            MessageBox.Show("Archivo guardado correctamente.");
        }

        private void picLineas_Paint(object sender, PaintEventArgs e)
        {
            int primerCaracterVisible = richProgramaFuente.GetCharIndexFromPosition(new System.Drawing.Point(0, 0));
            int primeraLineaVisible = richProgramaFuente.GetLineFromCharIndex(primerCaracterVisible);

            int ultimoCaracterVisible = richProgramaFuente.GetCharIndexFromPosition(new System.Drawing.Point(0, richProgramaFuente.Height));
            int ultimaLineaVisible = richProgramaFuente.GetLineFromCharIndex(ultimoCaracterVisible);

            System.Drawing.Font fuente = richProgramaFuente.Font;
            System.Drawing.Brush brocha = System.Drawing.Brushes.Teal;

            for (int i = primeraLineaVisible; i <= ultimaLineaVisible; i++)
            {
                int indicePrimerCaracterLinea = richProgramaFuente.GetFirstCharIndexFromLine(i);
                System.Drawing.Point posicion = richProgramaFuente.GetPositionFromCharIndex(indicePrimerCaracterLinea);
                string numeroDeLinea = (i + 1).ToString();
                System.Drawing.SizeF tamanoTexto = e.Graphics.MeasureString(numeroDeLinea, fuente);
                e.Graphics.DrawString(numeroDeLinea, fuente, brocha, picLineas.Width - tamanoTexto.Width - 5, posicion.Y);
            }
        }

        private void picLinea2_Paint(object sender, PaintEventArgs e)
        {
            int primerCaracterVisible = richArchivoDeTokens.GetCharIndexFromPosition(new System.Drawing.Point(0, 0));
            int primeraLineaVisible = richArchivoDeTokens.GetLineFromCharIndex(primerCaracterVisible);

            int ultimoCaracterVisible = richArchivoDeTokens.GetCharIndexFromPosition(new System.Drawing.Point(0, richArchivoDeTokens.Height));
            int ultimaLineaVisible = richArchivoDeTokens.GetLineFromCharIndex(ultimoCaracterVisible);

            System.Drawing.Font fuente = richArchivoDeTokens.Font;
            System.Drawing.Brush brocha = System.Drawing.Brushes.Teal;

            for (int i = primeraLineaVisible; i <= ultimaLineaVisible; i++)
            {
                int indicePrimerCaracterLinea = richArchivoDeTokens.GetFirstCharIndexFromLine(i);
                System.Drawing.Point posicion = richArchivoDeTokens.GetPositionFromCharIndex(indicePrimerCaracterLinea);
                string numeroDeLinea = (i + 1).ToString();
                System.Drawing.SizeF tamanoTexto = e.Graphics.MeasureString(numeroDeLinea, fuente);
                e.Graphics.DrawString(numeroDeLinea, fuente, brocha, picLinea2.Width - tamanoTexto.Width - 5, posicion.Y);
            }
        }

        private void richArchivoDeTokens_TextChanged(object sender, EventArgs e)
        {
            picLinea2.Invalidate();
        }

        /// <summary>
        /// Compila y genera el Árbol de Jerarquía al presionar el botón 'Analizar Todo'
        /// </summary>
        public void CompilarArbolJerarquia(AnalizadorSintactico? sintactico = null)
        {
            // Registrar variables de la tabla de símbolos si existen y aún no están registradas
            if (!ListaDeIdentificadoresOrdenada.Vacia)
            {
                foreach (var id in ListaDeIdentificadoresOrdenada)
                {
                    if (!analizadorSemantico.TieneVariable(id.Nombre))
                    {
                        string tipoInicial = (!string.IsNullOrEmpty(id.TipoDeDato) && id.TipoDeDato != "Null") ? id.TipoDeDato : "int";
                        string? valInicial = (!string.IsNullOrEmpty(id.Valor) && id.Valor != "Null") ? id.Valor : null;
                        analizadorSemantico.RegistrarVariable(id.Nombre, tipoInicial, valInicial);
                    }
                }
            }

            // Usar operaciones detectadas por el analizador sintáctico o extraerlas del código
            if (sintactico != null && sintactico.OperacionesDetectadas.Count > 0)
            {
                operacionesDetectadas = sintactico.OperacionesDetectadas;
            }
            else
            {
                operacionesDetectadas = analizadorSemantico.ExtraerOperacionesDeCodigoFuente(richProgramaFuente.Text);
            }

            // Cargar visualmente en tvArbolJerarquia
            analizadorSemantico.CargarArbolSincronizado(tvArbolJerarquia, operacionesDetectadas);

            // Sincronizar la tabla de símbolos en el DataGridView con los tipos y valores del árbol
            ActualizarTablaDeSimbolos();

            // Actualizar ComboBox con las operaciones detectadas
            cboOperacionesCodigo.SelectedIndexChanged -= cboOperacionesCodigo_SelectedIndexChanged;
            cboOperacionesCodigo.Items.Clear();

            if (operacionesDetectadas.Count > 0)
            {
                for (int i = 0; i < operacionesDetectadas.Count; i++)
                {
                    var op = operacionesDetectadas[i];
                    string estado = op.EsValida ? "✅" : "❌";
                    cboOperacionesCodigo.Items.Add($"{estado} Op {i + 1}: {op.ExpresionOriginal}");
                }
                cboOperacionesCodigo.SelectedIndex = 0;
                cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;

                MostrarPasosOperacion(operacionesDetectadas[0]);
            }
            else
            {
                cboOperacionesCodigo.Items.Add("(No se detectaron operaciones)");
                cboOperacionesCodigo.SelectedIndex = 0;
                cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;

                lblEstadoSemantico.Text = "No se detectaron operaciones en el código";
                lblEstadoSemantico.ForeColor = Color.DimGray;
                rtbPasosJerarquia.Clear();
            }
        }

        /// <summary>
        /// Sincroniza la tabla de símbolos (ListaDeIdentificadoresOrdenada y dgtTablaDeSimbolos)
        /// con los tipos de datos y valores determinados por el analizador semántico y el árbol jerárquico.
        /// </summary>
        private void ActualizarTablaDeSimbolos()
        {
            if (!ListaDeIdentificadoresOrdenada.Vacia)
            {
                foreach (var identificador in ListaDeIdentificadoresOrdenada)
                {
                    string tipoSemantico = analizadorSemantico.ObtenerTipo(identificador.Nombre);
                    if (!string.IsNullOrEmpty(tipoSemantico) && tipoSemantico != "Null")
                    {
                        identificador.TipoDeDato = tipoSemantico;
                    }
                    else if (string.IsNullOrEmpty(identificador.TipoDeDato) || identificador.TipoDeDato == "Null")
                    {
                        identificador.TipoDeDato = "int";
                    }

                    string? valorSemantico = analizadorSemantico.ObtenerValor(identificador.Nombre);
                    if (!string.IsNullOrEmpty(valorSemantico))
                    {
                        identificador.Valor = valorSemantico;
                    }
                }
            }

            dgtTablaDeSimbolos.Rows.Clear();
            foreach (var identificador in ListaDeIdentificadoresOrdenada)
            {
                dgtTablaDeSimbolos.Rows.Add(
                    identificador.NumeroDeIdentificador,
                    identificador.Nombre,
                    identificador.TipoDeDato,
                    identificador.Valor
                );
            }
        }

        /// <summary>
        /// Establece el árbol en estado de espera inicial hasta presionar 'Analizar Todo'
        /// </summary>
        private void InicializarArbolEnEspera()
        {
            tvArbolJerarquia.BeginUpdate();
            tvArbolJerarquia.Nodes.Clear();
            TreeNode nodoEspera = new TreeNode("⚡ Presiona 'Analizar Todo' para compilar el árbol")
            {
                ForeColor = Color.SlateGray
            };
            tvArbolJerarquia.Nodes.Add(nodoEspera);
            tvArbolJerarquia.EndUpdate();

            cboOperacionesCodigo.SelectedIndexChanged -= cboOperacionesCodigo_SelectedIndexChanged;
            cboOperacionesCodigo.Items.Clear();
            cboOperacionesCodigo.Items.Add("(Presiona 'Analizar Todo')");
            cboOperacionesCodigo.SelectedIndex = 0;
            cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;

            lblEstadoSemantico.Text = "Listo: presiona 'Analizar Todo'";
            lblEstadoSemantico.ForeColor = Color.SlateGray;
            rtbPasosJerarquia.Clear();
        }

        private void MostrarPasosOperacion(ResultadoSemanticoOperacion op)
        {
            rtbPasosJerarquia.Clear();
            if (op == null) return;

            if (op.EsValida)
            {
                string tipo = op.TipoDatoResultante;
                string valor = !string.IsNullOrEmpty(op.ValorCalculado) ? $" | Resultado: {op.ValorCalculado}" : "";
                lblEstadoSemantico.Text = $"✅ Jerarquía Válida ({tipo}{valor})";
                lblEstadoSemantico.ForeColor = Color.ForestGreen;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"OPERACIÓN: {op.ExpresionOriginal}");
                sb.AppendLine("---------------------------------------------");
                sb.AppendLine("ORDEN DE EVALUACIÓN SEMÁNTICA POR JERARQUÍA:");
                foreach (var paso in op.PasosEvaluacion)
                {
                    sb.AppendLine(paso);
                }
                if (!string.IsNullOrEmpty(op.ValorCalculado))
                {
                    sb.AppendLine("---------------------------------------------");
                    sb.AppendLine($"VALOR FINAL CALCULADO: {op.ValorCalculado}");
                }
                rtbPasosJerarquia.Text = sb.ToString();
            }
            else
            {
                lblEstadoSemantico.Text = "❌ Error Semántico en la Operación";
                lblEstadoSemantico.ForeColor = Color.Red;

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"OPERACIÓN CON ERROR: {op.ExpresionOriginal}");
                sb.AppendLine("---------------------------------------------");
                sb.AppendLine("ERRORES SEMÁNTICOS:");
                foreach (var err in op.ErroresSemanticos)
                {
                    sb.AppendLine("• " + err);
                }
                rtbPasosJerarquia.Text = sb.ToString();
            }
        }

        private void cboOperacionesCodigo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = cboOperacionesCodigo.SelectedIndex;
            if (idx >= 0 && idx < operacionesDetectadas.Count)
            {
                MostrarPasosOperacion(operacionesDetectadas[idx]);
            }
        }

        private void tvArbolJerarquia_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;

            TreeNode actual = e.Node;
            while (actual != null)
            {
                if (actual.Tag is ResultadoSemanticoOperacion opTag)
                {
                    MostrarPasosOperacion(opTag);
                    int idx = operacionesDetectadas.IndexOf(opTag);
                    if (idx >= 0 && idx < cboOperacionesCodigo.Items.Count && cboOperacionesCodigo.SelectedIndex != idx)
                    {
                        cboOperacionesCodigo.SelectedIndexChanged -= cboOperacionesCodigo_SelectedIndexChanged;
                        cboOperacionesCodigo.SelectedIndex = idx;
                        cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;
                    }
                    break;
                }
                actual = actual.Parent;
            }
        }
    }
}
