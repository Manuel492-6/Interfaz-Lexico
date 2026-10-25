using System;
using System.Drawing;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Libera los recursos no administrados utilizados por el formulario Dispose()
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        // Inicializa y configura los controles visuales y propiedades del formulario InitializeComponent()
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            richArchivoDeTokens = new RichTextBox();
            richProgramaFuente = new RichTextBox();
            dgtTablaDeSimbolos = new DataGridView();
            Identificador = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            TipoDato = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            Direccion = new DataGridViewTextBoxColumn();
            Bytes = new DataGridViewTextBoxColumn();
            Ambito = new DataGridViewTextBoxColumn();
            dgtErrores = new DataGridView();
            Linea = new DataGridViewTextBoxColumn();
            Error = new DataGridViewTextBoxColumn();
            btnGuardarPrograma = new Button();
            btnCargarPrograma = new Button();
            btnEditarPrograma = new Button();
            btnGuardarArchivo = new Button();
            btnAnalizar = new Button();
            lblNovaNyx = new Label();
            PicNovaNyx = new PictureBox();
            lblNovaNyxVersion = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            picLineas = new PictureBox();
            picLinea2 = new PictureBox();
            grpArbolJerarquia = new GroupBox();
            lblOperacionesCodigo = new Label();
            cboOperacionesCodigo = new ComboBox();
            lblEstadoSemantico = new Label();
            tabAnalisisJerarquia = new TabControl();
            tabArbol = new TabPage();
            tvArbolJerarquia = new TreeView();
            lblPasosJerarquia = new Label();
            rtbPasosJerarquia = new RichTextBox();
            tabCodigoIntermedio = new TabPage();
            lblPostfija = new Label();
            txtPostfija = new TextBox();
            lblCuadruplos = new Label();
            rbCuadruploOpActual = new RadioButton();
            rbCuadruploTodos = new RadioButton();
            dgtCuadruplos = new DataGridView();
            colNum = new DataGridViewTextBoxColumn();
            colOp = new DataGridViewTextBoxColumn();
            colArg1 = new DataGridViewTextBoxColumn();
            colArg2 = new DataGridViewTextBoxColumn();
            colRes = new DataGridViewTextBoxColumn();
            lblResumenCuadruplos = new Label();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgtErrores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicNovaNyx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLineas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLinea2).BeginInit();
            grpArbolJerarquia.SuspendLayout();
            tabAnalisisJerarquia.SuspendLayout();
            tabArbol.SuspendLayout();
            tabCodigoIntermedio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCuadruplos).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // richArchivoDeTokens
            // 
            richArchivoDeTokens.Location = new Point(812, 72);
            richArchivoDeTokens.Name = "richArchivoDeTokens";
            richArchivoDeTokens.Size = new Size(420, 337);
            richArchivoDeTokens.TabIndex = 4;
            richArchivoDeTokens.Text = "";
            richArchivoDeTokens.TextChanged += richArchivoDeTokens_TextChanged;
            // 
            // richProgramaFuente
            // 
            richProgramaFuente.Location = new Point(143, 67);
            richProgramaFuente.Name = "richProgramaFuente";
            richProgramaFuente.Size = new Size(420, 337);
            richProgramaFuente.TabIndex = 5;
            richProgramaFuente.Text = "";
            richProgramaFuente.TextChanged += richProgramaFuente_TextChanged;
            // 
            // dgtTablaDeSimbolos
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtTablaDeSimbolos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgtTablaDeSimbolos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgtTablaDeSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtTablaDeSimbolos.Columns.AddRange(new DataGridViewColumn[] { Identificador, Nombre, TipoDato, Valor, Direccion, Bytes, Ambito });
            dgtTablaDeSimbolos.GridColor = SystemColors.InactiveCaptionText;
            dgtTablaDeSimbolos.Location = new Point(18, 52);
            dgtTablaDeSimbolos.Name = "dgtTablaDeSimbolos";
            dgtTablaDeSimbolos.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgtTablaDeSimbolos.RowHeadersWidth = 51;
            dgtTablaDeSimbolos.Size = new Size(794, 210);
            dgtTablaDeSimbolos.TabIndex = 7;
            // 
            // Identificador
            // 
            Identificador.HeaderText = "Identificador";
            Identificador.MinimumWidth = 6;
            Identificador.Name = "Identificador";
            Identificador.ReadOnly = true;
            Identificador.Width = 125;
            // 
            // Nombre
            // 
            Nombre.HeaderText = "Nombre";
            Nombre.MinimumWidth = 6;
            Nombre.Name = "Nombre";
            Nombre.ReadOnly = true;
            Nombre.Width = 125;
            // 
            // TipoDato
            // 
            TipoDato.HeaderText = "Tipo De Dato";
            TipoDato.MinimumWidth = 6;
            TipoDato.Name = "TipoDato";
            TipoDato.ReadOnly = true;
            TipoDato.Width = 125;
            // 
            // Valor
            // 
            Valor.HeaderText = "Valor";
            Valor.MinimumWidth = 6;
            Valor.Name = "Valor";
            Valor.ReadOnly = true;
            Valor.Width = 90;
            // 
            // Direccion
            // 
            Direccion.HeaderText = "Dirección";
            Direccion.MinimumWidth = 6;
            Direccion.Name = "Direccion";
            Direccion.ReadOnly = true;
            Direccion.Width = 90;
            // 
            // Bytes
            // 
            Bytes.HeaderText = "Peso (Bytes)";
            Bytes.MinimumWidth = 6;
            Bytes.Name = "Bytes";
            Bytes.ReadOnly = true;
            Bytes.Width = 90;
            // 
            // Ambito
            // 
            Ambito.HeaderText = "Ámbito";
            Ambito.MinimumWidth = 6;
            Ambito.Name = "Ambito";
            Ambito.ReadOnly = true;
            Ambito.Width = 80;
            // 
            // dgtErrores
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtErrores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dgtErrores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtErrores.Columns.AddRange(new DataGridViewColumn[] { Linea, Error });
            dgtErrores.Location = new Point(16, 52);
            dgtErrores.Name = "dgtErrores";
            dgtErrores.RowHeadersWidth = 51;
            dgtErrores.Size = new Size(796, 210);
            dgtErrores.TabIndex = 8;
            // 
            // Linea
            // 
            Linea.HeaderText = "Linea";
            Linea.MinimumWidth = 6;
            Linea.Name = "Linea";
            Linea.ReadOnly = true;
            Linea.Width = 125;
            // 
            // Error
            // 
            Error.HeaderText = "Error";
            Error.MinimumWidth = 6;
            Error.Name = "Error";
            Error.ReadOnly = true;
            Error.Width = 125;
            // 
            // btnGuardarPrograma
            // 
            btnGuardarPrograma.Location = new Point(427, 432);
            btnGuardarPrograma.Name = "btnGuardarPrograma";
            btnGuardarPrograma.Size = new Size(136, 48);
            btnGuardarPrograma.TabIndex = 9;
            btnGuardarPrograma.Text = "Guardar Programa";
            btnGuardarPrograma.UseVisualStyleBackColor = true;
            btnGuardarPrograma.Click += btnGuardarPrograma_Click;
            // 
            // btnCargarPrograma
            // 
            btnCargarPrograma.Location = new Point(143, 432);
            btnCargarPrograma.Name = "btnCargarPrograma";
            btnCargarPrograma.Size = new Size(136, 48);
            btnCargarPrograma.TabIndex = 10;
            btnCargarPrograma.Text = "Cargar Programa";
            btnCargarPrograma.UseVisualStyleBackColor = true;
            btnCargarPrograma.Click += btnCargarPrograma_Click;
            // 
            // btnEditarPrograma
            // 
            btnEditarPrograma.Location = new Point(285, 432);
            btnEditarPrograma.Name = "btnEditarPrograma";
            btnEditarPrograma.Size = new Size(136, 48);
            btnEditarPrograma.TabIndex = 11;
            btnEditarPrograma.Text = "EditarPrograma";
            btnEditarPrograma.UseVisualStyleBackColor = true;
            btnEditarPrograma.Click += btnEditarPrograma_Click;
            // 
            // btnGuardarArchivo
            // 
            btnGuardarArchivo.Location = new Point(802, 432);
            btnGuardarArchivo.Name = "btnGuardarArchivo";
            btnGuardarArchivo.Size = new Size(136, 48);
            btnGuardarArchivo.TabIndex = 12;
            btnGuardarArchivo.Text = "Guardar Archivo";
            btnGuardarArchivo.UseVisualStyleBackColor = true;
            btnGuardarArchivo.Click += btnGuardarArchivo_Click;
            // 
            // btnAnalizar
            // 
            btnAnalizar.Location = new Point(593, 432);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(136, 48);
            btnAnalizar.TabIndex = 22;
            btnAnalizar.Text = "Analizar Todo";
            btnAnalizar.UseVisualStyleBackColor = true;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // lblNovaNyx
            // 
            lblNovaNyx.AutoSize = true;
            lblNovaNyx.Font = new Font("Stencil", 25.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNovaNyx.ForeColor = SystemColors.ActiveCaptionText;
            lblNovaNyx.Location = new Point(569, 2);
            lblNovaNyx.Name = "lblNovaNyx";
            lblNovaNyx.Size = new Size(212, 51);
            lblNovaNyx.TabIndex = 13;
            lblNovaNyx.Text = "NovaNyx";
            // 
            // PicNovaNyx
            // 
            PicNovaNyx.Image = Properties.Resources.WhatsApp_Image_2026_04_19_at_10_51_10_AM;
            PicNovaNyx.Location = new Point(569, 67);
            PicNovaNyx.Name = "PicNovaNyx";
            PicNovaNyx.Size = new Size(214, 158);
            PicNovaNyx.SizeMode = PictureBoxSizeMode.StretchImage;
            PicNovaNyx.TabIndex = 14;
            PicNovaNyx.TabStop = false;
            // 
            // lblNovaNyxVersion
            // 
            lblNovaNyxVersion.AutoSize = true;
            lblNovaNyxVersion.Location = new Point(635, 246);
            lblNovaNyxVersion.Name = "lblNovaNyxVersion";
            lblNovaNyxVersion.Size = new Size(91, 20);
            lblNovaNyxVersion.TabIndex = 15;
            lblNovaNyxVersion.Text = "Versión 1.4.3";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(143, 26);
            label1.Name = "label1";
            label1.Size = new Size(202, 27);
            label1.TabIndex = 16;
            label1.Text = "Programa Fuente";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(869, 26);
            label2.Name = "label2";
            label2.Size = new Size(223, 27);
            label2.TabIndex = 17;
            label2.Text = "Archivo De Tokens";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(16, 16);
            label3.Name = "label3";
            label3.Size = new Size(94, 27);
            label3.TabIndex = 18;
            label3.Text = "Errores";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(18, 16);
            label4.Name = "label4";
            label4.Size = new Size(217, 27);
            label4.TabIndex = 19;
            label4.Text = "Tabla De Simbolos";
            // 
            // picLineas
            // 
            picLineas.Location = new Point(69, 67);
            picLineas.Name = "picLineas";
            picLineas.Size = new Size(68, 337);
            picLineas.TabIndex = 20;
            picLineas.TabStop = false;
            // 
            // picLinea2
            // 
            picLinea2.Location = new Point(785, 67);
            picLinea2.Name = "picLinea2";
            picLinea2.Size = new Size(21, 337);
            picLinea2.TabIndex = 21;
            picLinea2.TabStop = false;
            // 
            // grpArbolJerarquia
            // 
            grpArbolJerarquia.Controls.Add(lblOperacionesCodigo);
            grpArbolJerarquia.Controls.Add(cboOperacionesCodigo);
            grpArbolJerarquia.Controls.Add(lblEstadoSemantico);
            grpArbolJerarquia.Controls.Add(tabAnalisisJerarquia);
            grpArbolJerarquia.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpArbolJerarquia.ForeColor = Color.DarkSlateBlue;
            grpArbolJerarquia.Location = new Point(1238, 26);
            grpArbolJerarquia.Name = "grpArbolJerarquia";
            grpArbolJerarquia.Size = new Size(476, 757);
            grpArbolJerarquia.TabIndex = 23;
            grpArbolJerarquia.TabStop = false;
            grpArbolJerarquia.Text = "Árbol y Código Intermedio";
            // 
            // lblOperacionesCodigo
            // 
            lblOperacionesCodigo.AutoSize = true;
            lblOperacionesCodigo.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOperacionesCodigo.ForeColor = Color.Black;
            lblOperacionesCodigo.Location = new Point(15, 26);
            lblOperacionesCodigo.Name = "lblOperacionesCodigo";
            lblOperacionesCodigo.Size = new Size(251, 18);
            lblOperacionesCodigo.TabIndex = 0;
            lblOperacionesCodigo.Text = "Operaciones en Programa Fuente:";
            // 
            // cboOperacionesCodigo
            // 
            cboOperacionesCodigo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOperacionesCodigo.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboOperacionesCodigo.FormattingEnabled = true;
            cboOperacionesCodigo.Location = new Point(15, 48);
            cboOperacionesCodigo.Name = "cboOperacionesCodigo";
            cboOperacionesCodigo.Size = new Size(405, 26);
            cboOperacionesCodigo.TabIndex = 1;
            cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;
            // 
            // lblEstadoSemantico
            // 
            lblEstadoSemantico.AutoSize = true;
            lblEstadoSemantico.Font = new Font("Arial", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstadoSemantico.ForeColor = Color.ForestGreen;
            lblEstadoSemantico.Location = new Point(15, 82);
            lblEstadoSemantico.Name = "lblEstadoSemantico";
            lblEstadoSemantico.Size = new Size(236, 18);
            lblEstadoSemantico.TabIndex = 2;
            lblEstadoSemantico.Text = "Estado: Presiona 'Analizar Todo'";
            // 
            // tabAnalisisJerarquia
            // 
            tabAnalisisJerarquia.Controls.Add(tabArbol);
            tabAnalisisJerarquia.Controls.Add(tabCodigoIntermedio);
            tabAnalisisJerarquia.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabAnalisisJerarquia.Location = new Point(15, 106);
            tabAnalisisJerarquia.Name = "tabAnalisisJerarquia";
            tabAnalisisJerarquia.SelectedIndex = 0;
            tabAnalisisJerarquia.Size = new Size(455, 645);
            tabAnalisisJerarquia.TabIndex = 3;
            // 
            // tabArbol
            // 
            tabArbol.BackColor = Color.White;
            tabArbol.Controls.Add(tvArbolJerarquia);
            tabArbol.Controls.Add(lblPasosJerarquia);
            tabArbol.Controls.Add(rtbPasosJerarquia);
            tabArbol.Location = new Point(4, 27);
            tabArbol.Name = "tabArbol";
            tabArbol.Padding = new Padding(3);
            tabArbol.Size = new Size(447, 614);
            tabArbol.TabIndex = 0;
            tabArbol.Text = "🌳 Árbol y Pasos";
            // 
            // tvArbolJerarquia
            // 
            tvArbolJerarquia.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tvArbolJerarquia.Location = new Point(6, 6);
            tvArbolJerarquia.Name = "tvArbolJerarquia";
            tvArbolJerarquia.Size = new Size(425, 360);
            tvArbolJerarquia.TabIndex = 0;
            tvArbolJerarquia.AfterSelect += tvArbolJerarquia_AfterSelect;
            // 
            // lblPasosJerarquia
            // 
            lblPasosJerarquia.AutoSize = true;
            lblPasosJerarquia.Font = new Font("Arial", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPasosJerarquia.ForeColor = Color.Black;
            lblPasosJerarquia.Location = new Point(6, 372);
            lblPasosJerarquia.Name = "lblPasosJerarquia";
            lblPasosJerarquia.Size = new Size(260, 18);
            lblPasosJerarquia.TabIndex = 1;
            lblPasosJerarquia.Text = "Pasos de Resolución por Jerarquía:";
            // 
            // rtbPasosJerarquia
            // 
            rtbPasosJerarquia.BackColor = Color.White;
            rtbPasosJerarquia.Font = new Font("Consolas", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbPasosJerarquia.ForeColor = Color.FromArgb(30, 30, 30);
            rtbPasosJerarquia.Location = new Point(6, 394);
            rtbPasosJerarquia.Name = "rtbPasosJerarquia";
            rtbPasosJerarquia.ReadOnly = true;
            rtbPasosJerarquia.Size = new Size(425, 214);
            rtbPasosJerarquia.TabIndex = 2;
            rtbPasosJerarquia.Text = "";
            // 
            // tabCodigoIntermedio
            // 
            tabCodigoIntermedio.BackColor = Color.White;
            tabCodigoIntermedio.Controls.Add(lblPostfija);
            tabCodigoIntermedio.Controls.Add(txtPostfija);
            tabCodigoIntermedio.Controls.Add(lblCuadruplos);
            tabCodigoIntermedio.Controls.Add(rbCuadruploOpActual);
            tabCodigoIntermedio.Controls.Add(rbCuadruploTodos);
            tabCodigoIntermedio.Controls.Add(dgtCuadruplos);
            tabCodigoIntermedio.Controls.Add(lblResumenCuadruplos);
            tabCodigoIntermedio.Location = new Point(4, 27);
            tabCodigoIntermedio.Name = "tabCodigoIntermedio";
            tabCodigoIntermedio.Padding = new Padding(3);
            tabCodigoIntermedio.Size = new Size(397, 561);
            tabCodigoIntermedio.TabIndex = 1;
            tabCodigoIntermedio.Text = "⚡ Código Intermedio";
            // 
            // lblPostfija
            // 
            lblPostfija.AutoSize = true;
            lblPostfija.Font = new Font("Arial", 8.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPostfija.ForeColor = Color.DarkSlateBlue;
            lblPostfija.Location = new Point(6, 8);
            lblPostfija.Name = "lblPostfija";
            lblPostfija.Size = new Size(295, 18);
            lblPostfija.TabIndex = 0;
            lblPostfija.Text = "Notación Postfija (Polaca Inversa - RPN):";
            // 
            // txtPostfija
            // 
            txtPostfija.BackColor = Color.FromArgb(245, 248, 255);
            txtPostfija.Font = new Font("Consolas", 10.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPostfija.ForeColor = Color.DarkBlue;
            txtPostfija.Location = new Point(6, 29);
            txtPostfija.Name = "txtPostfija";
            txtPostfija.ReadOnly = true;
            txtPostfija.Size = new Size(385, 28);
            txtPostfija.TabIndex = 1;
            // 
            // lblCuadruplos
            // 
            lblCuadruplos.AutoSize = true;
            lblCuadruplos.Font = new Font("Arial", 8.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCuadruplos.ForeColor = Color.DarkSlateBlue;
            lblCuadruplos.Location = new Point(6, 64);
            lblCuadruplos.Name = "lblCuadruplos";
            lblCuadruplos.Size = new Size(261, 18);
            lblCuadruplos.TabIndex = 2;
            lblCuadruplos.Text = "Cuádruplos de 3 Direcciones (C3D):";
            // 
            // rbCuadruploOpActual
            // 
            rbCuadruploOpActual.AutoSize = true;
            rbCuadruploOpActual.Checked = true;
            rbCuadruploOpActual.Font = new Font("Arial", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbCuadruploOpActual.ForeColor = Color.FromArgb(40, 40, 40);
            rbCuadruploOpActual.Location = new Point(6, 86);
            rbCuadruploOpActual.Name = "rbCuadruploOpActual";
            rbCuadruploOpActual.Size = new Size(150, 20);
            rbCuadruploOpActual.TabIndex = 3;
            rbCuadruploOpActual.TabStop = true;
            rbCuadruploOpActual.Text = "Op. Seleccionada";
            rbCuadruploOpActual.UseVisualStyleBackColor = true;
            rbCuadruploOpActual.CheckedChanged += rbCuadruploModo_CheckedChanged;
            // 
            // rbCuadruploTodos
            // 
            rbCuadruploTodos.AutoSize = true;
            rbCuadruploTodos.Font = new Font("Arial", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbCuadruploTodos.ForeColor = Color.FromArgb(40, 40, 40);
            rbCuadruploTodos.Location = new Point(148, 86);
            rbCuadruploTodos.Name = "rbCuadruploTodos";
            rbCuadruploTodos.Size = new Size(166, 20);
            rbCuadruploTodos.TabIndex = 4;
            rbCuadruploTodos.Text = "Programa Completo";
            rbCuadruploTodos.UseVisualStyleBackColor = true;
            rbCuadruploTodos.CheckedChanged += rbCuadruploModo_CheckedChanged;
            // 
            // dgtCuadruplos
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtCuadruplos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dgtCuadruplos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtCuadruplos.Columns.AddRange(new DataGridViewColumn[] { colNum, colOp, colArg1, colArg2, colRes });
            dgtCuadruplos.Location = new Point(6, 110);
            dgtCuadruplos.Name = "dgtCuadruplos";
            dgtCuadruplos.RowHeadersVisible = false;
            dgtCuadruplos.RowHeadersWidth = 51;
            dgtCuadruplos.Size = new Size(385, 412);
            dgtCuadruplos.TabIndex = 5;
            // 
            // colNum
            // 
            colNum.FillWeight = 28F;
            colNum.HeaderText = "#";
            colNum.MinimumWidth = 6;
            colNum.Name = "colNum";
            colNum.ReadOnly = true;
            colNum.Width = 35;
            // 
            // colOp
            // 
            colOp.FillWeight = 42F;
            colOp.HeaderText = "Operador";
            colOp.MinimumWidth = 6;
            colOp.Name = "colOp";
            colOp.ReadOnly = true;
            colOp.Width = 60;
            // 
            // colArg1
            // 
            colArg1.FillWeight = 50F;
            colArg1.HeaderText = "Arg 1";
            colArg1.MinimumWidth = 6;
            colArg1.Name = "colArg1";
            colArg1.ReadOnly = true;
            colArg1.Width = 70;
            // 
            // colArg2
            // 
            colArg2.FillWeight = 50F;
            colArg2.HeaderText = "Arg 2";
            colArg2.MinimumWidth = 6;
            colArg2.Name = "colArg2";
            colArg2.ReadOnly = true;
            colArg2.Width = 70;
            // 
            // colRes
            // 
            colRes.FillWeight = 52F;
            colRes.HeaderText = "Resultado";
            colRes.MinimumWidth = 6;
            colRes.Name = "colRes";
            colRes.ReadOnly = true;
            colRes.Width = 75;
            // 
            // lblResumenCuadruplos
            // 
            lblResumenCuadruplos.AutoSize = true;
            lblResumenCuadruplos.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblResumenCuadruplos.ForeColor = Color.DimGray;
            lblResumenCuadruplos.Location = new Point(6, 528);
            lblResumenCuadruplos.Name = "lblResumenCuadruplos";
            lblResumenCuadruplos.Size = new Size(203, 20);
            lblResumenCuadruplos.TabIndex = 6;
            lblResumenCuadruplos.Text = "Temporales: 0 | Cuádruplos: 0";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 486);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(878, 301);
            tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dgtTablaDeSimbolos);
            tabPage1.Controls.Add(label4);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(870, 268);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(dgtErrores);
            tabPage2.Controls.Add(label3);
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(870, 268);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            CancelButton = btnCargarPrograma;
            ClientSize = new Size(1726, 832);
            Controls.Add(tabControl1);
            Controls.Add(grpArbolJerarquia);
            Controls.Add(picLinea2);
            Controls.Add(picLineas);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblNovaNyxVersion);
            Controls.Add(PicNovaNyx);
            Controls.Add(lblNovaNyx);
            Controls.Add(btnGuardarArchivo);
            Controls.Add(btnEditarPrograma);
            Controls.Add(btnCargarPrograma);
            Controls.Add(btnGuardarPrograma);
            Controls.Add(btnAnalizar);
            Controls.Add(richProgramaFuente);
            Controls.Add(richArchivoDeTokens);
            Name = "Form1";
            Text = "NovaNyx 1.4.3";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgtErrores).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicNovaNyx).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLineas).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLinea2).EndInit();
            grpArbolJerarquia.ResumeLayout(false);
            grpArbolJerarquia.PerformLayout();
            tabAnalisisJerarquia.ResumeLayout(false);
            tabArbol.ResumeLayout(false);
            tabArbol.PerformLayout();
            tabCodigoIntermedio.ResumeLayout(false);
            tabCodigoIntermedio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCuadruplos).EndInit();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richArchivoDeTokens;
        private System.Windows.Forms.RichTextBox richProgramaFuente;
        private System.Windows.Forms.DataGridView dgtTablaDeSimbolos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Identificador;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoDato;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bytes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ambito;
        private System.Windows.Forms.DataGridView dgtErrores;
        private System.Windows.Forms.DataGridViewTextBoxColumn Linea;
        private System.Windows.Forms.DataGridViewTextBoxColumn Error;
        private System.Windows.Forms.Button btnGuardarPrograma;
        private System.Windows.Forms.Button btnCargarPrograma;
        private System.Windows.Forms.Button btnEditarPrograma;
        private System.Windows.Forms.Button btnGuardarArchivo;
        private System.Windows.Forms.Button btnAnalizar; // Instancia Nuevo botón
        private System.Windows.Forms.Label lblNovaNyx;
        private System.Windows.Forms.PictureBox PicNovaNyx;
        private System.Windows.Forms.Label lblNovaNyxVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox picLineas;
        private System.Windows.Forms.PictureBox picLinea2;
        private System.Windows.Forms.GroupBox grpArbolJerarquia;
        private System.Windows.Forms.Label lblOperacionesCodigo;
        private System.Windows.Forms.ComboBox cboOperacionesCodigo;
        private System.Windows.Forms.Label lblEstadoSemantico;
        private System.Windows.Forms.TabControl tabAnalisisJerarquia;
        private System.Windows.Forms.TabPage tabArbol;
        private System.Windows.Forms.TabPage tabCodigoIntermedio;
        private System.Windows.Forms.TreeView tvArbolJerarquia;
        private System.Windows.Forms.Label lblPasosJerarquia;
        private System.Windows.Forms.RichTextBox rtbPasosJerarquia;
        private System.Windows.Forms.Label lblPostfija;
        private System.Windows.Forms.TextBox txtPostfija;
        private System.Windows.Forms.Label lblCuadruplos;
        private System.Windows.Forms.RadioButton rbCuadruploOpActual;
        private System.Windows.Forms.RadioButton rbCuadruploTodos;
        private System.Windows.Forms.DataGridView dgtCuadruplos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArg2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRes;
        private System.Windows.Forms.Label lblResumenCuadruplos;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}