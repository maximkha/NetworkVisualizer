using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using myNN;
using System.Diagnostics;

namespace NetworkVisualizer
{
    public partial class Form1 : Form
    {
        Bitmap DrawArea;
        NN.neuralNet neuralNet = new NN.neuralNet();
        List<double[]> ins = new List<double[]>();
        List<double[]> outs = new List<double[]>();
        int ci = 0;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();
            DrawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            neuralNet.addLayer(2, 2, 2, NN.activationFunction.input); //Input
            neuralNet.addLayer(2, 2, 2, NN.activationFunction.relu, true); //Hidden
            neuralNet.addLayer(2, 2, 2, NN.activationFunction.relu, true); //Hidden
            neuralNet.addLayer(1, 2, 1, NN.activationFunction.relu);//, true); //Output
            neuralNet.setLearn(NN.learningMode.staticLearn, .25);//, .25);
            ins.Add(new double[] { 0, 0 }); outs.Add(new double[] { 0 });
            ins.Add(new double[] { 0, 1 }); outs.Add(new double[] { 1 });
            ins.Add(new double[] { 1, 0 }); outs.Add(new double[] { 1 });
            ins.Add(new double[] { 1, 1 }); outs.Add(new double[] { 0 });

            //ins.Add(new double[] { 0, 0 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 0, 1 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 1, 0 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 1, 1 }); outs.Add(new double[] { 1 });


            //ins.Add(new double[] { 0, 0 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 0, 1 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 1, 0 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 1, 1 }); outs.Add(new double[] { 0 });

            //ins.Add(new double[] { .5, 0 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 0, .5 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { .5, 1 }); outs.Add(new double[] { 0 });
            //ins.Add(new double[] { 1, .5 }); outs.Add(new double[] { 0 });

            //ins.Add(new double[] { .5, .5 }); outs.Add(new double[] { 1 });
            //ins.Add(new double[] { .5, .5 }); outs.Add(new double[] { 1 });
            //ins.Add(new double[] { .5, .5 }); outs.Add(new double[] { 1 });
            //ins.Add(new double[] { .5, .5 }); outs.Add(new double[] { 1 });

            //ins.Add(new double[] { .25, .5 }); outs.Add(new double[] { 1 });
            //ins.Add(new double[] { .5, .25 }); outs.Add(new double[] { 1 });
            myTimer.Tick += new EventHandler(step);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread.Sleep((int)numericUpDown4.Value);
            neuralNet.backward(ins, outs, false);
            double err = neuralNet.getError(ins, outs);
            label8.Text = "Err: " + err.ToString("f2");
            label2.Text = "0,0: " + neuralNet.forward(new double[] { 0, 0 })[0].ToString("f2");
            label5.Text = "0,1: " + neuralNet.forward(new double[] { 0, 1 })[0].ToString("f2");
            label6.Text = "1,0: " + neuralNet.forward(new double[] { 1, 0 })[0].ToString("f2");
            label7.Text = "1,1: " + neuralNet.forward(new double[] { 1, 1 })[0].ToString("f2");
            graph();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            myTimer.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Graph
            graph();
        }



        public void graph()
        {
            //Draw the X,Y key
            //Draw by the block size
            //Draw top corner block

            Graphics g = Graphics.FromImage(DrawArea);
            int numBlocksX = (int)Math.Ceiling(DrawArea.Size.Width / numericUpDown1.Value);
            int numBlocksY = (int)Math.Ceiling(DrawArea.Size.Height / numericUpDown1.Value);

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            for (int bx = 0; bx < numBlocksX; bx++)
            {
                SolidBrush myBrush = new SolidBrush(Color.Blue.Interpolate(Color.Red, (double)bx/(double)numBlocksX));
                g.FillRectangle(myBrush, new Rectangle(bx * (int)numericUpDown1.Value, 0, (int)numericUpDown1.Value, (int)numericUpDown1.Value));
                g.FillRectangle(blackBrush, new Rectangle(bx * (int)numericUpDown1.Value, (int)(numericUpDown1.Value/2), (int)(numericUpDown1.Value), (int)(numericUpDown1.Value / 2)));
            }

            for (int by = 0; by < numBlocksY; by++)
            {
                SolidBrush myBrush = new SolidBrush(Color.Blue.Interpolate(Color.Red, (double)by / (double)numBlocksY));
                g.FillRectangle(myBrush, new Rectangle(0, by * (int)numericUpDown1.Value, (int)numericUpDown1.Value, (int)numericUpDown1.Value));
                g.FillRectangle(blackBrush, new Rectangle((int)(numericUpDown1.Value / 2), by * (int)numericUpDown1.Value, (int)(numericUpDown1.Value / 2), (int)(numericUpDown1.Value)));
            }

            for (int bx = 1; bx < numBlocksX; bx++)
            {
                for (int by = 1; by < numBlocksY; by++)
                {
                    double x = (double)(bx - 1) / (double)(numBlocksX - 1);
                    double y = (double)(by - 1) / (double)(numBlocksY - 1);
                    double z = neuralNet.forward(new double[]{ x, y })[0];
                    SolidBrush myBrush = new SolidBrush(Color.Blue.Interpolate(Color.Red, z));
                    g.FillRectangle(myBrush, new Rectangle(bx * (int)numericUpDown1.Value, by * (int)numericUpDown1.Value, (int)numericUpDown1.Value, (int)numericUpDown1.Value));
                }
            }
            pictureBox1.Image = DrawArea;
            g.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ci = 0;
            myTimer.Stop();
        }

        public void step(Object myObject, EventArgs myEventArgs)
        {
            //Thread.Sleep((int)numericUpDown4.Value);
            neuralNet.backward(ins, outs, false);
            double err = neuralNet.getError(ins, outs);
            if (checkBox1.Checked)
            {
                //Debug.WriteLine(err);
                if (err < .02)
                {
                    ci = 0;
                    myTimer.Stop();
                }
            }
            label8.Text = "Err: " + err.ToString("f2");
            label2.Text = "0,0: " + neuralNet.forward(new double[] { 0, 0 })[0].ToString("f2");
            label5.Text = "0,1: " + neuralNet.forward(new double[] { 0, 1 })[0].ToString("f2");
            label6.Text = "1,0: " + neuralNet.forward(new double[] { 1, 0 })[0].ToString("f2");
            label7.Text = "1,1: " + neuralNet.forward(new double[] { 1, 1 })[0].ToString("f2");
            if (ci % ((int)numericUpDown3.Value) == 0) graph();
            ci++;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            myTimer.Interval = (int)numericUpDown4.Value;
        }
    }

    public static class ex
    {
        public static Color Interpolate(this Color source, Color target, double percent)
        {
            var r = (byte)(source.R + (target.R - source.R) * Math.Min(Math.Max(percent, 0), 1));
            var g = (byte)(source.G + (target.G - source.G) * Math.Min(Math.Max(percent, 0), 1));
            var b = (byte)(source.B + (target.B - source.B) * Math.Min(Math.Max(percent, 0), 1));

            return Color.FromArgb(255, r, g, b);
        }
    }
}
