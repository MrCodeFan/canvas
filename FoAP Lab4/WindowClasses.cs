using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;

namespace FoAP_Lab1_2
{
    class WindowClasses
    {

    }

    static public class CanvaBox
    {
        public static GroupBox selfGroupBox;
        public static PictureBox pictureBox;
        public static Label[] labels = new Label[4];
        public static Bitmap bitmap;
        public static FigureSettingsBox figureSettings;
        public static FigureMultiplySettingsBox figureMultiplySettings;

        public static bool locked = false;
        public static bool up = false;

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            defaultSettings();

            Figures.ShapeContainer.load();
            Figures.ShapeContainer.start();


            pictureBox.MouseClick += (o, e) => pictureBoxMouseClick(o, e);
            pictureBox.MouseMove += (o, e) => pictureBoxMouseMove(o, e);
            pictureBox.MouseLeave += (o, e) => pictureBoxMouseLeave(o, e);
        }

        static public void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("CanvaBox: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "label3":
                                labels[3] = (Label)control;
                                break;
                            case "sizeLabel":
                                labels[0] = (Label)control;
                                break;
                            case "canvaMessageLabel":
                                labels[1] = (Label)control;
                                break;
                            case "label1":
                                labels[2] = (Label)control;
                                break;
                        }
                        break;
                    case "PictureBox":
                        pictureBox = (PictureBox)control;
                        updateBitmap();
                        break;
                    case "GroupBox":
                        switch (control.Name)
                        {
                            case "figureSettingsGroupBox":
                                figureSettings = new FigureSettingsBox((GroupBox)control);
                                break;
                            case "figureMultiplySettingsGroupBox":
                                figureMultiplySettings = new FigureMultiplySettingsBox((GroupBox)control);
                                break;
                        }
                        break;
                }
            }
        }
        static public void Draw(Bitmap bitmap)
        {
            Figures.ShapeContainer.Draw(bitmap);

            updateImage(bitmap);
        }
        static public void Draw()
        {
            updateBitmap();
            Figures.ShapeContainer.Draw(bitmap);

            updateImage(bitmap);
        }
        static public void DrawAll()
        {
            updateBitmap();
            Figures.ShapeContainer.DrawAll(bitmap);
            updateImage(bitmap);
        }
        static public void DrawPoint(int x, int y)
        {
            Graphics g = Graphics.FromImage(bitmap);
            g.DrawLine(
                new Pen(Color.Black, 1),
                new Point(x - 5, y),
                new Point(x + 5, y)
                );
            g.DrawLine(
                new Pen(Color.Black, 1),
                new Point(x, y - 5),
                new Point(x, y + 5)
                );
            updateImage(bitmap);
        }

        static public void updateBitmap()
        {
            bitmap = new Bitmap(
                pictureBox.ClientSize.Width,
                pictureBox.ClientSize.Height
            );
        }
        static public void updateImage(Bitmap bitmap)
        {
            pictureBox.Image = bitmap;
        }
        public static void defaultSettings()
        {
            pictureBox.BackColor = Color.Gray;
            labels[0].Text = String.Format("{0} x {1}", pictureBox.Width, pictureBox.Height);

            labels[2].Text = "";
            labels[3].Text = "";
        }

        public static void pictureBoxMouseClick(object o, MouseEventArgs e)
        {
            figureSettings.close();
            figureMultiplySettings.close();

            if (e.Button == MouseButtons.Left)
            {
                locked = !locked;
                Figures.ShapeContainer.selectByCoords(e.X, e.Y, CanvaBox.bitmap);
                CanvaBox.updateImage(bitmap);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ArrayList figures = Figures.ShapeContainer.searchByCoords(e.X, e.Y);
                // menu
                Console.WriteLine("figures.Count = {0}", figures.Count);
                switch (figures.Count)
                {
                    case 0:

                        break;
                    case 1:
                        CanvaBox.figureSettings.open(e.X + CanvaBox.labels[3].Width, e.Y + CanvaBox.labels[0].Height);
                        CanvaBox.figureSettings.update((Figures.Figure)figures[0]);
                        break;
                    default:
                        CanvaBox.figureMultiplySettings.open(e.X, e.Y);
                        CanvaBox.figureMultiplySettings.update(figures);
                        break;
                }
            }
        }
        public static void pictureBoxMouseMove(object o, MouseEventArgs e)
        {
            if (!locked)
            {
                updateBitmap();
                if (up)
                {
                    CanvaBox.Draw(bitmap);
                }
                Graphics graphics = Graphics.FromImage(bitmap);

                if (Settings.realTimeSideLines)
                {
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(0, e.Y),
                        new Point(pictureBox.ClientSize.Width, e.Y)
                    );
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(e.X, 0),
                        new Point(e.X, pictureBox.ClientSize.Height)
                    );
                }



                if (Settings.realTimeAngelLines)
                {
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(0, 0),
                        new Point(e.X, e.Y)
                    );
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(0, CanvaBox.pictureBox.ClientSize.Height),
                        new Point(e.X, e.Y)
                    );
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(CanvaBox.pictureBox.ClientSize.Width, 0),
                        new Point(e.X, e.Y)
                    );
                    graphics.DrawLine(
                        Settings.movePen,
                        new Point(CanvaBox.pictureBox.ClientSize.Width, CanvaBox.pictureBox.ClientSize.Height),
                        new Point(e.X, e.Y)
                    );
                }

                labels[1].Text = String.Format("{0}:{1}", e.X, e.Y);

                if (Settings.realTimeSelect)
                {
                    Figures.ShapeContainer.selectByCoords(e.X, e.Y, CanvaBox.bitmap);
                }

                if (!up)
                {
                    CanvaBox.Draw(bitmap);
                }

                updateImage(bitmap);

            }
        }
        public static void pictureBoxMouseLeave(object o, EventArgs e)
        {
            if (!locked)
            {
                updateBitmap();
                CanvaBox.Draw();
                updateImage(bitmap);
            }

        }
    }
    public class figureMenu
    {
        public GroupBox selfGroupBox;
        public figureMenu(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
        }
    }
    static public class CreateBox
    {
        public static GroupBox selfGroupBox;
        public static TreeView treeView;
        public static CreateDataBox createDataBox;
        public static Label[] interfaceLabels = new Label[2];
        public static Label selectedLabel;

        public static Label[] labels = new Label[4];
        private static int typeId;

        public static ArrayList points = new ArrayList();

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            generateView();
        }

        static public void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("{0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "interfaceSelectLabel":
                                interfaceLabels[0] = (Label)control;
                                break;
                            case "interfaceSelectedLabel":
                                interfaceLabels[1] = (Label)control;
                                break;
                            case "label4":
                                selectedLabel = (Label)control;
                                break;
                        }
                        break;
                    case "TreeView":
                        treeView = (TreeView)control;
                        break;
                    case "GroupBox":
                        switch (control.Name)
                        {
                            case "figureGroupBox":
                                createDataBox = new CreateDataBox((GroupBox)control);
                                break;
                        }
                        break;
                }
            }
            treeView.Nodes.Clear();
        }
        static public void generateView()
        {
            TreeNode[] treeNodesParent = new TreeNode[4];
            TreeNode[] treeNodes = new TreeNode[8];
            treeNodes[0] = new TreeNode("Simple Ellipse");
            treeNodes[1] = new TreeNode("Circle");
            treeNodes[2] = new TreeNode("Simple Rectangle");
            treeNodes[3] = new TreeNode("Square");
            treeNodes[4] = new TreeNode("Simple Polygon");
            treeNodes[5] = new TreeNode("Triangle");
            treeNodes[6] = new TreeNode("Sun");
            treeNodes[7] = new TreeNode("Clock");
            treeNodes[0].Name = "1";
            treeNodes[1].Name = "2";
            treeNodes[2].Name = "3";
            treeNodes[3].Name = "4";
            treeNodes[4].Name = "5";
            treeNodes[5].Name = "6";
            treeNodes[6].Name = "7";
            treeNodes[7].Name = "8";

            treeNodesParent[0] = new TreeNode("Ellipse");
            treeNodesParent[1] = new TreeNode("Rectangle");
            treeNodesParent[2] = new TreeNode("Polygon");
            treeNodesParent[3] = new TreeNode("Special");

            treeNodesParent[0].Nodes.Add(treeNodes[0]);
            treeNodesParent[0].Nodes.Add(treeNodes[1]);
            treeNodesParent[1].Nodes.Add(treeNodes[2]);
            treeNodesParent[1].Nodes.Add(treeNodes[3]);
            treeNodesParent[2].Nodes.Add(treeNodes[4]);
            treeNodesParent[2].Nodes.Add(treeNodes[5]);
            treeNodesParent[3].Nodes.Add(treeNodes[6]);
            treeNodesParent[3].Nodes.Add(treeNodes[7]);

            treeView.Nodes.Add(treeNodesParent[0]);
            treeView.Nodes.Add(treeNodesParent[1]);
            treeView.Nodes.Add(treeNodesParent[2]);
            treeView.Nodes.Add(treeNodesParent[3]);
        }

        public static void treeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Nodes.Count == 0)
                {
                    try
                    {
                        typeId = int.Parse(e.Node.Name);
                        selectedLabel.Text = String.Format("{0} / {1}", e.Node.Text, e.Node.Name);

                        CreateBox.createDataBox.update(typeId);

                        openBig();
                    }
                    catch
                    {
                        // error
                    }

                }
                else
                {
                    e.Node.Expand();
                }
            }
            else
            {
                try
                {
                    typeId = int.Parse(e.Node.Name);
                    selectedLabel.Text = String.Format("{0} / {1}", e.Node.Text, e.Node.Name);

                    CreateBox.createDataBox.update(typeId);

                    openBig();
                }
                catch
                {
                    // error
                }
            }
        }

        static public void openSmall()
        {
            selfGroupBox.Location = new Point(selfGroupBox.Location.X, Settings.createBoxLocation[1]);
            selfGroupBox.Height = Settings.createBox[1];
            selectedLabel.Text = "-";
            CreateDataBox.selfGroupBox.Visible = false;
        }
        static public void openBig()
        {

            selfGroupBox.Location = new Point(selfGroupBox.Location.X, Settings.createBoxLocation[0]);
            selfGroupBox.Height = Settings.createBox[0];

            CreateDataBox.selfGroupBox.Visible = true;
        }

        static public void open()
        {
            openSmall();
        }
        static public void close()
        {
            selfGroupBox.Visible = false;
        }
    }
    public class CreateDataBox
    {
        private int typeId = 0, paramId = -1;
        public static GroupBox selfGroupBox;
        public static GroupBox addPointGroupBox;
        public static TrackBar paramsTrackBar;
        public static ListBox pointsListBox;
        public static Button pointsButton, createButton;
        public static TextBox textBox;

        public static Label[] paramsInterfaceLabels = new Label[4];
        public static Label[] paramsInputLabels = new Label[4];
        public static Label topLabel;

        public ArrayList points = new ArrayList();
        public CreateDataBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setListner();
        }
        private void set()
        {

            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("{0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "interfaceParams1Label":
                                paramsInterfaceLabels[0] = (Label)control;
                                break;
                            case "interfaceParams2Label":
                                paramsInterfaceLabels[1] = (Label)control;
                                break;
                            case "interfaceParams3Label":
                                paramsInterfaceLabels[2] = (Label)control;
                                break;
                            case "interfaceParams4Label":
                                paramsInterfaceLabels[3] = (Label)control;
                                break;
                            case "inputParams1Label":
                                paramsInputLabels[0] = (Label)control;
                                break;
                            case "inputParams2Label":
                                paramsInputLabels[1] = (Label)control;
                                break;
                            case "inputParams3Label":
                                paramsInputLabels[2] = (Label)control;
                                break;
                            case "inputParams4Label":
                                paramsInputLabels[3] = (Label)control;
                                break;
                            case "interfaceDataMessageLabel":
                                topLabel = (Label)control;
                                break;
                        }
                        break;
                    case "TrackBar":
                        paramsTrackBar = (TrackBar)control;
                        break;
                    case "ListBox":
                        pointsListBox = (ListBox)control;
                        break;
                    case "TextBox":
                        textBox = (TextBox)control;
                        break;
                    case "Button":
                        switch (control.Name)
                        {
                            case "button3":
                                pointsButton = (Button)control;
                                break;
                            case "createButton":
                                createButton = (Button)control;
                                break;
                        }

                        break;
                    case "GroupBox":
                        switch (control.Name)
                        {
                            case "figureGroupBox":
                                // createDataBox = new CreateDataBox((GroupBox)control);
                                break;
                        }
                        break;
                }
            }

            
        }



        public int[] getParams()
        {
            int[] res = new int[4];
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    res[i] = int.Parse(paramsInputLabels[i].Text);
                }
                catch
                {
                    if (i == 0 || i == 1) { res[i] = 1; }
                    else { res[i] = 0; }
                }
            }

            return res;
        }
        public void paramsClick(int index)
        {
            if (index != paramId)
            {
                paramId = index;
                for (int i = 0; i < 4; i++)
                {
                    Label label = paramsInputLabels[i];
                    label.BackColor = Settings.paramsStandartColor;
                }
                paramsInputLabels[index].BackColor = Settings.paramsSelectedColor;

                int[] par = getParams();
                switch (index)
                {
                    case 0:
                        paramsTrackBar.Minimum = 1;
                        paramsTrackBar.Maximum = CanvaBox.pictureBox.Width - par[2] - 1;
                        break;
                    case 1:
                        paramsTrackBar.Minimum = 1;
                        paramsTrackBar.Maximum = CanvaBox.pictureBox.Height - par[3] - 1;
                        break;
                    case 2:
                        paramsTrackBar.Minimum = 0;
                        paramsTrackBar.Maximum = CanvaBox.pictureBox.Width - par[0] - 1;
                        break;
                    case 3:
                        paramsTrackBar.Minimum = 0;
                        paramsTrackBar.Maximum = CanvaBox.pictureBox.Height - par[1] - 1;
                        break;
                }
                paramsTrackBar.Value = par[index];
                paramsInputLabels[index].Text = par[index].ToString();

            }
            else
            {
                paramId = -1;
                paramsInputLabels[index].BackColor = Settings.paramsStandartColor;
            }
        }
        public void paramsTrackBarScroll(object sender, EventArgs e)
        {
            if (paramId == -1)
            {
                paramsTrackBar.Value = paramsTrackBar.Minimum;
            }
            else
            {
                paramsInputLabels[paramId].Text = paramsTrackBar.Value.ToString();
            }
        }
        private void setListner()
        {
            for (int i = 0; i < 4; i++)
            {
                Label label = paramsInputLabels[i];
                int localIndex = i;
                label.Click += (sender, e) => paramsClick(localIndex);
            }

            paramsTrackBar.Scroll += new System.EventHandler(this.paramsTrackBarScroll);
            createButton.Click += new System.EventHandler(this.createButtonClick);
            pointsButton.Click += new System.EventHandler(this.pointsButtonClick);

        }

        public void update(int typeId)
        {
            this.typeId = typeId;

            if (typeId == 5 || typeId == 6)
            {
                pointsButton.Visible = true;
                pointsListBox.Visible = true;
                paramsTrackBar.Visible = false;
                foreach (Label label in paramsInterfaceLabels)
                {
                    label.Visible = false;
                }
                foreach (Label label in paramsInputLabels)
                {
                    label.Visible = false;
                }
            }
            else
            {
                paramsTrackBar.Visible = true;
                pointsButton.Visible = false;
                pointsListBox.Visible = false;
                foreach (Label label in paramsInterfaceLabels)
                {
                    label.Visible = true;
                }
                foreach (Label label in paramsInputLabels)
                {
                    label.Visible = true;
                }
            }


            textBox.Text = String.Format("{0}. {1}", Figures.ShapeContainer.container.Count, "Figure");

        }

        public void createButtonClick(object sender, EventArgs e)
        {
            if (Figures.ShapeContainer.nameTaken(textBox.Text))
            {
                MessageBox.Show("Name already taken!");
            }
            else
            {
                int[] par = getParams();
                string name = textBox.Text;
                Figures.Figure figure;
                switch (typeId)
                {
                    case 1:
                        figure = new Figures.Ellipse(par[2], par[3], par[1], par[0]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                    case 2:
                        figure = new Figures.Circle(par[2], par[3], par[1]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                    case 3:
                        figure = new Figures.Rectangle(par[2], par[3], par[1], par[0]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                    case 4:
                        figure = new Figures.Square(par[2], par[3], par[1]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                    case 5:
                        if (points.Count > 3)
                        {
                            figure = new Figures.Polygon(points);
                            figure.Name = name;
                            Figures.ShapeContainer.addFigure(figure);

                            CanvaBox.Draw();
                            CreateBox.openSmall();
                        }
                        else
                        {
                            MessageBox.Show(String.Format("Poligon has more then {0} points", points.Count));
                        }
                        break;
                    case 6:
                        if (points.Count == 3)
                        {
                            figure = new Figures.Triangle(points);
                            figure.Name = name;
                            Figures.ShapeContainer.addFigure(figure);

                            CanvaBox.Draw();
                            CreateBox.openSmall();
                        }
                        else
                        {
                            MessageBox.Show("Triangle has 3 points!");
                        }

                        break;

                    case 7:
                        figure = new Figures.Sun(par[2], par[3], par[1], par[1]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                    case 8:
                        figure = new Figures.Clock(par[2], par[3], par[1]);
                        figure.Name = name;
                        Figures.ShapeContainer.addFigure(figure);

                        CanvaBox.Draw();
                        CreateBox.openSmall();
                        break;
                }

            }
        }
        public void updateListBox()
        {
            pointsListBox.Items.Clear();
            foreach (Point point in points)
            {
                pointsListBox.Items.Add(String.Format("{0} : {1}", point.X, point.Y));
            }
        }
        public void pointsButtonClick(object sender, EventArgs e)
        {
            PointsBox.setPoints(points);
            PointsBox.open();
            PointsBox.setAction(setPoints);
        }


        public void setPoints(ArrayList arrayList)
        {
            this.points = arrayList;
            updateListBox();
        }

    }
    static public class CollectionBox
    {
        static public int index = -1;
        static public GroupBox selfGroupBox, dataGroupBox;
        static public TreeView treeView;
        static public Label topLabel;

        static public Label dataType,
            dataCoords, dataParams;
        static public Label interfaceDataType,
            interfaceDataCoords, interfaceDataParams;
        static public Button dataShow, dataDelete;
        static private Figures.Figure currFigure;
        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeViewAfterSelect);
            setClickListner();
        }
        static public void set()
        {

            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("CollectionBox: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "figuresTopLabel":
                                topLabel = (Label)control;
                                break;
                        }
                        break;

                    case "TreeView":
                        treeView = (TreeView)control;
                        break;
                    case "GroupBox":
                        switch (control.Name)
                        {
                            case "figuresInfoGroupBox":
                                dataGroupBox = (GroupBox)control;
                                break;
                            case "figuresGroupBox":
                                selfGroupBox = (GroupBox)control;
                                break;

                        }
                        break;
                }
            }


            foreach (Control control in dataGroupBox.Controls)
            {
                Console.WriteLine("CollectionBox Data: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "inputSettingsParams12Label":
                                dataParams = (Label)control;

                                break;
                            case "inputSettingsParams34Label":
                                dataCoords = (Label)control;
                                break;
                            case "inputSettingsTypeLabel":
                                dataType = (Label)control;
                                break;
                            case "interfaceSettingsParams34Label":
                                interfaceDataCoords = (Label)control;
                                interfaceDataCoords.Text = "X:Y";
                                break;
                            case "interfaceSettingsParams12Label":
                                interfaceDataType = (Label)control;
                                interfaceDataType.Text = "Height:Width";
                                break;
                        }
                        break;

                    case "Button":
                        switch (control.Name)
                        {
                            case "button1":
                                dataDelete = (Button)control;
                                break;
                            case "button2":
                                dataShow = (Button)control;
                                break;

                        }
                        break;
                }
            }
        }
        static public void setClickListner()
        {
            dataDelete.Click += (o, e) => dataDeleteClicked();
            dataShow.Click += (o, e) => dataShowClicked();
        }
        static public void dataDeleteClicked()
        {
            Figures.ShapeContainer.delFigure(currFigure);
            openSmall();
            update();
        }
        static public void dataShowClicked()
        {
            currFigure.Select(CanvaBox.bitmap);
            CanvaBox.updateImage(CanvaBox.bitmap);
        }
        static void updateData(Figures.Figure figure)
        {
            currFigure = figure;
            updateData();
        }
        static void updateData()
        {
            dataType.Text = currFigure.GetType().Name;
            dataCoords.Text = String.Format("( {0}:{1} )", currFigure.x, currFigure.y);
            dataParams.Text = String.Format("{0}x{1}", currFigure.width, currFigure.height);
        }
        static public void openSmall()
        {
            selfGroupBox.Visible = true;
            selfGroupBox.Location = new Point(selfGroupBox.Location.X, Settings.collectionBoxLocation[1]);
            selfGroupBox.Height = Settings.collectionBox[1];
            dataGroupBox.Visible = false;
        }
        static public void openBig()
        {
            selfGroupBox.Visible = true;
            selfGroupBox.Location = new Point(selfGroupBox.Location.X, Settings.collectionBoxLocation[0]);
            selfGroupBox.Height = Settings.collectionBox[0];

            dataGroupBox.Visible = true;
        }
        static public void close()
        {
            selfGroupBox.Visible = false;
        }

        static public void update()
        {
            treeView.Nodes.Clear();
            foreach (Figures.Figure figure in Figures.ShapeContainer.container)
            {
                Console.WriteLine("figure {0}", figure.Name);
                TreeNode treeNodeParent = new TreeNode(figure.GetType().Name);
                TreeNode treeNodeChild = new TreeNode(figure.Name);
                bool flag = true;
                foreach (TreeNode treeNode in treeView.Nodes)
                {
                    if (treeNode.Text == treeNodeParent.Text)
                    {
                        treeNode.Nodes.Add(treeNodeChild);
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    treeNodeParent.Nodes.Add(treeNodeChild);
                    treeView.Nodes.Add(treeNodeParent);
                }
            }
        }

        static public void treeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0)
            {
                openBig();
                int index = Figures.ShapeContainer.indexByName(e.Node.Text);
                if (index >= 0)
                {
                    updateData((Figures.Figure)Figures.ShapeContainer.container[index]);
                }
            }
            else
            {
                e.Node.Expand();
            }
        }
    }
    public class CollectoinDataBox
    {
        private GroupBox selfGroupBox;
        public CollectoinDataBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
        }

        private void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("CollectionDataBox: {0} {1}", control.GetType().Name, control.Name);
                //switch (control.GetType().Name)
                //{
                //    case "Label":
                //        switch (control.Name)
                //        {
                //            case "figuresTopLabel":
                //                topLabel = (Label)control;
                //                break;
                //        }
                //        break;

                //    case "TreeView":
                //        treeView = (TreeView)control;
                //        break;
                //    case "GroupBox":
                //        switch (control.Name)
                //        {
                //            case "figuresInfoGroupBox":
                //                dataGroupBox = (GroupBox)control;
                //                break;
                //            case "figuresGroupBox":
                //                selfGroupBox = (GroupBox)control;
                //                break;

                //        }
                //        break;
                //}
            }
        }

    }
    static public class PointsBox
    {
        static public ArrayList points = new ArrayList();
        static public GroupBox selfGroupBox, pointDataBox;
        static public Label[] paramsInterfaceLabels = new Label[1];
        static public TrackBar[] trackBars = new TrackBar[2];
        static public ListBox listBox;

        static public Action<ArrayList> action;

        static private int currPointNum = 0;
        static public bool opened;

        static public Button selectButton, addPointButton, closePointButton;
        static public Button savePointButton, showPointButton;
        static public Label pointNum, pointName, pointX, pointY;

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            opened = false;
            set();
            setClickListner();
        }
        static public void set()
        {

            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("PointsBox: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "interfacePointsCloseLabel":
                                paramsInterfaceLabels[0] = (Label)control;
                                break;
                        }
                        break;
                    case "TrackBar":
                        if (control.Name == "trackBar1") { trackBars[0] = (TrackBar)control; }
                        else { trackBars[1] = (TrackBar)control; }
                        break;
                    case "ListBox":
                        listBox = (ListBox)control;
                        break;
                    case "Button":
                        switch (control.Name)
                        {
                            case "pointsAddButton":
                                addPointButton = (Button)control;
                                break;
                            case "pointsCloseButton":
                                closePointButton = (Button)control;
                                break;
                        }
                        break;
                    case "GroupBox":
                        switch (control.Name)
                        {
                            case "groupBox2":
                                pointDataBox = (GroupBox)control;
                                break;
                        }
                        break;
                }
            }





            foreach (Control control in pointDataBox.Controls)
            {
                Console.WriteLine("     pointDataBox: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "label7":
                                pointName = (Label)control;
                                break;
                            case "label8":
                                pointX = (Label)control;
                                break;
                            case "label9":
                                pointY = (Label)control;
                                break;
                            case "label11":
                                pointNum = (Label)control;
                                break;
                        }
                        break;
                    case "Button":
                        switch (control.Name)
                        {
                            case "pointsSavePointButton":
                                savePointButton = (Button)control;
                                break;
                            case "pointsShowPointButton":
                                showPointButton = (Button)control;
                                break;
                        }
                        break;
                }
            }

        }
        static public void setClickListner()
        {
            addPointButton.Click += (o, e) => addPointButtonClicked();
            closePointButton.Click += (o, e) => closePointButtonClicked();
            trackBars[0].Scroll += (o, e) => trackBarsScroll(0);
            trackBars[1].Scroll += (o, e) => trackBarsScroll(1);

            savePointButton.Click += (o, e) => savePointButtonClicked();

            listBox.Click += (o, e) => listBoxClicked();
        }
        public static void setAction(Action<ArrayList> a)
        {
            action = a;
        }
        static public void addPointButtonClicked()
        {
            points.Add(new Point(0, 0));
            currPointNum = points.Count - 1;
            update();
        }
        static public void closePointButtonClicked()
        {
            try
            {
                action(points);
            }
            catch
            {

            }
            close();
        }
        static public void open()
        {
            int x = CanvaBox.selfGroupBox.Location.X + 20,
                y = CanvaBox.selfGroupBox.Location.Y + 20,
                w = CanvaBox.selfGroupBox.Width - 40,
                h = CanvaBox.selfGroupBox.Height - 40;

            open(x, y, w, h);
            listBox.Location = new Point(
                25, listBox.Location.Y
            );
            listBox.Width = pointDataBox.Location.X - listBox.Location.X * 2;
            addPointButton.Location = new Point(
                listBox.Location.X + (listBox.Width - addPointButton.Width) / 2,
                listBox.Location.Y + listBox.Height + 10
            );
        }
        static public void openCentered(int x, int y)
        {
            x = x - (selfGroupBox.Width / 2);
            y = y - (selfGroupBox.Height / 2);
            open(x, y);
        }
        static public void open(int x, int y, int w, int h)
        {
            open(x, y);
            selfGroupBox.Width = w;
            selfGroupBox.Height = h;
        }
        static public void open(int x, int y)
        {
            selfGroupBox.Visible = true;
            selfGroupBox.Location = new Point(x, y);
            opened = true;
        }
        static public void close()
        {
            selfGroupBox.Visible = false;
            opened = false;
        }


        static public void update(ArrayList arrayList)
        {
            points = arrayList;
            update();
        }
        static public void update()
        {
            listBox.Items.Clear();


            int maxX = 0, maxY = 0;
            foreach (Point point1 in points)
            {
                listBox.Items.Add(String.Format("x:{0}, y:{1}", point1.X, point1.Y));
                if (maxX < point1.X) { maxX = point1.X; }
                if (maxY < point1.Y) { maxY = point1.Y; }
            }
            try
            {
                Point point = (Point)points[currPointNum];


                pointNum.Text = currPointNum.ToString();
                pointName.Text = String.Format("Point( {0}, {1} )", point.X, point.Y);
                pointX.Text = point.X.ToString();
                pointY.Text = point.Y.ToString();

                if (CanvaBox.pictureBox.Width > maxX)
                {
                    trackBars[0].Maximum = CanvaBox.pictureBox.Width;
                }
                else
                {
                    trackBars[0].Maximum = maxX;
                }

                if (CanvaBox.pictureBox.Height > maxY)
                {
                    trackBars[1].Maximum = CanvaBox.pictureBox.Height;
                }
                else
                {
                    trackBars[1].Maximum = maxY;
                }

                trackBars[0].Value = point.X;
                trackBars[1].Value = point.Y;
            }
            catch { }

        }
        public static void trackBarsScroll(int index)
        {
            if (index == 0)
            {
                pointX.Text = trackBars[0].Value.ToString();
            }
            else
            {
                pointY.Text = trackBars[1].Value.ToString();
            }
        }
        public static void savePointButtonClicked()
        {
            try
            {
                points[currPointNum] = new Point(int.Parse(pointX.Text), int.Parse(pointY.Text));

                update();

                foreach (Figures.Figure figure in Figures.ShapeContainer.container)
                {
                    figure.changeRange();
                }
            }
            catch
            {

            }
        }
        public static void listBoxClicked()
        {
            try
            {
                currPointNum = listBox.SelectedIndex;
                update();
            }
            catch
            {

            }
        }

        public static void setPoints(ArrayList arrayList)
        {
            points = arrayList;
        }

    }
    public class FigureSettingsBox
    {
        public GroupBox selfGroupBox;
        public Label paramHLabel, paramWLabel, paramXLabel, paramYLabel, paramTypeLabel, paramNameLabel;
        public Label interfaceHLabel, interfaceWLabel, interfaceXLabel, interfaceYLabel;
        public Label cancelLabel;
        public Button editButton, deleteButton;
        public Figures.Figure currFigure;

        public FigureSettingsBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setClickListner();
        }
        private void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("FigureSettingsBox : {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "settingsCloseLabel":
                                cancelLabel = (Label)control;
                                break;
                            case "figureTypeSettingsLabel":
                                paramTypeLabel = (Label)control;
                                break;
                            case "paramHLabel":
                                paramHLabel = (Label)control;
                                break;
                            case "paramWLabel":
                                paramWLabel = (Label)control;
                                break;
                            case "paramXLabel":
                                paramXLabel = (Label)control;
                                break;
                            case "paramYLabel":
                                paramYLabel = (Label)control;
                                break;

                            case "label15":
                                interfaceHLabel = (Label)control;
                                break;
                            case "label16":
                                interfaceWLabel = (Label)control;
                                break;
                            case "label17":
                                interfaceXLabel = (Label)control;
                                break;
                            case "label18":
                                interfaceYLabel = (Label)control;
                                break;
                        }
                        break;
                    case "GroupBox":
                        foreach (Control control1 in ((GroupBox)control).Controls)
                        {
                            switch (control1.GetType().Name)
                            {
                                case "Button":
                                    switch (control1.Name)
                                    {
                                        case "settingsEditButton":
                                            editButton = (Button)control1;
                                            break;
                                        case "settingsDeleteButton":
                                            deleteButton = (Button)control1;
                                            break;

                                    }
                                    break;
                            }
                        }
                        break;
                    case "Button":
                        switch (control.Name)
                        {
                            case "figureGroupBox":
                                // createDataBox = new CreateDataBox((GroupBox)control);
                                break;
                        }
                        break;
                }


            }
            interfaceHLabel.Text = "Height: ";
            interfaceWLabel.Text = "Width: ";
            interfaceXLabel.Text = "X: ";
            interfaceYLabel.Text = "Y: ";
        }
        public void update(Figures.Figure figure)
        {
            currFigure = figure;
            update();
        }
        public void update()
        {
            paramTypeLabel.Text = currFigure.Name;
            paramHLabel.Text = currFigure.height.ToString();
            paramWLabel.Text = currFigure.width.ToString();
            paramXLabel.Text = currFigure.x.ToString();
            paramYLabel.Text = currFigure.y.ToString();
        }
        public void open(int x, int y)
        {
            selfGroupBox.Visible = true;
            selfGroupBox.Location = new Point(
                x, y + 12
            );
        }
        public void close()
        {
            selfGroupBox.Visible = false;
        }
        private void setClickListner()
        {
            editButton.Click += new System.EventHandler(this.editButtonClicked);
            deleteButton.Click += new System.EventHandler(this.deleteButtonClicked);
            cancelLabel.Click += new System.EventHandler(this.cancelLabelClicked);
        }

        public void editButtonClicked(object o, EventArgs e)
        {
            EditFigureBox.open(
                CanvaBox.selfGroupBox.Location.X + CanvaBox.selfGroupBox.Width / 2,
                CanvaBox.selfGroupBox.Location.Y + CanvaBox.selfGroupBox.Height / 2
                );
            EditFigureBox.update(currFigure);
        }
        public void cancelLabelClicked(object o, EventArgs e)
        {
            close();
        }
        public void deleteButtonClicked(object o, EventArgs e)
        {
            if (Figures.ShapeContainer.container.Contains(currFigure))
            {
                int id = Figures.ShapeContainer.container.IndexOf(currFigure);
                Figures.ShapeContainer.delVisible(id);
                CanvaBox.Draw();
                close();
            }
        }
    }
    public class FigureMultiplySettingsBox
    {
        public GroupBox selfGroupBox;
        private ListBox listBox;
        private Button button;
        private Label label;
        private int[] coords = new int[2];
        private bool flag = false;
        private Figures.Figure currFigure;

        public FigureMultiplySettingsBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setClickListner();
        }
        private void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("MyltiplySettings: {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        label = (Label)control;
                        break;
                    case "Button":
                        button = (Button)control;
                        break;
                    case "ListBox":
                        listBox = (ListBox)control;
                        break;
                }
            }
        }
        private void setClickListner()
        {
            button.Click += new System.EventHandler(buttonClick);
            listBox.Click += new System.EventHandler(listBoxClick);
        }
        public void buttonClick(object o, EventArgs e)
        {

            if (flag)
            {
                CanvaBox.figureSettings.open(coords[0] + CanvaBox.labels[3].Width, coords[1] + CanvaBox.labels[0].Height);
                CanvaBox.figureSettings.update(currFigure);
            }
            close();
        }
        public void listBoxClick(object o, EventArgs e)
        {
            button.Text = "Open";
            flag = true;
            int index;
            try
            {
                index = Figures.ShapeContainer.indexByName(listBox.SelectedItem.ToString());
            }
            catch
            {
                index = 0;
            }

            if (index >= 0)
            {
                currFigure = (Figures.Figure)Figures.ShapeContainer.container[index];
            }
        }
        public void update(ArrayList figures)
        {
            listBox.Items.Clear();
            foreach (Figures.Figure figure in figures)
            {
                listBox.Items.Add(figure.Name);
            }
        }
        public void openCentered(int x, int y)
        {
            selfGroupBox.Location = new Point(
                x - selfGroupBox.Width / 2 + CanvaBox.labels[0].Height,
                y - selfGroupBox.Height / 2 + CanvaBox.labels[3].Width
            );
        }
        public void open(int x, int y)
        {
            coords[0] = x; coords[1] = y;
            button.Text = "Cancel";
            selfGroupBox.Visible = true;
            openCentered(x, y);
            flag = false;
        }
        public void close()
        {
            selfGroupBox.Visible = false;
            flag = false;
        }
    }
    static public class EditFigureBox
    {
        static public GroupBox selfGroupBox;
        static public Label[][] paramsLabels = new Label[][] { new Label[4], new Label[6] };
        static public Label[] trackBarLabels = new Label[3];
        static public TrackBar paramsTrackBar;
        static public Label[] actionLabels = new Label[3];
        static public int currentIndex = -1;
        static public Panel pointsPanel;

        static public Figures.Figure figure;

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setClickListner();
        }
        static public void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("FigureSettingsBox : {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        switch (control.Name)
                        {
                            case "interfaceEditHLabel":
                                paramsLabels[0][0] = (Label)control;
                                break;

                            case "interfaceEditWLabel":
                                paramsLabels[0][1] = (Label)control;
                                break;
                            case "interfaceEditXLabel":
                                paramsLabels[0][2] = (Label)control;
                                break;
                            case "interfaceEditYLabel":
                                paramsLabels[0][3] = (Label)control;
                                break;


                            case "outputEditHLabel":
                                paramsLabels[1][0] = (Label)control;
                                break;
                            case "outputEditWLabel":
                                paramsLabels[1][1] = (Label)control;
                                break;
                            case "outputEditXLabel":
                                paramsLabels[1][2] = (Label)control;
                                break;
                            case "outputEditYLabel":
                                paramsLabels[1][3] = (Label)control;
                                break;

                            case "outputEditNameLabel":
                                paramsLabels[1][4] = (Label)control;
                                break;
                            case "outputEditTypeLabel":
                                paramsLabels[1][5] = (Label)control;
                                break;

                        }
                        break;
                    case "GroupBox":

                        foreach (Control control1 in ((GroupBox)control).Controls)
                        {
                            Console.WriteLine("Control1 : {0} {1}", control1.GetType().Name, control1.Name);
                            switch (control1.GetType().Name)
                            {

                                case "TrackBar":
                                    switch (control1.Name)
                                    {
                                        case "editParamsTrackBar":
                                            paramsTrackBar = (TrackBar)control1;
                                            break;
                                    }
                                    break;
                                case "Label":
                                    switch (control1.Name)
                                    {
                                        case "editMaxTrackBar":
                                            trackBarLabels[0] = (Label)control1;
                                            break;
                                        case "editMinTrackBar":
                                            trackBarLabels[1] = (Label)control1;
                                            break;
                                        case "label20":
                                            trackBarLabels[2] = (Label)control1;
                                            break;
                                    }
                                    break;
                                    //case "Panel":
                                    //    foreach(Control control2 in control1.Controls)
                                    //    {
                                    //        switch (control2.Name)
                                    //        {
                                    //            case "label29":
                                    //                actionLabels[0] = (Label)control2;
                                    //                break;
                                    //            case "label30":
                                    //                actionLabels[1] = (Label)control2;
                                    //                break;
                                    //            case "label31":
                                    //                actionLabels[2] = (Label)control2;
                                    //                break;
                                    //        }
                                    //    }
                                    //    break;
                            }
                        }
                        break;
                    case "Button":
                        switch (control.Name)
                        {
                            case "figureGroupBox":
                                // createDataBox = new CreateDataBox((GroupBox)control);
                                break;
                        }
                        break;
                    case "Panel":
                        if (control.Name == "editPointsPanel")
                        {
                            pointsPanel = (Panel)control;
                        }

                        foreach (Control control2 in control.Controls)
                        {
                            switch (control2.Name)
                            {
                                case "label29":
                                    actionLabels[0] = (Label)control2;
                                    break;
                                case "label30":
                                    actionLabels[1] = (Label)control2;
                                    actionLabels[1].Text = "Select";
                                    break;
                                case "label31":
                                    actionLabels[2] = (Label)control2;
                                    break;
                            }
                        }
                        break;
                }


            }
            paramsLabels[0][0].Text = "Height : ";
            paramsLabels[0][1].Text = "Width : ";
            paramsLabels[0][2].Text = "X : ";
            paramsLabels[0][3].Text = "Y : ";
        }
        static public void setClickListner()
        {
            for (int i = 0; i < actionLabels.Length; i++)
            {
                int index = i;
                actionLabels[index].MouseLeave += (o, e) => actionLabelsLeave(o, e, index);
                actionLabels[index].MouseMove += (o, e) => actionLabelsMove(o, e, index);
                actionLabels[index].MouseClick += (o, e) => actionLabelsClick(o, e, index);
            }
            for (int i = 0; i < paramsLabels[1].Length - 2; i++)
            {
                int index = i;
                paramsLabels[1][i].Click += (o, e) => paramsLabelsClicked(o, e, index);
            }
            paramsTrackBar.Scroll += (o, e) => paramsTrackBarScroll(o, e);


        }


        static public void update(Figures.Figure figure)
        {
            EditFigureBox.figure = figure;
            update();
        }
        static public void update()
        {
            paramsLabels[1][0].Text = figure.height.ToString();
            paramsLabels[1][1].Text = figure.width.ToString();
            paramsLabels[1][2].Text = figure.x.ToString();
            paramsLabels[1][3].Text = figure.y.ToString();
            paramsLabels[1][4].Text = figure.Name;
            paramsLabels[1][5].Text = figure.GetType().Name;
            pointGenPanel();
        }
        static public void pointGenPanel()
        {
            pointsPanel.Controls.Clear();
            int s = 10;
            for (int i = 0; i < figure.points.Count; i++)
            {
                s += 20;
                Point point = (Point)figure.points[i];
                Label label = new Label();
                pointsPanel.Controls.Add(label);

                label.AutoSize = true;
                label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                label.Location = new System.Drawing.Point(10, s);
                label.Name = "labelPointsPanel";
                label.Size = new System.Drawing.Size(70, 20);
                label.TabIndex = 13;
                label.Text = String.Format("Point( {0}, {1} )", point.X, point.Y);

            }

            if (figure.points.Count > 0)
            {
                Button button = new Button();
                pointsPanel.Controls.Add(button);
                button.Location = new System.Drawing.Point(pointsPanel.Width / 8, s + 30);
                button.Name = "pointsEditButton";
                button.Size = new System.Drawing.Size(pointsPanel.Width / 2, 23);
                button.TabIndex = 15;
                button.Text = "Edit";
                button.UseVisualStyleBackColor = true;
                button.Click += (o, e) => editButtonClicked();
            }



        }
        static public void editButtonClicked()
        {
            PointsBox.update(figure.points);
            PointsBox.open();
            PointsBox.setAction(EditFigureBox.setPoints);
        }
        static public void setPoints(ArrayList arrayList)
        {
            figure = new Figures.Polygon(arrayList);
            update();
        }
        static public void openCentred(int x, int y)
        {
            open(x - selfGroupBox.Width / 2, y - selfGroupBox.Height / 2);
        }
        static public void open(int x, int y)
        {
            selfGroupBox.Location = new Point(x, y + 10);
            selfGroupBox.Visible = true;
        }
        static public void close()
        {
            selfGroupBox.Visible = false;
            for (int i = 0; i < paramsLabels[1].Length - 2; i++)
            {
                Label label = paramsLabels[1][i];

                label.Text = "Click";
                label.BackColor = Settings.paramsStandartColor;
            }
        }


        static public void actionLabelsMove(object o, EventArgs e, int index)
        {
            actionLabels[index].BackColor = Settings.editSelectedButtonColor[index];
        }
        static public void actionLabelsLeave(object o, EventArgs e, int index)
        {
            actionLabels[index].BackColor = selfGroupBox.BackColor;
        }
        static public void actionLabelsClick(object o, EventArgs e, int index)
        {
            switch (index)
            {
                case (0):
                    int[] par = getParams();
                    figure.MoveTo(par[2], par[3]);

                    CanvaBox.Draw();
                    update();
                    break;
                case (1):
                    figure.Select(CanvaBox.bitmap);
                    CanvaBox.updateImage(CanvaBox.bitmap);
                    break;
                case (2):
                    close();
                    break;
            }
        }

        static public int[] getParams()
        {
            int[] res = new int[4];
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    res[i] = int.Parse(paramsLabels[1][i].Text);
                }
                catch
                {
                    if (i == 0 || i == 1) { res[i] = 1; }
                    else { res[i] = 0; }
                }
            }
            return res;
        }
        static public void paramsLabelsClicked(object o, EventArgs e, int index)
        {
            int[] paramsint = getParams();
            for (int i = 0; i < paramsLabels[1].Length - 2; i++)
            {
                paramsLabels[1][i].BackColor = Settings.paramsStandartColor;
            }

            if (currentIndex != index)
            {
                paramsLabels[1][index].Text = paramsint[index].ToString();
                paramsLabels[1][index].BackColor = Settings.paramsSelectedColor;
            }
            else { currentIndex = -1; }

            switch (index)
            {
                case 0:
                    paramsTrackBar.Minimum = 1;
                    paramsTrackBar.Maximum = CanvaBox.pictureBox.ClientSize.Height - paramsint[3] - 1;
                    break;
                case 1:
                    paramsTrackBar.Minimum = 1;
                    paramsTrackBar.Maximum = CanvaBox.pictureBox.ClientSize.Width - paramsint[2] - 1;
                    break;
                case 2:
                    paramsTrackBar.Minimum = 0;
                    paramsTrackBar.Maximum = CanvaBox.pictureBox.ClientSize.Width - paramsint[1] - 1;
                    break;
                case 3:
                    paramsTrackBar.Minimum = 0;
                    paramsTrackBar.Maximum = CanvaBox.pictureBox.ClientSize.Height - paramsint[0] - 1;
                    break;
            }


            try
            {
                paramsTrackBar.Value = paramsint[index];
            }
            catch
            {

            }

            trackBarLabels[0].Text = paramsTrackBar.Maximum.ToString();
            trackBarLabels[1].Text = paramsTrackBar.Minimum.ToString();
            trackBarLabels[2].Text = paramsTrackBar.Value.ToString();

            currentIndex = index;
        }
        static public void paramsTrackBarScroll(object o, EventArgs e)
        {
            try
            {
                paramsLabels[1][currentIndex].Text = paramsTrackBar.Value.ToString();
                trackBarLabels[2].Text = paramsTrackBar.Value.ToString();
            }
            catch
            {

            }
        }
    }






    static public class CommandLineBox
    {
        static public GroupBox selfGroupBox;

        static public Label label;
        static public TextBox commandLine;
        static public RichTextBox historyTextBox;
        static public Panel historyPanel;
        static public List<Button> historyButtons = new List<Button>();
        static public TrackBar trackBar;
        static public CommandLine commandLineHelper = new CommandLine();

        static private List<string> history = new List<string>();
        static private List<string> historyDesc = new List<string>();
        static private List<bool> historyFlag = new List<bool>();
        static private int currentCommand = 0;

        static public string desc = "";
        static public bool flag = false;

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setClickListner();
        }
        static public void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("CommandLineBox : {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        label = (Label)control;
                        break;
                    case "Panel":
                        historyPanel = (Panel)control;
                        break;
                    case "TextBox":
                        commandLine = (TextBox)control;
                        break;
                    case "RichTextBox":
                        historyTextBox = (RichTextBox)control;
                        break;
                    case "Button":
                        historyButtons.Add((Button)control);
                        break;
                    case "TrackBar":
                        trackBar = (TrackBar)control;
                        break;
                }


            }
        }
        static public void setClickListner()
        {
            commandLine.KeyDown += new KeyEventHandler( textBox_KeyDown );
            historyButtons[0].Click += (s, o) => nextCommand();
            historyButtons[1].Click += (s, o) => lastCommand();
            trackBar.ValueChanged += (s, o) => trackBarValueChanged();
        }

        static public  void trackBarValueChanged()
        {
            if (history.Count > 0 && ( trackBar.Value >= 0 || trackBar.Value < history.Count ))
            {
                currentCommand = trackBar.Value;
                updateHistoryBox();
            }
            if (currentCommand <= trackBar.Maximum && currentCommand >= trackBar.Minimum)
            {
                trackBar.Value = currentCommand;
            }
        }
        static public void nextCommand()
        {
            if (currentCommand < history.Count)
            {
                currentCommand++;
                updateHistoryBox();
            }
            if (currentCommand <= trackBar.Maximum && currentCommand >= trackBar.Minimum)
            {
                trackBar.Value = currentCommand;
            }

        }
        static public void lastCommand()
        {
            if (currentCommand > 0)
            {
                currentCommand--;
                updateHistoryBox();
            }
            if (currentCommand <= trackBar.Maximum && currentCommand >= trackBar.Minimum)
            {
                trackBar.Value = currentCommand;
            }
        }
        static public void updateHistoryBox()
        {
            if (currentCommand >= 0 && currentCommand < history.Count)
            {
                historyTextBox.Text = String.Format("{0}\n- - - - - - - - - - - - - - - - - - - - - - - - - -\n{1}", history[currentCommand], historyDesc[currentCommand]);
            }
        }
        static public void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter pressed
                if (commandLine.Text != "")
                {
                    // MessageBox.Show("Enter");

                    currentCommand++;
                    
                    commandLineHelper.ob(commandLine.Text);
                    checking(commandLineHelper);
                    addCommand(commandLine.Text, desc, flag);

                    commandLine.Text = "";

                    CanvaBox.Draw();
                    //ListViewItem item = new ListViewItem();
                    //item.Text = commandLine.Text;
                    //Form1.historyListView.Items.Add(item);
                }



            }
            else if (e.KeyCode == Keys.Down)
            {
                // Key Down pressed
                // MessageBox.Show("Key Down");
                if (currentCommand < history.Count)
                {
                    currentCommand++;
                    if (currentCommand >= 0 && currentCommand < history.Count) {
                        commandLine.Text = history[currentCommand];
                    }
                }

            }
            else if (e.KeyCode == Keys.Up)
            {
                // Key Up pressed
                // MessageBox.Show("Key Up");
                if(currentCommand > 0)
                {
                    currentCommand--;
                    if (currentCommand >= 0 && currentCommand < history.Count) 
                    { 
                        commandLine.Text = history[currentCommand]; 
                    }
                }
            }
            else
            {

            }
        }
        
        static public void open() {
            selfGroupBox.Visible = true;
        }
        static public void close() { 
            selfGroupBox.Visible = false;
        }

        static public void addCommand(string command, string desc = "...", bool flag = false)
        {
            history.Add(command);
            historyDesc.Add(desc);
            historyFlag.Add(flag);

            currentCommand = history.Count - 1;
            updateHistoryBox();
            
            while (trackBar.Maximum >= history.Count)
            {
                trackBar.Maximum--;
            }
            if (history.Count > 1)
                trackBar.Maximum = history.Count - 1;
                trackBar.Value = history.Count - 1;
        }


        static public void checking(CommandLine commandLine)
        {
            Stack<Operand> operands = commandLine.getOperands();
            Stack<Operator> operators = commandLine.getOperators();
            List<Operand> arrayOperands = new List<Operand>();
            List<Operator> arrayOperators = new List<Operator>();

            while (true)
            {
                bool a = false, b = false;

                try
                {
                    Operand operandLocal = operands.Pop();
                    if (operandLocal.Equals(null))
                    {

                    }
                    else if(operandLocal.value.ToString() != " ")
                    {
                        arrayOperands.Add(operandLocal);
                    }
                }
                catch
                {
                    a = true;
                }


                try
                {
                    Operator operatorLocal = operators.Pop();
                    if (operatorLocal == null)
                    {

                    }
                    else if (operatorLocal.symbolOperator != ' ')
                    {
                        arrayOperators.Add(operatorLocal);
                    }
                }
                catch
                {
                    b = true;
                }

                if (a && b)
                {
                    break;
                }
            }

            Operand operand = arrayOperands[arrayOperands.Count - 1];
            Operator operatorT = arrayOperators[arrayOperators.Count - 1];
            desc = "template";
            flag = false;

            if (!operatorT.Equals(null))
            {

                string name = operatorT.symbolOperator.ToString();
                string figureName = arrayOperands[arrayOperands.Count - 1].value.ToString();
                switch (name)
                {
                    case ("O"):
                        if (Figures.ShapeContainer.nameTaken(figureName))
                        {
                            desc = "Name already taken";
                        }
                        else if (arrayOperands.Count != 5)
                        {
                            desc = "Number if operands not 5";
                        }
                        else
                        {
                            desc = "Clock was added";
                            Figures.Clock clock = new Figures.Clock((int)arrayOperands[3].value, (int)arrayOperands[2].value, (int)arrayOperands[1].value);
                            clock.Name = figureName;
                            Figures.ShapeContainer.addFigure(clock);

                        }

                        break;
                    case ("M"):
                        if (!Figures.ShapeContainer.nameTaken(figureName))
                        {
                            desc = "Figure with this Name doesn`t exist";
                        } else if ( arrayOperands.Count != 3 && arrayOperands.Count != 5)
                        {
                            desc = "Format -> M(\"name\", x, y) or M(\"name\", x, y, w, h)";
                        }
                        else
                        {
                            desc = "Figure was moved";
                            int index = Figures.ShapeContainer.indexByName(figureName);
                            Figures.Figure fig = (Figures.Figure)Figures.ShapeContainer.container[index];
                            fig.MoveTo(
                                (int) arrayOperands[arrayOperands.Count - 3].value, 
                                (int) arrayOperands[arrayOperands.Count - 2].value
                                );
                        }
                        break;
                    case ("D"):

                        if (Figures.ShapeContainer.nameTaken(figureName))
                        {
                            int index = Figures.ShapeContainer.indexByName(figureName);
                            if (index >= 0)
                            {
                                desc = "Figure was deleted";
                                Figures.ShapeContainer.delVisible(index);
                            }

                        }
                        else { desc = "This figure not exist"; }

                        break;
                    case ("C"):
                        desc = "";
                        if (arrayOperands.Count == 0)
                        {
                            desc = "Format: C({0-255}, {0-255}, {0-255})";
                        } else if ( arrayOperands.Count > 3 )
                        {
                            desc = "Format: C({0-255}, {0-255}, {0-255})";
                        } else
                        {
                            desc = "Color changed";
                            
                            int temp;
                            int[] rgb = new int[3];
                            for (int i=0; i < 3; i++)
                            {
                                if (arrayOperands.Count > 3 - i - 1)
                                {
                                    if (int.TryParse(arrayOperands[3 - i - 1].value.ToString(), out temp))
                                    {
                                        rgb[i] = temp % 255;
                                    }
                                    else
                                    {
                                        rgb[i] = 0;
                                    }
                                } else { rgb[i] = 0; }
                                
                            }
                            
                            Figures.ShapeContainer.changePen(new Pen(Color.FromArgb(255, rgb[0], rgb[1], rgb[2]), 5));
                            

                            
                        }
                        break;

                    default:
                        desc = String.Format( 
                            "There is no \"{0}\" command", 
                            commandLine.getString(commandLine.command, 0, '(')
                            );
                        break;
                }


            }
            Figures.ShapeContainer.DrawAll(CanvaBox.bitmap);

            print(arrayOperands); 
            print(arrayOperators); 

        }

        static private void print(List<Operator> lo)
        {
            foreach (Operator ob in lo)
            {
                Console.Write("({0}) ", ob.symbolOperator);
            }
            Console.WriteLine();
        }
        static private void print(List<Operand> lo)
        {
            foreach (Operand ob in lo)
            {
                Console.Write("({0}) ", ob.value);
            }
            Console.WriteLine();
        }

    }


    static public class InputTypeBox
    {
        static public GroupBox selfGroupBox;

        static public TrackBar trackBar;
        static public Label label;

        static public void setSelfGroupBox(GroupBox groupBox)
        {
            selfGroupBox = groupBox;
            set();
            setClickListner();
        }
        static public void set()
        {
            foreach (Control control in selfGroupBox.Controls)
            {
                Console.WriteLine("FigureSettingsBox : {0} {1}", control.GetType().Name, control.Name);
                switch (control.GetType().Name)
                {
                    case "Label":
                        label = (Label)control;
                        break;
                    case "TrackBar":
                        trackBar = (TrackBar)control;
                        break;
                }


            }
        }
        static public void setClickListner()
        {
            trackBar.ValueChanged += (e, o) => changedTrackBar();
        }

        static public void changedTrackBar()
        {
            if (trackBar.Value == 0)
            {
                label.Text = "CLI";
                // Command Line
                CommandLineBox.open();

                CreateBox.close();
                CollectionBox.close();
                PointsBox.close();

            }
            else
            {
                label.Text = "GUI";

                CommandLineBox.close();

                CreateBox.openSmall();
                CollectionBox.openSmall(); 
            }
        }





    }
}
