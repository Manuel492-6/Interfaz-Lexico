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
            ((System.ComponentModel.ISupportInitialize)dgvMatriz).BeginInit();
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
            // 
            // dgvMatriz
            // 
            dgvMatriz.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMatriz.Location = new Point(115, 338);
            dgvMatriz.Name = "dgvMatriz";
            dgvMatriz.RowHeadersWidth = 51;
            dgvMatriz.Size = new Size(655, 188);
            dgvMatriz.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 583);
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
            ResumeLayout(false);
        }

        #endregion
        private Button button1;
        private ListBox listBox1;
        private Button btnAnalizar;
        private RichTextBox richArchivoDeTokens;
        private RichTextBox richProgramaFuente;
        private DataGridView dgvMatriz;
    }
}
