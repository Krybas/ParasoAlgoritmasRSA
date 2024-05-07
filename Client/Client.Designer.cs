namespace Client
{
    partial class Client
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
            label1 = new Label();
            txtClient = new TextBox();
            buttonClient = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 186);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(80, 25);
            label1.TabIndex = 0;
            label1.Text = "Klientas";
            // 
            // txtClient
            // 
            txtClient.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 186);
            txtClient.Location = new Point(12, 37);
            txtClient.Multiline = true;
            txtClient.Name = "txtClient";
            txtClient.Size = new Size(198, 170);
            txtClient.TabIndex = 1;
            // 
            // buttonClient
            // 
            buttonClient.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 186);
            buttonClient.Location = new Point(56, 228);
            buttonClient.Name = "buttonClient";
            buttonClient.Size = new Size(116, 45);
            buttonClient.TabIndex = 2;
            buttonClient.Text = "Siusti";
            buttonClient.UseVisualStyleBackColor = true;
            buttonClient.Click += buttonClient_Click;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(225, 322);
            Controls.Add(buttonClient);
            Controls.Add(txtClient);
            Controls.Add(label1);
            Name = "Client";
            Text = "Client";
            Load += Client_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtClient;
        private Button buttonClient;
    }
}
