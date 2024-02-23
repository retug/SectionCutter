using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ETABSv1;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;
using LiveCharts;
using LiveCharts.Wpf;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;
using LiveCharts.Helpers;
using System.Windows.Controls;

namespace SectionCutter
{
    public partial class Form1 : Form
    {
        private cPluginCallback _Plugin = null;
        private cSapModel _SapModel = null;

        //initiate lists
        List<SelectedObjects> SelectedObjectsList;
        List<LoadCase> LoadCaseList;

        List<AreaPoint> AreaPointList;
        List<List<AreaPoint>> AreaPointListStrucIntact;
        List<AreaPoint> tempAreaPointList;

        List<Line> AreaTempLineList;
        List<Line> AreaTempGloLineList;

        List<ETABS_Point> ETABsAreaPointsList;
        List<ETABS_Point> ETABSAReaPointsListGlo;
        List<MyPoint> MyPoints;
        List<MyPoint> MyTempPoints;


        // lists of lines from selected polygons
        List<List<Line>> AreaLineList;
        List<List<MyPoint>> MyGlobalAreaPoints;
        List<List<Line>> AreaLineListGlo;
        List<MyPoint> AreaTempPointList;

        //the unique label of the starting point in ETABs
        string startPoint = null;
        VectorValue vectorXValue = new VectorValue();
        VectorValue vectorYValue = new VectorValue();
        List<string> selectedLoadSteps;





        public Form1(ref cSapModel SapModel, ref cPluginCallback Plugin)
        {
            _Plugin = Plugin;
            _SapModel = SapModel;
            InitializeComponent();
            this.Load += Form1_Load;
            // Load Steps that Want to be plotted
            selectedLoadSteps = new List<string>();
            
            // Attach the SelectionChangeCommitted event handler
            listBoxLoadSteps.SelectedIndexChanged += ListBox_SelectedIndexChanged;
            
        }

        private void Form1_Paint(object send, PaintEventArgs e)
        {
            Graphics mgraphics = e.Graphics;
            Pen pen = new Pen(Color.FromArgb(255, 140, 105), 1);
            Rectangle area = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            System.Drawing.Drawing2D.LinearGradientBrush lGB2 = new System.Drawing.Drawing2D.LinearGradientBrush(area, Color.FromArgb(255, 255, 255), Color.FromArgb(159, 159, 159), LinearGradientMode.Vertical);
            mgraphics.FillRectangle(lGB2, area);
            mgraphics.DrawRectangle(pen, area);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // do setup things here
            this.Paint -= Form1_Paint;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            // must include a call to finish()
            _Plugin.Finish(0);
        }

        private void ShowLoadCase_Load(object sender, EventArgs e)
        {
            // Clear the existing items in LoadCaseList and LoadCaseComBox
            if (LoadCaseList != null)
            {
                LoadCaseList.Clear();
                LoadCaseComBox.Items.Clear();
            }


            int NumberNames = 1;
            string[] MyName = null;
            int NumberItems = 0;
            string[] CaseName = null;
            int[] status = null;

            _SapModel.LoadCases.GetNameList(ref NumberNames, ref MyName);
            _SapModel.Analyze.GetCaseStatus(ref NumberItems, ref CaseName, ref status);

            LoadCaseList = new List<LoadCase>();

            for (int i = 0; i < MyName.Length; i++)
            {
                LoadCase LComb = new LoadCase();
                LComb.NumberNames = NumberNames;
                LComb.MyName = MyName[i];
                LComb.Status = status[i];

                //This is used to determine if the load case is linear static, if it is, check to see if there is an auto seismic/wind case.
                
                string Name = MyName[i];
                eLoadCaseType CaseType = new eLoadCaseType();
                int SubType = 0;
                eLoadPatternType DesignType = new eLoadPatternType();
                int DesignTypeOption = 0;
                int Auto = 0;

                _SapModel.LoadCases.GetTypeOAPI_1(Name, ref CaseType, ref SubType, ref DesignType, ref DesignTypeOption, ref Auto);
                
                //If it is linear static, let's determine if there is autoseismic/autowind in the case in question. This gathers all the load patterns in a load case
                if (CaseType == eLoadCaseType.LinearStatic)
                {
                    int NumberLoads = 0;
                    string[] LoadType = null;
                    string[] LoadName = null;
                    double[] SF = null;
                    _SapModel.LoadCases.StaticLinear.GetLoads(Name, ref NumberItems, ref LoadType, ref LoadName, ref SF);

                    foreach (string loadName in LoadName)
                    {
                        eLoadPatternType MyType = new eLoadPatternType();
                        _SapModel.LoadPatterns.GetLoadType(loadName, ref MyType);
                        if (MyType == eLoadPatternType.Quake)
                        {
                            string CodeName = "";
                            _SapModel.LoadPatterns.GetAutoSeismicCode(loadName, ref CodeName);

                           
                            if (CodeName == "ASCE 7-16")
                            {
                                string myTableKey = "Load Pattern Definitions - Auto Seismic - ASCE 7-16";
                                string[] FieldKeyList = null;
                                string GroupName = "";
                                int TableVersion = 0;
                                string[] FieldsKeysIncluded = null;
                                int NumberRecords = 0;
                                string[] TableData = null;
                                _SapModel.DatabaseTables.GetTableForDisplayArray(myTableKey, ref FieldKeyList, GroupName, ref TableVersion, ref FieldsKeysIncluded, ref NumberRecords, ref TableData);

                                string[] seismicInfo = new string[6]; // Assuming you want 6 elements (index 2 to 7 inclusive)
                                Array.Copy(TableData, 2, seismicInfo, 0, 6); // Copies 6 elements starting from index 2

                                string[] SeismicName = { "EQx", "EQx + E", "EQx - E", "EQy", "EQy + E", "EQy - E" };

                                // Create the dictionary, this contains what type of loads are included.
                                LComb.SeismicInfo = new Dictionary<string, bool>();

                                // Populate the dictionary
                                for (int j = 0; j < SeismicName.Length; j++)
                                {
                                    bool seis; // Declare seis outside of if-else blocks
                                    if (seismicInfo[j] == "Yes")
                                    {
                                        seis = true;
                                    }
                                    else
                                    {
                                        seis = false;
                                    }
                                    LComb.SeismicInfo.Add(SeismicName[j], seis);
                                }
                            }
                            else if (CodeName == "ASCE 7-22")
                            {
                                string myTableKey = "Load Pattern Definitions - Auto Seismic - ASCE 7-22";
                                string[] FieldKeyList = null;
                                string GroupName = "";
                                int TableVersion = 0;
                                string[] FieldsKeysIncluded = null;
                                int NumberRecords = 0;
                                string[] TableData = null;
                                _SapModel.DatabaseTables.GetTableForDisplayArray(myTableKey, ref FieldKeyList, GroupName, ref TableVersion, ref FieldsKeysIncluded, ref NumberRecords, ref TableData);

                                string[] seismicInfo = new string[6]; // Assuming you want 6 elements (index 2 to 7 inclusive)
                                Array.Copy(TableData, 2, seismicInfo, 0, 6); // Copies 6 elements starting from index 2


                                string[] SeismicName = { "EQx", "EQx + E", "EQx - E", "EQy", "EQy + E", "EQy - E" };

                                // Create the dictionary this contains what type of loads are included.
                                LComb.SeismicInfo = new Dictionary<string, bool>();

                                // Populate the dictionary
                                for (int j = 0; j < SeismicName.Length; j++)
                                {
                                    bool seis; // Declare seis outside of if-else blocks
                                    if (seismicInfo[j] == "Yes")
                                    {
                                        seis = true;
                                    }
                                    else
                                    {
                                        seis = false;
                                    }
                                    // Assuming seismicInfo[i] contains "True" or "False", you need to convert it to bool
                                    
                                    LComb.SeismicInfo.Add(SeismicName[j], seis);
                                }
                            }
                            else 
                            { 
                                return; 
                            }    

                        }
                        
                    }
                }
                LoadCaseComBox.Items.Add(MyName[i]);
                LoadCaseList.Add(LComb);
            }
            // Set the DrawMode to OwnerDrawFixed to enable custom drawing of items
            LoadCaseComBox.DrawMode = DrawMode.OwnerDrawFixed;

            // Subscribe to the DrawItem event
            LoadCaseComBox.DrawItem += LoadCaseComBox_DrawItem;
        }
        // Function that changes the background color of the combo box dropdown menu
        // items to green if the status is 4 (this means the results are available from the selected load case)
        private void LoadCaseComBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                LoadCase LComb = LoadCaseList[e.Index];

                // Set the text color to green if the status is 4
                Brush brush = (LComb.Status == 4) ? Brushes.Green : Brushes.Black;

                // Draw the item text with the specified color
                e.Graphics.DrawString(LoadCaseComBox.Items[e.Index].ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);

                // If the item is selected, draw the focus rectangle
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.DrawFocusRectangle();
                }
            }
        }

        private void getSelNodeBtn_Click(object sender, EventArgs e)
        {
            int NumberItems = 0;
            int[] ObjectType = null;
            string[] ObjectName = null;
            _SapModel.SelectObj.GetSelected(ref NumberItems, ref ObjectType, ref ObjectName);
            SelectedObjectsList = new List<SelectedObjects>();

            //test to make sure the selected object is only 1 element long and a node
            if (ObjectType == null )
            {
                MessageBox.Show("Select node first, then click the button");
            }
            else if (ObjectType.Length > 1 || ObjectType[0] != 1)
            {
                MessageBox.Show("Select only one node");
            }
            else
            {
                for (int i = 0; i < ObjectType.Length; i++)
                {
                    SelectedObjects SelectedObject = new SelectedObjects();
                    SelectedObject.ObjectType = ObjectType[i];
                    SelectedObject.ObjectName = ObjectName[i];
                    startPoint = ObjectName[i];

                    SelectedObjectsList.Add(SelectedObject);
                }
                dataGridView1.DataSource = SelectedObjectsList;
            }
        }

        private void getSelAreas_Click(object sender, EventArgs e)
        {
            int NumberItems = 0;
            int[] ObjectType = null;
            string[] ObjectName = null;
            _SapModel.SelectObj.GetSelected(ref NumberItems, ref ObjectType, ref ObjectName);
            SelectedObjectsList = new List<SelectedObjects>();
            AreaPointList = new List<AreaPoint>();
            AreaPointListStrucIntact = new List<List<AreaPoint>>();
            

            int openings = 0;
            int areas_selected = 0;

            // ensures users select an object first
            if (ObjectType == null)
            {
                MessageBox.Show("Select area(s) first, then click the button");
            }
            else
            {
                //individual area, areas are made of points
                for (int i = 0; i < ObjectType.Length; i++)
                {
                    tempAreaPointList = new List<AreaPoint>();
                    SelectedObjects SelectedObject = new SelectedObjects();
                    SelectedObject.ObjectType = ObjectType[i];
                    SelectedObject.ObjectName = ObjectName[i];

                    int NumberAreaPoints = 0;
                    string[] ObjectNamePnts = null;


                    //if the object type is 5, this is a floor/opening
                    
                    if (ObjectType[i] == 5)
                    {
                        
                        SelectedObjectsList.Add(SelectedObject);
                        _SapModel.AreaObj.GetPoints(ObjectName[i], ref NumberAreaPoints, ref ObjectNamePnts);

                        bool IsOpening = false;
                        _SapModel.AreaObj.GetOpening(ObjectName[i], ref IsOpening);

                        if (IsOpening)
                        {
                            openings += 1;
                        }
                        else
                        {
                            areas_selected += 1;
                        }
                        //set the label string for # of openings
                        Opening_Label.Visible = true;
                        Opening_Label.Text = openings.ToString();
                        Area_Label.Visible = true;
                        Area_Label.Text = areas_selected.ToString();

                        //this accesses all points associated with area edges
                        for (int j = 0; j < ObjectNamePnts.Length; j++)
                        {
                            AreaPoint AreaPointObject = new AreaPoint();
                            AreaPointObject.NumberPoints = NumberAreaPoints;
                            AreaPointObject.Points = ObjectNamePnts[j];
                            AreaPointList.Add(AreaPointObject);

                            tempAreaPointList.Add(AreaPointObject);

                            


                        }
                        AreaPointListStrucIntact.Add(tempAreaPointList);
                    }

                    else
                    {
                        ;
                    }
                }
                //writes data to data
                dataGridView2.DataSource = SelectedObjectsList;
            }
        }

        private void vectorX_TextChanged(object sender, EventArgs e)
        {
            VectorValue vectorXValue = new VectorValue();
            try
            {
                //need to fix this, currently this does not work
                vectorXValue.Value = double.Parse(vectorX.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("You must enter decimal number");
            }

        }
        private void vectorY_TextChanged(object sender, EventArgs e)
        {
            VectorValue vectorYValue = new VectorValue();
            try
            {
                //need to fix this, currently this does not work
                vectorYValue.Value = double.Parse(vectorY.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("You must enter decimal number");
            }

        }
        //function below limits the input to a decimal number
        private void vectorX_KeyPress(object sender, KeyPressEventArgs e)
        {
            vectorX.MaxLength = 6;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as System.Windows.Forms.TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
        private void vectorX_Enter(object sender, EventArgs e)
        {
            vectorX.Text = "";
            vectorX.ForeColor = Color.Black;

        }
        private void vectorY_Enter(object sender, EventArgs e)
        {
            vectorY.Text = "";
            vectorY.ForeColor = Color.Black;
        }


        //limits input to numbers
        private void numSlices_KeyPress(object sender, KeyPressEventArgs e)
        {
            vectorX.MaxLength = 6;
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            // only allow one decimal point
        }

        //this function checks to make sure inputted value is above 2 and below 1000
        private void NumSlices_Leave(object sender, EventArgs e)
        {
            int num_slices = 0;
            Int32.TryParse(NumSlices.Text, out num_slices);
            if (num_slices < 1 && NumSlices.Text != "")
            {
                NumSlices.Text = "2";
                MessageBox.Show("Minimum Allowed Cuts is 2");

            }
            else if (num_slices > 1000 && NumSlices.Text != "")
            {
                NumSlices.Text = "1000";
                MessageBox.Show("Maximum Allowed Cuts is 1000");

            }
        }

        //this function clears current input
        private void NumSlices_Enter(object sender, EventArgs e)
        {
            NumSlices.Text = "";
            NumSlices.ForeColor = Color.Black;
        }


        public static double[] linspace(double startval, double endval, int steps)
        {
            double interval = (endval / Math.Abs(endval)) * Math.Abs(endval - startval) / (steps - 1);
            return (from val in Enumerable.Range(0, steps)
                    select startval + (val * interval)).ToArray();
        }

        private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the selectedSections list
            selectedLoadSteps.Clear();

            // Add the selected items to the selectedSections list
            foreach (var item in listBoxLoadSteps.SelectedItems)
            {
                selectedLoadSteps.Add(item.ToString());
            }
        }

        //static List<int> FindIndices(string[] array, string value)
        //{
        //    List<int> indices = new List<int>();
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (array[i] == value)
        //        {
        //            indices.Add(i);
        //        }
        //    }
        //    return indices;
        //}

        private void runAnalysis_Click(object sender, EventArgs e)
        {
            //Test if analysis needs to be run
            string mySelectedCase = LoadCaseComBox.SelectedItem.ToString();
            int index = LoadCaseList.FindIndex(x => x.MyName == mySelectedCase);

            // Clear existing items in the combo box
            listBoxLoadSteps.Items.Clear();

            // Ensure LoadCaseList is not null
            if (LoadCaseList != null)
            {
                // Ensure SeismicInfo is not null
                if (LoadCaseList[index].SeismicInfo != null)
                {
                    // Iterate over the key-value pairs in SeismicInfo
                    foreach (var kvp in LoadCaseList[index].SeismicInfo)
                    {
                        // Check if the value is true
                        if (kvp.Value)
                        {
                            listBoxLoadSteps.Items.Add(kvp.Key);
                        }
                    }
                }
            }

            if (US_Units.Checked == true)
            {
                _SapModel.SetPresentUnits(eUnits.kip_ft_F);
            }
            else
            {
                _SapModel.SetPresentUnits(eUnits.kN_m_C);
            }
            shearScatterPlot.Visible = true;
            momentScatterPlot.Visible = true;
            locationPlot.Visible = true;

            string Name = null;
            double X = 0;
            double Y = 0;
            double Z = 0;
            _SapModel.PointObj.GetCoordCartesian(startPoint, ref X, ref Y, ref Z);

            ETABS_Point ref_Point = new ETABS_Point();
            ref_Point.X = X;
            ref_Point.Y = Y;
            ref_Point.Z = Z;


            List<double> refPoint = new List<double>() { X, Y, Z };

            List<double> vector = new List<double>() { double.Parse(vectorX.Text), double.Parse(vectorY.Text), 0 };

            GlobalCoordinateSystem gcs = new GlobalCoordinateSystem(refPoint, vector);
            ETABsAreaPointsList = new List<ETABS_Point>();
            ETABSAReaPointsListGlo = new List<ETABS_Point>();

            AreaLineList = new List<List<Line>>();
            AreaLineListGlo = new List<List<Line>>();
            MyGlobalAreaPoints = new List<List<MyPoint>>();
            MyPoints = new List<MyPoint>();
            MyTempPoints = new List<MyPoint>();

            /// AreaPointListStrucIntact includes 

            for (int i = 0; i < AreaPointListStrucIntact.Count; i++)
            {
                AreaTempLineList = new List<Line>();
                AreaTempGloLineList = new List<Line>();
                AreaTempPointList = new List<MyPoint>();
                MyTempPoints = new List<MyPoint>();
                for (int j = 0; j <= AreaPointListStrucIntact[i].Count; j++)
                {
                    if (j < AreaPointListStrucIntact[i].Count)
                    {
                        _SapModel.PointObj.GetCoordCartesian(AreaPointListStrucIntact[i][j].Points, ref X, ref Y, ref Z);
                        ETABS_Point samplePoint = new ETABS_Point();
                        samplePoint.X = X;
                        samplePoint.Y = Y;
                        samplePoint.Z = Z;
                        ETABSAReaPointsListGlo.Add(samplePoint);
                        List<double> myPoint = new List<double>() { X, Y, Z };

                        MyPoint point = new MyPoint(myPoint);
                        point.X = myPoint[0];
                        point.Y = myPoint[1];
                        point.Z = myPoint[2];



                        //MyPoint point = new MyPoint(myPoint);
                        point.glo_to_loc(gcs);
                        ETABS_Point localPoint = new ETABS_Point();
                        localPoint.X = point.LocalCoords[0];
                        localPoint.Y = point.LocalCoords[1];
                        localPoint.Z = point.LocalCoords[2];
                        ETABsAreaPointsList.Add(localPoint);

                        List<double> myTempPoint = new List<double>() { X, Y, Z };

                        MyPoint LCpointArea = new MyPoint(myTempPoint);
                        LCpointArea.X = point.LocalCoords[0];
                        LCpointArea.Y = point.LocalCoords[1];
                        LCpointArea.Z = point.LocalCoords[2];



                        //we are adding the local coordinates of the line segements to the list
                        MyPoints.Add(LCpointArea);
                        MyTempPoints.Add(LCpointArea);
                        AreaTempPointList.Add(point);


                    }
                    else
                    {

                    }
                    MyGlobalAreaPoints.Add(AreaTempPointList);

                    if (j >= 1 && j < AreaPointListStrucIntact[i].Count)
                    {


                        Line myLine = new Line();
                        myLine.startPoint = MyTempPoints[j - 1];
                        myLine.endPoint = MyTempPoints[j];

                        Line myGlobalLine = new Line();
                        myGlobalLine.startPoint = AreaTempPointList[j - 1];
                        myGlobalLine.endPoint = AreaTempPointList[j];

                        AreaTempLineList.Add(myLine);

                        AreaTempGloLineList.Add(myGlobalLine);
                    }
                    else if (j == 0)
                    {

                    }
                    else
                    {
                        Line myLine = new Line();
                        myLine.startPoint = MyTempPoints[j - 1];
                        myLine.endPoint = MyTempPoints[0];

                        AreaTempLineList.Add(myLine);

                        Line myGlobalLine = new Line();

                        myGlobalLine.startPoint = AreaTempPointList[j - 1];
                        myGlobalLine.endPoint = AreaTempPointList[0];
                        AreaTempGloLineList.Add(myGlobalLine);

                    }
                }
                AreaLineList.Add(AreaTempLineList);
                AreaLineListGlo.Add(AreaTempGloLineList);
            }



            //this gathers all of the XYZ points of the area objects to determine the max and the minimum U and V values
            for (int i = 0; i < AreaPointList.Count; i++)
            {
                _SapModel.PointObj.GetCoordCartesian(AreaPointList[i].Points, ref X, ref Y, ref Z);
                List<double> myPoint = new List<double>() { X, Y, Z };

                MyPoint point = new MyPoint(myPoint);
                point.X = myPoint[0];
                point.Y = myPoint[1];
                point.Z = myPoint[2];

                point.glo_to_loc(gcs);
                ETABS_Point localPoint = new ETABS_Point();
                localPoint.X = point.LocalCoords[0];
                localPoint.Y = point.LocalCoords[1];
                localPoint.Z = point.LocalCoords[2];
                ETABsAreaPointsList.Add(localPoint);


            }


            //Finds the max U and V values
            double Umax = ETABsAreaPointsList.Max(x => x.X);
            double Umin = ETABsAreaPointsList.Min(x => x.X);
            double Vmax = ETABsAreaPointsList.Max(x => x.Y);
            double Vmin = ETABsAreaPointsList.Min(x => x.Y);

            //angle for rotating the local axis of the section cut

            double angle = 0;
            if (double.Parse(vectorY.Text) == 0)
            {
                angle = 90;
            }
            else
            {
                angle = Math.Atan(double.Parse(vectorX.Text) / double.Parse(vectorY.Text)) * 180 * Math.PI;
            }

            double distance = Umax - Umin;

            int n_cuts = int.Parse(NumSlices.Text);
            double height = ref_Point.Z;

            // creates a list of values between max and min u values.
            double[] range_values = linspace(Umin + 1, Umax - 1, n_cuts);


            //creates the lists of 4 points
            List<List<MyPoint>> sectionPlanes = new List<List<MyPoint>>();
            List<List<String>> ETABs_Section_Cut_Data = new List<List<String>>();

            /////// Generating all of the data to be utilized to make the section cuts /////

            int counter = 0;

            foreach (double i in range_values)
            {
                List<double> tempPoint1 = new List<double>() { i, Vmin, height - 0.5 };
                List<double> tempPoint2 = new List<double>() { i, Vmax, height + 0.5 };
                MyPoint sectionPoint1 = new MyPoint(tempPoint1);
                MyPoint sectionPoint2 = new MyPoint(tempPoint2);

                List<MyPoint> xingPoints = new List<MyPoint>();

                //Find intersection point with main polygon
                RayCasting.RayCast(sectionPoint1, sectionPoint2, gcs, AreaLineList, out int countCrosses, ref xingPoints);

                List<MyPoint> sectionPlane = new List<MyPoint>();

                List<double> listPoint1 = new List<double>() { xingPoints[0].X, xingPoints[0].Y, height - 0.5 };
                List<double> listPoint2 = new List<double>() { xingPoints[0].X, xingPoints[0].Y, height + 0.5 };
                List<double> listPoint3 = new List<double>() { xingPoints[1].X, xingPoints[1].Y, height + 0.5 };
                List<double> listPoint4 = new List<double>() { xingPoints[1].X, xingPoints[1].Y, height - 0.5 };


                string name = counter.ToString().PadLeft(4, '0');
                List<string> string1 = new List<string>

                { name, "Quads", "All", "Analysis", "Default", angle.ToString(), "0", "0", "Top or Right or Positive3", "1", "1", "1",
                    listPoint1[0].ToString(), listPoint1[1].ToString(), listPoint1[2].ToString(), "1"
                };
                List<string> string2 = new List<string>
                { name, null, null, null, null, null, null, null, null, null, "1", "2",
                    listPoint2[0].ToString(), listPoint2[1].ToString(), listPoint2[2].ToString(), null
                };
                List<string> string3 = new List<string>
                { name, null, null, null, null, null, null, null, null, null, "1", "3",
                    listPoint3[0].ToString(), listPoint3[1].ToString(), listPoint3[2].ToString(), null
                };
                List<string> string4 = new List<string>
                { name, null, null, null, null, null, null, null, null, null, "1", "4",
                    listPoint4[0].ToString(), listPoint4[1].ToString(), listPoint4[2].ToString(), null
                };

                ETABs_Section_Cut_Data.Add(string1);
                ETABs_Section_Cut_Data.Add(string2);
                ETABs_Section_Cut_Data.Add(string3);
                ETABs_Section_Cut_Data.Add(string4);
                counter++;
            }


            string TableKey = "Section Cut Definitions";
            string[] FieldKeysIncluded = ETABs_Section_Cut_Data.SelectMany(x => x).ToArray();

            int TableVersiontest = 1;
            string[] FieldKeysIncludedtest = new string[] { "Name", "DefinedBy", "Group", "ResultType", "ResultLoc", "RotAboutZ", "RotAboutY", "RotAboutX",
                "ElementSide", "NumQuads", "QuadNum", "PointNum", "QuadX", "QuadY", "QuadZ", "GUID" };
            int NumberRecordstest = ETABs_Section_Cut_Data.Count * 4;


            _SapModel.DatabaseTables.SetTableForEditingArray(TableKey, ref TableVersiontest, ref FieldKeysIncludedtest, NumberRecordstest, ref FieldKeysIncluded);

            bool FillImportLog = true;
            int NumFatalErrors = 0;
            int NumErrorMsgs = 0;
            int NumWarnMsgs = 0;
            int NumInfoMsgs = 0;
            string ImportLog = "";

            int ret = 1;
            ret = _SapModel.DatabaseTables.ApplyEditedTables(FillImportLog, ref NumFatalErrors, ref NumErrorMsgs, ref NumWarnMsgs, ref NumInfoMsgs, ref ImportLog);

            DatabaseTableInfo databaseTableInfo = new DatabaseTableInfo();
            databaseTableInfo.NumErrorMsgs = NumErrorMsgs;

            databaseTableInfo.ImportLog = ImportLog;
            //_SapModel.SetModelIsLocked(false);
            //_SapModel.Analyze.RunAnalysis();


            //If results are available, do not rerun the analysis.
            if (LoadCaseList[index].Status != 4)
            {
                // Display the message box
                MessageBox.Show("This requires you to rerun the analysis", "Rerun", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _SapModel.GetModelIsLocked();
                _SapModel.Analyze.RunAnalysis();
            }


            _SapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
            _SapModel.Results.Setup.SetCaseSelectedForOutput(LoadCaseComBox.SelectedItem.ToString());


            int NumberTables = 0;
            string[] TableKey2 = null;
            string[] TableName2 = null;
            int[] ImportType2 = null;
            _SapModel.DatabaseTables.GetAvailableTables(ref NumberTables, ref TableKey2, ref TableName2, ref ImportType2);

            string TableKey3 = "Section Cut Forces - Analysis";
            string[] FieldKeyList = null;
            string GroupName = "All";
            int TableVersion = 1;
            string[] FieldKeysIncluded2 = null;
            int NumRecords = 0;
            string[] TableData2 = null;

            _SapModel.DatabaseTables.GetTableForDisplayArray(TableKey3, ref FieldKeyList, GroupName, ref TableVersion, ref FieldKeysIncluded2, ref NumRecords, ref TableData2);
            //THIS IS THE LIST TO HOLD ALL LOAD STEP RESULTS FOR A SELECTED LOAD CASE WITH STEPS
            List<SectionResults> listSectionResults = new List<SectionResults>();

            //this is the multiple load steps
            if (FieldKeysIncluded2.Contains("StepNumber"))
            {
                //This list will contain all of the results from one load case with multiplesteps                
                
                List<int> indices = new List<int>();
                // Find indices of "EqAll" in TableData2
                int loadSteps = listBoxLoadSteps.Items.Count;
                indices = SectionCutResults.FindIndices(TableData2, LoadCaseComBox.SelectedItem.ToString());

                //int step = indices.Count() / loadSteps;
                // Grab specific items by index using LINQ
                var specificItems = indices.Where((item, index2) => index2 % loadSteps == 0);

                //run from 0 to load steps
                for (int i  = 0; i < loadSteps; i++)
                {
                    List<Double> F1loop = new List<double>();
                    List<Double> F2loop = new List<double>();
                    List<Double> F3loop = new List<double>();
                    List<Double> M1loop = new List<double>();
                    List<Double> M2loop = new List<double>();
                    List<Double> M3loop = new List<double>();
                    //IF THIS TABLE GETS REWORKED IN THE FUTURE, THIS WILL NEED TO BE RECODED!!!
                    SectionResults loadCaseResults = new SectionResults();
                    //We generatate all of the results for an individual load case, load step in this loop here.
                    foreach (int sectionResult in specificItems)
                    {
                        F1loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 4]));
                        F2loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 5]));
                        F3loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 6]));
                        M1loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 7]));
                        M2loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 8]));
                        M3loop.Add(Convert.ToDouble(TableData2[sectionResult + i * 14 + 9]));
                    }
                    loadCaseResults.F1 = F1loop.ToArray();
                    loadCaseResults.F2 = F2loop.ToArray();
                    loadCaseResults.F3 = F3loop.ToArray();
                    loadCaseResults.M1 = M1loop.ToArray();
                    loadCaseResults.M2 = M2loop.ToArray();
                    loadCaseResults.M3 = M3loop.ToArray();
                    listSectionResults.Add(loadCaseResults);
                }
            }

            else
            {
                int NumberResults = 1;
                string[] SCut = new string[0];
                string[] LoadCase = new string[0];
                string[] StepType = new string[0];
                double[] StepNum = new double[0];
                double[] F1 = new double[0];
                double[] F2 = new double[0];
                double[] F3 = new double[0];
                double[] M1 = new double[0];
                double[] M2 = new double[0];
                double[] M3 = new double[0];

                _SapModel.Results.SectionCutAnalysis(ref NumberResults, ref SCut, ref LoadCase, ref StepType, ref StepNum, ref F1, ref F2, ref F3, ref M1, ref M2, ref M3);

                SectionResults sectionResults = new SectionResults();
                sectionResults.F1 = F1;
                sectionResults.F2 = F2;
                sectionResults.F3 = F3;
                sectionResults.M1 = M1;
                sectionResults.M2 = M2;
                sectionResults.M3 = M3;
            }
            //int NumberResults = 1;
            //string[] SCut = new string[0];
            //string[] LoadCase = new string[0];
            //string[] StepType = new string[0];
            //double[] StepNum = new double[0];
            //double[] F1 = new double[0];
            //double[] F2 = new double[0];
            //double[] F3 = new double[0];
            //double[] M1 = new double[0];
            //double[] M2 = new double[0];
            //double[] M3 = new double[0];

            //_SapModel.Results.SectionCutAnalysis(ref NumberResults, ref SCut, ref LoadCase, ref StepType, ref StepNum, ref F1, ref F2, ref F3, ref M1, ref M2, ref M3);

            //SectionResults sectionResults = new SectionResults();
            //sectionResults.F1 = F1;
            //sectionResults.F2 = F2;
            //sectionResults.F3 = F3;
            //sectionResults.M1 = M1;
            //sectionResults.M2 = M2;
            //sectionResults.M3 = M3;



            //List<TabularData> TabDataList = new List<TabularData>();

            //for (int i = 0; i < sectionResults.F2.Length; i++)
            //{
            //    TabularData sampleData = new TabularData();
            //    sampleData.Location = range_values[i];
            //    sampleData.Shear = sectionResults.F1[i];
            //    sampleData.Moment = sectionResults.M3[i];
            //    sampleData.Axial = sectionResults.F2[i];

            //    TabDataList.Add(sampleData);
            //}

            //// Setting Up Unit Titles //////

            string titleMoment = "";
            string titleShear = "";
            string titleLocation = "";
            if (US_Units.Checked == true)
            {
                titleMoment = "Moment (kip*ft)";
                titleShear = "Shear (kip)";
                titleLocation = "Location (ft)";
            }
            else
            {
                titleMoment = "Moment (kN*m)";
                titleShear = "Shear (kN)";
                titleLocation = "Location (m)";
            }

            ////// Shear ScatterPlot //////////
            ///

            //ChartValues<LiveCharts.Defaults.ObservablePoint> shearPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();

            //for (int i = 0; i < sectionResults.F1.Length; i++)
            //{
            //    shearPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = range_values[i], Y = sectionResults.F1[i] });

            //}

            //var scatterShearSeries = new LiveCharts.Wpf.LineSeries
            //{
            //    Title = "ShearSeries",
            //    Values = shearPoints,
            //    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
            //    Fill = System.Windows.Media.Brushes.Transparent,


            //};

            for (int i = 0; i < listBoxLoadSteps.Items.Count; i++)
            {
                listBoxLoadSteps.SetSelected(i, true);
            }

            List<string> mySelectedCases = new List<string>();
            foreach (var selectedItem in listBoxLoadSteps.SelectedItems)
            {
                mySelectedCases.Add(selectedItem.ToString());
            }
            PlotResults.GraphShearResults(listSectionResults, mySelectedCases, range_values, shearScatterPlot);

            shearScatterPlot.AxisX.Clear();
            shearScatterPlot.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = titleLocation,
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(2),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }

            }); ;
            shearScatterPlot.AxisY.Clear();
            shearScatterPlot.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = titleShear,
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(2),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }


            });

            //shearScatterPlot.Series.Add(scatterShearSeries);

            //scatterPlot.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 245, 245));
            shearScatterPlot.Zoom = LiveCharts.ZoomingOptions.Xy;

            ////// Moment ScatterPlot //////////




            //ChartValues<LiveCharts.Defaults.ObservablePoint> momentPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();

            //for (int i = 0; i < sectionResults.M3.Length; i++)
            //{

            //    momentPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = range_values[i], Y = sectionResults.M3[i] });
            //}


            //var scatterMomentSeries = new LiveCharts.Wpf.LineSeries
            //{
            //    Title = titleMoment,
            //    Values = momentPoints,
            //    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
            //    Fill = System.Windows.Media.Brushes.Transparent,


            //};

            PlotResults.GraphMomentResults(listSectionResults, mySelectedCases, range_values, momentScatterPlot);
            momentScatterPlot.AxisX.Clear();
            momentScatterPlot.AxisX.Add(new LiveCharts.Wpf.Axis
            {
                Title = titleLocation,
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(2),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }

            });
            momentScatterPlot.AxisY.Clear();
            momentScatterPlot.AxisY.Add(new LiveCharts.Wpf.Axis
            {
                Title = titleMoment,
                Separator = new LiveCharts.Wpf.Separator
                {
                    StrokeThickness = 1,
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(2),
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 79, 86))
                }


            });

            //momentScatterPlot.Series.Add(scatterMomentSeries);
            momentScatterPlot.Zoom = LiveCharts.ZoomingOptions.Xy;

            ////// Location Plot //////////

            //AreaLineListGlo
            for (int i = 0; i < AreaLineListGlo.Count; i++)
            {
                for (int j = 0; j < AreaLineListGlo[i].Count(); j++)
                {
                    ChartValues<LiveCharts.Defaults.ObservablePoint> areaPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                    areaPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = AreaLineListGlo[i][j].startPoint.X, Y = AreaLineListGlo[i][j].startPoint.Y });
                    areaPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = AreaLineListGlo[i][j].endPoint.X, Y = AreaLineListGlo[i][j].endPoint.Y });
                    var locationSeries = new LiveCharts.Wpf.LineSeries
                    {
                        Title = "Area Locations",
                        Values = areaPoints,
                        Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 140, 105)),
                        Fill = System.Windows.Media.Brushes.Transparent,
                    };
                    locationPlot.Series.Add(locationSeries);
                }
            }


            for (int i = 0; i < range_values.Count(); i++)

            {
                ChartValues<LiveCharts.Defaults.ObservablePoint> cutPoints = new LiveCharts.ChartValues<LiveCharts.Defaults.ObservablePoint>();
                cutPoints.Add(new LiveCharts.Defaults.ObservablePoint { X = Double.Parse(ETABs_Section_Cut_Data[i * 4][12]), Y = Double.Parse(ETABs_Section_Cut_Data[i * 4][13]) });
                cutPoints.Add(new LiveCharts.Defaults.ObservablePoint
                {
                    X = Double.Parse(ETABs_Section_Cut_Data[i * 4 + 2][12]),
                    Y = Double.Parse(ETABs_Section_Cut_Data[i * 4 + 2][13
                    ])
                });
                var cutSeries = new LiveCharts.Wpf.LineSeries
                {
                    Title = "Cut Locations",
                    Values = cutPoints,
                    Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0)),
                    Fill = System.Windows.Media.Brushes.Transparent,
                };

                locationPlot.Series.Add(cutSeries);
            }

            //dataGridView3.DataSource = TabDataList;

        }

        private void US_Units_CheckedChanged(object sender, EventArgs e)
        {
            all_Other_Units.Checked = false;
        }

        private void all_Other_Units_CheckedChanged(object sender, EventArgs e)
        {
            US_Units.Checked = false;
        }


    }
}

