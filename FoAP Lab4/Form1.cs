using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoAP_Lab1_2
{
    public partial class Form1 : Form
    {
        private bool maxSize = false;
        private Label[] formLabels;
        private CommandLine commandLine = new CommandLine();
        public Form1()
        {
            InitializeComponent();
            backgroundPictureBox.MouseLeave += (o, e) => backgroundPictureBoxLeave();
            backgroundPictureBox.MouseHover += (o, e) => backgroundPictureBoxHover();
            //Figures.ShapeContainer.load();
            //Figures.ShapeContainer.start();
            formSettingsBox.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CanvaBox.setSelfGroupBox(canvaGroupBox);
            CreateBox.setSelfGroupBox(createGroupBox);
            PointsBox.setSelfGroupBox(pointsGroupBox);
            CollectionBox.setSelfGroupBox(figuresGroupBox);
            EditFigureBox.setSelfGroupBox(groupBox4);
            InputTypeBox.setSelfGroupBox(groupBox6);
            CommandLineBox.setSelfGroupBox(commandLineBox);


            Data.input();


            CreateBox.openSmall();
            PointsBox.close();
            EditFigureBox.close();

            CanvaBox.figureMultiplySettings.close();
            CanvaBox.figureSettings.close();
            CanvaBox.Draw();

            CollectionBox.update();
            CollectionBox.openSmall();

            formLabels = new Label[] {
                formCloseLabel, formSizeLabel, formBackLabel
            };
            for( int i = 0; i < formLabels.Length; i++)
            {
                int index = i;
                formLabels[i].MouseLeave += (s, ev) => formLabelsLeave(s, ev, index);
                formLabels[i].MouseMove += (s, ev) => formLabelsMove(s, ev, index);
            }

            InputTypeBox.changedTrackBar();

            //commandLine.ob("C(150, 150, 150)");
            

        }
        //private void checking()
        //{
        //    Stack<Operand> operands = commandLine.getOperands();
        //    Stack<Operator> operators = commandLine.getOperators();
        //    List<Operand> arrayOperands = new List<Operand>();
        //    List<Operator> arrayOperators = new List<Operator>();
            
        //    while (true)
        //    {
        //        bool a = false, b = false;

        //        try {
        //            Operand operandLocal = operands.Pop();
        //            if (operandLocal.Equals(null))
        //            {

        //            }
        //            else
        //            {
        //                arrayOperands.Add(operandLocal);
        //            }
        //        } catch {
        //            a = true;
        //        }
                

        //        try
        //        {
        //            Operator operatorLocal = operators.Pop();
        //            if (operatorLocal.Equals(null))
        //            {

        //            }
        //            else
        //            {
        //                arrayOperators.Add(operatorLocal);
        //            }
        //        } catch {
        //            b = true;
        //        }

        //        if (a && b)
        //        {
        //            break;
        //        }
        //    }

        //    Operand operand = arrayOperands[arrayOperands.Count-1];
        //    string desc = "template";

        //    if (!operand.Equals(null))
        //    {

        //        string name = "";
        //        switch (operand.value)
        //        {
        //            case ('O'):
        //                if ( Figures.ShapeContainer.nameTaken(name))
        //                {
        //                    desc = "Name already taken";
        //                } 
        //                else if (arrayOperands.Count != 5)
        //                {
        //                    desc = "Number if operands not 5";
        //                }
        //                else
        //                {
        //                    desc = "Clock was added";
        //                    Figures.Clock clock = new Figures.Clock((int)arrayOperands[3].value, (int)arrayOperands[2].value, (int)arrayOperands[1].value);
        //                    clock.Name = arrayOperands[4].value.ToString();
        //                    Figures.ShapeContainer.addFigure(clock);
                            
        //                }

        //                break;
        //            case ('M'):

        //                break;
        //            case ('D'):

        //                if (Figures.ShapeContainer.nameTaken(name))
        //                {
        //                    int index = Figures.ShapeContainer.indexByName(name);
        //                    if (index >= 0)
        //                    {
        //                        Figures.ShapeContainer.delFigure( index );
        //                    }
                            
        //                }
                        

        //                break;
        //            case ('C'):
        //                desc = "";
        //                break;

        //            default:
        //                desc = "There is no that comamnd";
        //                break;
        //        }

        //    }
        //    Figures.ShapeContainer.DrawAll(CanvaBox.bitmap);

        //}
        private void label2_Click(object sender, EventArgs e)
        {
            CanvaBox.Draw();
        }
        private void formLabelsMove(object o, EventArgs e, int index)
        {
            formLabels[index].BackColor = Settings.formLabelSelectColor;
        }
        private void formLabelsLeave(object o, EventArgs e, int index)
        {
            formLabels[index].BackColor = this.BackColor;
        }


        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CreateBox.treeViewAfterSelect(sender, e);
        }

        private void formCloseLabel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formSizeLabel_Click(object sender, EventArgs e)
        {
            if (maxSize)
            {
                this.WindowState = FormWindowState.Normal;
                formSizeLabel.Text = "□";
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                formSizeLabel.Text = "❐";
            }
            maxSize = !maxSize;


            if (PointsBox.opened)
            {
                PointsBox.openCentered(this.Width / 2, this.Height / 2);
            }
            CanvaBox.defaultSettings();
        }

        private void formBackLabel_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CanvaBox.Draw();
        }
        public void backgroundPictureBoxLeave()
        {
            timer1.Stop();
        }
        public void backgroundPictureBoxHover()
        {
            timer1.Start();
        }

        private void commandLineHistoryLabel_Click(object sender, EventArgs e)
        {
            string 
                operands = "Operators: \n\n",
                operators = "Operands: \n\n";
            

            MessageBox.Show(operands);
            MessageBox.Show(operators);
        }
    }
}
