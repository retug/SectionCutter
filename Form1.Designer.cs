﻿
namespace SectionCutter
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.getSelNodeBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.vectorX = new System.Windows.Forms.TextBox();
            this.vectorY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.LoadCaseComBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.NumSlices = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.dataGridView3 = new System.Windows.Forms.DataGridView();
            this.runAnalysisBtn = new System.Windows.Forms.Button();
            this.shearScatterPlot = new LiveCharts.WinForms.CartesianChart();
            this.momentScatterPlot = new LiveCharts.WinForms.CartesianChart();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.locationPlot = new LiveCharts.WinForms.CartesianChart();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.all_Other_Units = new System.Windows.Forms.RadioButton();
            this.US_Units = new System.Windows.Forms.RadioButton();
            this.Opening_Label = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Area_Label = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.listBoxLoadSteps = new System.Windows.Forms.ListBox();
            this.listBoxResultSelected = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // getSelNodeBtn
            // 
            this.getSelNodeBtn.Location = new System.Drawing.Point(33, 22);
            this.getSelNodeBtn.Name = "getSelNodeBtn";
            this.getSelNodeBtn.Size = new System.Drawing.Size(136, 31);
            this.getSelNodeBtn.TabIndex = 0;
            this.getSelNodeBtn.Text = "Get Start Node";
            this.getSelNodeBtn.UseVisualStyleBackColor = true;
            this.getSelNodeBtn.Click += new System.EventHandler(this.getSelNodeBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(233, 57);
            this.dataGridView1.TabIndex = 1;
            // 
            // vectorX
            // 
            this.vectorX.Location = new System.Drawing.Point(8, 42);
            this.vectorX.Name = "vectorX";
            this.vectorX.Size = new System.Drawing.Size(100, 20);
            this.vectorX.TabIndex = 2;
            this.vectorX.Text = "Enter X Value";
            this.vectorX.Enter += new System.EventHandler(this.vectorX_Enter);
            this.vectorX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vectorX_KeyPress);
            this.vectorX.Leave += new System.EventHandler(this.vectorX_TextChanged);
            // 
            // vectorY
            // 
            this.vectorY.Location = new System.Drawing.Point(125, 42);
            this.vectorY.Name = "vectorY";
            this.vectorY.Size = new System.Drawing.Size(100, 20);
            this.vectorY.TabIndex = 3;
            this.vectorY.Text = "Enter Y Value";
            this.vectorY.Enter += new System.EventHandler(this.vectorY_Enter);
            this.vectorY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vectorX_KeyPress);
            this.vectorY.Leave += new System.EventHandler(this.vectorY_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 566);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "Section Cut Plot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y Vector";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.vectorY);
            this.groupBox1.Controls.Add(this.vectorX);
            this.groupBox1.Location = new System.Drawing.Point(532, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 71);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input Vector";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "X Vector";
            // 
            // LoadCaseComBox
            // 
            this.LoadCaseComBox.FormattingEnabled = true;
            this.LoadCaseComBox.Location = new System.Drawing.Point(802, 63);
            this.LoadCaseComBox.Name = "LoadCaseComBox";
            this.LoadCaseComBox.Size = new System.Drawing.Size(151, 21);
            this.LoadCaseComBox.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(802, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 25);
            this.button1.TabIndex = 8;
            this.button1.Text = "Gather Load Cases";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ShowLoadCase_Load);
            // 
            // NumSlices
            // 
            this.NumSlices.Location = new System.Drawing.Point(15, 30);
            this.NumSlices.Name = "NumSlices";
            this.NumSlices.Size = new System.Drawing.Size(154, 20);
            this.NumSlices.TabIndex = 9;
            this.NumSlices.Text = "Number of Sections";
            this.NumSlices.Enter += new System.EventHandler(this.NumSlices_Enter);
            this.NumSlices.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.numSlices_KeyPress);
            this.NumSlices.Leave += new System.EventHandler(this.NumSlices_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.NumSlices);
            this.groupBox2.Location = new System.Drawing.Point(991, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(189, 60);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Number of Sections";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 22);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 31);
            this.button2.TabIndex = 0;
            this.button2.Text = "Get Areas";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.getSelAreas_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(275, 64);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(219, 57);
            this.dataGridView2.TabIndex = 1;
            // 
            // dataGridView3
            // 
            this.dataGridView3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView3.Location = new System.Drawing.Point(603, 610);
            this.dataGridView3.Name = "dataGridView3";
            this.dataGridView3.Size = new System.Drawing.Size(544, 186);
            this.dataGridView3.TabIndex = 11;
            // 
            // runAnalysisBtn
            // 
            this.runAnalysisBtn.BackColor = System.Drawing.Color.Transparent;
            this.runAnalysisBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.runAnalysisBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runAnalysisBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(140)))), ((int)(((byte)(105)))));
            this.runAnalysisBtn.Location = new System.Drawing.Point(802, 99);
            this.runAnalysisBtn.Name = "runAnalysisBtn";
            this.runAnalysisBtn.Size = new System.Drawing.Size(151, 36);
            this.runAnalysisBtn.TabIndex = 12;
            this.runAnalysisBtn.Text = "Run Analysis";
            this.runAnalysisBtn.UseVisualStyleBackColor = false;
            this.runAnalysisBtn.Click += new System.EventHandler(this.runAnalysis_Click);
            // 
            // shearScatterPlot
            // 
            this.shearScatterPlot.Location = new System.Drawing.Point(15, 35);
            this.shearScatterPlot.Name = "shearScatterPlot";
            this.shearScatterPlot.Size = new System.Drawing.Size(1132, 262);
            this.shearScatterPlot.TabIndex = 14;
            this.shearScatterPlot.Text = "Units";
            this.shearScatterPlot.Visible = false;
            // 
            // momentScatterPlot
            // 
            this.momentScatterPlot.Location = new System.Drawing.Point(15, 300);
            this.momentScatterPlot.Name = "momentScatterPlot";
            this.momentScatterPlot.Size = new System.Drawing.Size(1132, 265);
            this.momentScatterPlot.TabIndex = 14;
            this.momentScatterPlot.Text = "cartesianChart1";
            this.momentScatterPlot.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.listBoxResultSelected);
            this.groupBox3.Controls.Add(this.locationPlot);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.momentScatterPlot);
            this.groupBox3.Controls.Add(this.shearScatterPlot);
            this.groupBox3.Controls.Add(this.dataGridView3);
            this.groupBox3.Location = new System.Drawing.Point(33, 141);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1162, 802);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Results";
            // 
            // locationPlot
            // 
            this.locationPlot.Location = new System.Drawing.Point(15, 591);
            this.locationPlot.Name = "locationPlot";
            this.locationPlot.Size = new System.Drawing.Size(582, 211);
            this.locationPlot.TabIndex = 14;
            this.locationPlot.Text = "Location Plot";
            this.locationPlot.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(38, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Shear Results";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(38, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Moment Results";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Underline);
            this.label5.Location = new System.Drawing.Point(600, 566);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "Tabular Data";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.all_Other_Units);
            this.groupBox4.Controls.Add(this.US_Units);
            this.groupBox4.Location = new System.Drawing.Point(533, 89);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(240, 46);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Units";
            // 
            // all_Other_Units
            // 
            this.all_Other_Units.AutoSize = true;
            this.all_Other_Units.Location = new System.Drawing.Point(129, 19);
            this.all_Other_Units.Name = "all_Other_Units";
            this.all_Other_Units.Size = new System.Drawing.Size(54, 17);
            this.all_Other_Units.TabIndex = 0;
            this.all_Other_Units.Text = "kN, M";
            this.all_Other_Units.UseVisualStyleBackColor = true;
            this.all_Other_Units.CheckedChanged += new System.EventHandler(this.all_Other_Units_CheckedChanged);
            // 
            // US_Units
            // 
            this.US_Units.AutoSize = true;
            this.US_Units.Checked = true;
            this.US_Units.Location = new System.Drawing.Point(25, 19);
            this.US_Units.Name = "US_Units";
            this.US_Units.Size = new System.Drawing.Size(55, 17);
            this.US_Units.TabIndex = 0;
            this.US_Units.TabStop = true;
            this.US_Units.Text = "Kip, Ft";
            this.US_Units.UseVisualStyleBackColor = true;
            this.US_Units.CheckedChanged += new System.EventHandler(this.US_Units_CheckedChanged);
            // 
            // Opening_Label
            // 
            this.Opening_Label.AutoSize = true;
            this.Opening_Label.Location = new System.Drawing.Point(280, 129);
            this.Opening_Label.Name = "Opening_Label";
            this.Opening_Label.Size = new System.Drawing.Size(41, 13);
            this.Opening_Label.TabIndex = 17;
            this.Opening_Label.Text = "#Areas";
            this.Opening_Label.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(321, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Openings";
            // 
            // Area_Label
            // 
            this.Area_Label.AutoSize = true;
            this.Area_Label.Location = new System.Drawing.Point(381, 129);
            this.Area_Label.Name = "Area_Label";
            this.Area_Label.Size = new System.Drawing.Size(37, 13);
            this.Area_Label.TabIndex = 17;
            this.Area_Label.Text = "#Floor";
            this.Area_Label.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(422, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Areas";
            // 
            // listBoxLoadSteps
            // 
            this.listBoxLoadSteps.FormattingEnabled = true;
            this.listBoxLoadSteps.Location = new System.Drawing.Point(991, 89);
            this.listBoxLoadSteps.Name = "listBoxLoadSteps";
            this.listBoxLoadSteps.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBoxLoadSteps.Size = new System.Drawing.Size(189, 56);
            this.listBoxLoadSteps.TabIndex = 19;
            // 
            // listBoxResultSelected
            // 
            this.listBoxResultSelected.FormattingEnabled = true;
            this.listBoxResultSelected.Location = new System.Drawing.Point(880, 571);
            this.listBoxResultSelected.Name = "listBoxResultSelected";
            this.listBoxResultSelected.Size = new System.Drawing.Size(267, 30);
            this.listBoxResultSelected.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1353, 982);
            this.Controls.Add(this.listBoxLoadSteps);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.Area_Label);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Opening_Label);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.runAnalysisBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LoadCaseComBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.getSelNodeBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Section Cut Tool";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getSelNodeBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox vectorX;
        private System.Windows.Forms.TextBox vectorY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox LoadCaseComBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox NumSlices;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView3;
        private System.Windows.Forms.Button runAnalysisBtn;
        private LiveCharts.WinForms.CartesianChart shearScatterPlot;
        private LiveCharts.WinForms.CartesianChart momentScatterPlot;
        private System.Windows.Forms.GroupBox groupBox3;
        private LiveCharts.WinForms.CartesianChart locationPlot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton all_Other_Units;
        private System.Windows.Forms.RadioButton US_Units;
        private System.Windows.Forms.Label Opening_Label;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label Area_Label;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox listBoxLoadSteps;
        private System.Windows.Forms.ListBox listBoxResultSelected;
    }
}