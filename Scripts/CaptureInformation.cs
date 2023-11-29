using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IGC
{
    [System.Serializable]
    public class CaptureInformation
    {
        public float camera_angle_x;
        public float camera_angle_y;
        public float fl_x;
        public float fl_y;
        public float k1;
        public float k2;
        public float p1;
        public float p2;
        public float cx;
        public float cy;
        public float w;
        public float h;
        public int aabb_scale;
        //public FrameInformation[] frames;
        public List<FrameInformation> frames;

        public CaptureInformation(float camera_angle_x, float camera_angle_y, float fl_x, float fl_y, float w, float h, int aabb_scale, int frameCount)
        {
            this.camera_angle_x = camera_angle_x;
            this.camera_angle_y = camera_angle_y;
            this.fl_x = fl_x;
            this.fl_y = fl_y;
            this.w = w;
            this.h = h;
            this.aabb_scale = aabb_scale;
            cx = w / 2;
            cy = h / 2;
            //frames = new FrameInformation[frameCount];
            frames = new List<FrameInformation>();
        }

        public void AddFrameInformation(int index, FrameInformation fi)
        {
            //frames[index] = fi;
            frames.Add(fi);
        }
    }


    [System.Serializable]
    public class FrameInformation
    {
        public string file_path;
        public float sharpness;
        public List<Vector4> transform_matrix = new List<Vector4>();

        public FrameInformation()
        {
            file_path = "";
            sharpness = 50;
            transform_matrix = new List<Vector4> { new Vector4(0, 0, 0, 0),
                                                    new Vector4(0, 0, 0, 0),
                                                    new Vector4(0, 0, 0, 0),
                                                    new Vector4(0, 0, 0, 0)};
        }

        public FrameInformation(string fp, float s, List<Vector4> tm)
        {
            file_path = fp;
            sharpness = s;
            transform_matrix = tm;
        }
    }
}