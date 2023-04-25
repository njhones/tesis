using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using BorradorTesis.Polygonals;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using BorradorTesis;
using BigNumbers;
using BorradorTesis.Plane_Sweep;
using BorradorTesis.Plane_Sweep.Nodes; // temporal
using Structures;
using System.Drawing.Drawing2D;

namespace BorradorTesis
{
	/// <summary>
	/// Descripción breve de Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		#region MisAtributos

		private ArrayList line ;
		private ArrayList textBoxes;
		private RPoint pt;
		private bool[] adverse;
		private Segment[] seg, pseg;
		private bool pintando, been_painted, asigned, close;
		private Polygon poly_paralela, poly_original, poly_step;
		private Polygon poly_paralelaSin;
		private ArrayList lpolys;		
		private int closer, current;
		private ArrayList[] parallel;		
		private SkeletonVertex[] skvertex;
		private RBTree queue;
		private SkeletonVertex[] skvertexbystep;
		private ArrayList pSegs;
		private Polygonal abierta;
		
		#endregion		
		
		private System.Windows.Forms.PictureBox pbmap;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem mI_archivo;
		private System.Windows.Forms.MenuItem mI_cargar;
		private System.Windows.Forms.MenuItem mI_guardar;
		private System.Windows.Forms.MenuItem mI_salir;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;		
		private System.Windows.Forms.MenuItem mI;
		private System.Windows.Forms.MenuItem mI_conSeg;
		private System.Windows.Forms.MenuItem mI_sinSeg;		
		private System.Windows.Forms.CheckBox chb_editor;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem mI_editarvertices;
		private System.Windows.Forms.MenuItem mI_eraseParallel;
		private System.Windows.Forms.Button btn_Next;
		private System.Windows.Forms.Button btn_Back;
		private System.Windows.Forms.MenuItem mI_psweep;
		private System.Windows.Forms.MenuItem prueba;
		private System.Windows.Forms.MenuItem mI_skeleton;
		private System.Windows.Forms.MenuItem mI_close;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem mI_new;
		private System.Windows.Forms.MenuItem mI_eraseVertex;
		private System.Windows.Forms.MenuItem mI_ByIntersection;
		private System.Windows.Forms.MenuItem mI_BySkeleton;
		private System.Windows.Forms.MenuItem prueba2;
		private System.Windows.Forms.ToolBar toolBar1;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolBarButton tb_new;
		private System.Windows.Forms.ToolBarButton tb_open;
		private System.Windows.Forms.ToolBarButton tb_save;
		private System.Windows.Forms.ToolBarButton toolBarButton4;
		private System.Windows.Forms.ToolBarButton tb_undo;
		private System.Windows.Forms.ToolBarButton tb_redo;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.NumericUpDown nUDistancia;
		private System.Windows.Forms.ToolBarButton tb_close;
		private System.Windows.Forms.ToolBarButton tb_parallel;
		private System.Windows.Forms.ToolBarButton tb_painting;
		private System.Windows.Forms.MenuItem mI_xp;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.ComponentModel.IContainer components;

		public Form1()
		{
			//
			// Necesario para admitir el Diseñador de Windows Forms
			//
			InitializeComponent();
			//
			// TODO: agregar código de constructor después de llamar a InitializeComponent
			//
			this.line = new ArrayList();
			this.lpolys = new ArrayList();
			this.current = -1;
			this.textBoxes = new ArrayList();
			this.pintando = true;
			this.toolBar1.Buttons[7].Pushed = true;
		}

		/// <summary>
		/// Limpiar los recursos que se estén utilizando.
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

		#region Código generado por el Diseñador de Windows Forms
		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
			this.pbmap = new System.Windows.Forms.PictureBox();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.mI_archivo = new System.Windows.Forms.MenuItem();
			this.mI_new = new System.Windows.Forms.MenuItem();
			this.mI_cargar = new System.Windows.Forms.MenuItem();
			this.mI_guardar = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.mI_salir = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.mI_ByIntersection = new System.Windows.Forms.MenuItem();
			this.mI_BySkeleton = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mI_eraseParallel = new System.Windows.Forms.MenuItem();
			this.mI_close = new System.Windows.Forms.MenuItem();
			this.mI = new System.Windows.Forms.MenuItem();
			this.mI_conSeg = new System.Windows.Forms.MenuItem();
			this.mI_sinSeg = new System.Windows.Forms.MenuItem();
			this.mI_psweep = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.mI_skeleton = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mI_editarvertices = new System.Windows.Forms.MenuItem();
			this.mI_eraseVertex = new System.Windows.Forms.MenuItem();
			this.prueba = new System.Windows.Forms.MenuItem();
			this.prueba2 = new System.Windows.Forms.MenuItem();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.chb_editor = new System.Windows.Forms.CheckBox();
			this.btn_Next = new System.Windows.Forms.Button();
			this.btn_Back = new System.Windows.Forms.Button();
			this.toolBar1 = new System.Windows.Forms.ToolBar();
			this.tb_new = new System.Windows.Forms.ToolBarButton();
			this.tb_open = new System.Windows.Forms.ToolBarButton();
			this.tb_save = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton4 = new System.Windows.Forms.ToolBarButton();
			this.tb_undo = new System.Windows.Forms.ToolBarButton();
			this.tb_redo = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.tb_painting = new System.Windows.Forms.ToolBarButton();
			this.tb_close = new System.Windows.Forms.ToolBarButton();
			this.tb_parallel = new System.Windows.Forms.ToolBarButton();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.nUDistancia = new System.Windows.Forms.NumericUpDown();
			this.mI_xp = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			((System.ComponentModel.ISupportInitialize)(this.nUDistancia)).BeginInit();
			this.SuspendLayout();
			// 
			// pbmap
			// 
			this.pbmap.BackColor = System.Drawing.Color.White;
			this.pbmap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pbmap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pbmap.Location = new System.Drawing.Point(0, 0);
			this.pbmap.Name = "pbmap";
			this.pbmap.Size = new System.Drawing.Size(804, 565);
			this.pbmap.TabIndex = 0;
			this.pbmap.TabStop = false;
			this.pbmap.Paint += new System.Windows.Forms.PaintEventHandler(this.pbmap_Paint);
			this.pbmap.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pbmap_MouseUp);
			this.pbmap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbmap_MouseMove);
			this.pbmap.MouseLeave += new System.EventHandler(this.pbmap_MouseLeave);
			this.pbmap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pbmap_MouseDown);
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mI_archivo,
																					  this.menuItem6,
																					  this.menuItem7,
																					  this.mI_close,
																					  this.mI,
																					  this.menuItem1,
																					  this.prueba,
																					  this.prueba2,
																					  this.menuItem3});
			// 
			// mI_archivo
			// 
			this.mI_archivo.Index = 0;
			this.mI_archivo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.mI_new,
																					   this.mI_cargar,
																					   this.mI_guardar,
																					   this.menuItem4,
																					   this.mI_salir});
			this.mI_archivo.Text = "&Archivo";
			// 
			// mI_new
			// 
			this.mI_new.Index = 0;
			this.mI_new.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mI_new.Text = "&Nuevo";
			this.mI_new.Click += new System.EventHandler(this.mI_new_Click);
			// 
			// mI_cargar
			// 
			this.mI_cargar.Index = 1;
			this.mI_cargar.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mI_cargar.Text = "&Abrir";
			this.mI_cargar.Click += new System.EventHandler(this.mI_cargar_Click);
			// 
			// mI_guardar
			// 
			this.mI_guardar.Index = 2;
			this.mI_guardar.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mI_guardar.Text = "&Guardar";
			this.mI_guardar.Click += new System.EventHandler(this.mI_guardar_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 3;
			this.menuItem4.Text = "-";
			// 
			// mI_salir
			// 
			this.mI_salir.Index = 4;
			this.mI_salir.Text = "&Salir";
			this.mI_salir.Click += new System.EventHandler(this.mI_salir_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 1;
			this.menuItem6.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mI_ByIntersection,
																					  this.mI_BySkeleton});
			this.menuItem6.Text = "&TrazarParalela";
			// 
			// mI_ByIntersection
			// 
			this.mI_ByIntersection.Index = 0;
			this.mI_ByIntersection.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftT;
			this.mI_ByIntersection.Text = "By &intersection";
			this.mI_ByIntersection.Click += new System.EventHandler(this.mI_ByIntersection_Click);
			// 
			// mI_BySkeleton
			// 
			this.mI_BySkeleton.Index = 1;
			this.mI_BySkeleton.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
			this.mI_BySkeleton.Text = "By &skeleton bisector";
			this.mI_BySkeleton.Click += new System.EventHandler(this.mI_BySkeleton_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 2;
			this.menuItem7.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mI_eraseParallel});
			this.menuItem7.Text = "Borrar&Todo";
			// 
			// mI_eraseParallel
			// 
			this.mI_eraseParallel.Index = 0;
			this.mI_eraseParallel.Text = "Borrar &papalela";
			this.mI_eraseParallel.Click += new System.EventHandler(this.mI_eraseParallel_Click);
			// 
			// mI_close
			// 
			this.mI_close.Index = 3;
			this.mI_close.Text = "&Cerrar";
			this.mI_close.Click += new System.EventHandler(this.mI_close_Click);
			// 
			// mI
			// 
			this.mI.Index = 4;
			this.mI.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																			   this.mI_conSeg,
																			   this.mI_sinSeg,
																			   this.mI_psweep,
																			   this.menuItem2,
																			   this.mI_skeleton});
			this.mI.Text = "&Vistas";
			// 
			// mI_conSeg
			// 
			this.mI_conSeg.Checked = true;
			this.mI_conSeg.Index = 0;
			this.mI_conSeg.RadioCheck = true;
			this.mI_conSeg.Text = "&Segmentos";
			this.mI_conSeg.Click += new System.EventHandler(this.mI_conSeg_Click);
			// 
			// mI_sinSeg
			// 
			this.mI_sinSeg.Index = 1;
			this.mI_sinSeg.RadioCheck = true;
			this.mI_sinSeg.Text = "Sin Segmentos &Contrarios";
			this.mI_sinSeg.Click += new System.EventHandler(this.mI_sinSeg_Click);
			// 
			// mI_psweep
			// 
			this.mI_psweep.Index = 2;
			this.mI_psweep.RadioCheck = true;
			this.mI_psweep.Text = "Plane S&weep";
			this.mI_psweep.Click += new System.EventHandler(this.mI_psweep_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "-";
			// 
			// mI_skeleton
			// 
			this.mI_skeleton.Index = 4;
			this.mI_skeleton.Shortcut = System.Windows.Forms.Shortcut.CtrlK;
			this.mI_skeleton.Text = "Stright S&keleton";
			this.mI_skeleton.Click += new System.EventHandler(this.mI_skeleton_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 5;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.mI_editarvertices,
																					  this.mI_eraseVertex});
			this.menuItem1.Text = "&Edición";
			// 
			// mI_editarvertices
			// 
			this.mI_editarvertices.Index = 0;
			this.mI_editarvertices.RadioCheck = true;
			this.mI_editarvertices.Text = "Editar &vertices";
			this.mI_editarvertices.Click += new System.EventHandler(this.mI_editarvertices_Click);
			// 
			// mI_eraseVertex
			// 
			this.mI_eraseVertex.Index = 1;
			this.mI_eraseVertex.RadioCheck = true;
			this.mI_eraseVertex.Text = "&Borrar vertice";
			this.mI_eraseVertex.Click += new System.EventHandler(this.mI_eraseVertex_Click);
			// 
			// prueba
			// 
			this.prueba.Index = 6;
			this.prueba.Text = "prueba";
			this.prueba.Click += new System.EventHandler(this.prueba_Click);
			// 
			// prueba2
			// 
			this.prueba2.Index = 7;
			this.prueba2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mI_xp});
			this.prueba2.Text = "trazar paralela";
			this.prueba2.Click += new System.EventHandler(this.prueba2_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "*.bin|";
			this.saveFileDialog1.RestoreDirectory = true;
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "*.bin|";
			this.openFileDialog1.InitialDirectory = "c:\\\\";
			this.openFileDialog1.RestoreDirectory = true;
			// 
			// chb_editor
			// 
			this.chb_editor.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chb_editor.Location = new System.Drawing.Point(528, 32);
			this.chb_editor.Name = "chb_editor";
			this.chb_editor.Size = new System.Drawing.Size(64, 24);
			this.chb_editor.TabIndex = 6;
			this.chb_editor.Text = "Editor";
			this.chb_editor.CheckedChanged += new System.EventHandler(this.chb_editor_CheckedChanged);
			// 
			// btn_Next
			// 
			this.btn_Next.Location = new System.Drawing.Point(720, 32);
			this.btn_Next.Name = "btn_Next";
			this.btn_Next.TabIndex = 8;
			this.btn_Next.Text = "Siguiente";
			this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
			// 
			// btn_Back
			// 
			this.btn_Back.Location = new System.Drawing.Point(624, 32);
			this.btn_Back.Name = "btn_Back";
			this.btn_Back.TabIndex = 9;
			this.btn_Back.Text = "Atras";
			this.btn_Back.Click += new System.EventHandler(this.btn_Back_Click);
			// 
			// toolBar1
			// 
			this.toolBar1.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						this.tb_new,
																						this.tb_open,
																						this.tb_save,
																						this.toolBarButton4,
																						this.tb_undo,
																						this.tb_redo,
																						this.toolBarButton1,
																						this.tb_painting,
																						this.tb_close,
																						this.tb_parallel});
			this.toolBar1.DropDownArrows = true;
			this.toolBar1.ImageList = this.imageList1;
			this.toolBar1.Location = new System.Drawing.Point(0, 0);
			this.toolBar1.Name = "toolBar1";
			this.toolBar1.ShowToolTips = true;
			this.toolBar1.Size = new System.Drawing.Size(804, 28);
			this.toolBar1.TabIndex = 10;
			this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
			// 
			// tb_new
			// 
			this.tb_new.ImageIndex = 2;
			// 
			// tb_open
			// 
			this.tb_open.ImageIndex = 4;
			// 
			// tb_save
			// 
			this.tb_save.ImageIndex = 3;
			// 
			// toolBarButton4
			// 
			this.toolBarButton4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tb_undo
			// 
			this.tb_undo.ImageIndex = 1;
			// 
			// tb_redo
			// 
			this.tb_redo.ImageIndex = 0;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// tb_painting
			// 
			this.tb_painting.ImageIndex = 7;
			this.tb_painting.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.tb_painting.Text = "Pintando";
			// 
			// tb_close
			// 
			this.tb_close.ImageIndex = 6;
			this.tb_close.Text = "Cerrar";
			// 
			// tb_parallel
			// 
			this.tb_parallel.ImageIndex = 5;
			this.tb_parallel.Text = "Trazar paralela";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// statusBar1
			// 
			this.statusBar1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.statusBar1.Location = new System.Drawing.Point(0, 543);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(804, 22);
			this.statusBar1.TabIndex = 11;
			this.statusBar1.Text = "Localización : ";
			// 
			// nUDistancia
			// 
			this.nUDistancia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.nUDistancia.Location = new System.Drawing.Point(384, 5);
			this.nUDistancia.Maximum = new System.Decimal(new int[] {
																		300,
																		0,
																		0,
																		0});
			this.nUDistancia.Minimum = new System.Decimal(new int[] {
																		300,
																		0,
																		0,
																		-2147483648});
			this.nUDistancia.Name = "nUDistancia";
			this.nUDistancia.Size = new System.Drawing.Size(80, 20);
			this.nUDistancia.TabIndex = 12;
			// 
			// mI_xp
			// 
			this.mI_xp.Index = 0;
			this.mI_xp.Text = "xp";
			this.mI_xp.Click += new System.EventHandler(this.mI_xp_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 8;
			this.menuItem3.Text = "ghgh";
			this.menuItem3.Click += new System.EventHandler(this.prueba2_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(804, 565);
			this.Controls.Add(this.nUDistancia);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.toolBar1);
			this.Controls.Add(this.btn_Back);
			this.Controls.Add(this.btn_Next);
			this.Controls.Add(this.chb_editor);
			this.Controls.Add(this.pbmap);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Tesis";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			((System.ComponentModel.ISupportInitialize)(this.nUDistancia)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Punto de entrada principal de la aplicación.
		/// </summary>
		[STAThread]

		static void Main() 
		{
			Application.Run(new Form1());
		}
		
		/**/ 
		private void pbmap_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			
			if(this.line.Count > 0)
			{
				for(int i = 1; i < this.line.Count; i++)
				{
					g.DrawLine(System.Drawing.Pens.Black,
						((RPoint)this.line[i-1]).XToLong,
						this.pbmap.Height - ((RPoint)this.line[i-1]).YToLong,
						((RPoint)this.line[i]).XToLong,
						this.pbmap.Height - ((RPoint)this.line[i]).YToLong);
				}
				if(pintando)
				{					
					g.DrawLine(System.Drawing.Pens.Black,
						((RPoint)this.line[this.line.Count - 1]).XToLong,
						this.pbmap.Height - ((RPoint)this.line[this.line.Count - 1]).YToLong,
						this.pt.XToLong, this.pbmap.Height - this.pt.YToLong);
				}				
			}
			if(open != null)
			{
				open.Pinta(g, Pens.Blue, Pens.Red, pbmap.Height);
			}
			if(this.mI_psweep.Checked && parallel != null )
			{
				for(int j=0;j<parallel.Length;j++)
				{
					if( (parallel != null) && ( parallel[j].Count > 0 ) )
					{				
						for(int i=1; i<(parallel[j].Count); i++)
						{
							RPoint P1, P2;
							P1 = (RPoint)parallel[j][i-1];
							P2 = (RPoint)parallel[j][ i ];
							
							g.DrawLine(System.Drawing.Pens.Blue, 
								P1.XToLong, this.pbmap.Height - P1.YToLong, 
								P2.XToLong , this.pbmap.Height - P2.YToLong );
						}
				
					}
				}
			}
			
			else if(this.mI_conSeg.Checked && this.poly_step != null)
			{
				this.poly_step.Pinta(g, Pens.Blue, Pens.Red, this.pbmap.Height);
			}
			else if(this.mI_conSeg.Checked && this.poly_paralela != null)
			{
				this.poly_paralela.Pinta(g, Pens.Blue, Pens.Red, this.pbmap.Height);
			}
			else if(this.poly_paralelaSin != null)
			{
				this.poly_paralelaSin.Pinta(g, Pens.Blue, Pens.Red, this.pbmap.Height);				
			}
			if(this.pSegs != null)
			{
				foreach(Segment seg in this.pSegs)
					seg.Paint(g, Pens.Blue, this.pbmap.Height);
			}
			if(this.mI_skeleton.Checked)
			{
				CircularDoublyConnected mlist = (Math.Sign(this.nUDistancia.Value) == -1)? mexlist : minlist;
                if (mlist != null)
                {
                    foreach (SkVertex k in mlist)
                        DrawTree(k, g);
                }

			}			
			if(mlistaux != null)
			{
				foreach (SkNode k in mlistaux)
					DrawTreeA(k, g);
			}
		}
		CircularDoublyConnected mlistaux;
        private void DrawTree(SkVertex t, Graphics g)
        {
			Pen p = new Pen(System.Drawing.Color.DarkBlue);
			p.DashPattern = new float[]{4, 2};			
			p.DashStyle = DashStyle.Custom;
            foreach (SkVertex c in t.Children)
            {
                Segment seg = new Segment(t.Point, c.Point);
                seg.Paint(g, p, this.pbmap.Height);
                DrawTree(c, g);
            }
			p.Dispose();
        }
		private void DrawTreeA(SkNode t, Graphics g)
		{
			
			Pen p = new Pen(System.Drawing.Color.DarkBlue);
			p.DashPattern = new float[]{4, 2};			
			p.DashStyle = DashStyle.Custom;			
			
			foreach (SkNode c in t.Children)
			{
				Segment seg = new Segment(t.Point, c.Point);
				seg.Paint(g, p, this.pbmap.Height);
				DrawTreeA(c, g);
			}
			p.Dispose();
		}
		/**/ 
		private void pbmap_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			this.statusBar1.Text = "Localización : " + e.X.ToString() + "." + (this.pbmap.Height - e.Y).ToString();

			if(this.chb_editor.Checked && this.asigned)
			{
				RPoint _p = new RPoint(new Point(e.X, this.pbmap.Height -  e.Y));

				this.line[this.closer] = _p;

				((Label)this.textBoxes[this.closer]).Location = new Point(e.X, e.Y);
				((Label)this.textBoxes[this.closer]).Text = "{X=" + e.X.ToString()  + ",Y=" + (this.pbmap.Height - e.Y).ToString() + "}";					
				if(this.mI_editarvertices.Checked)
				{
					this.pbmap.Controls[this.closer].Location = ((Label)this.textBoxes[this.closer]).Location;
					this.pbmap.Controls[this.closer].Text = ((Label)this.textBoxes[this.closer]).Text;
				}
				if(this.close && this.closer == 0)
				{
					this.line[this.line.Count - 1] = this.line[this.closer];
				}
				if(this.poly_paralela != null)
				{
					this.btn_parallel_Click(sender, e);
				}
				this.poly_original = this.poly_paralela = this.poly_paralelaSin = null;
				this.poly_step = null;
				this.ClosedLine();
				this.pbmap.Invalidate();
				
			}
			else if(this.pintando)
			{
				this.pt = new RPoint(new Point(e.X, this.pbmap.Height - e.Y));
				this.pbmap.Invalidate();
			}
		}

		/**/ 
		private void pbmap_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(this.mI_eraseVertex.Checked)
			{
				RPoint _rp = new RPoint(new Point(e.X, this.pbmap.Height - e.Y));
				BigRational _dist = _rp.QuadraticDistance((RPoint)this.line[0]);
				BigRational _auxdist;	
				this.closer = 0;
				#region Calculo del punto mas cercano a _rp
				for(int i = 1; i < this.line.Count; i++)
				{
					_auxdist = _rp.QuadraticDistance((RPoint)line[i]);
					if(_auxdist < _dist)
					{
						this.closer = i;
						_dist = _auxdist;
					}
				}				
				#endregion

				this.line.RemoveAt(this.closer);
//				if(this.closer >= this.line.Count)
//				{
//					this.line.RemoveAt(0);
//				}
//				if(this.closer == 0)
//				{
//					this.line.RemoveAt(this.line.Count - 1);
//				}
				if(!((RPoint)this.line[0]).Equals((RPoint)this.line[this.line.Count - 1]))
				{
					this.line.Add(this.line[0]);
				}
				this.ClosedLine();
				this.pbmap.Invalidate();
			}
			if(this.chb_editor.Checked)
			{				
				RPoint _rp = new RPoint(new Point(e.X, this.pbmap.Height - e.Y));
				this.closer = 0;
				BigRational _dist = _rp.QuadraticDistance((RPoint)this.line[0]);
				BigRational _auxdist;				
				#region Calculo del punto mas cercano a _rp
				for(int i = 1; i < this.line.Count; i++)
				{
					_auxdist = _rp.QuadraticDistance((RPoint)line[i]);
					if(_auxdist < _dist)
					{
						this.closer = i;
						_dist = _auxdist;
					}
				}				
				#endregion

				this.line[this.closer] = _rp;

				if(this.mI_editarvertices.Checked)
				{
					this.pbmap.Controls[this.closer].Location = new Point(e.X, this.pbmap.Height - e.Y);
					this.pbmap.Controls[this.closer].Text = this.pbmap.Controls[this.closer].Location.ToString();
				}

				if(this.close && this.closer == 0)
				{
					this.line[this.line.Count - 1] = _rp;
				}
				this.asigned = true;
				
				if(this.poly_paralela != null)
				{
					this.btn_parallel_Click(sender, e);
				}
				this.poly_original = null;
				this.poly_paralela = null;
				this.ClosedLine();
				this.pbmap.Invalidate();	
			}
			else if(this.pintando)
			{
				this.pt = new RPoint(new Point(e.X, this.pbmap.Height - e.Y));
				if(!(this.line.Count > 0 && ((RPoint)this.line[this.line.Count - 1]).Equals(pt)))
				{
					this.line.Add(this.pt);				
				}
				this.textBoxes.Add(pt.InitialyzeLabel(this.pbmap.Height));
				this.mI_editarvertices.Enabled = true;
				if(this.mI_editarvertices.Checked)
				{
					this.pbmap.Controls.Add((Label)textBoxes[this.textBoxes.Count - 1]);
				}
				this.pbmap.Invalidate();					
			}
			
		}
		
		/**/ 
		private void btn_Cerrar_Click(object sender, System.EventArgs e)
		{
			if(!this.close)
			{
				this.pintando = false;
				this.close = true;
				this.line.Add(this.line[0]);				
				this.pbmap.Invalidate();			
			}
		}

		/**/ 
		private void btn_Reset_Click(object sender, System.EventArgs e)
		{
			this.queue = null;
			this.skvertexbystep = null;
			this.pSegs = null;
			this.btn_Next.Enabled = true;
			this.close = false;
			this.pintando = true;
			this.line.Clear();
			this.textBoxes.Clear();
			this.pbmap.Controls.Clear();
			this.poly_paralela = null;
			this.poly_paralelaSin = null;
			this.poly_step = null;	
			this.parallel = null;	
			this.poly_original = null;
			this.skvertex = null;
			
	
			if(this.chb_editor.Checked)
			{
				this.chb_editor_CheckedChanged(sender, e);
			}	
			this.mI_new_Click(sender, e);
			this.pbmap.Invalidate();			
		}

		/**/ 
		private void btn_parallel_Click(object sender, System.EventArgs e)
		{
			
			this.MakeSkeleton();
			CircularDoublyConnected mlist = (Math.Sign(this.nUDistancia.Value) == -1)? mexlist : minlist;
			if(mlist != null)
			{
				this.pSegs = this.poly_original.PaintParallel(mlist, (short)this.nUDistancia.Value);
			}
//			this.mI_skeleton.Checked = true;
			this.pbmap.Invalidate();
		}
		/**/
		private CircularDoublyConnected mexlist;
		/**/
		private CircularDoublyConnected minlist;
		/**/
		public void MakeSkeleton()
		{
			this.ClosedLine();
			if(this.poly_original != null)
			{
				if(Math.Sign(this.nUDistancia.Value) == -1 && this.mexlist == null)
				{
					this.mexlist = this.poly_original.CreateExteriorSkeleton();		
				}
				else if(this.minlist == null)
					this.minlist = this.poly_original.CreateInteriorSkeleton();
			}	
		}
		/**/
		private void Stright()
		{
			this.ClosedLine();
			if(this.poly_original != null && this.skvertex == null)
			{
				this.skvertex = this.poly_original.SKeleton();				
			}
		}
		/**/ 
		private void pbmap_MouseLeave(object sender, System.EventArgs e)
		{
			this.statusBar1.Text = "Localización :";

			if(this.line.Count > 0)
			{
				this.pt = (RPoint)this.line[this.line.Count - 1];
				this.pbmap.Invalidate();
			}
		}

		/**/ 
		private void mI_guardar_Click(object sender, System.EventArgs e)
		{
//			Stream _stream;			
//			if(this.saveFileDialog1.ShowDialog() == DialogResult.OK)
//			{
//				if((_stream = this.saveFileDialog1.OpenFile()) != null)
//				{					
//					IFormatter _formatter = new BinaryFormatter();
//					_formatter.Serialize(_stream, this.line);
//					_stream.Close();
//				}
//			}
			if(this.saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				System.IO.StreamWriter myFile =
					new System.IO.StreamWriter(saveFileDialog1.FileName);
				myFile.WriteLine(line.Count.ToString());
				for(int i = 0; i<line.Count; i++)
				{
					myFile.WriteLine(((RPoint)this.line[i]).XToLong.ToString());
					myFile.WriteLine(((RPoint)line[i]).YToLong);
				}

				myFile.Close();		
			}
		}

		/**/ 
		private void mI_cargar_Click(object sender, System.EventArgs e)
		{
			Stream _stream;		
			
			if(this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if((_stream = this.openFileDialog1.OpenFile())!= null)
					{	
						this.btn_Reset_Click(sender, e);					
					
						IFormatter _formatter = new BinaryFormatter();
						this.line = (ArrayList)_formatter.Deserialize(_stream);
						_stream.Close();
					
						for(int i = 0; i < this.line.Count; i++)
						{
							this.textBoxes.Add(((RPoint)this.line[i]).InitialyzeLabel(this.pbmap.Height));
							if(this.mI_editarvertices.Checked)
							{
								this.pbmap.Controls.Add((Label)this.textBoxes[i]);
							}
						}

						this.Text = "Tesis - " + Path.GetFileName(this.openFileDialog1.FileName); 
						this.pintando = false;
						this.pbmap.Invalidate();
					}
				}
				catch
				{
					using (System.IO.Stream userStream = openFileDialog1.OpenFile()) 
					{
						this.btn_Reset_Click(sender, e);
						// Read the file as one string.
						System.IO.StreamReader myFile =
							new System.IO.StreamReader(this.openFileDialog1.FileName);
						
						int n = Convert.ToInt32( myFile.ReadLine() );
						for(int i = 0; i<n; i++)
						{
							BigRational x = new BigRational(Convert.ToInt64(myFile.ReadLine()), 0, 1);
							BigRational y = new BigRational(Convert.ToInt64(myFile.ReadLine()), 0, 1);							
							line.Add(new RPoint(x, y));
						}

						myFile.Close();		
						for(int i = 0; i < this.line.Count; i++)
						{
							this.textBoxes.Add(((RPoint)this.line[i]).InitialyzeLabel(this.pbmap.Height));
							if(this.mI_editarvertices.Checked)
							{
								this.pbmap.Controls.Add((Label)this.textBoxes[i]);
							}
						}

						this.Text = "Tesis - " + Path.GetFileName(this.openFileDialog1.FileName); 
						this.pintando = false;
					}
					if(((RPoint)this.line[0]).Equals(this.line[this.line.Count - 1]))
					{
						RPoint[] vertices = new RPoint[this.line.Count - 1];

						for(int i = 0; i < line.Count - 1; i++)
						{
							vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();
						}				
						this.poly_original = new Polygon(vertices);
					}
					this.pbmap.Invalidate(this.pbmap.ClientRectangle,true);	
				}
				if(((RPoint)this.line[0]).Equals(this.line[this.line.Count - 1]))
				{
					this.close = true;
				}
			}
			this.toolBar1.Buttons[7].Enabled = true;
		}

		/**/ 
		private void mI_salir_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		/**/ 
		private void mI_sinSeg_Click(object sender, System.EventArgs e)
		{
			if(!this.mI_sinSeg.Checked)
			{
				this.mI_sinSeg.Checked = true;
				this.mI_conSeg.Checked = this.mI_psweep.Checked  = false;
				this.poly_step = null;
				this.pbmap.Invalidate();
			}
		}

		/**/ 
		private void mI_conSeg_Click(object sender, System.EventArgs e)
		{
			if(!this.mI_conSeg.Checked)
			{
				this.mI_conSeg.Checked = true;
				this.mI_sinSeg.Checked = this.mI_psweep.Checked = false;
				this.poly_step = null;
				this.pbmap.Invalidate();
			}
		}
		
		/**/ 
		private void chb_editor_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.line.Count == 0)
				this.chb_editor.Checked = false;
			if(this.chb_editor.Checked)
			{
				this.been_painted = this.pintando;
				this.pintando = false;
			}
			else 
			{
				this.asigned = false;
				if(this.been_painted)
					this.pintando = true;
			}
		}

		/**/ 
		private void pbmap_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(this.asigned)
				this.asigned = false;
		}
		/**/ 
		private void mI_editarvertices_Click(object sender, System.EventArgs e)
		{
			if(this.mI_editarvertices.Checked)
			{
				this.mI_editarvertices.Checked = false;
				this.pbmap.Controls.Clear();
			}
			else
			{
				this.mI_editarvertices.Checked = true;	
				foreach(object o in this.textBoxes)
				{
					this.pbmap.Controls.Add((Label)o);
				}
			}
			this.pbmap.Invalidate();
			
		}

		/**/ 
		private void mI_eraseParallel_Click(object sender, System.EventArgs e)
		{
			this.poly_paralela = null;
			this.poly_paralelaSin = null;
			this.poly_step = null;
			this.parallel = null;
			this.pbmap.Invalidate();
		}

		/**/ 
		private void nUDistancia_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if(e.KeyChar == (char)13)
				this.btn_parallel_Click(sender, e);
			this.pbmap.Invalidate();
		}
		/**/ 
		private void btn_Next_Click(object sender, System.EventArgs e)
		{
			this.ClosedLine();
			if(this.poly_original != null)
			{
				if(this.queue == null)
				{
					this.queue = new RBTree();	
					this.skvertexbystep = this.poly_original.Inicialitation(this.queue);
				}
				if(this.queue.IsEmpty)
				{
					this.btn_Next.Enabled = false;
				}
				else
				{
					this.poly_original.MakeSkeletonByStep(this.queue);
					this.mI_skeleton.Checked = true;
					this.pbmap.Invalidate();
				}
				return;
			}
			if(this.poly_paralela != null && this.btn_Next.Enabled)
			{
				if(!this.mI_conSeg.Checked)
				{
					this.mI_conSeg_Click(sender, e);
				}
				else
				{
					if(this.lpolys.Count == 0)					{
												
						this.pseg = this.seg = this.poly_paralela.Segments;
						this.adverse = (bool[])this.poly_paralela.AdversedSegments.Clone();
					}
					if(this.current + 1 < this.lpolys.Count)
					{
						this.poly_step = (Polygon)this.lpolys[++this.current]; 
					}
					else
					{
						this.poly_step = Polygon.RemoveSegment(this.seg, this.adverse,
							this.poly_original.Segments, true, this.pseg);
						this.lpolys.Add(this.poly_step);
						this.current++;						
					}
					this.btn_Back.Enabled = true;
					
				}
				this.pbmap.Invalidate();
			}
		}

		/**/ 
		private void btn_Back_Click(object sender, System.EventArgs e)
		{
			if(this.skvertexbystep != null)
			{
				this.skvertexbystep = null;
				this.queue = null;
				this.mI_skeleton.Checked = false;
				this.btn_Next.Enabled = true;
				this.pbmap.Invalidate();
				return;
			}
			if(this.current > 0)
			{
				this.poly_step = (Polygon)this.lpolys[--this.current];
			}
			else
			{
				this.poly_step = null;
				this.current = -1;
				this.btn_Back.Enabled = false;
			}
			this.pbmap.Invalidate();
		}

		/**/ 
		private void mI_psweep_Click(object sender, System.EventArgs e)
		{
			if(!this.mI_psweep.Checked)
			{
				this.mI_psweep.Checked = true;
				this.mI_conSeg.Checked = mI_sinSeg.Checked = false;
				if(this.poly_paralelaSin != null)
				{					
					ArrayList List = this.poly_paralelaSin.GetValid(Math.Sign((int)this.nUDistancia.Value));
					this.parallel = new ArrayList[List.Count];

					for(int i =0; i< List.Count; i++ )
					{
						parallel[i] = new ArrayList();
						for( int j =0;j<((ArrayList)List[i]).Count;j++)
						{
							parallel[i].Add( (RPoint)((ArrayList)List[i])[j]  );
						}
						if(parallel[i].Count > 0 ) parallel[i].Add( parallel[i][0] );
					}					
					this.pbmap.Invalidate();
				}
				
			}
		}

		/*prueba
		 * */
		private void prueba_Click(object sender, System.EventArgs e)
		{
			this.mlistaux = this.poly_original.CreateSk(true);
//			this.poly_original.OnStep += new Polygon.Step(poly_original_OnStep);
			this.pbmap.Invalidate();
		}

		/**/
		private void mI_skeleton_Click(object sender, System.EventArgs e)
		{
			if(!this.mI_skeleton.Checked)
			{
				this.mI_skeleton.Checked = true;
                //this.Stright();
				if(this.poly_original != null)
				{
					//this.skvertex =  this.poly_original.SKeleton();
//                    this.poly_original.OnStep += new Polygon.Step(poly_original_OnStep);
					if(Math.Sign(this.nUDistancia.Value) == -1 && this.mexlist == null)
					{
						  this.mexlist = this.poly_original.CreateExteriorSkeleton();		
					}
					else if(this.minlist == null)
						this.minlist = this.poly_original.CreateInteriorSkeleton();
                  
//					this.pSegs = this.poly_original.PaintParallel(mexlist, (short)this.nUDistancia.Value);
				}									
			}
			else
			{
				this.mI_skeleton.Checked = false;
			}
			this.pbmap.Invalidate();
		}

        void poly_original_OnStep(CircularDoublyConnected list)
        {
            mlistaux = list;
            this.pbmap.Invalidate();
            Application.DoEvents();
            for (int i = 0; i < int.MaxValue / 4; i++)
                i += 1 - 1;
        }

        /**/
        private void ClosedLine()
		{
			if(this.poly_original == null && this.line.Count > 0 && !this.pintando &&
				((RPoint)this.line[0]).Equals(this.line[this.line.Count - 1]))
			{
				RPoint[] vertices = new RPoint[this.line.Count - 1];

				for(int i = 0; i < line.Count - 1; i++)
				{
					vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();
				}			
				this.poly_original = new Polygon(vertices);
			}

		}

		/**/
		private void mI_close_Click(object sender, System.EventArgs e)
		{
			if(!this.close && this.line.Count > 0)
			{
				this.pintando = false;
				this.close = true;
				this.line.Add(this.line[0]);
				this.toolBar1.Buttons[7].Enabled = false;
				RPoint[] vertices = new RPoint[this.line.Count - 1];
				for(int i = 0; i < line.Count - 1; i++)
				{
					vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();
				}					
				this.poly_original = new Polygon(vertices);
				this.pbmap.Invalidate();	
			}
		}

		/**/
		private void mI_new_Click(object sender, System.EventArgs e)
		{
			this.close = false;
			this.queue = null;
			this.pSegs = null;
			this.skvertexbystep = null;
			this.mI_skeleton.Checked = false;
			this.btn_Next.Enabled = true;
			this.pintando = true;
			this.line.Clear();
			this.textBoxes.Clear();
			this.pbmap.Controls.Clear();
			this.poly_paralela = null;
			this.poly_paralelaSin = null;
			this.poly_step = null;	
			this.parallel = null;			
			this.poly_original = null;
			this.skvertex = null;
            this.minlist = null;
			this.mexlist = null;
			this.mlistaux = null;
			this.open = null;
			this.toolBar1.Buttons[7].Enabled = true;
			this.toolBar1.Buttons[7].Pushed = true;
			
			if(this.chb_editor.Checked)
			{
				this.chb_editor_CheckedChanged(sender, e);
			}		
			this.pbmap.Invalidate();
		}

		private void mI_eraseVertex_Click(object sender, System.EventArgs e)
		{
			if(!this.mI_eraseVertex.Checked)
			{
				this.mI_eraseVertex.Checked = true;
				this.mI_editarvertices.Checked = false;
			}
			else
			{
				this.mI_eraseVertex.Checked = false;
			}
		}

		private void mI_ByIntersection_Click(object sender, System.EventArgs e)
		{
			if(this.line.Count > 0)
			{
				if(((RPoint)this.line[0]).Equals(this.line[this.line.Count - 1]))
				{
					RPoint[] vertices = new RPoint[this.line.Count - 1];

					for(int i = 0; i < line.Count - 1; i++)
					{
						vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();
					}	
				
				
					this.poly_original = new Polygon(vertices);
					this.poly_paralela = this.poly_original.Parallel
						(System.Decimal.ToInt16(this.nUDistancia.Value));
					
					if(this.poly_paralela != null)
					{
						this.poly_paralelaSin = 
							((Polygon)this.poly_paralela).SegmentAdverseOut
							((Polygon)this.poly_original, false);
					}
					this.lpolys.Clear();
					this.current = -1;
					this.pbmap.Invalidate();					
				}
			}
		}
		Polygonal open;
		private void prueba2_Click(object sender, System.EventArgs e)
		{
			RPoint[] vertices = new RPoint[this.line.Count];
			for(int i = 0; i < line.Count; i++)
				vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();

			this.abierta = new Polygonal(vertices);
			this.open = this.abierta.Parallel(System.Decimal.ToInt16(this.nUDistancia.Value));

			this.pbmap.Invalidate();
		}

		private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			switch(toolBar1.Buttons.IndexOf(e.Button))
			{
				case 0:
					this.mI_new_Click(sender, e);
					break;
				case 1:
					this.mI_cargar_Click(sender, e);
					break;
				case 2:
					this.mI_guardar_Click(sender, e);
					break;
				case 8:
					this.mI_close_Click(sender, e);
					break;
				case 9:
					this.mI_BySkeleton_Click(sender, e);
					break;
			}
		}

		private void mI_BySkeleton_Click(object sender, System.EventArgs e)
		{
			this.MakeSkeleton();
			CircularDoublyConnected mlist = (Math.Sign(this.nUDistancia.Value) == -1)? mexlist : minlist;
			if(mlist != null)
				this.pSegs = this.poly_original.PaintParallel(mlist, (short)this.nUDistancia.Value);
			
//			this.mI_skeleton.Checked = true;
			this.pbmap.Invalidate();
		}

		private void mI_xp_Click(object sender, System.EventArgs e)
		{
			RPoint[] vertices = new RPoint[this.line.Count];
			for(int i = 0; i < line.Count; i++)
				vertices[i] = (RPoint)((ICloneable)this.line[i]).Clone();

			this.abierta = new Polygonal(vertices);

			this.mlistaux = this.abierta.CreateSkleton(true);
			this.pbmap.Invalidate();
		}		
		
	}
}
