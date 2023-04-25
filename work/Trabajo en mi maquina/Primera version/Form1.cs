using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Crecimiento
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
		private LtnLine line;
		private LtnLine line1;
		private LtnLine[] parallel;
		private int Cant;
		private LtnPoint3D Last,Next;
		#endregion

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
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 390);
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
			Cant = 0;
		}

		private void btnOk_Click(object sender, System.EventArgs e)
		{
			LtnPoint3D Pt= new LtnPoint3D();
			Pt.x = Decimal.ToInt32(this.nudX.Value);
			Pt.y = Decimal.ToInt32(this.nudY.Value);
			Pt.z = 0;
			line.AddVertex(Pt);

			#region ej12
			 /*LtnPoint3D Pt;
			Pt= new LtnPoint3D(20,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,280,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(270,260,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,240,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,260,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(190,252,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(215,265,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,265,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(80,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(80,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(50,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(50,380,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej11
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(20,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,280,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(270,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,260,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(180,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(80,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(80,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(50,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(50,380,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej10
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(20,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(320,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,275,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(270,270,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,265,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,180,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(290,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,295,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,370,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,380,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej9
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(50,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,205,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,360,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(155,260,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(310,260,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(210,255,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(210,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(205,200,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej8
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(50,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,205,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,205,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,200,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej7
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(50,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,205,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,200,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej6
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(20,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,330,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(68,330,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(68,70,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,70,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(70,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(70,68,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(330,68,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(330,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,70,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(332,70,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(332,330,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,330,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(330,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(330,332,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(70,332,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(70,380,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej5
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(145,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(255,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(210,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(210,280,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(255,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(145,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(190,280,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(190,120,0);
			line.AddVertex(Pt);*/
			#endregion
						
			#region ej4_5
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(150,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,200,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej4.2

			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(100,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(175,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(145,175,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,145,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(255,175,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(225,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(210,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,275,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(190,300,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej4.1

			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(100,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(280,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,300,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej4

			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(100,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(280,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,220,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(120,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,300,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region ej3.2
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(150,350,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,190,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(270,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,310,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,350,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej3.1
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(150,350,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(170,210,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,210,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,350,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej3
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(150,350,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(160,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(350,350,0);
			line.AddVertex(Pt);*/
			#endregion
            
			#region ej2
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(220,230,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(220,170,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,170,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,100,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(100,170,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,170,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,120,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,190,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,190,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,240,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(280,240,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(280,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,300,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,230,0);
			line.AddVertex(Pt);*/
			#endregion
			
			#region EJ1
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(240,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(220,380,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(220,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,340,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(180,340,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(180,240,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,240,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(20,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,20,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(380,160,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(240,160,0);
			line.AddVertex(Pt);*/
			#endregion

			#region Ej0
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(150,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,200,0);
			line.AddVertex(Pt);*/
			#endregion

			#region Ej-1
			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(300,290,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,290,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(150,350,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(260,350,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(260,310,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(300,310,0);
			line.AddVertex(Pt);*/
			#endregion

			#region ej-2

			/*LtnPoint3D Pt;
			Pt= new LtnPoint3D(125,250,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(125,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(200,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,200,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(250,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(325,150,0);
			line.AddVertex(Pt);
			Pt= new LtnPoint3D(325,250,0);
			line.AddVertex(Pt);*/
			

			#endregion
			//dist 95

			
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
			
			if(line==null) line = new LtnLine();
			for(int i=1; i<(line.CantOfVertex()); i++)
			{
				g.DrawLine(System.Drawing.Pens.Black, 
					line.getVertex(i-1).x, 400-line.getVertex(i-1).y,
					line.getVertex( i ).x, 400-line.getVertex( i ).y);
			}
			
			if(line1 != null) 
			{
				for(int i=1; i < line1.CantOfVertex(); i++)
				{
					g.DrawLine(System.Drawing.Pens.Red, 
						line1.getVertex(i-1).x, 400-line1.getVertex(i-1).y,
						line1.getVertex( i ).x, 400-line1.getVertex( i ).y);
				}
			}

			if( parallel != null )
			{
				for(int j=0;j<Cant;j++)
				{
					if( (parallel != null) && ( parallel[j].CantOfVertex() > 0 ) )
					{
				
						for(int i=1; i<(parallel[j].CantOfVertex()); i++)
						{
							g.DrawLine(System.Drawing.Pens.Blue, 
								parallel[j].getVertex(i-1).x, 400-parallel[j].getVertex(i-1).y,
								parallel[j].getVertex( i ).x, 400-parallel[j].getVertex( i ).y);
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
			LtnPoint3D Pt = new LtnPoint3D();
			Pt.x = line.getVertex(0).x;
			Pt.y = line.getVertex(0).y;
			Pt.z = line.getVertex(0).z;
			line.AddVertex(Pt);
			Next = null;
			Last = null;
			
			this.pbMapa.Invalidate(this.pbMapa.ClientRectangle,true);
		}

		private void btnParallel_Click(object sender, System.EventArgs e)
		{
			parallel = line.Parallel(Decimal.ToInt32(this.nudDist.Value),out line1,out Cant );
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
			Next = new LtnPoint3D(e.X,400-e.Y,0);
			if(Last == null) Last = Next.Clone();
			
			if(line==null) line = new LtnLine();
				
			line.AddVertex(Next.Clone());
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

		
	}
}
