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
    public partial class Form1 : Form // dichiarate le variabili necessarie
    {
        private Timer timer = new Timer(); //dichiarato un timer che aggiorna l'orologio ogni secondo
        private DateTime OraAttuale; //dichiarata per tenere traccia dell'ora corrente

        public Form1()//configura la larghezza del form dell'orologio. E imposta l'intervallo del timer a 1 sec, avvia il timer in modo che l'orologio venga aggiornato con l'orario attuale
        {
            Size = new Size(400, 400); // viene data una dimensione alla finestra, grazie a Size 

            timer.Interval = 1000; // timer configurato per scattare ogni secondo (1000 millesecondi)
            timer.Tick += TimerTick; // ogni volta che il timer scatta (ogni secondo) la funzione TimerTick verrà eseguita
            timer.Start(); //il timer viene avviato
        }
        private void TimerTick(object sender, EventArgs e) //funzione chiamata ogni volta che il timer scatta (ogni secondo)
        {
            OraAttuale = DateTime.Now;
            Invalidate(); // forza il ridisegno dell'orologio con l'orario attuale
        }

        protected override void OnPaint(PaintEventArgs e)//funzione che disegna l'orologio, ogni volta che viene chiamato Invalidate(), viene eseguita la funzione
        {
            base.OnPaint(e); //assicura che il comportamento predefinito della funzione OnPaint venga eseguito prima i disegnare l'orologio
            Graphics g = e.Graphics; //oggetto unsato per disegnare strumenti sul form

            int centroX = ClientSize.Width / 2; //calcola il centro del form per inserire correttamente l'orologio
            int centroY = ClientSize.Height / 2;
            int LunghezzaLancette = Math.Min(centroX, centroY) - 20;//calcola la lunghezza delle lancette in base al minimo  tra largezza e altezza del form
                                                                    //-20 tiene conto dello spazio per il contorno dell'orologio
           
            // Disegno del contorno dell'orologio
            g.DrawEllipse(Pens.Black, centroX - LunghezzaLancette, centroY - LunghezzaLancette, 2 * LunghezzaLancette, 2 * LunghezzaLancette);
            //sono segnate le coordinate del rettangolo che contine l'ellisse, in questo caso centrato nel punto (centroX, centroY) con dimensioni 2*LunghezzaLancette per l'altezza e la larghezza


            // Disegno dei contorni, segnando i minuti
            for (int i = 0; i < 60; i++) //scorre i 60 minuti dell'orologio per disegnare i segni dei minuti
            {
                double angle = 2 * Math.PI * i / 60 - Math.PI / 2; // calcoliamo un'angolo (angle) per ogni minuto all'interno del ciclo
                int markX1 = centroX + (int)((LunghezzaLancette - 10) * Math.Cos(angle)); //gli angoli in radinti si estendono da 0 a 2pigreco, vengono scalati di conseguenza
                int markY1 = centroY + (int)((LunghezzaLancette - 10) * Math.Sin(angle));//markX1 e markY1 sono le coordinate del punto iniiale del segno
                int markX2 = centroX + (int)(LunghezzaLancette * Math.Cos(angle)); //markX2 e markY2 sono le coordinate finali del puntodel segno
                int markY2 = centroY + (int)(LunghezzaLancette * Math.Sin(angle)); //mark vengono calcolati in base all'angolo e alla lunghezza delle lancette

                if (i % 5 == 0) //verifica se i (rappresenta i minuti) è un multiplo di 5, ossia il segno di un'ora 
                {
                    // nel caso sia verificato if, si disegnano le ore 12, 3, 6, 9 
                    int numeroX = centroX + (int)((LunghezzaLancette - 25) * Math.Cos(angle));//numeroX e numeroY calcolano le coordinate in cui verra disegnato il numero corrispondente all'ora
                    int numeroY = centroY + (int)((LunghezzaLancette - 25) * Math.Sin(angle)); //le coordinate sono calcolate in base all' "angle" dell'orologio e alla lunghezza delle lancette. 
                    //la posizione del numero è posta a lunghezzalancette-25 (unità della circonferenza dell'orologio)
                    g.DrawString((i == 0 ? "12" : (i == 15 ? "3" : (i == 30 ? "6" : (i == 45 ? "9" : "")))), //questa funzione disegna il numero corrispondente all'ora.Viene visualizzata per determinare quale numero disegnare
                        new Font("Arial", 12), Brushes.Black, numeroX, numeroY);//viene disegnato il testo
                }
                else
                {
                    g.DrawLine(Pens.Black, markX1, markY1, markX2, markY2); // se non è multiplo di 5 disegnare una linea nera tra markX1, markY1 e markX2, markY2
                }
            }

            // Calcolo degli angoli
            double AngoloSecondi = 2 * Math.PI * OraAttuale.Second / 60 - Math.PI / 2; //moltiplica la frazione nata da secondiattuali\60secondi, per 2pigreco (giro completo) e sottrae Math.PI/2 per garantire che la lancetta dei secondi punti verso l'alto quando è 0
            double AngoloMinuti = 2 * Math.PI * OraAttuale.Minute / 60 - Math.PI / 2;//simile all'angolo dei secondi
            double AngoloOre= 2 * Math.PI * (OraAttuale.Hour % 12) / 12 - Math.PI / 2;//è limitato alle 12 ore (usa modulo 12) per evitare angoli superiori a 360 gradi

            // Disegno lancetta dei secondi
            int secondiX = centroX + (int)(LunghezzaLancette * Math.Cos(AngoloSecondi)); //secondiX e Y sono le coordinate finali, mentre centroX e Y i punti di origine
            int secondiY = centroY + (int)(LunghezzaLancette * Math.Sin(AngoloSecondi));
            g.DrawLine(Pens.Red, centroX, centroY, secondiX, secondiY); // disegna la lancetta dei secondi

            // Disegno lancetta dei minuti
            int minutiX = centroX + (int)(LunghezzaLancette * 0.8 * Math.Cos(AngoloMinuti)); //simile ai secondi, ma utilizzando 0.8 come lunghezza delle lancette anzichè la lunghezza completa del raggio del cerchio
            int minutiY = centroY + (int)(LunghezzaLancette * 0.8 * Math.Sin(AngoloMinuti));
            g.DrawLine(Pens.Blue, centroX, centroY, minutiX, minutiY); // disegna la lancetta dei minuti

            //Disegno lancetta delle ore
            int oreX= centroX + (int)(LunghezzaLancette * 0.6 * Math.Cos (AngoloOre));
            int oreY= centroY + (int)(LunghezzaLancette * 0.6 * Math.Sin (AngoloOre));
            g.DrawLine(Pens.Black, centroX, centroY, oreX, oreY);


        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
