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
            button1 = new Button();
            listBox1 = new ListBox();
            btnAnalizar = new Button();
            richArchivoDeTokens = new RichTextBox();
            richProgramaFuente = new RichTextBox();
            dgvMatriz = new DataGridView();
            dgtTablaDeSimbolos = new DataGridView();
            Identificador = new DataGridViewTextBoxColumn();
            Nombre = new DataGridViewTextBoxColumn();
            TipoDato = new DataGridViewTextBoxColumn();
            Valor = new DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvMatriz).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(404, 303);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(620, 69);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(150, 104);
            listBox1.TabIndex = 2;
            // 
            // btnAnalizar
            // 
            btnAnalizar.Location = new Point(89, 303);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(94, 29);
            btnAnalizar.TabIndex = 3;
            btnAnalizar.Text = "Analizar";
            btnAnalizar.UseVisualStyleBackColor = true;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // richArchivoDeTokens
            // 
            richArchivoDeTokens.Location = new Point(295, 32);
            richArchivoDeTokens.Name = "richArchivoDeTokens";
            richArchivoDeTokens.Size = new Size(230, 252);
            richArchivoDeTokens.TabIndex = 4;
            richArchivoDeTokens.Text = "";
            // 
            // richProgramaFuente
            // 
            richProgramaFuente.Location = new Point(26, 32);
            richProgramaFuente.Name = "richProgramaFuente";
            richProgramaFuente.Size = new Size(224, 252);
            richProgramaFuente.TabIndex = 5;
            richProgramaFuente.Text = "";
            richProgramaFuente.TextChanged += richProgramaFuente_TextChanged;
            // 
            // dgvMatriz
            // 
            dgvMatriz.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatriz.Location = new Point(686, 189);
            dgvMatriz.Name = "dgvMatriz";
            dgvMatriz.RowHeadersWidth = 51;
            dgvMatriz.Size = new Size(308, 188);
            dgvMatriz.TabIndex = 6;
            // 
            // dgtTablaDeSimbolos
            // 
            dgtTablaDeSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgtTablaDeSimbolos.Columns.AddRange(new DataGridViewColumn[] { Identificador, Nombre, TipoDato, Valor });
            dgtTablaDeSimbolos.Location = new Point(26, 357);
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1185, 583);
            Controls.Add(dgtTablaDeSimbolos);
            Controls.Add(dgvMatriz);
            Controls.Add(richProgramaFuente);
            Controls.Add(richArchivoDeTokens);
            Controls.Add(btnAnalizar);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMatriz).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgtTablaDeSimbolos).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private ListBox listBox1;
        private Button btnAnalizar;
        private RichTextBox richArchivoDeTokens;
        private RichTextBox richProgramaFuente;
        private DataGridView dgvMatriz;
        private DataGridView dgtTablaDeSimbolos;
        private DataGridViewTextBoxColumn Identificador;
        private DataGridViewTextBoxColumn Nombre;
        private DataGridViewTextBoxColumn TipoDato;
        private DataGridViewTextBoxColumn Valor;
    }
}
