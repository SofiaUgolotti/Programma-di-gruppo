using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orologioo
{
    public partial class Form1 : Form 
    {
        private Timer timer = new Timer(); 
        private DateTime OraAttuale; 

        public Form1()
        {
            Size = new Size(400, 400);

            timer.Interval = 1000; 
            timer.Tick += TimerTick; 
            timer.Start(); 
        }
        private void TimerTick(object sender, EventArgs e) 
        {
            OraAttuale = DateTime.Now;
            Invalidate(); 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //LunghezzaLancette rappresenta la lancetta dei secondi (=raggio del cerchio)
            int centroX = ClientSize.Width / 2; 
            int centroY = ClientSize.Height / 2;
            int LunghezzaLancette = Math.Min(centroX, centroY) - 20;//-20 spazio per il contorno dell'orologio nel form
            
            base.OnPaint(e); 
            Graphics g = e.Graphics; 
            //contorno dell'orologio
            g.DrawEllipse(Pens.Black, centroX - LunghezzaLancette, centroY - LunghezzaLancette, 2 * LunghezzaLancette, 2 * LunghezzaLancette);
            


            for (int i = 0; i < 60; i++) 
            {
                double angle = 2 * Math.PI * i / 60 - Math.PI / 2; 
                int markX1 = centroX + (int)((LunghezzaLancette - 10) * Math.Cos(angle)); 
                int markY1 = centroY + (int)((LunghezzaLancette - 10) * Math.Sin(angle));//markX1 e markY1 coordinate del punto iniiale del segno
                int markX2 = centroX + (int)(LunghezzaLancette * Math.Cos(angle)); //markX2 e markY2 coordinate finali del punto del segno
                int markY2 = centroY + (int)(LunghezzaLancette * Math.Sin(angle)); 

                if (i % 5 == 0) //i (= minuti)
                {
                    // disegnate le ore 12, 3, 6, 9 
                    int numeroX = centroX + (int)((LunghezzaLancette - 25) * Math.Cos(angle));//numeroX e numeroY calcolano le coordinate di dove verrano disegnate
                    int numeroY = centroY + (int)((LunghezzaLancette - 25) * Math.Sin(angle)); 
                    g.DrawString((i == 0 ? "12" : (i == 15 ? "3" : (i == 30 ? "6" : (i == 45 ? "9" : "")))), new Font("Arial", 12), Brushes.Black, numeroX, numeroY);

                }
                // disegna una linea nera tra markX1, markY1 e markX2, markY2
                else
                {
                    g.DrawLine(Pens.Black, markX1, markY1, markX2, markY2);      
                }
            }

            //calcolo degli angoli
            double AngoloSecondi = 2 * Math.PI * OraAttuale.Second / 60 - Math.PI / 2; 
            double AngoloMinuti = 2 * Math.PI * OraAttuale.Minute / 60 - Math.PI / 2;
            double AngoloOre= 2 * Math.PI * (OraAttuale.Hour % 12) / 12 - Math.PI / 2;//è limitato alle 12 ore (usa modulo 12) per evitare angoli superiori a 360 gradi

            //disegno lancetta dei secondi
            int secondiX = centroX + (int)(LunghezzaLancette * Math.Cos(AngoloSecondi)); //secondiX e Y coordinate finali,  centroX e Y coordinate di origine
            int secondiY = centroY + (int)(LunghezzaLancette * Math.Sin(AngoloSecondi));
            g.DrawLine(Pens.Red, centroX, centroY, secondiX, secondiY); 

            //disegno lancetta dei minuti
            int minutiX = centroX + (int)(LunghezzaLancette * 0.8 * Math.Cos(AngoloMinuti)); 
            int minutiY = centroY + (int)(LunghezzaLancette * 0.8 * Math.Sin(AngoloMinuti));
            g.DrawLine(Pens.Blue, centroX, centroY, minutiX, minutiY); 

            //disegno lancetta delle ore
            int oreX= centroX + (int)(LunghezzaLancette * 0.6 * Math.Cos (AngoloOre));
            int oreY= centroY + (int)(LunghezzaLancette * 0.6 * Math.Sin (AngoloOre));
            g.DrawLine(Pens.Black, centroX, centroY, oreX, oreY);


        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
