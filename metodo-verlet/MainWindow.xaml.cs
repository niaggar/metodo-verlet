using HelixToolkit.Wpf;
using metodo_verlet.Model;
using metodo_verlet.Model3D;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace metodo_verlet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<CuerpoCeleste3D> cuerpos;
        Verlet verlet;

        private Timer _timer;
        private bool verletEnProgreso = false;

        public MainWindow()
        {
            InitializeComponent();

            CrearPlanetas();    
        }

        private void InicializarTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1; // Intervalo de tiempo en milisegundos
            _timer.Elapsed += ActualizarModelo;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }


        public int numeroDePaso = 0;
        private void ActualizarModelo(object sender, ElapsedEventArgs e)
        {
            if (verletEnProgreso) return;
            verletEnProgreso = true;

            Application.Current.Dispatcher.Invoke(() =>
            {
                verlet.Paso();
                numeroDePaso++;

                foreach (var cuerpo in cuerpos)
                {
                    cuerpo.ActualizarMovimiento();

                    if (numeroDePaso == 10)
                    {
                        var color = cuerpo.CuerpoCelesteProp.Color;
                        contenedor.Children.Add(new SphereVisual3D() { ThetaDiv = 10, PhiDiv = 10, Radius = 5, Center = cuerpo.CuerpoCelesteProp.Posicion.ToPoint3D(), Fill = color });
                    }
                }

                if (numeroDePaso == 10)
                {
                    numeroDePaso = 0;
                }
            });

            verletEnProgreso = false;
        }

        private void CrearPlanetas()
        {
            var luna = new CuerpoCeleste()
            {
                Nombre = "Luna",
                Posicion = new Vector3D(200, 0, 0),
                Masa = 200,
                Radio = 30,
                Velocidad = new Vector3D(20, 10, 0),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.White
            };

            var tierra = new CuerpoCeleste()
            {
                Nombre = "Tierra",
                Posicion = new Vector3D(0, 1000, 0),
                Masa = 2000,
                Radio = 100,
                Velocidad = new Vector3D(3, -3, 1),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.Blue
            };

            //var venus = new CuerpoCeleste()
            //{
            //    Nombre = "Tierra",
            //    Posicion = new Vector3D(0, 1000, 0),
            //    Masa = 1900,
            //    Radio = 90,
            //    Velocidad = new Vector3D(10, 0, 0),
            //    Aceleracion = new Vector3D(0, 0, 0)
            //};

            var marte = new CuerpoCeleste()
            {
                Nombre = "Marte",
                Posicion = new Vector3D(1000, 1000, 0),
                Masa = 1700,
                Radio = 70,
                Velocidad = new Vector3D(-4, 1, 0),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.Red
            };

            var saturno = new CuerpoCeleste()
            {
                Nombre = "Saturno",
                Posicion = new Vector3D(3000, -1000, 200),
                Masa = 3000,
                Radio = 230,
                Velocidad = new Vector3D(-12, 3, 0),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.Yellow
            };

            var jupiter = new CuerpoCeleste()
            {
                Nombre = "Jupiter",
                Posicion = new Vector3D(-10000, 1000, -100),
                Masa = 3700,
                Radio = 250,
                Velocidad = new Vector3D(2, 0, 1),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.Orange
            };

            var luna3d = new CuerpoCeleste3D(luna);
            var tierra3d = new CuerpoCeleste3D(tierra);
            //var venus3d = new CuerpoCeleste3D(venus);
            var marte3d = new CuerpoCeleste3D(marte);
            var saturno3d = new CuerpoCeleste3D(saturno);
            var jupiter3d = new CuerpoCeleste3D(jupiter);

            cuerpos = new List<CuerpoCeleste3D>() { luna3d, tierra3d, marte3d, saturno3d, jupiter3d };

            contenedor.Children.Add(luna3d);
            contenedor.Children.Add(tierra3d);
            //contenedor.Children.Add(venus3d);
            contenedor.Children.Add(marte3d);
            contenedor.Children.Add(saturno3d);
            contenedor.Children.Add(jupiter3d);
        }

        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {

            InicializarTimer();
            verlet = new Verlet(cuerpos, 0.1);
        }

        private void Detener_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            _timer = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}
