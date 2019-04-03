namespace TempGetGUI_Raspberry
{
    partial class TempGet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TempGet));
            this.lbl_Temp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_current_duration = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_total_duration = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_current_cost = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_total_cost = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.myButton1 = new TempGetGUI_Raspberry.MyButton();
            this.SuspendLayout();
            // 
            // lbl_Temp
            // 
            resources.ApplyResources(this.lbl_Temp, "lbl_Temp");
            this.lbl_Temp.Name = "lbl_Temp";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.MediumOrchid;
            this.label3.Name = "label3";
            // 
            // lbl_current_duration
            // 
            resources.ApplyResources(this.lbl_current_duration, "lbl_current_duration");
            this.lbl_current_duration.ForeColor = System.Drawing.Color.MediumOrchid;
            this.lbl_current_duration.Name = "lbl_current_duration";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // lbl_total_duration
            // 
            resources.ApplyResources(this.lbl_total_duration, "lbl_total_duration");
            this.lbl_total_duration.Name = "lbl_total_duration";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // lbl_current_cost
            // 
            resources.ApplyResources(this.lbl_current_cost, "lbl_current_cost");
            this.lbl_current_cost.Name = "lbl_current_cost";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lbl_total_cost
            // 
            resources.ApplyResources(this.lbl_total_cost, "lbl_total_cost");
            this.lbl_total_cost.Name = "lbl_total_cost";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button1.FlatAppearance.BorderSize = 3;
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.DarkRed;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // myButton1
            // 
            this.myButton1.BackColor = System.Drawing.Color.White;
            this.myButton1.BackgroundImage = global::TempGetGUI_Raspberry.Resource1.power_off;
            resources.ApplyResources(this.myButton1, "myButton1");
            this.myButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.myButton1.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
            this.myButton1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.myButton1.Name = "myButton1";
            this.myButton1.UseVisualStyleBackColor = false;
            this.myButton1.Click += new System.EventHandler(this.myButton1_Click);
            // 
            // TempGet
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_total_cost);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbl_current_cost);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lbl_total_duration);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbl_current_duration);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.myButton1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Temp);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "TempGet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.TempGet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_Temp;
        private System.Windows.Forms.Label label1;
        private MyButton myButton1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_current_duration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_total_duration;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_current_cost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_total_cost;
        private System.Windows.Forms.Button button1;
    }
}



