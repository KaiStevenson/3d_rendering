
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace _3d_rendering
{
    class Program
    {
        static void Main()
        {
            var p = new Program();
            p.ProgramMain();
        }
        public void ProgramMain()
        {
            //init
            //render vertices
            var o = Instantiate("testobj", new CCoordObj(10, 10, 0), new CCoordObj(0, 0, 0), new CCoordObj(6, 6, 0));
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
            for (int i = 0; i < face.vertices.Count; i++)
            {
                //move object to center before rotating
                var sPos = face.parentObject.cPos;
                face.parentObject.cPos = new CCoordObj(0, 0, 0);
                var pos = face.vertices[i];
                var summedPos = new CCoordObj(pos.xPos + face.parentObject.cPos.xPos, pos.yPos + face.parentObject.cPos.yPos, pos.zPos + face.parentObject.cPos.zPos);
                //rotate vert around object center
                var rotatedPos = CCoordObj.RotatePoint(summedPos, face.parentObject.cPos, face.parentObject.cRot);
                RenderPosition((int)Math.Round(rotatedPos.xPos + sPos.xPos), (int)Math.Round(rotatedPos.yPos + sPos.yPos));
                //reset object position
                face.parentObject.cPos = sPos;
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
        public static CCoordObj RotatePoint(CCoordObj point, CCoordObj center, CCoordObj rotation)
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
