namespace Interfaz_Lexico
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richArchivoDeTokens = new RichTextBox();
            richProgramaFuente = new RichTextBox();
            dgtTablaDeSimbolos = new DataGridView();
            Identificador = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            TipoDato = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            dgtErrores = new DataGridView();
            Linea = new DataGridViewTextBoxColumn();
            Error = new DataGridViewTextBoxColumn();
            btnGuardarPrograma = new Button();
            btnCargarPrograma = new Button();
            btnEditarPrograma = new Button();
            btnGuardarArchivo = new Button();
            lblNovaNyx = new Label();
            PicNovaNyx = new PictureBox();
            lblNovaNyxVersion = new Label();
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgtErrores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicNovaNyx).BeginInit();
            SuspendLayout();
            // 
            // richArchivoDeTokens
            // 
            richArchivoDeTokens.Location = new Point(748, 32);
            richArchivoDeTokens.Name = "richArchivoDeTokens";
            richArchivoDeTokens.Size = new Size(420, 337);
            richArchivoDeTokens.TabIndex = 4;
            richArchivoDeTokens.Text = "";
            // 
            // richProgramaFuente
            // 
            richProgramaFuente.Location = new Point(26, 32);
            richProgramaFuente.Name = "richProgramaFuente";
            richProgramaFuente.Size = new Size(420, 337);
            richProgramaFuente.TabIndex = 5;
            richProgramaFuente.Text = "";
            richProgramaFuente.TextChanged += richProgramaFuente_TextChanged;
            // 
            // dgtTablaDeSimbolos
            // 
            dgtTablaDeSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtTablaDeSimbolos.Columns.AddRange(new DataGridViewColumn[] { Identificador, Nombre, TipoDato, Valor });
            dgtTablaDeSimbolos.Location = new Point(26, 458);
            dgtTablaDeSimbolos.Name = "dgtTablaDeSimbolos";
            dgtTablaDeSimbolos.RowHeadersWidth = 51;
            dgtTablaDeSimbolos.Size = new Size(552, 188);
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
            Valor.Width = 125;
            // 
            // dgtErrores
            // 
            dgtErrores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtErrores.Columns.AddRange(new DataGridViewColumn[] { Linea, Error });
            dgtErrores.Location = new Point(600, 458);
            dgtErrores.Name = "dgtErrores";
            dgtErrores.RowHeadersWidth = 51;
            dgtErrores.Size = new Size(603, 188);
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
            btnGuardarPrograma.Location = new Point(310, 393);
            btnGuardarPrograma.Name = "btnGuardarPrograma";
            btnGuardarPrograma.Size = new Size(136, 48);
            btnGuardarPrograma.TabIndex = 9;
            btnGuardarPrograma.Text = "Guardar Programa";
            btnGuardarPrograma.UseVisualStyleBackColor = true;
            btnGuardarPrograma.Click += btnGuardarPrograma_Click;
            // 
            // btnCargarPrograma
            // 
            btnCargarPrograma.Location = new Point(26, 393);
            btnCargarPrograma.Name = "btnCargarPrograma";
            btnCargarPrograma.Size = new Size(136, 48);
            btnCargarPrograma.TabIndex = 10;
            btnCargarPrograma.Text = "Cargar Programa";
            btnCargarPrograma.UseVisualStyleBackColor = true;
            btnCargarPrograma.Click += btnCargarPrograma_Click;
            // 
            // btnEditarPrograma
            // 
            btnEditarPrograma.Location = new Point(168, 393);
            btnEditarPrograma.Name = "btnEditarPrograma";
            btnEditarPrograma.Size = new Size(136, 48);
            btnEditarPrograma.TabIndex = 11;
            btnEditarPrograma.Text = "EditarPrograma";
            btnEditarPrograma.UseVisualStyleBackColor = true;
            btnEditarPrograma.Click += btnEditarPrograma_Click;
            // 
            // btnGuardarArchivo
            // 
            btnGuardarArchivo.Location = new Point(748, 393);
            btnGuardarArchivo.Name = "btnGuardarArchivo";
            btnGuardarArchivo.Size = new Size(136, 48);
            btnGuardarArchivo.TabIndex = 12;
            btnGuardarArchivo.Text = "Guardar Archivo";
            btnGuardarArchivo.UseVisualStyleBackColor = true;
            btnGuardarArchivo.Click += btnGuardarArchivo_Click;
            // 
            // lblNovaNyx
            // 
            lblNovaNyx.AutoSize = true;
            lblNovaNyx.Font = new Font("Stencil", 25.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNovaNyx.ForeColor = SystemColors.ActiveCaptionText;
            lblNovaNyx.Location = new Point(489, 12);
            lblNovaNyx.Name = "lblNovaNyx";
            lblNovaNyx.Size = new Size(217, 52);
            lblNovaNyx.TabIndex = 13;
            lblNovaNyx.Text = "NovaNyx";
            // 
            // PicNovaNyx
            // 
            PicNovaNyx.Image = Properties.Resources.WhatsApp_Image_2026_04_19_at_10_51_10_AM;
            PicNovaNyx.Location = new Point(485, 67);
            PicNovaNyx.Name = "PicNovaNyx";
            PicNovaNyx.Size = new Size(214, 158);
            PicNovaNyx.TabIndex = 14;
            PicNovaNyx.TabStop = false;
            PicNovaNyx.Click += PicNovaNyx_Click;
            // 
            // lblNovaNyxVersion
            // 
            lblNovaNyxVersion.AutoSize = true;
            lblNovaNyxVersion.Location = new Point(1112, 660);
            lblNovaNyxVersion.Name = "lblNovaNyxVersion";
            lblNovaNyxVersion.Size = new Size(91, 20);
            lblNovaNyxVersion.TabIndex = 15;
            lblNovaNyxVersion.Text = "Versión 1.4.2";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            CancelButton = btnCargarPrograma;
            ClientSize = new Size(1215, 689);
            Controls.Add(lblNovaNyxVersion);
            Controls.Add(PicNovaNyx);
            Controls.Add(lblNovaNyx);
            Controls.Add(btnGuardarArchivo);
            Controls.Add(btnEditarPrograma);
            Controls.Add(btnCargarPrograma);
            Controls.Add(btnGuardarPrograma);
            Controls.Add(dgtErrores);
            Controls.Add(dgtTablaDeSimbolos);
            Controls.Add(richProgramaFuente);
            Controls.Add(richArchivoDeTokens);
            Name = "Form1";
            Text = "NovaNyx 1.4.2";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgtErrores).EndInit();
            ((System.ComponentModel.ISupportInitialize)PicNovaNyx).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox richArchivoDeTokens;
        private RichTextBox richProgramaFuente;
        private DataGridView dgtTablaDeSimbolos;
        private DataGridViewTextBoxColumn Identificador;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn TipoDato;
        private DataGridViewTextBoxColumn Valor;
        private DataGridView dgtErrores;
        private DataGridViewTextBoxColumn Linea;
        private DataGridViewTextBoxColumn Error;
        private Button btnGuardarPrograma;
        private Button btnCargarPrograma;
        private Button btnEditarPrograma;
        private Button btnGuardarArchivo;
        private Label lblNovaNyx;
        private PictureBox PicNovaNyx;
        private Label lblNovaNyxVersion;
    }
}
