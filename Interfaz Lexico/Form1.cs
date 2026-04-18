using System.Data;
using System.Drawing.Drawing2D;
using Microsoft.Data.SqlClient;

namespace Interfaz_Lexico
{
    public partial class Form1 : Form
    {

        private string ConexionBD = @"Server=DESKTOP-3G6AMVL\SQLEXPRESS; Database=NovaNyx; Integrated Security=True; TrustServerCertificate=True;";
        private Dictionary<string, string> mapaColumnas = new Dictionary<string, string>();
        private List<string> alfabeto = new List<string>();
        private int[,] matriz;
        private Dictionary<int, string> categoriasEstado = new Dictionary<int, string>();
        private int maxEstado = 0;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Se usan los metdos combinados para detectar la estructura y cargar los datos
                CargarEstructuraYDatosDesdeSQL();
                CargarMatrizEnGrid();
                MessageBox.Show("Conexión exitosa y datos cargados correctamente.");
                MessageBox.Show(matriz[1,2].ToString());

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar o procesar la tabla:\n\n" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int lineCount = richProgramaFuente.Lines.Count(linea => !string.IsNullOrWhiteSpace(linea));
            MessageBox.Show(lineCount.ToString());

            MessageBox.Show(richProgramaFuente.Lines[1]);
            int lineasescritas = richProgramaFuente.Lines.Count();
            MessageBox.Show(lineasescritas.ToString());

            for (int i = 0; i < richProgramaFuente.Lines.Length; i++)
            {

                string[] palabras = richProgramaFuente.Lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < palabras.Length; j++)
                {
                    listBox1.Items.Add(palabras[j]);
                }
            }
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            //Verifica el numero de lineas
            int NumeroDeLineas = richProgramaFuente.Lines.Count();

            //Crea una lista de tokens que se llenara con los tokens de cada linea
            List<string> Tokens = new List<string>();


            //Recorre cada linea del programa fuente, separa las palabras por espacios y las agrega a la lista de tokens
            for (int i = 0; i < NumeroDeLineas; i++)
            {
                //Almacena Cada token de cada linea
                string NuevoToken = "";
                //Separa las palabras de cada linea por espacios y las agrega a la lista de tokens
                string[] palabras = richProgramaFuente.Lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);

                //Agrega cada palabra de la linea a la variable NuevoToken, separada por un espacio
                for (int j = 0; j < palabras.Length; j++)
                {
                    if (palabras[j] == "START")
                    {
                        NuevoToken += "RW0";
                        continue;
                    }
                    NuevoToken += palabras[j] + " ";
                }

                //Agrega el token de la linea a la lista de tokens, eliminando el espacio al final
                Tokens.Add(NuevoToken.TrimEnd());


                //Agrega los token al nuevo archivo de tokens
                richArchivoDeTokens.Lines = Tokens.ToArray();

                palabras = null;
            }


        }


        //Se combinan los pasos de detección de estructura y carga de datos en un solo método para mayor eficiencia y simplicidad
        private void CargarEstructuraYDatosDesdeSQL()
        {
            using (SqlConnection conn = new SqlConnection(ConexionBD))
            {
                conn.Open();

                // 1. Obtenemos todos los datos de la tabla MatrizTransicion para analizar su estructura y contenido
                string query = "SELECT * FROM MatrizTransicion ORDER BY 1";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable tablaDB = new DataTable();
                adapter.Fill(tablaDB);

                // Validación básica para asegurar que la tabla no esté vacía
                if (tablaDB.Rows.Count == 0)
                    throw new Exception("La tabla MatrizTransicion está vacía.");

                // 2. DETECCIÓN AUTOMÁTICA DEL ALFABETO
                alfabeto.Clear();
                foreach (DataColumn col in tablaDB.Columns)
                {
                    // El nombre real de la columna en la base de datos en la iteración actual
                    string nombreReal = col.ColumnName;

                    // Saltamos la columna ID (índice 0) y ACEPTA
                    if (nombreReal != tablaDB.Columns[0].ColumnName && nombreReal.ToUpper() != "ACEPTA")
                    {
                        //Si es 'a1' y mide 2 caracteres, lo limpiamos a 'a'
                        string nombreLimpio = nombreReal;
                        if (nombreReal.Length == 2 && nombreReal.EndsWith("1"))
                        {
                            nombreLimpio = nombreReal.Substring(0, 1);
                        }

                        alfabeto.Add(nombreLimpio);// Agregamos el símbolo limpio al alfabeto
                        mapaColumnas[nombreLimpio] = nombreReal; // Guardamos la relación para cuando el usuario ejemplo ingrese 'a' y nosotros busquemos 'a1' en la tabla
                    }
                }

                // 3. DIMENSIONAR MATRIZ
                // Buscamos la fila con el ID más alto para determinar el número de estados (filas) que necesitamos en la matriz
                int realMaxId = 0;
                foreach (DataRow row in tablaDB.Rows)
                {
                    if (int.TryParse(row[0].ToString(), out int id) && id > realMaxId)
                        realMaxId = id;
                }
                // Guardamos el número máximo de estados detectado para usarlo en la lógica de análisis
                maxEstado = realMaxId;
                // Dimensionamos la matriz con base en el número máximo de estados detectado y el tamaño del alfabeto
                matriz = new int[maxEstado + 2, alfabeto.Count];

                // 4. LLENAR LA MATRIZ CON LOS VALORES DE LAS CELDAS
                foreach (DataRow row in tablaDB.Rows)
                {
                    // Obtenemos el ID del estado actual desde la primera columna de la fila
                    if (!int.TryParse(row[0].ToString(), out int idEstadoActual)) continue;


                    for (int i = 0; i < alfabeto.Count; i++)
                    {
                        string simbolo = alfabeto[i];
                        object valorCelda = row[simbolo];

                        if (valorCelda == DBNull.Value ||
                            string.IsNullOrWhiteSpace(valorCelda.ToString()) ||
                            valorCelda.ToString().ToLower() == "error")
                        {
                            matriz[idEstadoActual, i] = -1; // -1 significa transición a ERROR
                        }
                        else
                        {
                            if (int.TryParse(valorCelda.ToString(), out int destino))
                                matriz[idEstadoActual, i] = destino;
                            else
                                matriz[idEstadoActual, i] = -1;
                        }
                    }

                    // Guardar categoría de aceptación
                    string acepta = row["ACEPTA"]?.ToString() ?? "No valido";
                    categoriasEstado[idEstadoActual] = acepta;
                }
            }
        }


        private void CargarMatrizEnGrid()
        {
            if (dgvMatriz == null) return;

            dgvMatriz.Columns.Clear();
            dgvMatriz.Rows.Clear();

            // Configurar columnas visuales
            dgvMatriz.Columns.Add("Estado", "No.");
            foreach (var simbolo in alfabeto)
            {
                dgvMatriz.Columns.Add(simbolo, simbolo);
            }
            dgvMatriz.Columns.Add("ACEPTA", "Categoría");

            // Llenar filas visuales
            var estadosOrdenados = categoriasEstado.Keys.OrderBy(k => k);
            foreach (var estadoId in estadosOrdenados)
            {
                string[] fila = new string[alfabeto.Count + 2];
                fila[0] = estadoId.ToString();
                for (int j = 0; j < alfabeto.Count; j++)
                {
                    int val = matriz[estadoId, j];
                    fila[j + 1] = (val == -1) ? "Error" : val.ToString();
                }
                fila[alfabeto.Count + 1] = categoriasEstado[estadoId];
                dgvMatriz.Rows.Add(fila);
            }
        }

    }
}
