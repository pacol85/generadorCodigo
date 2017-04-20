using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;
using System.Globalization;
using System.IO;

namespace generadorCodigo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }


        public static void WriteTextToImage(string inputFile, string outputFile, string[] text)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(inputFile)); // inputFile must be absolute path
            DrawingVisual visual = new DrawingVisual();

            using (DrawingContext dc = visual.RenderOpen())
            {
                dc.DrawImage(bitmap, new Rect(0, 0, 600, 135)); // bitmap.PixelWidth, bitmap.PixelHeight));

                int pos = 37;
                int obpos = 1;
                int tam = 22;

                string tf = "Futura PT Heavy";
                foreach (string word in text)
                {
                    var brush = Brushes.Gray;
                    if (obpos == 1)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)96, (byte)156));
                    }
                    if(obpos == 2)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)76, (byte)76, (byte)77));
                        tf = "Futura PT";
                        tam = 12;
                        pos = pos + 25;
                    }
                    if (obpos == 3)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)76, (byte)76, (byte)77));
                        pos = pos + 11;
                    }
                    if (obpos > 3)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)117, (byte)117, (byte)117));
                        tam = 10;
                        
                    }

                    if(obpos == 4)
                    {
                        pos = pos + 18;
                    }
                   
                    FormattedText texto = new FormattedText(
                    word,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tf),
                    tam,
                    brush);
                    Point p = new Point(300 - texto.Width / 2, pos);
                    dc.DrawText(texto, p);
                    
                    if(obpos > 3)
                    {
                        pos = pos + 10;
                    }
                    obpos = obpos + 1;
                }
                //dc.DrawText(text, position);
                /*
                FormattedText text = new FormattedText(
                    "Hello",
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Segeo UI"),
                    20,
                    Brushes.Red);
                */
                //Point p = new Point(300 - text.Width / 2, 44);
            }

            RenderTargetBitmap target = new RenderTargetBitmap(600, 135, 96, 96, PixelFormats.Default);
                /*
                 bitmap.PixelWidth, bitmap.PixelHeight,
                                                               bitmap.DpiX, bitmap.DpiY, PixelFormats.Default);
*/
                target.Render(visual);

            BitmapEncoder encoder = null;

            switch (Path.GetExtension(outputFile))
            {
                case ".png":
                    encoder = new PngBitmapEncoder();
                    break;
                    // more encoders here
            }

            if (encoder != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(target));
                using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
                {
                    encoder.Save(outputStream);
                }
            }
        }

        private void EnviarTexto(object sender, RoutedEventArgs e)
        {
            var puesto = tbpuesto.Text;
            var puestoa = "";
            var puestob = "";
            var largo = puesto.Length;
            if( largo > 32)
            {
                var half = largo / 2;
                var espacio = puesto.LastIndexOf(" ", half);
                puestoa = puesto.Substring(0, espacio);
                puestob = puesto.Substring(espacio);
            }
            

            string[] text = new string[5] { tbnombre.Text.ToUpper(), puestoa.ToUpper(), puestob.ToUpper(), "+503 " +tbtel1.Text, "+503 " + tbtel2.Text };
            if(tbtel2.Text == "" || tbtel2 == null)
            {
                text = new string[4] { tbnombre.Text.ToUpper(), puestoa.ToUpper(), puestob.ToUpper(), "+503 " + tbtel1.Text};
            }
            String inputFile = "C:\\drivers\\firma.png";

            string output = "C:\\drivers\\" + text[0] + ".png";
            WriteTextToImage(inputFile, output, text);
        }

        private void LeerCSV(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\drivers\paraFirma.csv";
            StreamReader sr = new StreamReader(filePath, System.Text.Encoding.Default);
            var lines = new List<string[]>();
            int Row = 0;
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(',');
                lines.Add(Line);
                Row++;
                Console.WriteLine(Row);
            }

            var data = lines.ToArray();

            EnviarTextoCSV(data);
        }

        private void EnviarTextoCSV(string[][] data)
        {
            foreach (var usuario in data)
            {
                var puesto = usuario[1];
                var puestoa = "";
                var puestob = "";
                var largo = puesto.Length;
                if (largo > 32)
                {
                    var half = largo / 2;
                    var espacio = puesto.LastIndexOf(" ", half);
                    puestoa = puesto.Substring(0, espacio);
                    puestob = puesto.Substring(espacio);
                }
                else
                {
                    puestoa = puesto;
                }

                var num2 = "";
                if(usuario.Length > 3)
                {
                    if(usuario[3] != null && usuario[3] != "")
                    {
                        num2 = "+503 " + usuario[3];
                    }
                    
                }
                
                string[] text = new string[5] { usuario[0].ToUpper(), puestoa.ToUpper(), puestob.ToUpper(), "+503 " + usuario[2], num2 };
                String inputFile = "C:\\drivers\\firma.png";

                string output = "C:\\drivers\\" + text[0] + ".png";
                WriteTextToImage(inputFile, output, text);
            }
        }
    }

    
}
