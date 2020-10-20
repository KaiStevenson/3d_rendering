
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
            var o = Instantiate("plane", new CCoordObj(0, 0, 0), new CCoordObj(0, 0, 0));
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
                Console.Clear();

            }
        }
        //create new prefab
        RenderObject Instantiate(string type, CCoordObj position, CCoordObj rotation)
        {
            if (type == "plane")
            {
                var o = new RenderObject();
                var f = new Face();
                f.vertices.Add(new CCoordObj(0, 0, 3));
                f.vertices.Add(new CCoordObj(0, 10, 3));
                f.vertices.Add(new CCoordObj(10, 10, 3));
                f.vertices.Add(new CCoordObj(10, 0, 3));
                f.parentObject = o;
                o.cPos = position;
                o.cRot = rotation;
                o.faces.Add(f);
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
                RenderPosition((int)Math.Round(face.vertices[i].xPos + face.parentObject.cPos.xPos), (int)Math.Round(face.vertices[i].yPos + face.parentObject.cPos.yPos));
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
    }
    class Face
    {
        public RenderObject parentObject;
        public List<CCoordObj> vertices = new List<CCoordObj>();
    }
}
