using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    public partial class Form1 : Form
    {
        string NombreArchivo = "..\\..\\..\\..\\ArchivosTexto";
        string NombreArchivoTokens = "..\\..\\..\\..\\ArchivoTokens";
        List<Identificador> ListaDeIdentificadores = new List<Identificador>();
        int ContadorErrores = 0;


        // Cambia aquí los datos a los reales de tu gestor
        private string ConexionBD = @"Server=DESKTOP-3G6AMVL\SQLEXPRESS; Database=NovaNyx; Integrated Security=True; TrustServerCertificate=True;";

        ClaseListaSimpleOrdenada<Identificador> ListaDeIdentificadoresOrdenada = new ClaseListaSimpleOrdenada<Identificador>();
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

        // Se elimina la ejecución léxica en tiempo real para usar el botón "Analizar Todo"
        private void richProgramaFuente_TextChanged(object sender, EventArgs e)
        {
            picLineas.Invalidate(); // Solo se actualizan las líneas del editor visualmente
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            // Limpieza General
            richArchivoDeTokens.Clear();
            dgtErrores.Rows.Clear();
            if (!ListaDeIdentificadoresOrdenada.Vacia) ListaDeIdentificadoresOrdenada.Vaciar();

            List<string> Tokens = new List<string>();
            int NumeroDeLineas = richProgramaFuente.Lines.Length;

            // Ejecutar Analizador LÉXICO Línea por Línea
            for (int i = 0; i < NumeroDeLineas; i++)
            {
                VerificarToken(i, Tokens);
            }
            // Agregar tokens al UI
            richArchivoDeTokens.Lines = Tokens.ToArray();

            // Preparar los tokens generados para el Analizador SINTÁCTICO
            List<TokenSintactico> listaTokensSintacticos = new List<TokenSintactico>();
            for (int i = 0; i < richArchivoDeTokens.Lines.Length; i++)
            {
                string lineaTokens = richArchivoDeTokens.Lines[i];
                string[] partes = lineaTokens.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string parte in partes)
                {
                    // Evitamos mandar los errores léxicos al sintáctico para no crear cascadas de fallos
                    if (parte == "__ERROR__" || parte.StartsWith("Error")) continue;

                    listaTokensSintacticos.Add(new TokenSintactico() { Tipo = parte, Linea = i + 1 });
                }
            }

            //Ejecutar Analizador SINTÁCTICO
            AnalizadorSintactico sintactico = new AnalizadorSintactico(listaTokensSintacticos, dgtErrores);
            sintactico.ParsearPrograma();

            // Totalizador de Errores al final de la tabla
            int conteoErrores = dgtErrores.Rows.Count;
            ContadorErrores = conteoErrores;
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
            string ArchivoGuardar = "";

            OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.InitialDirectory = NombreArchivo;
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            saveFileDialog.Title = "Seleccionar ubicación para guardar el programa fuente";
            saveFileDialog.CheckFileExists = false;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ArchivoGuardar = saveFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo. Operación cancelada.");
                return;
            }

            Archivo<string> archivoTexto = new Archivo<string>(ArchivoGuardar);

            if (File.Exists(archivoTexto.NombreArchivo))
            {
                MessageBox.Show("El archivo ya existe. ");
                return;
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
            string NombreArchivoCargar = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = NombreArchivo;
            openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            openFileDialog.Title = "Seleccionar programa fuente";
            openFileDialog.Multiselect = false;
            openFileDialog.CheckFileExists = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                NombreArchivoCargar = openFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo. Operación cancelada.");
                return;
            }

            Archivo<string> archivoTexto = new Archivo<string>(NombreArchivoCargar);
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
        }

        private void btnEditarPrograma_Click(object sender, EventArgs e)
        {
            richProgramaFuente.ReadOnly = false;
            richProgramaFuente.Enabled = true;
            MessageBox.Show("El programa fuente ahora es editable.");
        }

        private void btnGuardarArchivo_Click(object sender, EventArgs e)
        {

            string ArchivoTokensGuardar = "";

            if (ContadorErrores > 0)
            {
                MessageBox.Show("No se puede guardar el archivo debido a errores.");
                return;
            }

            OpenFileDialog saveFileDialog = new OpenFileDialog();
            saveFileDialog.InitialDirectory = "C:\\Users\\DELL\\Desktop\\Interfaz Lexico\\ArchivoTokens";
            saveFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            saveFileDialog.Title = "Seleccionar ubicación para guardar el archivo de tokens";
            saveFileDialog.Multiselect = false;
            saveFileDialog.CheckFileExists = false;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ArchivoTokensGuardar = saveFileDialog.FileName;
            }
            else
            {
                MessageBox.Show("No se seleccionó ningún archivo. Operación cancelada.");
                return;
            }

            Archivo<string> archivoTexto = new Archivo<string>(ArchivoTokensGuardar);

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
    }
}
