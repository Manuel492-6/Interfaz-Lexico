namespace Interfaz_Lexico
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
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
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgtErrores).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PicNovaNyx).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLineas).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLinea2).BeginInit();
            SuspendLayout();
            // 
            // richArchivoDeTokens
            // 
            richArchivoDeTokens.Location = new Point(956, 67);
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
            dgtTablaDeSimbolos.Columns.AddRange(new DataGridViewColumn[] { Identificador, Nombre, TipoDato, Valor });
            dgtTablaDeSimbolos.GridColor = SystemColors.InactiveCaptionText;
            dgtTablaDeSimbolos.Location = new Point(26, 546);
            dgtTablaDeSimbolos.Name = "dgtTablaDeSimbolos";
            dgtTablaDeSimbolos.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
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
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgtErrores.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            dgtErrores.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtErrores.Columns.AddRange(new DataGridViewColumn[] { Linea, Error });
            dgtErrores.Location = new Point(600, 544);
            dgtErrores.Name = "dgtErrores";
            dgtErrores.RowHeadersWidth = 51;
            dgtErrores.Size = new Size(1055, 188);
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
            btnGuardarArchivo.Location = new Point(959, 432);
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
            lblNovaNyx.Location = new Point(616, 9);
            lblNovaNyx.Name = "lblNovaNyx";
            lblNovaNyx.Size = new Size(212, 51);
            lblNovaNyx.TabIndex = 13;
            lblNovaNyx.Text = "NovaNyx";
            // 
            // PicNovaNyx
            // 
            PicNovaNyx.Image = Properties.Resources.WhatsApp_Image_2026_04_19_at_10_51_10_AM;
            PicNovaNyx.Location = new Point(616, 67);
            PicNovaNyx.Name = "PicNovaNyx";
            PicNovaNyx.Size = new Size(214, 158);
            PicNovaNyx.SizeMode = PictureBoxSizeMode.StretchImage;
            PicNovaNyx.TabIndex = 14;
            PicNovaNyx.TabStop = false;
            // 
            // lblNovaNyxVersion
            // 
            lblNovaNyxVersion.AutoSize = true;
            lblNovaNyxVersion.Location = new Point(1112, 735);
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
            label2.Location = new Point(956, 26);
            label2.Name = "label2";
            label2.Size = new Size(223, 27);
            label2.TabIndex = 17;
            label2.Text = "Archivo De Tokens";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(600, 502);
            label3.Name = "label3";
            label3.Size = new Size(94, 27);
            label3.TabIndex = 18;
            label3.Text = "Errores";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Arial", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(26, 502);
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
            picLinea2.Location = new Point(876, 67);
            picLinea2.Name = "picLinea2";
            picLinea2.Size = new Size(74, 337);
            picLinea2.TabIndex = 21;
            picLinea2.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(627, 244);
            label5.Name = "label5";
            label5.Size = new Size(227, 140);
            label5.TabIndex = 23;
            label5.Text = "Integrantes\r\n\r\nVictor Manuel Martinez Sifuentes\r\n\r\nYadhira Luna Carrillo\r\n\r\nChristian Medina Cital\r\n";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            CancelButton = btnCargarPrograma;
            ClientSize = new Size(1769, 764);
            Controls.Add(label5);
            Controls.Add(picLinea2);
            Controls.Add(picLineas);
            Controls.Add(label4);
            Controls.Add(label3);
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
            Controls.Add(dgtErrores);
            Controls.Add(dgtTablaDeSimbolos);
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
        private Label label5;
    }
}