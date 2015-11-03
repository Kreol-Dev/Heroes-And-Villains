using System;
using Demiurg;
using UnityEngine;

namespace CoreMod
{
    public class NoiseModule : CreationNode
    {
        NodeOutput<float[,]> main; 

        IntParam sizeX;
        IntParam sizeY;

        protected override void SetupIOP ()
        {
            main = Output<float[,]> ("main");
            sizeX = Config<IntParam> ("planet_width");
            sizeY = Config<IntParam> ("planet_height");
        }

        protected override void Work ()
        {
            DiamondSquare ds = new DiamondSquare (sizeX, sizeY, sizeX / 8, 0, false);
            float[,] values = ds.GetNormalValues ();
            main.Finish (values);
        }
        class DiamondSquare
        {
            double[] values;
            int featuresize;
            int height;
            int width;
            System.Random rand;
            public DiamondSquare (int width, int height, int featuresize, int seed, bool nullEdges)
            {
                rand = new System.Random (seed);
                values = new double[width * height];
                this.height = height;
                this.width = width;
                this.featuresize = featuresize;
				
                for (int y = 0; y < height; y += 1)
                    for (int x = 0; x < width; x += 1)
                    {
                        if (x != 0 && x != width - 1 && y != 0 && y != height - 1 || !nullEdges)
                            SetSample (x, y, (rand.NextDouble () * 2.0 - 1.0));
                        else
                            SetSample (x, y, -1.0);
                    }
				
				
                int samplesize = featuresize;
				
                double scale = 1.0;
				
                while (samplesize > 1)
                {
					
                    DiamondSquarePass (samplesize, scale);
					
                    samplesize /= 2;
                    scale /= 2.0;
                }
				
            }
			
            public float[,] GetNormalValues ()
            {
                float[,] dimensionalValues = new float[width, height];
                double maxValue = int.MinValue;
                double minValue = int.MaxValue;
                for (int i = 0; i < width; i++)
                    for (int j = 0; j < height; j++)
                    {
                        if (values [i + j * height] > maxValue)
                            maxValue = values [i + j * height];
                        if (values [i + j * height] < minValue)
                            minValue = values [i + j * height];
                    }
				
                float fMaxValue = (float)maxValue;
                float fMinValue = (float)minValue;
                for (int i = 0; i < width; i++)
                {
					
                    for (int j = 0; j < height; j++)
                    {
                        dimensionalValues [i, j] = Mathf.InverseLerp (fMinValue, fMaxValue, (float)GetSample (i, j));
						
                    }
                }
				
				
                return dimensionalValues;
            }
			
            public float[,] GetFloatValues ()
            {
                float[,] dimensionalValues = new float[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        dimensionalValues [i, j] = (float)GetSample (i, j);
						
                    }
                }
				
				
                return dimensionalValues;
            }
			
            public static float[, ] DSNoiseNormal (int width, int height, bool nullEdges, int scale, int seed)
            {
                int featureSize = width / scale;
                featureSize = featureSize > 0 ? featureSize : 1;
                DiamondSquare ds = new DiamondSquare (width, height, featureSize, seed, nullEdges);
                return ds.GetNormalValues ();
            }
			
            public static float[, ] DSNoise (int width, int height, bool nullEdges, int scale, int seed)
            {
                int featureSize = width / scale;
                featureSize = featureSize > 0 ? featureSize : 1;
                DiamondSquare ds = new DiamondSquare (width, height, featureSize, seed, nullEdges);
                return ds.GetFloatValues ();
            }
			
            double GetSample (int x, int y)
            {
                if (y >= height || y < 0)
                    return -1;
                return values [(x & (width - 1)) + y * width];
            }
			
            void SetSample (int x, int y, double value)
            {
                if (y >= height || y < 0)
                    return;
                values [(x & (width - 1)) + y * width] = value;
            }
			
            void ComputerSquare (int x, int y, int size, double value)
            {
                int hs = size / 2;
				
                // a     b 
                //
                //    x
                //
                // c     d
				
                double a = GetSample (x - hs, y - hs);
                double b = GetSample (x + hs, y - hs);
                double c = GetSample (x - hs, y + hs);
                double d = GetSample (x + hs, y + hs);
				
                SetSample (x, y, ((a + b + c + d) / 4.0) + value);
				
            }
			
            void ComputeDiamond (int x, int y, int size, double value)
            {
                int hs = size / 2;
				
                //   c
                //
                //a  x  b
                //
                //   d
				
                double a = GetSample (x - hs, y);
                double b = GetSample (x + hs, y);
                double c = GetSample (x, y - hs);
                double d = GetSample (x, y + hs);
				
                SetSample (x, y, ((a + b + c + d) / 4.0) + value);
            }
			
            void DiamondSquarePass (int stepsize, double scale)
            {
				
                int halfstep = stepsize / 2;
				
                for (int y = halfstep; y < height + halfstep; y += stepsize)
                {
                    for (int x = halfstep; x < width + halfstep; x += stepsize)
                    {
                        ComputerSquare (x, y, stepsize, (rand.NextDouble () * 2.0 - 1.0) * scale);
                    }
                }
				
                for (int y = 0; y < height; y += stepsize)
                {
                    for (int x = 0; x < width; x += stepsize)
                    {
                        ComputeDiamond (x + halfstep, y, stepsize, (rand.NextDouble () * 2.0 - 1.0) * scale);
                        ComputeDiamond (x, y + halfstep, stepsize, (rand.NextDouble () * 2.0 - 1.0) * scale);
                    }
                }
				
            }
        }
    }

}

