using HelixToolkit.Wpf;
using metodo_verlet.Model;
using metodo_verlet.Model3D;
using metodo_verlet.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private List<CuerpoCeleste3D> cuerpos;
        private Dictionary<Guid, ArrowVisual3D> velocidadArrow = new Dictionary<Guid, ArrowVisual3D>();
        private Dictionary<Guid, ArrowVisual3D> aceleracionArrow = new Dictionary<Guid, ArrowVisual3D>();
        private Verlet verlet;
        private Timer _timer;

        private bool verletEnProgreso = false;
        private int numeroDePaso = 0;


        public MainWindow()
        {
            InitializeComponent();

            CrearPlanetas();    
        }



        private void MostrarEdicion(Guid guidEdicion)
        {
            var cuerpoEnEdicion = cuerpos.First(c => c.Guid == guidEdicion);
            var velocidadEnEdicion = velocidadArrow[guidEdicion];
            var editarCondicionesWindow = new EditarCondicionesWindow(ref cuerpoEnEdicion, ref velocidadEnEdicion);
            editarCondicionesWindow.Show();
        }


        private void InicializarTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1;
            _timer.Elapsed += ActualizarModelo;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }


        private void ActualizarModelo(object? sender, ElapsedEventArgs e)
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
                        contenedor.Children.Add(new SphereVisual3D() { ThetaDiv = 2, PhiDiv = 2, Radius = 5, Center = cuerpo.CuerpoCelesteProp.Posicion.ToPoint3D(), Fill = color });
                    }

                    var proporcionVectorVelocidadRadio = cuerpo.CuerpoCelesteProp.Radio / cuerpo.CuerpoCelesteProp.Velocidad.Length;
                    var proporcionVectorAceleracionRadio = cuerpo.CuerpoCelesteProp.Radio / cuerpo.CuerpoCelesteProp.Aceleracion.Length;

                    var vectorVelocidad3D = velocidadArrow[cuerpo.Guid];
                    vectorVelocidad3D.Point1 = (cuerpo.CuerpoCelesteProp.Posicion + new Vector3D(0, 0, cuerpo.CuerpoCelesteProp.Radio)).ToPoint3D();
                    vectorVelocidad3D.Point2 = (cuerpo.CuerpoCelesteProp.Posicion + (cuerpo.CuerpoCelesteProp.Velocidad * proporcionVectorVelocidadRadio * 2)).ToPoint3D();

                    var vectorAceleracion3D = aceleracionArrow[cuerpo.Guid];
                    vectorAceleracion3D.Point1 = (cuerpo.CuerpoCelesteProp.Posicion + new Vector3D(0, 0, -cuerpo.CuerpoCelesteProp.Radio)).ToPoint3D();
                    vectorAceleracion3D.Point2 = (cuerpo.CuerpoCelesteProp.Posicion + (cuerpo.CuerpoCelesteProp.Aceleracion * proporcionVectorAceleracionRadio * 2)).ToPoint3D();
                }

                if (numeroDePaso == 10) numeroDePaso = 0;
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
                Velocidad = new Vector3D(0, 0, 0),
                Aceleracion = new Vector3D(0, 0, 0),
                Color = Brushes.Blue
            };

            var marte = new CuerpoCeleste()
            {
                Nombre = "Marte",
                Posicion = new Vector3D(0, 0, 0),
                Masa = 2000,
                Radio = 100,
                Velocidad = new Vector3D(0, 0, 0),
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
                Color = Brushes.Orange,
            };

            var luna3d = new CuerpoCeleste3D(luna);
            luna3d.CuerpoCeleste3DClick += MostrarEdicion;
            var tierra3d = new CuerpoCeleste3D(tierra);
            tierra3d.CuerpoCeleste3DClick += MostrarEdicion;
            var marte3d = new CuerpoCeleste3D(marte);
            marte3d.CuerpoCeleste3DClick += MostrarEdicion;
            var saturno3d = new CuerpoCeleste3D(saturno);
            saturno3d.CuerpoCeleste3DClick += MostrarEdicion;
            var jupiter3d = new CuerpoCeleste3D(jupiter);
            jupiter3d.CuerpoCeleste3DClick += MostrarEdicion;

            cuerpos = new List<CuerpoCeleste3D>() { luna3d, tierra3d, marte3d, saturno3d, jupiter3d };
            //cuerpos = new List<CuerpoCeleste3D>() { marte3d, tierra3d, };

            foreach (var item in cuerpos)
            {
                var vectorVelocidad = new ArrowVisual3D() { Diameter = 5, Fill = Brushes.White };
                vectorVelocidad.Point1 = (item.CuerpoCelesteProp.Posicion + new Vector3D(0, 0, item.CuerpoCelesteProp.Radio)).ToPoint3D();
                vectorVelocidad.Point2 = (item.CuerpoCelesteProp.Posicion + (item.CuerpoCelesteProp.Velocidad * 2)).ToPoint3D();

                var vectorAceleracion = new ArrowVisual3D() { Diameter = 5, Fill = Brushes.Red };
                vectorAceleracion.Point1 = (item.CuerpoCelesteProp.Posicion + new Vector3D(0, 0, -item.CuerpoCelesteProp.Radio)).ToPoint3D();
                vectorAceleracion.Point2 = (item.CuerpoCelesteProp.Posicion + (item.CuerpoCelesteProp.Aceleracion * 2)).ToPoint3D();

                velocidadArrow.Add(item.Guid, vectorVelocidad);
                aceleracionArrow.Add(item.Guid, vectorAceleracion);

                contenedor.Children.Add(item);
                contenedor.Children.Add(vectorVelocidad);
                contenedor.Children.Add(vectorAceleracion);
            }
        }


        #region Eventos de ventana
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
        #endregion
    }
}
