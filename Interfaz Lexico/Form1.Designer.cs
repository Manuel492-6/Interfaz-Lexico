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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
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
            rbCuadruploOpActual = new RadioButton();
            rbCuadruploTodos = new RadioButton();
            lblEstadoSemantico = new Label();
            tabAnalisisJerarquia = new TabControl();
            tabArbol = new TabPage();
            tvArbolJerarquia = new TreeView();
            lblPasosJerarquia = new Label();
            rtbPasosJerarquia = new RichTextBox();
            tabNotaciones = new TabPage();
            lblInfija = new Label();
            txtInfija = new TextBox();
            lblPrefija = new Label();
            txtPrefija = new TextBox();
            lblPostfija = new Label();
            txtPostfija = new TextBox();
            lblTablaNotaciones = new Label();
            dgtNotaciones = new DataGridView();
            colNotNum = new DataGridViewTextBoxColumn();
            colNotInfija = new DataGridViewTextBoxColumn();
            colNotPrefija = new DataGridViewTextBoxColumn();
            colNotPostfija = new DataGridViewTextBoxColumn();
            tabCodigoIntermedio = new TabPage();
            lblCuadruplos = new Label();
            dgtCuadruplos = new DataGridView();
            colNum = new DataGridViewTextBoxColumn();
            colOp = new DataGridViewTextBoxColumn();
            colArg1 = new DataGridViewTextBoxColumn();
            colArg2 = new DataGridViewTextBoxColumn();
            colRes = new DataGridViewTextBoxColumn();
            lblTriplos = new Label();
            dgtTriplos = new DataGridView();
            colTripRef = new DataGridViewTextBoxColumn();
            colTripOp = new DataGridViewTextBoxColumn();
            colTripArg1 = new DataGridViewTextBoxColumn();
            colTripArg2 = new DataGridViewTextBoxColumn();
            lblResumenCuadruplos = new Label();
            tabCodigoP = new TabPage();
            lblTablaCodigoP = new Label();
            dgtCodigoP = new DataGridView();
            colPNum = new DataGridViewTextBoxColumn();
            colPInst = new DataGridViewTextBoxColumn();
            colPOp = new DataGridViewTextBoxColumn();
            colPDesc = new DataGridViewTextBoxColumn();
            lblTextoCodigoP = new Label();
            rtbCodigoP = new RichTextBox();
            lblResumenCodigoP = new Label();
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
            tabNotaciones.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgtNotaciones).BeginInit();
            tabCodigoIntermedio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCuadruplos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgtTriplos).BeginInit();
            tabCodigoP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCodigoP).BeginInit();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            SuspendLayout();
            // 
            // richArchivoDeTokens
            // 
            richArchivoDeTokens.BackColor = Color.FromArgb(250, 252, 255);
            richArchivoDeTokens.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richArchivoDeTokens.Location = new Point(494, 96);
            richArchivoDeTokens.Name = "richArchivoDeTokens";
            richArchivoDeTokens.Size = new Size(392, 325);
            richArchivoDeTokens.TabIndex = 4;
            richArchivoDeTokens.Text = "";
            richArchivoDeTokens.TextChanged += richArchivoDeTokens_TextChanged;
            // 
            // richProgramaFuente
            // 
            richProgramaFuente.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richProgramaFuente.Location = new Point(62, 96);
            richProgramaFuente.Name = "richProgramaFuente";
            richProgramaFuente.Size = new Size(392, 325);
            richProgramaFuente.TabIndex = 5;
            richProgramaFuente.Text = "";
            richProgramaFuente.TextChanged += richProgramaFuente_TextChanged;
            // 
            // dgtTablaDeSimbolos
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtTablaDeSimbolos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgtTablaDeSimbolos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtTablaDeSimbolos.BackgroundColor = Color.White;
            dgtTablaDeSimbolos.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgtTablaDeSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtTablaDeSimbolos.Columns.AddRange(new DataGridViewColumn[] { Identificador, Nombre, TipoDato, Valor, Direccion, Bytes, Ambito });
            dgtTablaDeSimbolos.GridColor = SystemColors.InactiveCaptionText;
            dgtTablaDeSimbolos.Location = new Point(10, 30);
            dgtTablaDeSimbolos.Name = "dgtTablaDeSimbolos";
            dgtTablaDeSimbolos.RowHeadersVisible = false;
            dgtTablaDeSimbolos.RowHeadersWidth = 51;
            dgtTablaDeSimbolos.Size = new Size(844, 265);
            dgtTablaDeSimbolos.TabIndex = 7;
            // 
            // Identificador
            // 
            Identificador.FillWeight = 60F;
            Identificador.HeaderText = "ID";
            Identificador.MinimumWidth = 6;
            Identificador.Name = "Identificador";
            Identificador.ReadOnly = true;
            // 
            // Nombre
            // 
            Nombre.FillWeight = 110F;
            Nombre.HeaderText = "Nombre";
            Nombre.MinimumWidth = 6;
            Nombre.Name = "Nombre";
            Nombre.ReadOnly = true;
            // 
            // TipoDato
            // 
            TipoDato.FillWeight = 95F;
            TipoDato.HeaderText = "Tipo De Dato";
            TipoDato.MinimumWidth = 6;
            TipoDato.Name = "TipoDato";
            TipoDato.ReadOnly = true;
            // 
            // Valor
            // 
            Valor.FillWeight = 120F;
            Valor.HeaderText = "Valor";
            Valor.MinimumWidth = 6;
            Valor.Name = "Valor";
            Valor.ReadOnly = true;
            // 
            // Direccion
            // 
            Direccion.FillWeight = 85F;
            Direccion.HeaderText = "Dirección";
            Direccion.MinimumWidth = 6;
            Direccion.Name = "Direccion";
            Direccion.ReadOnly = true;
            // 
            // Bytes
            // 
            Bytes.FillWeight = 85F;
            Bytes.HeaderText = "Peso (Bytes)";
            Bytes.MinimumWidth = 6;
            Bytes.Name = "Bytes";
            Bytes.ReadOnly = true;
            // 
            // Ambito
            // 
            Ambito.FillWeight = 80F;
            Ambito.HeaderText = "Ámbito";
            Ambito.MinimumWidth = 6;
            Ambito.Name = "Ambito";
            Ambito.ReadOnly = true;
            // 
            // dgtErrores
            // 
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgtErrores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dgtErrores.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtErrores.BackgroundColor = Color.White;
            dgtErrores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtErrores.Columns.AddRange(new DataGridViewColumn[] { Linea, Error });
            dgtErrores.Location = new Point(10, 30);
            dgtErrores.Name = "dgtErrores";
            dgtErrores.RowHeadersVisible = false;
            dgtErrores.RowHeadersWidth = 51;
            dgtErrores.Size = new Size(844, 265);
            dgtErrores.TabIndex = 8;
            // 
            // Linea
            // 
            Linea.FillWeight = 25F;
            Linea.HeaderText = "Línea";
            Linea.MinimumWidth = 6;
            Linea.Name = "Linea";
            Linea.ReadOnly = true;
            // 
            // Error
            // 
            Error.FillWeight = 175F;
            Error.HeaderText = "Descripción del Error";
            Error.MinimumWidth = 6;
            Error.Name = "Error";
            Error.ReadOnly = true;
            // 
            // btnGuardarPrograma
            // 
            btnGuardarPrograma.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardarPrograma.Location = new Point(360, 430);
            btnGuardarPrograma.Name = "btnGuardarPrograma";
            btnGuardarPrograma.Size = new Size(165, 42);
            btnGuardarPrograma.TabIndex = 9;
            btnGuardarPrograma.Text = "💾 Guardar Programa";
            btnGuardarPrograma.UseVisualStyleBackColor = true;
            btnGuardarPrograma.Click += btnGuardarPrograma_Click;
            // 
            // btnCargarPrograma
            // 
            btnCargarPrograma.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCargarPrograma.Location = new Point(14, 430);
            btnCargarPrograma.Name = "btnCargarPrograma";
            btnCargarPrograma.Size = new Size(165, 42);
            btnCargarPrograma.TabIndex = 10;
            btnCargarPrograma.Text = "📂 Cargar Programa";
            btnCargarPrograma.UseVisualStyleBackColor = true;
            btnCargarPrograma.Click += btnCargarPrograma_Click;
            // 
            // btnEditarPrograma
            // 
            btnEditarPrograma.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditarPrograma.Location = new Point(187, 430);
            btnEditarPrograma.Name = "btnEditarPrograma";
            btnEditarPrograma.Size = new Size(165, 42);
            btnEditarPrograma.TabIndex = 11;
            btnEditarPrograma.Text = "✏️ Editar Programa";
            btnEditarPrograma.UseVisualStyleBackColor = true;
            btnEditarPrograma.Click += btnEditarPrograma_Click;
            // 
            // btnGuardarArchivo
            // 
            btnGuardarArchivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardarArchivo.Location = new Point(721, 430);
            btnGuardarArchivo.Name = "btnGuardarArchivo";
            btnGuardarArchivo.Size = new Size(165, 42);
            btnGuardarArchivo.TabIndex = 12;
            btnGuardarArchivo.Text = "📄 Guardar Tokens";
            btnGuardarArchivo.UseVisualStyleBackColor = true;
            btnGuardarArchivo.Click += btnGuardarArchivo_Click;
            // 
            // btnAnalizar
            // 
            btnAnalizar.BackColor = Color.DarkSlateBlue;
            btnAnalizar.FlatStyle = FlatStyle.Flat;
            btnAnalizar.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizar.ForeColor = Color.White;
            btnAnalizar.Location = new Point(533, 430);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(180, 42);
            btnAnalizar.TabIndex = 22;
            btnAnalizar.Text = "🚀 Analizar Todo";
            btnAnalizar.UseVisualStyleBackColor = false;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // lblNovaNyx
            // 
            lblNovaNyx.AutoSize = true;
            lblNovaNyx.Font = new Font("Stencil", 20F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNovaNyx.ForeColor = Color.DarkSlateBlue;
            lblNovaNyx.Location = new Point(84, 10);
            lblNovaNyx.Name = "lblNovaNyx";
            lblNovaNyx.Size = new Size(168, 40);
            lblNovaNyx.TabIndex = 13;
            lblNovaNyx.Text = "NovaNyx";
            // 
            // PicNovaNyx
            // 
            PicNovaNyx.Image = Properties.Resources.WhatsApp_Image_2026_04_19_at_10_51_10_AM;
            PicNovaNyx.Location = new Point(14, 10);
            PicNovaNyx.Name = "PicNovaNyx";
            PicNovaNyx.Size = new Size(64, 52);
            PicNovaNyx.SizeMode = PictureBoxSizeMode.StretchImage;
            PicNovaNyx.TabIndex = 14;
            PicNovaNyx.TabStop = false;
            // 
            // lblNovaNyxVersion
            // 
            lblNovaNyxVersion.AutoSize = true;
            lblNovaNyxVersion.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNovaNyxVersion.ForeColor = Color.SlateGray;
            lblNovaNyxVersion.Location = new Point(88, 46);
            lblNovaNyxVersion.Name = "lblNovaNyxVersion";
            lblNovaNyxVersion.Size = new Size(100, 20);
            lblNovaNyxVersion.TabIndex = 15;
            lblNovaNyxVersion.Text = "Versión 1.7.0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.DarkSlateBlue;
            label1.Location = new Point(62, 68);
            label1.Name = "label1";
            label1.Size = new Size(194, 25);
            label1.TabIndex = 16;
            label1.Text = "📝 Programa Fuente";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.DarkSlateBlue;
            label2.Location = new Point(494, 68);
            label2.Name = "label2";
            label2.Size = new Size(203, 25);
            label2.TabIndex = 17;
            label2.Text = "🔑 Archivo De Tokens";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.Firebrick;
            label3.Location = new Point(10, 6);
            label3.Name = "label3";
            label3.Size = new Size(341, 21);
            label3.TabIndex = 18;
            label3.Text = "Diagnóstico Léxico, Sintáctico y Semántico:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.DarkSlateBlue;
            label4.Location = new Point(10, 6);
            label4.Name = "label4";
            label4.Size = new Size(283, 21);
            label4.TabIndex = 19;
            label4.Text = "Variables, Tipos, Valores y Memoria:";
            // 
            // picLineas
            // 
            picLineas.BackColor = Color.FromArgb(235, 240, 248);
            picLineas.Location = new Point(14, 96);
            picLineas.Name = "picLineas";
            picLineas.Size = new Size(46, 325);
            picLineas.TabIndex = 20;
            picLineas.TabStop = false;
            // 
            // picLinea2
            // 
            picLinea2.BackColor = Color.FromArgb(235, 240, 248);
            picLinea2.Location = new Point(466, 96);
            picLinea2.Name = "picLinea2";
            picLinea2.Size = new Size(26, 325);
            picLinea2.TabIndex = 21;
            picLinea2.TabStop = false;
            // 
            // grpArbolJerarquia
            // 
            grpArbolJerarquia.Controls.Add(lblOperacionesCodigo);
            grpArbolJerarquia.Controls.Add(cboOperacionesCodigo);
            grpArbolJerarquia.Controls.Add(rbCuadruploOpActual);
            grpArbolJerarquia.Controls.Add(rbCuadruploTodos);
            grpArbolJerarquia.Controls.Add(lblEstadoSemantico);
            grpArbolJerarquia.Controls.Add(tabAnalisisJerarquia);
            grpArbolJerarquia.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            grpArbolJerarquia.ForeColor = Color.DarkSlateBlue;
            grpArbolJerarquia.Location = new Point(896, 12);
            grpArbolJerarquia.Name = "grpArbolJerarquia";
            grpArbolJerarquia.Size = new Size(848, 808);
            grpArbolJerarquia.TabIndex = 23;
            grpArbolJerarquia.TabStop = false;
            grpArbolJerarquia.Text = "Panel de Análisis Semántico, Notaciones y Código Intermedio";
            // 
            // lblOperacionesCodigo
            // 
            lblOperacionesCodigo.AutoSize = true;
            lblOperacionesCodigo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblOperacionesCodigo.ForeColor = Color.Black;
            lblOperacionesCodigo.Location = new Point(14, 28);
            lblOperacionesCodigo.Name = "lblOperacionesCodigo";
            lblOperacionesCodigo.Size = new Size(188, 20);
            lblOperacionesCodigo.TabIndex = 0;
            lblOperacionesCodigo.Text = "Operación a inspeccionar:";
            // 
            // cboOperacionesCodigo
            // 
            cboOperacionesCodigo.DropDownStyle = ComboBoxStyle.DropDownList;
            cboOperacionesCodigo.Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboOperacionesCodigo.FormattingEnabled = true;
            cboOperacionesCodigo.Location = new Point(14, 50);
            cboOperacionesCodigo.Name = "cboOperacionesCodigo";
            cboOperacionesCodigo.Size = new Size(490, 27);
            cboOperacionesCodigo.TabIndex = 1;
            cboOperacionesCodigo.SelectedIndexChanged += cboOperacionesCodigo_SelectedIndexChanged;
            // 
            // rbCuadruploOpActual
            // 
            rbCuadruploOpActual.AutoSize = true;
            rbCuadruploOpActual.Checked = true;
            rbCuadruploOpActual.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbCuadruploOpActual.ForeColor = Color.DarkSlateBlue;
            rbCuadruploOpActual.Location = new Point(518, 52);
            rbCuadruploOpActual.Name = "rbCuadruploOpActual";
            rbCuadruploOpActual.Size = new Size(147, 24);
            rbCuadruploOpActual.TabIndex = 3;
            rbCuadruploOpActual.TabStop = true;
            rbCuadruploOpActual.Text = "Op. Seleccionada";
            rbCuadruploOpActual.UseVisualStyleBackColor = true;
            rbCuadruploOpActual.CheckedChanged += rbCuadruploModo_CheckedChanged;
            // 
            // rbCuadruploTodos
            // 
            rbCuadruploTodos.AutoSize = true;
            rbCuadruploTodos.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbCuadruploTodos.ForeColor = Color.DarkSlateBlue;
            rbCuadruploTodos.Location = new Point(672, 52);
            rbCuadruploTodos.Name = "rbCuadruploTodos";
            rbCuadruploTodos.Size = new Size(171, 24);
            rbCuadruploTodos.TabIndex = 4;
            rbCuadruploTodos.Text = "Programa Completo";
            rbCuadruploTodos.UseVisualStyleBackColor = true;
            rbCuadruploTodos.CheckedChanged += rbCuadruploModo_CheckedChanged;
            // 
            // lblEstadoSemantico
            // 
            lblEstadoSemantico.AutoSize = true;
            lblEstadoSemantico.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEstadoSemantico.ForeColor = Color.ForestGreen;
            lblEstadoSemantico.Location = new Point(14, 82);
            lblEstadoSemantico.Name = "lblEstadoSemantico";
            lblEstadoSemantico.Size = new Size(231, 20);
            lblEstadoSemantico.TabIndex = 2;
            lblEstadoSemantico.Text = "Estado: Presiona 'Analizar Todo'";
            // 
            // tabAnalisisJerarquia
            // 
            tabAnalisisJerarquia.Controls.Add(tabArbol);
            tabAnalisisJerarquia.Controls.Add(tabNotaciones);
            tabAnalisisJerarquia.Controls.Add(tabCodigoIntermedio);
            tabAnalisisJerarquia.Controls.Add(tabCodigoP);
            tabAnalisisJerarquia.Font = new Font("Segoe UI", 9.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabAnalisisJerarquia.Location = new Point(12, 108);
            tabAnalisisJerarquia.Name = "tabAnalisisJerarquia";
            tabAnalisisJerarquia.SelectedIndex = 0;
            tabAnalisisJerarquia.Size = new Size(824, 688);
            tabAnalisisJerarquia.TabIndex = 3;
            // 
            // tabArbol
            // 
            tabArbol.BackColor = Color.White;
            tabArbol.Controls.Add(tvArbolJerarquia);
            tabArbol.Controls.Add(lblPasosJerarquia);
            tabArbol.Controls.Add(rtbPasosJerarquia);
            tabArbol.Location = new Point(4, 29);
            tabArbol.Name = "tabArbol";
            tabArbol.Padding = new Padding(3);
            tabArbol.Size = new Size(816, 655);
            tabArbol.TabIndex = 0;
            tabArbol.Text = "🌳 1. Árbol y Pasos";
            // 
            // tvArbolJerarquia
            // 
            tvArbolJerarquia.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tvArbolJerarquia.Location = new Point(10, 10);
            tvArbolJerarquia.Name = "tvArbolJerarquia";
            tvArbolJerarquia.Size = new Size(796, 370);
            tvArbolJerarquia.TabIndex = 0;
            tvArbolJerarquia.AfterSelect += tvArbolJerarquia_AfterSelect;
            // 
            // lblPasosJerarquia
            // 
            lblPasosJerarquia.AutoSize = true;
            lblPasosJerarquia.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPasosJerarquia.ForeColor = Color.DarkSlateBlue;
            lblPasosJerarquia.Location = new Point(10, 388);
            lblPasosJerarquia.Name = "lblPasosJerarquia";
            lblPasosJerarquia.Size = new Size(357, 21);
            lblPasosJerarquia.TabIndex = 1;
            lblPasosJerarquia.Text = "Pasos de Resolución Semántica por Jerarquía:";
            // 
            // rtbPasosJerarquia
            // 
            rtbPasosJerarquia.BackColor = Color.FromArgb(250, 252, 255);
            rtbPasosJerarquia.Font = new Font("Consolas", 9.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbPasosJerarquia.ForeColor = Color.FromArgb(30, 30, 30);
            rtbPasosJerarquia.Location = new Point(10, 412);
            rtbPasosJerarquia.Name = "rtbPasosJerarquia";
            rtbPasosJerarquia.ReadOnly = true;
            rtbPasosJerarquia.Size = new Size(796, 232);
            rtbPasosJerarquia.TabIndex = 2;
            rtbPasosJerarquia.Text = "";
            // 
            // tabNotaciones
            // 
            tabNotaciones.BackColor = Color.White;
            tabNotaciones.Controls.Add(lblInfija);
            tabNotaciones.Controls.Add(txtInfija);
            tabNotaciones.Controls.Add(lblPrefija);
            tabNotaciones.Controls.Add(txtPrefija);
            tabNotaciones.Controls.Add(lblPostfija);
            tabNotaciones.Controls.Add(txtPostfija);
            tabNotaciones.Controls.Add(lblTablaNotaciones);
            tabNotaciones.Controls.Add(dgtNotaciones);
            tabNotaciones.Location = new Point(4, 29);
            tabNotaciones.Name = "tabNotaciones";
            tabNotaciones.Padding = new Padding(3);
            tabNotaciones.Size = new Size(816, 655);
            tabNotaciones.TabIndex = 1;
            tabNotaciones.Text = "📝 2. Notaciones";
            // 
            // lblInfija
            // 
            lblInfija.AutoSize = true;
            lblInfija.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblInfija.ForeColor = Color.DarkSlateGray;
            lblInfija.Location = new Point(12, 12);
            lblInfija.Name = "lblInfija";
            lblInfija.Size = new Size(156, 21);
            lblInfija.TabIndex = 0;
            lblInfija.Text = "📌 Notación Infija:";
            // 
            // txtInfija
            // 
            txtInfija.BackColor = Color.FromArgb(245, 250, 245);
            txtInfija.Font = new Font("Consolas", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtInfija.ForeColor = Color.DarkGreen;
            txtInfija.Location = new Point(12, 36);
            txtInfija.Name = "txtInfija";
            txtInfija.ReadOnly = true;
            txtInfija.Size = new Size(790, 29);
            txtInfija.TabIndex = 1;
            // 
            // lblPrefija
            // 
            lblPrefija.AutoSize = true;
            lblPrefija.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPrefija.ForeColor = Color.DarkMagenta;
            lblPrefija.Location = new Point(12, 78);
            lblPrefija.Name = "lblPrefija";
            lblPrefija.Size = new Size(226, 21);
            lblPrefija.TabIndex = 2;
            lblPrefija.Text = "⬅️ Notación Prefija / Polaca:";
            // 
            // txtPrefija
            // 
            txtPrefija.BackColor = Color.FromArgb(252, 245, 255);
            txtPrefija.Font = new Font("Consolas", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPrefija.ForeColor = Color.Purple;
            txtPrefija.Location = new Point(12, 102);
            txtPrefija.Name = "txtPrefija";
            txtPrefija.ReadOnly = true;
            txtPrefija.Size = new Size(790, 29);
            txtPrefija.TabIndex = 3;
            // 
            // lblPostfija
            // 
            lblPostfija.AutoSize = true;
            lblPostfija.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPostfija.ForeColor = Color.DarkSlateBlue;
            lblPostfija.Location = new Point(12, 144);
            lblPostfija.Name = "lblPostfija";
            lblPostfija.Size = new Size(336, 21);
            lblPostfija.TabIndex = 4;
            lblPostfija.Text = "➡️ Notación Postfija / Polaca Inversa RPN:";
            // 
            // txtPostfija
            // 
            txtPostfija.BackColor = Color.FromArgb(245, 248, 255);
            txtPostfija.Font = new Font("Consolas", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txtPostfija.ForeColor = Color.DarkBlue;
            txtPostfija.Location = new Point(12, 168);
            txtPostfija.Name = "txtPostfija";
            txtPostfija.ReadOnly = true;
            txtPostfija.Size = new Size(790, 29);
            txtPostfija.TabIndex = 5;
            // 
            // lblTablaNotaciones
            // 
            lblTablaNotaciones.AutoSize = true;
            lblTablaNotaciones.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTablaNotaciones.ForeColor = Color.DarkSlateBlue;
            lblTablaNotaciones.Location = new Point(12, 212);
            lblTablaNotaciones.Name = "lblTablaNotaciones";
            lblTablaNotaciones.Size = new Size(378, 21);
            lblTablaNotaciones.TabIndex = 6;
            lblTablaNotaciones.Text = "Tabla Comparativa de Notaciones del Programa:";
            // 
            // dgtNotaciones
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgtNotaciones.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            dgtNotaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtNotaciones.BackgroundColor = Color.White;
            dgtNotaciones.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtNotaciones.Columns.AddRange(new DataGridViewColumn[] { colNotNum, colNotInfija, colNotPrefija, colNotPostfija });
            dgtNotaciones.Location = new Point(12, 238);
            dgtNotaciones.Name = "dgtNotaciones";
            dgtNotaciones.RowHeadersVisible = false;
            dgtNotaciones.RowHeadersWidth = 51;
            dgtNotaciones.Size = new Size(790, 405);
            dgtNotaciones.TabIndex = 7;
            // 
            // colNotNum
            // 
            colNotNum.FillWeight = 22F;
            colNotNum.HeaderText = "#";
            colNotNum.MinimumWidth = 6;
            colNotNum.Name = "colNotNum";
            colNotNum.ReadOnly = true;
            // 
            // colNotInfija
            // 
            colNotInfija.FillWeight = 95F;
            colNotInfija.HeaderText = "Notación Infija";
            colNotInfija.MinimumWidth = 6;
            colNotInfija.Name = "colNotInfija";
            colNotInfija.ReadOnly = true;
            // 
            // colNotPrefija
            // 
            colNotPrefija.FillWeight = 95F;
            colNotPrefija.HeaderText = "Notación Prefija (Polaca)";
            colNotPrefija.MinimumWidth = 6;
            colNotPrefija.Name = "colNotPrefija";
            colNotPrefija.ReadOnly = true;
            // 
            // colNotPostfija
            // 
            colNotPostfija.FillWeight = 95F;
            colNotPostfija.HeaderText = "Notación Postfija (RPN)";
            colNotPostfija.MinimumWidth = 6;
            colNotPostfija.Name = "colNotPostfija";
            colNotPostfija.ReadOnly = true;
            // 
            // tabCodigoIntermedio
            // 
            tabCodigoIntermedio.BackColor = Color.White;
            tabCodigoIntermedio.Controls.Add(lblCuadruplos);
            tabCodigoIntermedio.Controls.Add(dgtCuadruplos);
            tabCodigoIntermedio.Controls.Add(lblTriplos);
            tabCodigoIntermedio.Controls.Add(dgtTriplos);
            tabCodigoIntermedio.Controls.Add(lblResumenCuadruplos);
            tabCodigoIntermedio.Location = new Point(4, 29);
            tabCodigoIntermedio.Name = "tabCodigoIntermedio";
            tabCodigoIntermedio.Padding = new Padding(3);
            tabCodigoIntermedio.Size = new Size(816, 655);
            tabCodigoIntermedio.TabIndex = 2;
            tabCodigoIntermedio.Text = "🔢 3. Cuádruplos y Triplos";
            // 
            // lblCuadruplos
            // 
            lblCuadruplos.AutoSize = true;
            lblCuadruplos.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCuadruplos.ForeColor = Color.DarkSlateBlue;
            lblCuadruplos.Location = new Point(10, 10);
            lblCuadruplos.Name = "lblCuadruplos";
            lblCuadruplos.Size = new Size(299, 21);
            lblCuadruplos.TabIndex = 2;
            lblCuadruplos.Text = "Cuádruplos (Con Temporales T1, T2...):";
            // 
            // dgtCuadruplos
            // 
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtCuadruplos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgtCuadruplos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtCuadruplos.BackgroundColor = Color.White;
            dgtCuadruplos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtCuadruplos.Columns.AddRange(new DataGridViewColumn[] { colNum, colOp, colArg1, colArg2, colRes });
            dgtCuadruplos.Location = new Point(10, 35);
            dgtCuadruplos.Name = "dgtCuadruplos";
            dgtCuadruplos.RowHeadersVisible = false;
            dgtCuadruplos.RowHeadersWidth = 51;
            dgtCuadruplos.Size = new Size(405, 580);
            dgtCuadruplos.TabIndex = 5;
            // 
            // colNum
            // 
            colNum.FillWeight = 28F;
            colNum.HeaderText = "#";
            colNum.MinimumWidth = 6;
            colNum.Name = "colNum";
            colNum.ReadOnly = true;
            // 
            // colOp
            // 
            colOp.FillWeight = 45F;
            colOp.HeaderText = "Operador";
            colOp.MinimumWidth = 6;
            colOp.Name = "colOp";
            colOp.ReadOnly = true;
            // 
            // colArg1
            // 
            colArg1.FillWeight = 52F;
            colArg1.HeaderText = "Arg 1";
            colArg1.MinimumWidth = 6;
            colArg1.Name = "colArg1";
            colArg1.ReadOnly = true;
            // 
            // colArg2
            // 
            colArg2.FillWeight = 52F;
            colArg2.HeaderText = "Arg 2";
            colArg2.MinimumWidth = 6;
            colArg2.Name = "colArg2";
            colArg2.ReadOnly = true;
            // 
            // colRes
            // 
            colRes.FillWeight = 55F;
            colRes.HeaderText = "Resultado";
            colRes.MinimumWidth = 6;
            colRes.Name = "colRes";
            colRes.ReadOnly = true;
            // 
            // lblTriplos
            // 
            lblTriplos.AutoSize = true;
            lblTriplos.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTriplos.ForeColor = Color.DarkGreen;
            lblTriplos.Location = new Point(425, 10);
            lblTriplos.Name = "lblTriplos";
            lblTriplos.Size = new Size(316, 21);
            lblTriplos.TabIndex = 7;
            lblTriplos.Text = "Triplos (Referencias por Índice (1), (2)...):";
            // 
            // dgtTriplos
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtTriplos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dgtTriplos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtTriplos.BackgroundColor = Color.White;
            dgtTriplos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtTriplos.Columns.AddRange(new DataGridViewColumn[] { colTripRef, colTripOp, colTripArg1, colTripArg2 });
            dgtTriplos.Location = new Point(425, 35);
            dgtTriplos.Name = "dgtTriplos";
            dgtTriplos.RowHeadersVisible = false;
            dgtTriplos.RowHeadersWidth = 51;
            dgtTriplos.Size = new Size(380, 580);
            dgtTriplos.TabIndex = 8;
            // 
            // colTripRef
            // 
            colTripRef.FillWeight = 35F;
            colTripRef.HeaderText = "Índice";
            colTripRef.MinimumWidth = 6;
            colTripRef.Name = "colTripRef";
            colTripRef.ReadOnly = true;
            // 
            // colTripOp
            // 
            colTripOp.FillWeight = 50F;
            colTripOp.HeaderText = "Operador";
            colTripOp.MinimumWidth = 6;
            colTripOp.Name = "colTripOp";
            colTripOp.ReadOnly = true;
            // 
            // colTripArg1
            // 
            colTripArg1.FillWeight = 60F;
            colTripArg1.HeaderText = "Arg 1";
            colTripArg1.MinimumWidth = 6;
            colTripArg1.Name = "colTripArg1";
            colTripArg1.ReadOnly = true;
            // 
            // colTripArg2
            // 
            colTripArg2.FillWeight = 60F;
            colTripArg2.HeaderText = "Arg 2";
            colTripArg2.MinimumWidth = 6;
            colTripArg2.Name = "colTripArg2";
            colTripArg2.ReadOnly = true;
            // 
            // lblResumenCuadruplos
            // 
            lblResumenCuadruplos.AutoSize = true;
            lblResumenCuadruplos.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResumenCuadruplos.ForeColor = Color.DimGray;
            lblResumenCuadruplos.Location = new Point(10, 622);
            lblResumenCuadruplos.Name = "lblResumenCuadruplos";
            lblResumenCuadruplos.Size = new Size(293, 20);
            lblResumenCuadruplos.TabIndex = 6;
            lblResumenCuadruplos.Text = "Temporales: 0 | Cuádruplos: 0 | Triplos: 0";
            // 
            // tabCodigoP
            // 
            tabCodigoP.BackColor = Color.White;
            tabCodigoP.Controls.Add(lblTablaCodigoP);
            tabCodigoP.Controls.Add(dgtCodigoP);
            tabCodigoP.Controls.Add(lblTextoCodigoP);
            tabCodigoP.Controls.Add(rtbCodigoP);
            tabCodigoP.Controls.Add(lblResumenCodigoP);
            tabCodigoP.Location = new Point(4, 29);
            tabCodigoP.Name = "tabCodigoP";
            tabCodigoP.Padding = new Padding(3);
            tabCodigoP.Size = new Size(816, 655);
            tabCodigoP.TabIndex = 3;
            tabCodigoP.Text = "⚙️ 4. Código P (P-Code)";
            // 
            // lblTablaCodigoP
            // 
            lblTablaCodigoP.AutoSize = true;
            lblTablaCodigoP.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTablaCodigoP.ForeColor = Color.DarkSlateBlue;
            lblTablaCodigoP.Location = new Point(10, 10);
            lblTablaCodigoP.Name = "lblTablaCodigoP";
            lblTablaCodigoP.Size = new Size(413, 21);
            lblTablaCodigoP.TabIndex = 0;
            lblTablaCodigoP.Text = "Instrucciones de Máquina Abstracta de Pila (P-Code):";
            // 
            // dgtCodigoP
            // 
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgtCodigoP.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            dgtCodigoP.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgtCodigoP.BackgroundColor = Color.White;
            dgtCodigoP.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtCodigoP.Columns.AddRange(new DataGridViewColumn[] { colPNum, colPInst, colPOp, colPDesc });
            dgtCodigoP.Location = new Point(10, 35);
            dgtCodigoP.Name = "dgtCodigoP";
            dgtCodigoP.RowHeadersVisible = false;
            dgtCodigoP.RowHeadersWidth = 51;
            dgtCodigoP.Size = new Size(495, 580);
            dgtCodigoP.TabIndex = 1;
            // 
            // colPNum
            // 
            colPNum.FillWeight = 22F;
            colPNum.HeaderText = "#";
            colPNum.MinimumWidth = 6;
            colPNum.Name = "colPNum";
            colPNum.ReadOnly = true;
            // 
            // colPInst
            // 
            colPInst.FillWeight = 40F;
            colPInst.HeaderText = "Mnemónico";
            colPInst.MinimumWidth = 6;
            colPInst.Name = "colPInst";
            colPInst.ReadOnly = true;
            // 
            // colPOp
            // 
            colPOp.FillWeight = 55F;
            colPOp.HeaderText = "Operando";
            colPOp.MinimumWidth = 6;
            colPOp.Name = "colPOp";
            colPOp.ReadOnly = true;
            // 
            // colPDesc
            // 
            colPDesc.FillWeight = 115F;
            colPDesc.HeaderText = "Acción en la Pila";
            colPDesc.MinimumWidth = 6;
            colPDesc.Name = "colPDesc";
            colPDesc.ReadOnly = true;
            // 
            // lblTextoCodigoP
            // 
            lblTextoCodigoP.AutoSize = true;
            lblTextoCodigoP.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTextoCodigoP.ForeColor = Color.DarkSlateBlue;
            lblTextoCodigoP.Location = new Point(515, 10);
            lblTextoCodigoP.Name = "lblTextoCodigoP";
            lblTextoCodigoP.Size = new Size(232, 21);
            lblTextoCodigoP.TabIndex = 2;
            lblTextoCodigoP.Text = "Listado Ensamblador P-Code:";
            // 
            // rtbCodigoP
            // 
            rtbCodigoP.BackColor = Color.FromArgb(245, 248, 255);
            rtbCodigoP.Font = new Font("Consolas", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rtbCodigoP.ForeColor = Color.FromArgb(25, 45, 90);
            rtbCodigoP.Location = new Point(515, 35);
            rtbCodigoP.Name = "rtbCodigoP";
            rtbCodigoP.ReadOnly = true;
            rtbCodigoP.Size = new Size(290, 580);
            rtbCodigoP.TabIndex = 3;
            rtbCodigoP.Text = "";
            // 
            // lblResumenCodigoP
            // 
            lblResumenCodigoP.AutoSize = true;
            lblResumenCodigoP.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResumenCodigoP.ForeColor = Color.DimGray;
            lblResumenCodigoP.Location = new Point(10, 622);
            lblResumenCodigoP.Name = "lblResumenCodigoP";
            lblResumenCodigoP.Size = new Size(212, 20);
            lblResumenCodigoP.TabIndex = 4;
            lblResumenCodigoP.Text = "Total Instrucciones P-Code: 0";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabControl1.Location = new Point(14, 482);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(872, 338);
            tabControl1.TabIndex = 24;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.White;
            tabPage1.Controls.Add(dgtTablaDeSimbolos);
            tabPage1.Controls.Add(label4);
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(864, 304);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "📊 Tabla de Símbolos";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = Color.White;
            tabPage2.Controls.Add(dgtErrores);
            tabPage2.Controls.Add(label3);
            tabPage2.Location = new Point(4, 30);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(864, 304);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "⚠️ Tabla de Errores";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            CancelButton = btnCargarPrograma;
            ClientSize = new Size(1756, 832);
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
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NovaNyx 1.7.0 - Compilador y Generador de Código Intermedio";
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
            tabNotaciones.ResumeLayout(false);
            tabNotaciones.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgtNotaciones).EndInit();
            tabCodigoIntermedio.ResumeLayout(false);
            tabCodigoIntermedio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCuadruplos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgtTriplos).EndInit();
            tabCodigoP.ResumeLayout(false);
            tabCodigoP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgtCodigoP).EndInit();
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
        private System.Windows.Forms.Button btnAnalizar;
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
        private System.Windows.Forms.TabPage tabNotaciones;
        private System.Windows.Forms.TabPage tabCodigoIntermedio;
        private System.Windows.Forms.TabPage tabCodigoP;
        private System.Windows.Forms.TreeView tvArbolJerarquia;
        private System.Windows.Forms.Label lblPasosJerarquia;
        private System.Windows.Forms.RichTextBox rtbPasosJerarquia;
        private System.Windows.Forms.Label lblInfija;
        private System.Windows.Forms.TextBox txtInfija;
        private System.Windows.Forms.Label lblPrefija;
        private System.Windows.Forms.TextBox txtPrefija;
        private System.Windows.Forms.Label lblPostfija;
        private System.Windows.Forms.TextBox txtPostfija;
        private System.Windows.Forms.Label lblTablaNotaciones;
        private System.Windows.Forms.DataGridView dgtNotaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotInfija;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotPrefija;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNotPostfija;
        private System.Windows.Forms.Label lblCuadruplos;
        private System.Windows.Forms.RadioButton rbCuadruploOpActual;
        private System.Windows.Forms.RadioButton rbCuadruploTodos;
        private System.Windows.Forms.DataGridView dgtCuadruplos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArg2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRes;
        private System.Windows.Forms.Label lblTriplos;
        private System.Windows.Forms.DataGridView dgtTriplos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTripRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTripOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTripArg1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTripArg2;
        private System.Windows.Forms.Label lblResumenCuadruplos;
        private System.Windows.Forms.Label lblTablaCodigoP;
        private System.Windows.Forms.DataGridView dgtCodigoP;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPInst;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPOp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPDesc;
        private System.Windows.Forms.Label lblTextoCodigoP;
        private System.Windows.Forms.RichTextBox rtbCodigoP;
        private System.Windows.Forms.Label lblResumenCodigoP;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
    }
}
