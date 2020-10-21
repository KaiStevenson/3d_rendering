
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace _3d_rendering
{
    class Program
    {
        int renderMode;
        static void Main()
        {
            var p = new Program();
            p.ProgramMain();
        }
        public void ProgramMain()
        {
            //init
            //create object
            var o = Instantiate("plane", new CCoordObj(50, 50, 0), new CCoordObj(0, 0, 0), new CCoordObj(6, 6, 0));
            //var o = Instantiate("cube", new CCoordObj(10, 10, 0), new CCoordObj(0, 0, 0), new CCoordObj(6, 6, 6));
            Console.WriteLine("V = vertices only, E = edges only, F = faces only");
            Console.WriteLine("Please make a selection");
            var ks = Console.ReadKey();
            if (ks.Key == ConsoleKey.V)
            {
                renderMode = 0;
            }
            if (ks.Key == ConsoleKey.E)
            {
                renderMode = 1;
            }
            if (ks.Key == ConsoleKey.F)
            {
                renderMode = 2;
            }
            while (true)
            {
                RenderRObject(o);
                var k = Console.ReadKey();
                //controls
                if (k.Key == ConsoleKey.W)
                {
                    o.cPos.yPos += 1;
                }
                if (k.Key == ConsoleKey.S)
                {
                    o.cPos.yPos -= 1;
                }
                if (k.Key == ConsoleKey.D)
                {
                    o.cPos.xPos += 1;
                }
                if (k.Key == ConsoleKey.A)
                {
                    o.cPos.xPos -= 1;
                }
                if (k.Key == ConsoleKey.Q)
                {
                    o.cRot.xPos -= 15;
                }
                if (k.Key == ConsoleKey.E)
                {
                    o.cRot.xPos += 15;
                }
                if (k.Key == ConsoleKey.LeftArrow)
                {
                    o.cRot.yPos -= 15;
                }
                if (k.Key == ConsoleKey.RightArrow)
                {
                    o.cRot.yPos += 15;
                }
                if (k.Key == ConsoleKey.UpArrow)
                {
                    o.cRot.zPos -= 15;
                }
                if (k.Key == ConsoleKey.DownArrow)
                {
                    o.cRot.zPos += 15;
                }
                Console.Clear();

            }
        }
        //create new prefab
        RenderObject Instantiate(string type, CCoordObj position, CCoordObj rotation, CCoordObj scale)
        {
            if (type == "plane")
            {
                var o = new RenderObject();
                var f = new Face();
                f.vertices.Add(new CCoordObj(-scale.xPos, -scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(-scale.xPos, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, -scale.yPos, scale.zPos));
                f.parentObject = o;
                o.cPos = position;
                o.cRot = rotation;
                o.faces.Add(f);
                return o;
            }
            if (type == "cube")
            {
                var o = new RenderObject();
                var f = new Face();
                f.vertices.Add(new CCoordObj(-scale.xPos, -scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(-scale.xPos, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, -scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(-scale.xPos, -scale.yPos, -scale.zPos));
                f.vertices.Add(new CCoordObj(-scale.xPos, scale.yPos, -scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, scale.yPos, -scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, -scale.yPos, -scale.zPos));
                f.parentObject = o;
                o.cPos = position;
                o.cRot = rotation;
                o.faces.Add(f);
                return o;
            }
            if (type == "testobj")
            {
                //plane with a second smaller plane inside
                var o = new RenderObject();
                var f = new Face();
                f.vertices.Add(new CCoordObj(-scale.xPos, -scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(0, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, -scale.yPos, scale.zPos));
                f.parentObject = o;
                o.faces.Add(f);
                var f2 = new Face();
                f2.vertices.Add(new CCoordObj(-scale.xPos * 0.2f, -scale.yPos * 0.2f, scale.zPos));
                f2.vertices.Add(new CCoordObj(-scale.xPos * 0.2f, scale.yPos * 0.2f, scale.zPos));
                f2.vertices.Add(new CCoordObj(scale.xPos * 0.2f, scale.yPos * 0.2f, scale.zPos));
                f2.vertices.Add(new CCoordObj(scale.xPos * 0.2f, -scale.yPos * 0.2f, scale.zPos));
                f2.parentObject = o;
                o.cPos = position;
                o.cRot = rotation;
                o.faces.Add(f2);
                return o;
            }
            else
            {
                return null;
            }
        }
        //render all faces in an object
        void RenderRObject(RenderObject obj)
        {
            for (int i = 0; i < obj.faces.Count; i++)
            {
                RenderFace(obj.faces[i]);
            }
            //display information below object
            //TODO: auto positioning based on object scaling (pos.x - scale.x)
            DrawText((int)Math.Round(obj.cPos.xPos - 8), (int)Math.Round(obj.cPos.yPos - 8), "POS: " + obj.cPos.xPos + "X " + obj.cPos.yPos + "Y " + obj.cPos.zPos + "Z");
            DrawText((int)Math.Round(obj.cPos.xPos - 8), (int)Math.Round(obj.cPos.yPos - 9), "ROT: " + (int)obj.cRot.xPos + "X " + (int)obj.cRot.yPos + "Y " + (int)obj.cRot.zPos + "Z");

        }
        //render all verts in a face
        void RenderFace(Face face)
        {
            if (renderMode == 0)
            {
                for (int i = 0; i < face.vertices.Count; i++)
                {
                    //move object to center before rotating
                    //TODO: rotate before summing, avoid having to move object
                    var sPos = face.parentObject.cPos;
                    face.parentObject.cPos = new CCoordObj(0, 0, 0);
                    var pos = face.vertices[i];
                    var summedPos = new CCoordObj(pos.xPos + face.parentObject.cPos.xPos, pos.yPos + face.parentObject.cPos.yPos, pos.zPos + face.parentObject.cPos.zPos);
                    //rotate vert around object center
                    var rotatedPos = CCoordObj.RotatePoint(summedPos, face.parentObject.cRot);
                    RenderPosition((int)Math.Round(rotatedPos.xPos + sPos.xPos), (int)Math.Round(rotatedPos.yPos + sPos.yPos));
                    //reset object position
                    face.parentObject.cPos = sPos;
                }
            }
            if (renderMode == 1)
            {
                //establish bounding box
                var lowX = 0;
                var lowY = 0;
                var highX = 0;
                var highY = 0;
                for (int i = 0; i < face.vertices.Count; i++)
                {
                    //move object to center before rotating
                    var rotatedPos = CCoordObj.RotatePoint(face.vertices[i], face.parentObject.cRot);
                    if (rotatedPos.xPos < lowX)
                    {
                        lowX = (int)Math.Round(rotatedPos.xPos);
                    }
                    if (rotatedPos.yPos < lowY)
                    {
                        lowY = (int)Math.Round(rotatedPos.yPos);
                    }
                    if (rotatedPos.xPos > highX)
                    {
                        highX = (int)Math.Round(rotatedPos.xPos);
                    }
                    if (rotatedPos.yPos > highY)
                    {
                        highY = (int)Math.Round(rotatedPos.yPos);
                    }
                }
                var r1 = CCoordObj.RotatePoint(face.vertices[0], face.parentObject.cRot);
                var r2 = CCoordObj.RotatePoint(face.vertices[1], face.parentObject.cRot);
                var line = FindLine(r1, r2);
                Console.WriteLine(line.Count);
                for (int r = lowY -1; r < highY + 2; r++)
                {
                    for (int c = lowX -1; c < highX + 2; c++)
                    {
                        if (line.Contains(new CCoordObj(c, r, 0)))
                        {
                            RenderPosition(c + (int)Math.Round(face.parentObject.cPos.xPos), r + (int)Math.Round(face.parentObject.cPos.yPos));
                        }
                    }
                }
            }
            if (renderMode == 2)
            {
                //establish bounding box
                var lowX = 0;
                var lowY = 0;
                var highX = 0;
                var highY = 0;
                for (int i = 0; i < face.vertices.Count; i++)
                {
                    //move object to center before rotating
                    var rotatedPos = CCoordObj.RotatePoint(face.vertices[i], face.parentObject.cRot);
                    if (rotatedPos.xPos < lowX)
                    {
                        lowX = (int)Math.Round(rotatedPos.xPos);
                    }
                    if (rotatedPos.yPos < lowY)
                    {
                        lowY = (int)Math.Round(rotatedPos.yPos);
                    }
                    if (rotatedPos.xPos > highX)
                    {
                        highX = (int)Math.Round(rotatedPos.xPos);
                    }
                    if (rotatedPos.yPos > highY)
                    {
                        highY = (int)Math.Round(rotatedPos.yPos);
                    }
                }
                for (int r = lowY; r < highY + 1; r++)
                {
                    for (int c = lowX; c < highX + 1; c++)
                    {
                        if (CCoordObj.PointInFace(face.vertices, new CCoordObj(c, r, 0)))
                        {
                            //need to calculate Z position at location in plane
                            //need to add parent object position to child
                            var rotatedPos = CCoordObj.RotatePoint(new CCoordObj(c, r, 0), face.parentObject.cRot);
                            RenderPosition(c + (int)Math.Round(rotatedPos.xPos), r + (int)Math.Round(rotatedPos.yPos));
                            //TODO: REIMPLEMENT CORNER HIGHLIGHTS
                            //bool isVert = false;
                            //foreach (var v in face.vertices)
                            //{
                            //    var rotatedPos = CCoordObj.RotatePoint(v, face.parentObject.cRot);
                            //    if (c == (int)Math.Round(rotatedPos.xPos) && r == (int)Math.Round(rotatedPos.yPos))
                            //    {
                            //        isVert = true;
                            //    }
                            //}
                        }
                    }
                }
            }
            
        }
        //draw at position
        void RenderPosition(int x, int y)
        {
            Console.CursorTop = 100 - y;
            Console.CursorLeft = x;
            Console.Write("#");
            Console.WriteLine();
        }
        //write a line starting at a given position
        void DrawText(int x, int y, string text)
        {
            Console.CursorTop = 100 - y;
            Console.CursorLeft = x;
            Console.WriteLine(text);
        }
        List<CCoordObj> FindLine(CCoordObj point1, CCoordObj point2)
        {

            var points = new List<CCoordObj>();
            var diagonalDistance = Math.Sqrt(Math.Pow(point2.xPos - point1.xPos, 2) + Math.Pow(point2.yPos - point1.yPos, 2));
            for (int i = 0; i < diagonalDistance; i++)
            {
                var t = i / diagonalDistance;
                var lp = Vector2.Lerp(new Vector2((float)point1.xPos,(float)point1.yPos), new Vector2((float)point1.xPos,(float)point2.yPos), (float)t);
                var outL = new CCoordObj((int)Math.Round(lp.X), (int)Math.Round(lp.Y),0);
                points.Add(outL);
            }
            return points;

        }
    }
    class RenderObject
    {
        public List<Face> faces = new List<Face>();
        public CCoordObj cPos;
        public CCoordObj cRot;
    }
    class CCoordObj
    {
        public double xPos;
        public double yPos;
        public double zPos;
        public CCoordObj(double x, double y, double z)
        {
            xPos = x;
            yPos = y;
            zPos = z;
        }
        public static CCoordObj RotatePoint(CCoordObj point, CCoordObj rotation)
        {
            var cosa = Math.Cos(CCoordObj.DegreesToRadians(-rotation.xPos));
            var sina = Math.Sin(CCoordObj.DegreesToRadians(-rotation.xPos));

            var cosb = Math.Cos(CCoordObj.DegreesToRadians(-rotation.yPos));
            var sinb = Math.Sin(CCoordObj.DegreesToRadians(-rotation.yPos));

            var cosc = Math.Cos(CCoordObj.DegreesToRadians(-rotation.zPos));
            var sinc = Math.Sin(CCoordObj.DegreesToRadians(-rotation.zPos));

            var Axx = cosa * cosb;
            var Axy = cosa * sinb * sinc - sina * cosc;
            var Axz = cosa * sinb * cosc + sina * sinc;

            var Ayx = sina * cosb;
            var Ayy = sina * sinb * sinc + cosa * cosc;
            var Ayz = sina * sinb * cosc - cosa * sinc;

            var Azx = -sinb;
            var Azy = cosb * sinc;
            var Azz = cosb * cosc;

            var endRot = new CCoordObj(Axx * point.xPos + Axy * point.yPos + Axz * point.zPos, Ayx * point.xPos + Ayy * point.yPos + Ayz * point.zPos, Azx * point.xPos + Azy * point.yPos + Azz * point.zPos);
            return endRot;
        }
        public static bool PointInFace(List<CCoordObj> face, CCoordObj point)
        {
            var i = 0;
            var j = 0;
            bool c = false;
            for (i = 0, j = face.Count - 1; i < face.Count; j = i++)
            {
                if ((((face[i].yPos <= point.yPos) && (point.yPos < face[j].yPos))
                        || ((face[j].yPos <= point.yPos) && (point.yPos < face[i].yPos)))
                        && (point.xPos < (face[j].xPos - face[i].xPos) * (point.yPos - face[i].yPos)
                            / (face[j].yPos - face[i].yPos) + face[i].xPos))
                {

                    c = !c;
                }
            }

            return c;
        }
        public static double DegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return radians;
        }
        public static double RadiansToDegrees(double radians)
        {
            double degrees = (radians * Math.PI / 180);
            return degrees;
        }
    }
    class Face
    {
        public RenderObject parentObject;
        public List<CCoordObj> vertices = new List<CCoordObj>();
    }
}
