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
        ClaseListaSimpleOrdenada<Identificador> ListaDeIdentificadoresOrdenada = new ClaseListaSimpleOrdenada<Identificador>();
        private string ConexionBD = @"Server=DESKTOP-3G6AMVL\SQLEXPRESS; Database=NovaNyx; Integrated Security=True; TrustServerCertificate=True;";
        private string[,] matrizCompleta;
        private List<string> alfabetoTemporal = new List<string>();

        Dictionary<string, string> Errores = new Dictionary<string, string>()
        {
            {"Error identificador no valido","__EIDNV__" },
            {"Error operador aritmetico no valido","__EARONV__" },
            {"Error operador logico no valido","__ELOPNV__" },
            { "Error operador relacional no valido","__EREONV__"},
            {"Error operador de asignacion no valido","__EALONV__" },
            { "Error constante numerica no valida","__ENUCNV__"},
            { "Error cadena no valida","__ESTRNV__"},
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
                // Se usan los métodos combinados para detectar la estructura y cargar los datos
                CargarEstructuraYDatosDesdeSQL();
                ConfigurarDataGridView();
                richArchivoDeTokens.ReadOnly = true;

                // Conectamos el evento Paint de nuestro PictureBox a nuestro método
                picLineas.Paint += picLineas_Paint;
                picLinea2.Paint += picLinea2_Paint;

                // Conectamos el Scroll del RichTextBox para que los números bajen al usar la rueda del ratón
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
            //Almacena Cada token de cada linea
            string NuevoToken = "";

            //Separa las palabras de cada linea por espacios y las agrega a la lista de tokens
            string[] palabras = richProgramaFuente.Lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

            bool Error = false;

            richArchivoDeTokens.Clear();


            //Agrega cada palabra de la linea a la variable NuevoToken, separada por un espacio
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
                        Debug.WriteLine($"Simbolo: {simbolo} no reconocido en el alfabeto.");
                        AgregarErrores("__ERROR__", i);
                        Error = true;
                        continue;
                    }

                    SiguienteEstado = matrizCompleta[EstadoActual, columna + 1] == "Error" ? -1 : int.Parse(matrizCompleta[EstadoActual, columna + 1]);
                    Debug.WriteLine($"Simbolo: {simbolo} | Estado Actual: {EstadoActual} | Columna: {columna + 1} | Siguiente Estado: {SiguienteEstado}");

                    if (SiguienteEstado == -1)
                    {
                        //Error porque no se acepta el token
                        NuevoToken += Errores.ContainsKey(matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1]) ? Errores[matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1]] : "__ERROR__";
                        AgregarErrores(Errores[matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1]], i);
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
                        //Error porque no se acepta el token
                        Debug.WriteLine($"Simbolo: EOC | Estado Actual: {EstadoActual} | Columna: {columna + 1} | Siguiente Estado: {SiguienteEstado}");
                        Debug.WriteLine(matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1].ToString());
                        NuevoToken += Errores[matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1]] + " ";
                        AgregarErrores(Errores[matrizCompleta[EstadoActual, matrizCompleta.GetLength(1) - 1]], i);
                        continue;
                    }
                    else
                    {
                        if (matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] == "IDV")
                        {

                            if (contadorChar == palabras[j].Length)
                            {
                                Debug.WriteLine("Misma longitud");
                                Identificador nuevoIdentificador = new Identificador();
                                nuevoIdentificador.NumeroDeIdentificador = j;
                                nuevoIdentificador.Nombre = palabras[j];
                                nuevoIdentificador.Valor = "Null";
                                nuevoIdentificador.TipoDeDato = "Null";
                                if (ListaDeIdentificadoresOrdenada.Vacia == true)
                                {
                                    ListaDeIdentificadoresOrdenada.Insertar(nuevoIdentificador);
                                }

                                if (ListaDeIdentificadoresOrdenada.Buscar(nuevoIdentificador) == false)
                                {
                                    ListaDeIdentificadoresOrdenada.Insertar(nuevoIdentificador);
                                }
                                else
                                {
                                    Debug.WriteLine($"El identificador '{palabras[j]}' ya existe en la tabla de símbolos.");
                                }
                            }
                            NuevoToken += matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] + " ";
                        }
                        else
                        {
                            //Token aceptado
                            NuevoToken += matrizCompleta[SiguienteEstado, matrizCompleta.GetLength(1) - 1] + " ";
                        }



                    }


                }
                Error = false;
                Debug.WriteLine("---------------------------------------");

            }


            dgtTablaDeSimbolos.Rows.Clear();
            foreach (var identificador in ListaDeIdentificadoresOrdenada)
            {
                dgtTablaDeSimbolos.Rows.Add(identificador.NumeroDeIdentificador + 1, identificador.Nombre, identificador.TipoDeDato, identificador.Valor);
            }

            //Agrega el token de la linea a la lista de tokens, eliminando el espacio al final
            Tokens.Add(NuevoToken.TrimEnd());


            //Agrega los token al nuevo archivo de tokens
            richArchivoDeTokens.Lines = Tokens.ToArray();
            palabras = null;
        }




        private void CargarEstructuraYDatosDesdeSQL()
        {
            using (SqlConnection conn = new SqlConnection(ConexionBD))
            {
                conn.Open();

                string query = "SELECT * FROM MatrizTransicion"; // Mejor ordenamiento seguro
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tablaDB = new DataTable();
                adapter.Fill(tablaDB);

                if (tablaDB.Rows.Count == 0)
                    throw new Exception("La tabla MatrizTransicion está vacía.");


                // 1. RECOPILAR EL ALFABETO TEMPORALMENTE (Para saber de qué tamaño será la matriz)
                //List<string> alfabetoTemporal = new List<string>();
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

                // 2. DIMENSIONAR LA MATRIZ ÚNICA
                int maxEstado = 0;
                foreach (DataRow row in tablaDB.Rows)
                {
                    if (int.TryParse(row[0].ToString(), out int id) && id > maxEstado)
                    {
                        maxEstado = id;
                    }


                }


                int totalFilas = maxEstado + 1; // +1 para la fila de ENCABEZADOS (Fila 0)
                int totalColumnas = alfabetoTemporal.Count + 2; // +1 para columna "Estado" y +1 para columna "ACEPTA"

                matrizCompleta = new string[totalFilas, totalColumnas];

                // 3. LLENAR LA FILA CERO (0) CON LOS ENCABEZADOS
                matrizCompleta[0, 0] = "Estado";
                for (int c = 0; c < alfabetoTemporal.Count; c++)
                {
                    matrizCompleta[0, c + 1] = alfabetoTemporal[c];
                }
                matrizCompleta[0, totalColumnas - 1] = "ACEPTA";

                // 4. INICIALIZAR EL RESTO DE LA MATRIZ POR DEFECTO (Para evitar nulos)
                for (int f = 1; f < totalFilas; f++)
                {
                    matrizCompleta[f, 0] = f.ToString(); // Guardamos el Estado en la col 0
                    for (int c = 1; c < totalColumnas - 1; c++)
                    {
                        matrizCompleta[f, c] = "Error"; // Transición por defecto
                    }
                    matrizCompleta[f, totalColumnas - 1] = "No valido"; // Acepta por defecto
                }

                // 5. VOLCAR LOS DATOS REALES DE LA BASE DE DATOS A LA MATRIZ
                foreach (DataRow row in tablaDB.Rows)
                {
                    if (!int.TryParse(row[0].ToString(), out int idEstadoActual))
                    {
                        continue;
                    }

                    // La fila en la matriz será el (Estado + 1) porque la fila 0 son los encabezados
                    int filaMatriz = idEstadoActual;

                    // Llenar transiciones
                    for (int c = 0; c < alfabetoTemporal.Count; c++)
                    {
                        string simbolo = alfabetoTemporal[c];
                        string nombreColDB = mapaColumnas[simbolo]; // Buscamos cómo se llama en SQL (ej: "a1")

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

                    // Llenar columna ACEPTA
                    matrizCompleta[filaMatriz, totalColumnas - 1] = row["ACEPTA"]?.ToString() ?? "No valido";
                }
            }
        }



        private void richProgramaFuente_TextChanged(object sender, EventArgs e)
        {
            picLineas.Invalidate();

            //Verifica el numero de lineas
            int NumeroDeLineas = richProgramaFuente.Lines.Count();

            //Crea una lista de tokens que se llenara con los tokens de cada linea
            List<string> Tokens = new List<string>();

            Tokens.Clear();
            if (ListaDeIdentificadoresOrdenada.Vacia == false)
            {
                ListaDeIdentificadoresOrdenada.Vaciar();
            }

            dgtErrores.Rows.Clear();

            //Recorre cada linea del programa fuente, separa las palabras por espacios y las agrega a la lista de tokens
            for (int i = 0; i < NumeroDeLineas; i++)
            {
                Debug.WriteLine($"Numero de filas {dgtErrores.Rows.Count}");

                if (dgtErrores.Rows.Count > 0)
                {
                    dgtErrores.Rows.RemoveAt(dgtErrores.Rows.Count - 1);
                }
                VerificarToken(i, Tokens);


                int conteoErrores = (dgtErrores.Rows.Count == 0) ? 0 : dgtErrores.Rows.Count;

                dgtErrores.Rows.Add("Total de Errores", conteoErrores);
            }

        }

        private void AgregarErrores(string error, int linea)
        {
            error = Errores2.ContainsKey(error) ? Errores2[error] : "Error desconocido";

            dgtErrores.Rows.Add(linea + 1, error);



            foreach (DataGridViewRow row in dgtErrores.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    row.DefaultCellStyle.BackColor = Color.Red;
                }
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

            // Leemos hasta que se acabe el archivo
            while (!archivoTexto.FinArchivo)
            {
                string lineaLeida = archivoTexto.LeerObjeto();

                if (lineaLeida != null)
                {
                    // Agregamos la línea al RichTextBox y damos un salto de línea
                    richProgramaFuente.AppendText(lineaLeida + Environment.NewLine);
                }
            }

            archivoTexto.CerrarArchivo();

            richProgramaFuente.ReadOnly = true;
            richProgramaFuente.Enabled = false;

            richProgramaFuente.HandleCreated += (s, ev) =>
            {
                // Forzamos el evento TextChanged para procesar el programa fuente cargado
                richProgramaFuente_TextChanged(s, EventArgs.Empty);
            };
            richProgramaFuente.AppendText(""); // Esto disparará el evento TextChanged
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

        // Método que dibuja los números en el PictureBox
        private void picLineas_Paint(object sender, PaintEventArgs e)
        {
            // 1. Calculamos cuál es la primera y última línea que el usuario tiene en pantalla
            int primerCaracterVisible = richProgramaFuente.GetCharIndexFromPosition(new Point(0, 0));
            int primeraLineaVisible = richProgramaFuente.GetLineFromCharIndex(primerCaracterVisible);

            int ultimoCaracterVisible = richProgramaFuente.GetCharIndexFromPosition(new Point(0, richProgramaFuente.Height));
            int ultimaLineaVisible = richProgramaFuente.GetLineFromCharIndex(ultimoCaracterVisible);

            // 2. Elegimos la fuente y el color de los números (Usa la misma fuente que tu código)
            Font fuente = richProgramaFuente.Font;
            Brush brocha = Brushes.Teal; // Color de los números (puedes cambiarlo a Gray, Blue, etc.)

            // 3. Dibujamos los números uno por uno
            for (int i = primeraLineaVisible; i <= ultimaLineaVisible; i++)
            {
                // Buscamos la coordenada 'Y' exacta de esa línea dentro del RichTextBox
                int indicePrimerCaracterLinea = richProgramaFuente.GetFirstCharIndexFromLine(i);
                Point posicion = richProgramaFuente.GetPositionFromCharIndex(indicePrimerCaracterLinea);

                // Alineamos el número a la derecha del PictureBox y lo dibujamos
                string numeroDeLinea = (i + 1).ToString();
                SizeF tamanoTexto = e.Graphics.MeasureString(numeroDeLinea, fuente);

                // Dibuja el texto en la posición calculada
                e.Graphics.DrawString(numeroDeLinea, fuente, brocha, picLineas.Width - tamanoTexto.Width - 5, posicion.Y);
            }
        }

        private void picLinea2_Paint(object sender, PaintEventArgs e)
        {
            // 1. Calculamos cuál es la primera y última línea que el usuario tiene en pantalla
            int primerCaracterVisible = richArchivoDeTokens.GetCharIndexFromPosition(new Point(0, 0));
            int primeraLineaVisible = richArchivoDeTokens.GetLineFromCharIndex(primerCaracterVisible);

            int ultimoCaracterVisible = richArchivoDeTokens.GetCharIndexFromPosition(new Point(0, richArchivoDeTokens.Height));
            int ultimaLineaVisible = richArchivoDeTokens.GetLineFromCharIndex(ultimoCaracterVisible);

            // 2. Elegimos la fuente y el color de los números (Usa la misma fuente que tu código)
            Font fuente = richArchivoDeTokens.Font;
            Brush brocha = Brushes.Teal; // Color de los números (puedes cambiarlo a Gray, Blue, etc.)

            // 3. Dibujamos los números uno por uno
            for (int i = primeraLineaVisible; i <= ultimaLineaVisible; i++)
            {
                // Buscamos la coordenada 'Y' exacta de esa línea dentro del RichTextBox
                int indicePrimerCaracterLinea = richArchivoDeTokens.GetFirstCharIndexFromLine(i);
                Point posicion = richArchivoDeTokens.GetPositionFromCharIndex(indicePrimerCaracterLinea);

                // Alineamos el número a la derecha del PictureBox y lo dibujamos
                string numeroDeLinea = (i + 1).ToString();
                SizeF tamanoTexto = e.Graphics.MeasureString(numeroDeLinea, fuente);

                // Dibuja el texto en la posición calculada
                e.Graphics.DrawString(numeroDeLinea, fuente, brocha, picLinea2.Width - tamanoTexto.Width - 5, posicion.Y);
            }
        }

        private void richArchivoDeTokens_TextChanged(object sender, EventArgs e)
        {
            picLinea2.Invalidate();
        }
    }
}
