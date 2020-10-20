
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
            var o = Instantiate("testobj", new CCoordObj(0, 0, 0), new CCoordObj(0, 0, 0), new CCoordObj(5, 5, 0));
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
                if (k.Key == ConsoleKey.E)
                {
                    o.cRot.xPos += CCoordObj.DegreesToRadians(15);
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
                var o = new RenderObject();
                var f = new Face();
                f.vertices.Add(new CCoordObj(-scale.xPos, -scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(-scale.xPos, scale.yPos, scale.zPos));
                f.vertices.Add(new CCoordObj(scale.xPos, scale.yPos, scale.zPos));
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
        }
        //render all verts in a face
        void RenderFace(Face face)
        {
            for (int i = 0; i < face.vertices.Count; i++)
            {
                var pos = face.vertices[i];
                var summedPos = new CCoordObj(pos.xPos + face.parentObject.cPos.xPos, pos.yPos + face.parentObject.cPos.yPos, pos.zPos + face.parentObject.cPos.zPos);
                var rotatedPos = CCoordObj.RotatePoint(summedPos, face.parentObject.cPos, face.parentObject.cRot.xPos);
                RenderPosition((int)Math.Round(rotatedPos.xPos), (int)Math.Round(rotatedPos.yPos));
            }
        }
        //draw at position
        void RenderPosition(int x, int y)
        {
            //TODO: INVERT Y (console writes from top down, y counts from bottom up)
            Console.CursorTop = y + 10;
            Console.CursorLeft = x + 10;
            Console.Write("X");
            Console.WriteLine();
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
        public static CCoordObj RotatePoint(CCoordObj point, CCoordObj center, double angle)
        {
            var rotatedX = Math.Cos(angle) * (point.xPos - center.xPos) - Math.Sin(angle) * (point.yPos - center.yPos) + center.xPos;
            var rotatedY = Math.Sin(angle) * (point.xPos - center.xPos) + Math.Cos(angle) * (point.yPos - center.yPos) + center.yPos;
            return new CCoordObj(rotatedX, rotatedY, 0);
        }
        public static double DegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return radians;
        }
    }
    class Face
    {
        public RenderObject parentObject;
        public List<CCoordObj> vertices = new List<CCoordObj>();
    }
}
