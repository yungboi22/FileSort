
namespace FileSort
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Timer_Check = new System.Windows.Forms.Timer(this.components);
            this.timer_SortByMonths = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Timer_Check
            // 
            this.Timer_Check.Interval = 1;
            this.Timer_Check.Tick += new System.EventHandler(this.Timer_Check_Tick);
            // 
            // timer_SortByMonths
            // 
            this.timer_SortByMonths.Interval = 3600;
            this.timer_SortByMonths.Tick += new System.EventHandler(this.timer_SortByMonths_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 305);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer Timer_Check;
        private System.Windows.Forms.Timer timer_SortByMonths;
    }
}

