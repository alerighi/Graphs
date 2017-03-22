using System.Windows.Forms;

namespace Graph
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveWithNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addVertexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteVertexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEdgeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeGraphNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.randomizeWeightsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetColorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.algorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dijkstraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceVectorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dFSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tOPSORTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addVertex = new System.Windows.Forms.ToolStripButton();
            this.deleteVertex = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.edgeButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveImageDialog = new System.Windows.Forms.SaveFileDialog();
            this.transposeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sCCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.algorithmToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(12, 4, 0, 4);
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(2342, 46);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveWithNameToolStripMenuItem,
            this.exportImageToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 38);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.OnNewMenuItemClick);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.loadToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.loadToolStripMenuItem.Text = "Open";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.OnLoadMenuItemClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.OnSaveMenuItemClick);
            // 
            // saveWithNameToolStripMenuItem
            // 
            this.saveWithNameToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveWithNameToolStripMenuItem.Name = "saveWithNameToolStripMenuItem";
            this.saveWithNameToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.saveWithNameToolStripMenuItem.Text = "Save with name";
            this.saveWithNameToolStripMenuItem.Click += new System.EventHandler(this.OnSaveWithNameMenuItemClick);
            // 
            // exportImageToolStripMenuItem
            // 
            this.exportImageToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exportImageToolStripMenuItem.Name = "exportImageToolStripMenuItem";
            this.exportImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportImageToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.exportImageToolStripMenuItem.Text = "Export image";
            this.exportImageToolStripMenuItem.Click += new System.EventHandler(this.OnExportImageMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(351, 38);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClick);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVertexToolStripMenuItem,
            this.deleteVertexToolStripMenuItem,
            this.addEdgeToolStripMenuItem,
            this.deleteEdgeToolStripMenuItem,
            this.changeGraphNameToolStripMenuItem,
            this.randomizeWeightsToolStripMenuItem,
            this.resetColorsToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(67, 38);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // addVertexToolStripMenuItem
            // 
            this.addVertexToolStripMenuItem.Name = "addVertexToolStripMenuItem";
            this.addVertexToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.addVertexToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.addVertexToolStripMenuItem.Text = "Add Vertex";
            this.addVertexToolStripMenuItem.Click += new System.EventHandler(this.OnAddVertexButtonClick);
            // 
            // deleteVertexToolStripMenuItem
            // 
            this.deleteVertexToolStripMenuItem.Name = "deleteVertexToolStripMenuItem";
            this.deleteVertexToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.deleteVertexToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.deleteVertexToolStripMenuItem.Text = "Delete Vertex";
            this.deleteVertexToolStripMenuItem.Click += new System.EventHandler(this.OnDeleteVertexButtonClick);
            // 
            // addEdgeToolStripMenuItem
            // 
            this.addEdgeToolStripMenuItem.Name = "addEdgeToolStripMenuItem";
            this.addEdgeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.A)));
            this.addEdgeToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.addEdgeToolStripMenuItem.Text = "Add Edge";
            this.addEdgeToolStripMenuItem.Click += new System.EventHandler(this.OnAddEdgeButtonClick);
            // 
            // deleteEdgeToolStripMenuItem
            // 
            this.deleteEdgeToolStripMenuItem.Name = "deleteEdgeToolStripMenuItem";
            this.deleteEdgeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D)));
            this.deleteEdgeToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.deleteEdgeToolStripMenuItem.Text = "Delete Edge";
            this.deleteEdgeToolStripMenuItem.Click += new System.EventHandler(this.OnRemoveEdgeButtonClick);
            // 
            // changeGraphNameToolStripMenuItem
            // 
            this.changeGraphNameToolStripMenuItem.Name = "changeGraphNameToolStripMenuItem";
            this.changeGraphNameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.N)));
            this.changeGraphNameToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.changeGraphNameToolStripMenuItem.Text = "Change graph name";
            this.changeGraphNameToolStripMenuItem.Click += new System.EventHandler(this.OnChangeGraphNameMenuItemClick);
            // 
            // randomizeWeightsToolStripMenuItem
            // 
            this.randomizeWeightsToolStripMenuItem.Name = "randomizeWeightsToolStripMenuItem";
            this.randomizeWeightsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.randomizeWeightsToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.randomizeWeightsToolStripMenuItem.Text = "Randomize weights";
            this.randomizeWeightsToolStripMenuItem.Click += new System.EventHandler(this.OnRandomizeWeightsMenuItemClick);
            // 
            // resetColorsToolStripMenuItem
            // 
            this.resetColorsToolStripMenuItem.Name = "resetColorsToolStripMenuItem";
            this.resetColorsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.R)));
            this.resetColorsToolStripMenuItem.Size = new System.Drawing.Size(421, 38);
            this.resetColorsToolStripMenuItem.Text = "Reset colors";
            this.resetColorsToolStripMenuItem.Click += new System.EventHandler(this.OnResetColorsMenuItemClick);
            // 
            // algorithmToolStripMenuItem
            // 
            this.algorithmToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dijkstraToolStripMenuItem,
            this.distanceVectorToolStripMenuItem,
            this.dFSToolStripMenuItem,
            this.tOPSORTToolStripMenuItem,
            this.transposeToolStripMenuItem,
            this.sCCToolStripMenuItem});
            this.algorithmToolStripMenuItem.Name = "algorithmToolStripMenuItem";
            this.algorithmToolStripMenuItem.Size = new System.Drawing.Size(140, 38);
            this.algorithmToolStripMenuItem.Text = "Algorithm ";
            // 
            // dijkstraToolStripMenuItem
            // 
            this.dijkstraToolStripMenuItem.Name = "dijkstraToolStripMenuItem";
            this.dijkstraToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.D)));
            this.dijkstraToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.dijkstraToolStripMenuItem.Text = "Dijkstra";
            this.dijkstraToolStripMenuItem.Click += new System.EventHandler(this.OnDijkstraMenuItemClick);
            // 
            // distanceVectorToolStripMenuItem
            // 
            this.distanceVectorToolStripMenuItem.Name = "distanceVectorToolStripMenuItem";
            this.distanceVectorToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.distanceVectorToolStripMenuItem.Text = "Distance Vector";
            this.distanceVectorToolStripMenuItem.Click += new System.EventHandler(this.OnDistanceVectorMenuItemClick);
            // 
            // dFSToolStripMenuItem
            // 
            this.dFSToolStripMenuItem.Name = "dFSToolStripMenuItem";
            this.dFSToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.dFSToolStripMenuItem.Text = "DFS";
            this.dFSToolStripMenuItem.Click += new System.EventHandler(this.dFSToolStripMenuItem_Click);
            // 
            // tOPSORTToolStripMenuItem
            // 
            this.tOPSORTToolStripMenuItem.Name = "tOPSORTToolStripMenuItem";
            this.tOPSORTToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.tOPSORTToolStripMenuItem.Text = "TOP SORT";
            this.tOPSORTToolStripMenuItem.Click += new System.EventHandler(this.OnTopSortMenuItemClick);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(77, 38);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(220, 38);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.OnAboutMenuItemClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(840, 423);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addVertex,
            this.deleteVertex,
            this.toolStripSeparator1,
            this.edgeButton,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 46);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip1.Size = new System.Drawing.Size(2342, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addVertex
            // 
            this.addVertex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addVertex.Image = ((System.Drawing.Image)(resources.GetObject("addVertex.Image")));
            this.addVertex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addVertex.Name = "addVertex";
            this.addVertex.Size = new System.Drawing.Size(135, 36);
            this.addVertex.Text = "Add Vertex";
            this.addVertex.ToolTipText = "Add";
            this.addVertex.Click += new System.EventHandler(this.OnAddVertexButtonClick);
            // 
            // deleteVertex
            // 
            this.deleteVertex.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteVertex.Image = ((System.Drawing.Image)(resources.GetObject("deleteVertex.Image")));
            this.deleteVertex.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteVertex.Name = "deleteVertex";
            this.deleteVertex.Size = new System.Drawing.Size(162, 36);
            this.deleteVertex.Text = "Delete Vertex";
            this.deleteVertex.Click += new System.EventHandler(this.OnDeleteVertexButtonClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // edgeButton
            // 
            this.edgeButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.edgeButton.Image = ((System.Drawing.Image)(resources.GetObject("edgeButton.Image")));
            this.edgeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edgeButton.Name = "edgeButton";
            this.edgeButton.Size = new System.Drawing.Size(122, 36);
            this.edgeButton.Text = "Add Edge";
            this.edgeButton.Click += new System.EventHandler(this.OnAddEdgeButtonClick);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(149, 36);
            this.toolStripButton1.Text = "Delete Edge";
            this.toolStripButton1.Click += new System.EventHandler(this.OnRemoveEdgeButtonClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1273);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 28, 0);
            this.statusStrip1.Size = new System.Drawing.Size(2342, 37);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(79, 32);
            this.status.Text = "Graph";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 85);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2342, 1188);
            this.panel1.TabIndex = 4;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "graph";
            this.openFileDialog1.Filter = "Graph Files|* .graph|Txt Files|*.txt|All Files|*.*";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "graph";
            this.saveFileDialog1.Filter = "Graph Files|* .graph|Txt Files|*.txt|All Files|*.*";
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // saveImageDialog
            // 
            this.saveImageDialog.DefaultExt = "jpeg";
            this.saveImageDialog.Filter = "Jpeg|*.jpeg|PNG|*.png|Gif|*.gif|Bitmap|*.bmp";
            // 
            // transposeToolStripMenuItem
            // 
            this.transposeToolStripMenuItem.Name = "transposeToolStripMenuItem";
            this.transposeToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.transposeToolStripMenuItem.Text = "Transpose";
            this.transposeToolStripMenuItem.Click += new System.EventHandler(this.transposeToolStripMenuItem_Click);
            // 
            // sCCToolStripMenuItem
            // 
            this.sCCToolStripMenuItem.Name = "sCCToolStripMenuItem";
            this.sCCToolStripMenuItem.Size = new System.Drawing.Size(348, 38);
            this.sCCToolStripMenuItem.Text = "SCC";
            this.sCCToolStripMenuItem.Click += new System.EventHandler(this.sCCToolStripMenuItem_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2342, 1310);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainWindow";
            this.Text = "Graphs";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem algorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addVertex;
        private System.Windows.Forms.ToolStripButton deleteVertex;
        private System.Windows.Forms.ToolStripButton edgeButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.Panel panel1;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
        private ToolStripMenuItem newToolStripMenuItem;
        private ToolStripMenuItem addVertexToolStripMenuItem;
        private ToolStripMenuItem deleteVertexToolStripMenuItem;
        private ToolStripMenuItem addEdgeToolStripMenuItem;
        private ToolStripMenuItem deleteEdgeToolStripMenuItem;
        private ToolStripMenuItem dijkstraToolStripMenuItem;
        private ToolStripMenuItem changeGraphNameToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripButton1;
        private ToolStripMenuItem randomizeWeightsToolStripMenuItem;
        private ToolStripMenuItem resetColorsToolStripMenuItem;
        private ToolStripMenuItem saveWithNameToolStripMenuItem;
        private ToolStripMenuItem exportImageToolStripMenuItem;
        private SaveFileDialog saveImageDialog;
        private ToolStripMenuItem distanceVectorToolStripMenuItem;
        private ToolStripMenuItem dFSToolStripMenuItem;
        private ToolStripMenuItem tOPSORTToolStripMenuItem;
        private ToolStripMenuItem transposeToolStripMenuItem;
        private ToolStripMenuItem sCCToolStripMenuItem;
    }
}