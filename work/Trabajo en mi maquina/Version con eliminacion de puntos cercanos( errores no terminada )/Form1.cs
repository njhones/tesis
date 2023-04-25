using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using BigNumbers;

namespace Growth_Polygon
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lY;
		private System.Windows.Forms.Label lX;
		private System.Windows.Forms.Button btnCerrar;
		private System.Windows.Forms.Button btnResetParallel;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.NumericUpDown nudDist;
		private System.Windows.Forms.Button btnParallel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudY;
		private System.Windows.Forms.NumericUpDown nudX;
		private System.Windows.Forms.PictureBox pbMapa;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#region atributos
		private ArrayList line;
		private ArrayList line1;
		private ArrayList[] parallel;
		Polygonal polygonal;
		private L_Point Last,Next;
		#endregion
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lY = new System.Windows.Forms.Label();
			this.lX = new System.Windows.Forms.Label();
			this.btnCerrar = new System.Windows.Forms.Button();
			this.btnResetParallel = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.nudDist = new System.Windows.Forms.NumericUpDown();
			this.btnParallel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.nudY = new System.Windows.Forms.NumericUpDown();
			this.nudX = new System.Windows.Forms.NumericUpDown();
			this.pbMapa = new System.Windows.Forms.PictureBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.nudDist)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
			this.SuspendLayout();
			// 
			// lY
			// 
			this.lY.Location = new System.Drawing.Point(436, 355);
			this.lY.Name = "lY";
			this.lY.Size = new System.Drawing.Size(48, 23);
			this.lY.TabIndex = 29;
			// 
			// lX
			// 
			this.lX.Location = new System.Drawing.Point(380, 355);
			this.lX.Name = "lX";
			this.lX.Size = new System.Drawing.Size(48, 23);
			this.lX.TabIndex = 28;
			// 
			// btnCerrar
			// 
			this.btnCerrar.Location = new System.Drawing.Point(396, 147);
			this.btnCerrar.Name = "btnCerrar";
			this.btnCerrar.Size = new System.Drawing.Size(72, 24);
			this.btnCerrar.TabIndex = 27;
			this.btnCerrar.Text = "Cerrar";
			this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
			// 
			// btnResetParallel
			// 
			this.btnResetParallel.Location = new System.Drawing.Point(388, 315);
			this.btnResetParallel.Name = "btnResetParallel";
			this.btnResetParallel.Size = new System.Drawing.Size(96, 23);
			this.btnResetParallel.TabIndex = 26;
			this.btnResetParallel.Text = "Borrar Paralela";
			this.btnResetParallel.Click += new System.EventHandler(this.btnResetParallel_Click);
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(396, 283);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 25;
			this.btnReset.Text = "Borrar Todo";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// nudDist
			// 
			this.nudDist.Location = new System.Drawing.Point(436, 211);
			this.nudDist.Maximum = new System.Decimal(new int[] {
																	200,
																	0,
																	0,
																	0});
			this.nudDist.Minimum = new System.Decimal(new int[] {
																	200,
																	0,
																	0,
																	-2147483648});
			this.nudDist.Name = "nudDist";
			this.nudDist.Size = new System.Drawing.Size(48, 20);
			this.nudDist.TabIndex = 22;
			// 
			// btnParallel
			// 
			this.btnParallel.Location = new System.Drawing.Point(396, 243);
			this.btnParallel.Name = "btnParallel";
			this.btnParallel.Size = new System.Drawing.Size(80, 32);
			this.btnParallel.TabIndex = 24;
			this.btnParallel.Text = "Trazar Paralela";
			this.btnParallel.Click += new System.EventHandler(this.btnParallel_Click);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(388, 211);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 23);
			this.label4.TabIndex = 23;
			this.label4.Text = "Distancia";
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(396, 115);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(72, 24);
			this.btnOk.TabIndex = 21;
			this.btnOk.Text = "Aceptar";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(388, 27);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 23);
			this.label3.TabIndex = 20;
			this.label3.Text = "Adicionar Vertice";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(388, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 23);
			this.label2.TabIndex = 19;
			this.label2.Text = "Cord Y";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(388, 59);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 23);
			this.label1.TabIndex = 18;
			this.label1.Text = "Cord X";
			// 
			// nudY
			// 
			this.nudY.Location = new System.Drawing.Point(436, 83);
			this.nudY.Maximum = new System.Decimal(new int[] {
																 380,
																 0,
																 0,
																 0});
			this.nudY.Minimum = new System.Decimal(new int[] {
																 20,
																 0,
																 0,
																 0});
			this.nudY.Name = "nudY";
			this.nudY.Size = new System.Drawing.Size(48, 20);
			this.nudY.TabIndex = 17;
			this.nudY.Value = new System.Decimal(new int[] {
															   20,
															   0,
															   0,
															   0});
			// 
			// nudX
			// 
			this.nudX.Location = new System.Drawing.Point(436, 59);
			this.nudX.Maximum = new System.Decimal(new int[] {
																 380,
																 0,
																 0,
																 0});
			this.nudX.Minimum = new System.Decimal(new int[] {
																 20,
																 0,
																 0,
																 0});
			this.nudX.Name = "nudX";
			this.nudX.Size = new System.Drawing.Size(48, 20);
			this.nudX.TabIndex = 16;
			this.nudX.Value = new System.Decimal(new int[] {
															   20,
															   0,
															   0,
															   0});
			// 
			// pbMapa
			// 
			this.pbMapa.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.pbMapa.Location = new System.Drawing.Point(-20, 3);
			this.pbMapa.Name = "pbMapa";
			this.pbMapa.Size = new System.Drawing.Size(396, 384);
			this.pbMapa.TabIndex = 15;
			this.pbMapa.TabStop = false;
			this.pbMapa.Paint += new System.Windows.Forms.PaintEventHandler(this.pbMapa_Paint);
			this.pbMapa.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMapa_MouseMove);
			this.pbMapa.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbMapa_MouseDown);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(392, 180);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(40, 24);
			this.btnSave.TabIndex = 30;
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(440, 179);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(40, 24);
			this.btnLoad.TabIndex = 31;
			this.btnLoad.Text = "Load";
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.CheckPathExists = false;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 390);
			this.Controls.Add(this.btnLoad);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lY);
			this.Controls.Add(this.lX);
			this.Controls.Add(this.btnCerrar);
			this.Controls.Add(this.btnResetParallel);
			this.Controls.Add(this.btnReset);
			this.Controls.Add(this.nudDist);
			this.Controls.Add(this.btnParallel);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudY);
			this.Controls.Add(this.nudX);
			this.Controls.Add(this.pbMapa);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudDist)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}
		private void Form1_Load(object sender, System.EventArgs e)
		{
			Next = Last = null;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			/*L_Point Pt= new L_Point();
			Pt.x = Decimal.ToInt32(this.nudX.Value);
			Pt.y = Decimal.ToInt32(this.nudY.Value);
			line.Add(Pt);*/

			#region ej12
			/*L_Point Pt;
			Pt= new L_Point(20,380);
			line.Add(Pt);
			Pt= new L_Point(20,20);
			line.Add(Pt);
			Pt= new L_Point(320,20);
			line.Add(Pt);
			Pt= new L_Point(320,380);
			line.Add(Pt);
			Pt= new L_Point(120,380);
			line.Add(Pt);
			Pt= new L_Point(120,280);
			line.Add(Pt);
			Pt= new L_Point(270,260);
			line.Add(Pt);
			Pt= new L_Point(120,240);
			line.Add(Pt);
			Pt= new L_Point(200,260);
			line.Add(Pt);
			Pt= new L_Point(190,252);
			line.Add(Pt);
			Pt= new L_Point(215,265);
			line.Add(Pt);
			Pt= new L_Point(120,265);
			line.Add(Pt);
			Pt= new L_Point(120,270);
			line.Add(Pt);
			Pt= new L_Point(80,270);
			line.Add(Pt);
			Pt= new L_Point(80,180);
			line.Add(Pt);
			Pt= new L_Point(290,180);
			line.Add(Pt);
			Pt= new L_Point(290,295);
			line.Add(Pt);
			Pt= new L_Point(170,295);
			line.Add(Pt);
			Pt= new L_Point(170,370);
			line.Add(Pt);
			Pt= new L_Point(300,370);
			line.Add(Pt);
			Pt= new L_Point(300,120);
			line.Add(Pt);
			Pt= new L_Point(50,120);
			line.Add(Pt);
			Pt= new L_Point(50,380);
			line.Add(Pt);*/
			#endregion

			#region ej11
			/*L_Point Pt;
			Pt= new L_Point(20,380);
			line.Add(Pt);
			Pt= new L_Point(20,20);
			line.Add(Pt);
			Pt= new L_Point(320,20);
			line.Add(Pt);
			Pt= new L_Point(320,380);
			line.Add(Pt);
			Pt= new L_Point(120,380);
			line.Add(Pt);
			Pt= new L_Point(120,280);
			line.Add(Pt);
			Pt= new L_Point(270,270);
			line.Add(Pt);
			Pt= new L_Point(120,260);
			line.Add(Pt);
			Pt= new L_Point(180,270);
			line.Add(Pt);
			Pt= new L_Point(80,270);
			line.Add(Pt);
			Pt= new L_Point(80,180);
			line.Add(Pt);
			Pt= new L_Point(290,180);
			line.Add(Pt);
			Pt= new L_Point(290,295);
			line.Add(Pt);
			Pt= new L_Point(170,295);
			line.Add(Pt);
			Pt= new L_Point(170,370);
			line.Add(Pt);
			Pt= new L_Point(300,370);
			line.Add(Pt);
			Pt= new L_Point(300,120);
			line.Add(Pt);
			Pt= new L_Point(50,120);
			line.Add(Pt);
			Pt= new L_Point(50,380);
			line.Add(Pt);*/
			#endregion

			#region ej10
			/*L_Point Pt;
			Pt= new L_Point(20,380);
			line.Add(Pt);
			Pt= new L_Point(20,20);
			line.Add(Pt);
			Pt= new L_Point(320,20);
			line.Add(Pt);
			Pt= new L_Point(320,380);
			line.Add(Pt);
			Pt= new L_Point(120,380);
			line.Add(Pt);
			Pt= new L_Point(120,275);
			line.Add(Pt);
			Pt= new L_Point(270,270);
			line.Add(Pt);
			Pt= new L_Point(120,265);
			line.Add(Pt);
			Pt= new L_Point(120,180);
			line.Add(Pt);
			Pt= new L_Point(290,180);
			line.Add(Pt);
			Pt= new L_Point(290,295);
			line.Add(Pt);
			Pt= new L_Point(170,295);
			line.Add(Pt);
			Pt= new L_Point(170,370);
			line.Add(Pt);
			Pt= new L_Point(300,370);
			line.Add(Pt);
			Pt= new L_Point(300,120);
			line.Add(Pt);
			Pt= new L_Point(100,120);
			line.Add(Pt);
			Pt= new L_Point(100,380);
			line.Add(Pt);*/
			#endregion

			#region ej9
			/*L_Point Pt;
			Pt= new L_Point(205,200);
			line.Add(Pt);
			Pt= new L_Point(210,100);
			line.Add(Pt);
			Pt= new L_Point(210,255);
			line.Add(Pt);
			Pt= new L_Point(310,260);
			line.Add(Pt);
			Pt= new L_Point(155,260);
			line.Add(Pt);
			Pt= new L_Point(150,360);
			line.Add(Pt);
			Pt= new L_Point(150,205);
			line.Add(Pt);
			Pt= new L_Point(50,200);
			line.Add(Pt);*/
			#endregion
			
			#region ej8
			/*L_Point Pt;
			Pt= new L_Point(350,200);
			line.Add(Pt);
			Pt= new L_Point(250,205);
			line.Add(Pt);
			Pt= new L_Point(250,300);
			line.Add(Pt);
			Pt= new L_Point(150,300);
			line.Add(Pt);
			Pt= new L_Point(150,205);
			line.Add(Pt);
			Pt= new L_Point(50,200);
			line.Add(Pt);*/
			#endregion
			
			#region ej7
			/*L_Point Pt;
			Pt= new L_Point(250,200);
			line.Add(Pt);
			Pt= new L_Point(250,300);
			line.Add(Pt);
			Pt= new L_Point(150,300);
			line.Add(Pt);
			Pt= new L_Point(150,205);
			line.Add(Pt);
			Pt= new L_Point(50,200);
			line.Add(Pt);*/
			#endregion
			
			#region ej6
			/*L_Point Pt;
			Pt= new L_Point(20,380);
			line.Add(Pt);
			Pt= new L_Point(20,330);
			line.Add(Pt);
			Pt= new L_Point(68,330);
			line.Add(Pt);
			Pt= new L_Point(68,70);
			line.Add(Pt);
			Pt= new L_Point(20,70);
			line.Add(Pt);
			Pt= new L_Point(20,20);
			line.Add(Pt);
			Pt= new L_Point(70,20);
			line.Add(Pt);
			Pt= new L_Point(70,68);
			line.Add(Pt);
			Pt= new L_Point(330,68);
			line.Add(Pt);
			Pt= new L_Point(330,20);
			line.Add(Pt);
			Pt= new L_Point(380,20);
			line.Add(Pt);
			Pt= new L_Point(380,70);
			line.Add(Pt);
			Pt= new L_Point(332,70);
			line.Add(Pt);
			Pt= new L_Point(332,330);
			line.Add(Pt);
			Pt= new L_Point(380,330);
			line.Add(Pt);
			Pt= new L_Point(380,380);
			line.Add(Pt);
			Pt= new L_Point(330,380);
			line.Add(Pt);
			Pt= new L_Point(330,332);
			line.Add(Pt);
			Pt= new L_Point(70,332);
			line.Add(Pt);
			Pt= new L_Point(70,380);
			line.Add(Pt);*/
			#endregion
			
			#region ej5
			/*L_Point Pt;
			Pt= new L_Point(145,150);
			line.Add(Pt);
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(255,150);
			line.Add(Pt);
			Pt= new L_Point(210,120);
			line.Add(Pt);
			Pt= new L_Point(210,280);
			line.Add(Pt);
			Pt= new L_Point(255,250);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(145,250);
			line.Add(Pt);
			Pt= new L_Point(190,280);
			line.Add(Pt);
			Pt= new L_Point(190,120);
			line.Add(Pt);*/
			#endregion
						
			#region ej4_5
			/*L_Point Pt;
			Pt= new L_Point(150,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(300,200);
			line.Add(Pt);
			Pt= new L_Point(150,200);
			line.Add(Pt);*/
			#endregion
			
			#region ej4.2

			/*L_Point Pt;
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(100,200);
			line.Add(Pt);
			Pt= new L_Point(175,200);
			line.Add(Pt);
			Pt= new L_Point(145,175);
			line.Add(Pt);
			Pt= new L_Point(200,145);
			line.Add(Pt);
			Pt= new L_Point(255,175);
			line.Add(Pt);
			Pt= new L_Point(225,200);
			line.Add(Pt);
			Pt= new L_Point(300,200);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(210,300);
			line.Add(Pt);
			Pt= new L_Point(200,275);
			line.Add(Pt);
			Pt= new L_Point(190,300);
			line.Add(Pt);*/
			#endregion

			#region ej4.1

			/*L_Point Pt;
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(100,200);
			line.Add(Pt);
			Pt= new L_Point(150,200);
			line.Add(Pt);
			Pt= new L_Point(120,150);
			line.Add(Pt);
			Pt= new L_Point(200,120);
			line.Add(Pt);
			Pt= new L_Point(280,150);
			line.Add(Pt);
			Pt= new L_Point(250,200);
			line.Add(Pt);
			Pt= new L_Point(300,200);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);*/
			#endregion

			#region ej4

			/*L_Point Pt;
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(100,200);
			line.Add(Pt);
			Pt= new L_Point(300,200);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(250,300);
			line.Add(Pt);
			Pt= new L_Point(280,250);
			line.Add(Pt);
			Pt= new L_Point(200,220);
			line.Add(Pt);
			Pt= new L_Point(120,250);
			line.Add(Pt);
			Pt= new L_Point(150,300);
			line.Add(Pt);*/
			#endregion
			
			#region ej3.2
			/*L_Point Pt;
			Pt= new L_Point(150,350);
			line.Add(Pt);
			Pt= new L_Point(150,150);
			line.Add(Pt);
			Pt= new L_Point(350,150);
			line.Add(Pt);
			Pt= new L_Point(350,190);
			line.Add(Pt);
			Pt= new L_Point(270,250);
			line.Add(Pt);
			Pt= new L_Point(350,310);
			line.Add(Pt);
			Pt= new L_Point(350,350);
			line.Add(Pt);*/
			#endregion

			#region ej3.1
			/*L_Point Pt;
			Pt= new L_Point(150,350);
			line.Add(Pt);
			Pt= new L_Point(150,150);
			line.Add(Pt);
			Pt= new L_Point(250,150);
			line.Add(Pt);
			Pt= new L_Point(170,210);
			line.Add(Pt);
			Pt= new L_Point(350,210);
			line.Add(Pt);
			Pt= new L_Point(350,350);
			line.Add(Pt);*/
			#endregion

			#region ej3
			/*L_Point Pt;
			Pt= new L_Point(150,350);
			line.Add(Pt);
			Pt= new L_Point(150,150);
			line.Add(Pt);
			Pt= new L_Point(250,150);
			line.Add(Pt);
			Pt= new L_Point(160,200);
			line.Add(Pt);
			Pt= new L_Point(350,200);
			line.Add(Pt);
			Pt= new L_Point(350,350);
			line.Add(Pt);*/
			#endregion
            
			#region ej2
			/*L_Point Pt;
			Pt= new L_Point(220,230);
			line.Add(Pt);
			Pt= new L_Point(220,170);
			line.Add(Pt);
			Pt= new L_Point(200,170);
			line.Add(Pt);
			Pt= new L_Point(200,200);
			line.Add(Pt);
			Pt= new L_Point(20,200);
			line.Add(Pt);
			Pt= new L_Point(20,100);
			line.Add(Pt);
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(100,170);
			line.Add(Pt);
			Pt= new L_Point(150,170);
			line.Add(Pt);
			Pt= new L_Point(150,120);
			line.Add(Pt);
			Pt= new L_Point(300,120);
			line.Add(Pt);
			Pt= new L_Point(300,190);
			line.Add(Pt);
			Pt= new L_Point(250,190);
			line.Add(Pt);
			Pt= new L_Point(250,240);
			line.Add(Pt);
			Pt= new L_Point(280,240);
			line.Add(Pt);
			Pt= new L_Point(280,300);
			line.Add(Pt);
			Pt= new L_Point(150,300);
			line.Add(Pt);
			Pt= new L_Point(150,230);
			line.Add(Pt);*/
			#endregion
			//Dist -15
			
			#region EJ1
		/*	L_Point Pt;
			Pt= new L_Point(240,380);
			line.Add(Pt);
			Pt= new L_Point(220,380);
			line.Add(Pt);
			Pt= new L_Point(220,200);
			line.Add(Pt);
			Pt= new L_Point(200,200);
			line.Add(Pt);
			Pt= new L_Point(200,340);
			line.Add(Pt);
			Pt= new L_Point(180,340);
			line.Add(Pt);
			Pt= new L_Point(180,240);
			line.Add(Pt);
			Pt= new L_Point(20,240);
			line.Add(Pt);
			Pt= new L_Point(20,20);
			line.Add(Pt);
			Pt= new L_Point(380,20);
			line.Add(Pt);
			Pt= new L_Point(380,160);
			line.Add(Pt);
			Pt= new L_Point(240,160);
			line.Add(Pt);*/
			#endregion

			#region Ej0
			/*L_Point Pt;
			Pt= new L_Point(150,150);
			line.Add(Pt);
			Pt= new L_Point(200,150);
			line.Add(Pt);
			Pt= new L_Point(200,200);
			line.Add(Pt);
			Pt= new L_Point(150,200);
			line.Add(Pt);*/
			#endregion

			#region Ej-1
			/*L_Point Pt;
			Pt= new L_Point(150,250);
			line.Add(Pt);
			Pt= new L_Point(250,250);
			line.Add(Pt);
			Pt= new L_Point(250,290);
			line.Add(Pt);
			Pt= new L_Point(300,290);
			line.Add(Pt);
			Pt= new L_Point(300,310);
			line.Add(Pt);
			Pt= new L_Point(260,310);
			line.Add(Pt);
			Pt= new L_Point(260,350);
			line.Add(Pt);
			Pt= new L_Point(150,350);
			line.Add(Pt);*/
			
			#endregion

			#region ej-2

			/*L_Point Pt;
			Pt= new L_Point(125,250);
			line.Add(Pt);
			Pt= new L_Point(125,150);
			line.Add(Pt);
			Pt= new L_Point(200,150);
			line.Add(Pt);
			Pt= new L_Point(200,200);
			line.Add(Pt);
			Pt= new L_Point(250,200);
			line.Add(Pt);
			Pt= new L_Point(250,150);
			line.Add(Pt);
			Pt= new L_Point(325,150);
			line.Add(Pt);
			Pt= new L_Point(325,250);
			line.Add(Pt);*/
			

			#endregion

			#region ej-3

			/*L_Point Pt;
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(300,200);
			line.Add(Pt);
			Pt= new L_Point(228,190);
			line.Add(Pt);
			Pt= new L_Point(225,220);
			line.Add(Pt);
			Pt= new L_Point(222,200);
			line.Add(Pt);
			Pt= new L_Point(150,210);
			line.Add(Pt);
			Pt= new L_Point(150,250);
			line.Add(Pt);
			Pt= new L_Point(300,250);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);*/
			

			#endregion

			#region ej-4

			L_Point Pt;
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(300,210);
			line.Add(Pt);
			Pt= new L_Point(228,200);
			line.Add(Pt);
			Pt= new L_Point(225,220);
			line.Add(Pt);
			Pt= new L_Point(222,200);
			line.Add(Pt);
			Pt= new L_Point(150,210);
			line.Add(Pt);
			Pt= new L_Point(150,250);
			line.Add(Pt);
			Pt= new L_Point(300,250);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);
			

			#endregion

			#region ej-5

			/*L_Point Pt;
			Pt= new L_Point(100,100);
			line.Add(Pt);
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(300,210);
			line.Add(Pt);
			Pt= new L_Point(228,200);
			line.Add(Pt);
			Pt= new L_Point(225,220);
			line.Add(Pt);
			Pt= new L_Point(222,190);
			line.Add(Pt);
			Pt= new L_Point(150,200);
			line.Add(Pt);
			Pt= new L_Point(150,250);
			line.Add(Pt);
			Pt= new L_Point(300,250);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);*/
			

			#endregion

			#region ej-6

			/*L_Point Pt;
			Pt= new L_Point(300,100);
			line.Add(Pt);
			Pt= new L_Point(300,300);
			line.Add(Pt);
			Pt= new L_Point(250,300);
			line.Add(Pt);
			Pt= new L_Point(250,150);
			line.Add(Pt);
			Pt= new L_Point(210,150);
			line.Add(Pt);
			Pt= new L_Point(200,222);
			line.Add(Pt);
			Pt= new L_Point(220,225);
			line.Add(Pt);
			Pt= new L_Point(200,228);
			line.Add(Pt);
			Pt= new L_Point(210,300);
			line.Add(Pt);
			Pt= new L_Point(100,300);
			line.Add(Pt);
			Pt= new L_Point(100,100);
			line.Add(Pt);*/
			

			#endregion
			
			
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void pbMapa_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			// Create a local version of the graphics object for the PictureBox.
			Graphics g = e.Graphics;

			// Draw a string on the PictureBox.
			//g.DrawString("This is a diagonal line drawn on the control",
			//new Font("Arial",10), System.Drawing.Brushes.Blue, new Point(30,30));
			
			// Draw a line in the PictureBox.
			
			if(line==null) line = new ArrayList();
			for(int i=1; i<line.Count; i++)
			{
				g.DrawLine(System.Drawing.Pens.Black, 
					((L_Point)line[i-1]).x, 400-((L_Point)line[i-1]).y,
					((L_Point)line[ i ]).x, 400-((L_Point)line[ i ]).y);
			}
			
			if(line1 != null) 
			{
				for(int i=1; i < line1.Count; i++)
				{
					g.DrawLine(System.Drawing.Pens.Red, 
						((L_Point)line1[i-1]).x, 400-((L_Point)line1[i-1]).y,
						((L_Point)line1[ i ]).x, 400-((L_Point)line1[ i ]).y);
				}
			}

			if( parallel != null )
			{
				for(int j=0;j<parallel.Length;j++)
				{
					if( (parallel != null) && ( parallel[j].Count > 0 ) )
					{
				
						for(int i=1; i<(parallel[j].Count); i++)
						{
							L_Point P1, P2;
							P1 = (L_Point)parallel[j][i-1];
							P2 = (L_Point)parallel[j][ i ];
							
							g.DrawLine(System.Drawing.Pens.Blue, 
								P1.x, 400 - P1.y, P2.x ,400 - P2.y );
						}
				
					}
				}
			}
			

			if((Last != null) && (Next != null))
			{
				g.DrawLine( System.Drawing.Pens.Black, 
					Last.x, 400-Last.y,
					Next.x, 400-Next.y);
			}
				
		}

		private void btnCerrar_Click(object sender, System.EventArgs e)
		{
			L_Point Pt = new L_Point( (L_Point )line[0] );
			
			line.Add(Pt);
			Next = null;
			Last = null;
			
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void btnParallel_Click(object sender, System.EventArgs e)
		{
			int i = 0;

			if( ( ((L_Point)line[0]).x == ((L_Point)line[line.Count-1]).x ) &&
				( ((L_Point)line[0]).y == ((L_Point)line[line.Count-1]).y ) )
			{
				polygonal = new Simple_Polygon(line);	
				Polygonal R = null;
				ArrayList List = ((Simple_Polygon)polygonal).Parallel(Decimal.ToInt32(this.nudDist.Value),out R);

				line1 = new ArrayList();
				for( i =0;i<R.CantOfVertex();i++)
				{
					line1.Add( new L_Point( R.Get_Vertex(i) ) );
				}
				if(line1.Count > 1) line1.Add( line1[0] );

				parallel = new ArrayList[List.Count];

				for( i =0; i< List.Count; i++ )
				{
					parallel[i] = new ArrayList();
					for( int j =0;j<((ArrayList)List[i]).Count;j++)
					{
						parallel[i].Add( new L_Point( (R_Point)((ArrayList)List[i])[j] ) );
					}
					if(parallel[i].Count > 0 ) parallel[i].Add( parallel[i][0] );
				}
			}
			else
			{
				polygonal = new Simple_Polygonal(line);	
				Polygonal R = null;
				ArrayList List = ((Simple_Polygonal)polygonal).Parallel(Decimal.ToInt32(this.nudDist.Value),out R);

				line1 = new ArrayList();
				for( i =0;i<R.CantOfVertex();i++)
				{
					line1.Add( new L_Point( R.Get_Vertex(i) ) );
				}
				if(line1.Count > 1) line1.Add( line1[0] );

				parallel = new ArrayList[List.Count];

				for( i =0; i< List.Count; i++ )
				{
					parallel[i] = new ArrayList();
					for( int j =0;j<((ArrayList)List[i]).Count;j++)
					{
						parallel[i].Add( new L_Point( (R_Point)((ArrayList)List[i])[j] ) );
					}
					if(parallel[i].Count > 0 )
                        parallel[i].Add( parallel[i][0] );
				}
			}
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void btnReset_Click(object sender, System.EventArgs e)
		{
			line = null;
			line1 = null;
			parallel = null;
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void btnResetParallel_Click(object sender, System.EventArgs e)
		{
			parallel = null;
			line1 = null;
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void pbMapa_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{			
			Last = Next;
			Next = new L_Point(e.X,400-e.Y);
			if(Last == null) Last = Next.Clone();
			
			if(line==null) line = new ArrayList();
				
			line.Add(Next.Clone());
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
			
		}

		private void pbMapa_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			lX.Text = e.X.ToString();
			lY.Text = (400-e.Y).ToString();

			if(Next != null)
			{
				Next.x = e.X;
				Next.y = 400-e.Y;
				this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
			}

		}

		private void btnSave_Click(object sender, System.EventArgs e)
		{			
			// Displays the OpenFileDialog.
			if (openFileDialog1.ShowDialog() == DialogResult.OK) 
			{
				
				// Read the file as one string.
				System.IO.StreamWriter myFile =
					new System.IO.StreamWriter(saveFileDialog1.FileName);
				myFile.WriteLine(line.Count.ToString());
				for(int i = 0; i<line.Count; i++)
				{
					myFile.WriteLine(((L_Point)line[i]).x.ToString());
					myFile.WriteLine(((L_Point)line[i]).y.ToString());
				}

				myFile.Close();					
			}

		
		}

		private void btnLoad_Click(object sender, System.EventArgs e)
		{
			// Displays the OpenFileDialog.
			if (openFileDialog1.ShowDialog() == DialogResult.OK) 
			{
				// Opens the file stream for the file selected by the user.
				using (System.IO.Stream userStream = openFileDialog1.OpenFile()) 
				{
					// Read the file as one string.
					System.IO.StreamReader myFile =
						new System.IO.StreamReader(openFileDialog1.FileName);
					line.Clear();
					int n = Convert.ToInt32( myFile.ReadLine() );
					for(int i = 0; i<n; i++)
					{
						L_Point aux = new L_Point();
						aux.x = Convert.ToInt64(myFile.ReadLine());
						aux.y = Convert.ToInt64(myFile.ReadLine());
						line.Add(aux);
					}

					myFile.Close();					
				}
				this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);				
			}			
		}

		
	}	
}
