namespace PromotionAlert
{
  partial class Alert
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Alert));
      this.dataGridView1 = new System.Windows.Forms.DataGridView();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.listBox1 = new System.Windows.Forms.ListBox();
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.button1 = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
      this.SuspendLayout();
      // 
      // dataGridView1
      // 
      this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridView1.Location = new System.Drawing.Point(12, 50);
      this.dataGridView1.Name = "dataGridView1";
      this.dataGridView1.RowTemplate.Height = 28;
      this.dataGridView1.Size = new System.Drawing.Size(935, 401);
      this.dataGridView1.TabIndex = 0;
      // 
      // timer1
      // 
      this.timer1.Enabled = true;
      this.timer1.Interval = 60000;
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // listBox1
      // 
      this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
      this.listBox1.FormattingEnabled = true;
      this.listBox1.ItemHeight = 20;
      this.listBox1.Location = new System.Drawing.Point(12, 467);
      this.listBox1.Name = "listBox1";
      this.listBox1.Size = new System.Drawing.Size(935, 124);
      this.listBox1.TabIndex = 1;
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
      this.notifyIcon1.Text = "notifyIcon1";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(12, 10);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(98, 34);
      this.button1.TabIndex = 2;
      this.button1.Text = "Refresh";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // Alert
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(959, 600);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.listBox1);
      this.Controls.Add(this.dataGridView1);
      this.Name = "Alert";
      this.Text = "Form1";
      ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DataGridView dataGridView1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.Button button1;
  }
}

