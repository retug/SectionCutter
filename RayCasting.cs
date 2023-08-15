using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectionCutter
{
    public class RayCasting
    {
        // Function to check if two line segments intersect and calculate the intersection point
        public static bool Intersect(double x1, double y1, double x2, double y2, double x3, double y3, double x4, double y4, out double xi, out double yi)
        {
            // Calculate the denominator for parametric equations
            double denominator = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);

            // Check if the line segments are parallel or coincident
            if (denominator == 0)
            {
                xi = 0;
                yi = 0;
                return false;
            }

            // Calculate the parameters for each line segment
            double ua = ((x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3)) / denominator;
            double ub = ((x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3)) / denominator;

            // Check if the intersection point lies within both line segments
            if (ua >= 0 && ua <= 1 && ub >= 0 && ub <= 1)
            {
                // Calculate the intersection point
                xi = x1 + ua * (x2 - x1);
                yi = y1 + ua * (y2 - y1);
                return true;
            }

            xi = 0;
            yi = 0;
            return false;
        }

        // Function Takes a start point, a List of all lines that need to be checked (inc. openings) and will return the quantity of crosses
        // and the location of crosses. These crosses will be used to generate section cuts
        public static bool RayCast(MyPoint startPoint, MyPoint endPoint, GlobalCoordinateSystem gcs, List<List<Line>> checkLines, out int countCrosses, ref List<MyPoint> xingPoints)
        {
            countCrosses = 0;
            int nLines = checkLines.Count();
            double uStart = startPoint.X;
            double vStart = startPoint.Y-1;
            double wStart = startPoint.Z;
            double uEnd = endPoint.X;
            double vEnd = endPoint.Y+1;
            List<MyPoint> xingPntLocal = new List<MyPoint>();
            List<MyPoint> xingPntGlo = new List<MyPoint>();
            for (int i = 0; i < checkLines.Count(); i++)
            {
                for (int j = 0; j < checkLines[i].Count(); j++)
                {
                    double u1 = checkLines[i][j].startPoint.X;
                    double v1 = checkLines[i][j].startPoint.Y;
                    double u2 = checkLines[i][j].endPoint.X;
                    double v2 = checkLines[i][j].endPoint.Y;
                    //if this condition is passed, we know that the point intersects the given line segment
                    //if (vStart <= v1 != vStart <= v2 && uStart <= (u2 - u1) * (vStart - v1) / (v2 - v1) + u1)
                    bool cross = Intersect(uStart, vStart, uEnd, vEnd, u1, v1, u2, v2, out double xi, out double yi);
                    if (cross)
                    {
                        countCrosses += 1;
                        MyPoint crsPnt = new MyPoint(new List<double> { xi, yi, wStart });
                        xingPntLocal.Add(crsPnt);
                    }

                    else
                    {

                    }
                }
            }


            double Vmax = xingPntLocal.Max(x => x.Y);
            double Vmin = xingPntLocal.Min(x => x.Y);

            MyPoint crsLocal1 = new MyPoint(new List<double> { uStart, Vmin-0.25, wStart });
            MyPoint crsLocal2 = new MyPoint(new List<double> { uStart, Vmax+0.25, wStart });
            crsLocal1.loc_to_glo(gcs);
            crsLocal2.loc_to_glo(gcs);
            //convert results back to global coordinates, plug these values into ETABs
            MyPoint gloPoint1 = new MyPoint(new List<double> { crsLocal1.GlobalCoords[0], crsLocal1.GlobalCoords[1], crsLocal1.GlobalCoords[2] });
            MyPoint gloPoint2 = new MyPoint(new List<double> { crsLocal2.GlobalCoords[0], crsLocal2.GlobalCoords[1], crsLocal2.GlobalCoords[2] });

            //returns the max and min v crossings from the algorith
            xingPoints.Add(gloPoint1);
            xingPoints.Add(gloPoint2);
            
            
            return false;

        }
        


    }
}
