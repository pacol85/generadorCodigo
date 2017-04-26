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
                dc.DrawImage(bitmap, new Rect(0, 0, 1550, 315)); //600, 135)); // bitmap.PixelWidth, bitmap.PixelHeight));

                int pos = 0; //37;
                int obpos = 1;
                int tam = 31; //22;
                int space = 6;

                //string tf = "Futura PT"; // Heavy";
                foreach (string word in text)
                {
                    var brush = new SolidColorBrush(Color.FromArgb(255, (byte)127, (byte)127, (byte)127)); //brush = Brushes.Gray;
                    /*if (obpos == 1)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)0, (byte)96, (byte)156));
                    }
                    if(obpos == 2)
                    {
                        brush = new SolidColorBrush(Color.FromArgb(255, (byte)76, (byte)76, (byte)77));
                        tf = "Futura PT";
                        //tam = 12;
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
                        //tam = 10;
                        
                    }

                    if(obpos == 4)
                    {
                        pos = pos + 18;
                    }
                    */
                    pos = pos + tam + space;
                    var tf = "Futura PT";
                    switch (obpos)
                    {
                        case 1:
                            pos = pos - tam;
                            tf = "Futura PT Heavy";
                            brush = new SolidColorBrush(Color.FromArgb(255, (byte)36, (byte)176, (byte)227));
                            break;
                        case 2:
                            tf = "Futura PT Heavy";
                            brush = new SolidColorBrush(Color.FromArgb(255, (byte)36, (byte)176, (byte)227));
                            break;
                        case 4:
                            if(word == "")
                            pos = pos - tam - space;
                            break;
                        case 6:
                            pos = pos + tam + space;
                            break;
                    }
                   
                    FormattedText texto = new FormattedText(
                    word,
                    CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(tf),
                    tam,
                    brush);
                    Point p = new Point(1, pos); //300 - texto.Width / 2, pos);
                    dc.DrawText(texto, p);
                    /*
                    if(obpos > 3)
                    {
                        pos = pos + 10;
                    }*/
                    obpos = obpos + 1;
                }
                
            }

            RenderTargetBitmap target = new RenderTargetBitmap(1550, 315, 96, 96, PixelFormats.Default);//600, 135, 96, 96, PixelFormats.Default);
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
            if( largo > 42) //32)
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
            var company = "AIG";
            var lcompany = "AIG Seguros, El Salvador, S.A.";
            var address = "Calle Loma Linda, No. 265, Col. San Benito,";
            var city = "San Salvador, El Salvador.";

            var tel = "Tel 503-" + tbtel1.Text.Substring(0, 4) + "-" + tbtel1.Text.Substring(4, 4);
            if (!(tbtel2.Text == "" || tbtel2 == null))
            {
                tel = tel + " | Cel 503-" + tbtel2.Text.Substring(0, 4) + "-" + tbtel2.Text.Substring(4, 4);
            }
            tel = tel + " | Fax 503-2250-3201";

            string[] text = new string[8] { tbnombre.Text, company, puestoa, puestob, lcompany, address, city, tel };
            //"+503 " +tbtel1.Text, "+503 " + tbtel2.Text };
            /*if(tbtel2.Text == "" || tbtel2 == null)
            {
                text = new string[8] { tbnombre.Text.ToUpper(), company, puestoa.ToUpper(), puestob.ToUpper(), lcompany, address, city,
                    "+503 " + tbtel1.Text};
            }*/
            String inputFile = "C:\\drivers\\firmaX.png";

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
                if (largo > 42)//32)
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
                /*
                var num2 = "";
                if(usuario.Length > 3)
                {
                    if(usuario[3] != null && usuario[3] != "")
                    {
                        num2 = "+503 " + usuario[3];
                    }
                    
                }
                */
                var company = "AIG";
                var lcompany = "AIG Seguros, El Salvador, S.A.";
                var address = "Calle Loma Linda, No. 265, Col. San Benito,";
                var city = "San Salvador, El Salvador.";

                var tel = "Tel 503-" + usuario[2].Substring(0, 4) + "-" + usuario[2].Substring(4, 4);
                if (!(usuario[3] == "" || usuario[3] == null))
                {
                    tel = tel + " | Cel 503-" + usuario[3].Substring(0, 4) + "-" + usuario[3].Substring(4, 4);
                }
                tel = tel + " | Fax 503-2250-3201";

                string[] text = new string[8] { usuario[0], company, puestoa, puestob, lcompany, address, city, tel };
              
                //string[] text = new string[5] { usuario[0].ToUpper(), puestoa.ToUpper(), puestob.ToUpper(), "+503 " + usuario[2], num2 };
                String inputFile = "C:\\drivers\\firmaX.png";

                string output = "C:\\drivers\\" + text[0] + ".png";
                WriteTextToImage(inputFile, output, text);
            }
        }
    }

    
}
