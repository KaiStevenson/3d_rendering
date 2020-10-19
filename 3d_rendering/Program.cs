using System;
using System.Collections.Generic;

namespace _3d_rendering
{
    class Program
    {
        static void Main()
        {
        }
    }
    class PObject
    {

    }
    class CCoordObj
    {
        double xPos;
        double yPos;
        double zPos;
        public CCoordObj(double x, double y, double z)
        {
            xPos = x;
            yPos = y;
            zPos = z;
        }
    }
    class Face
    {
        public List<CCoordObj> vertices = new List<CCoordObj>();
    }
}
