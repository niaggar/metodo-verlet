using HelixToolkit.Wpf;
using metodo_verlet.Model3D;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace metodo_verlet.Windows
{
    /// <summary>
    /// Interaction logic for EditarCondicionesWindow.xaml
    /// </summary>
    public partial class EditarCondicionesWindow : Window, INotifyPropertyChanged
    {
        CuerpoCeleste3D CuerpoCeleste3D { get; set; }
        ArrowVisual3D VelocidadArrow { get; set; }

        public delegate void CuerpoCelesteEnEdicion(Guid guidEdicion, Vector3D posicion, Vector3D velocidad);
        public event CuerpoCelesteEnEdicion ActualizarPosicionVelocidad;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public double PosicionX
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Posicion.X;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Posicion = new Vector3D(value, PosicionY, PosicionZ);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("PosicionX");
            }
        }
        public double PosicionY
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Posicion.Y;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Posicion = new Vector3D(PosicionX, value, PosicionZ);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("PosicionY");
            }
        }
        public double PosicionZ
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Posicion.Z;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Posicion = new Vector3D(PosicionX, PosicionY, value);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("PosicionZ");
            }
        }

        public double VelocidadX
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Velocidad.X;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Velocidad = new Vector3D(value, VelocidadY, VelocidadZ);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("VelocidadX");
            }
        }
        public double VelocidadY
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Velocidad.Y;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Velocidad = new Vector3D(VelocidadX, value, VelocidadZ);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("VelocidadY");
            }
        }
        public double VelocidadZ
        {
            get => CuerpoCeleste3D.CuerpoCelesteProp.Velocidad.Z;
            set
            {
                CuerpoCeleste3D.CuerpoCelesteProp.Velocidad = new Vector3D(VelocidadX, VelocidadY, value);
                CuerpoCeleste3D.ActualizarMovimiento();
                ActualizarVectorVelocidad();
                this.OnPropertyChanged("VelocidadZ");
            }
        }



        public EditarCondicionesWindow(ref CuerpoCeleste3D cuerpoEnEdicion, ref ArrowVisual3D velocidadArrow)
        {
            InitializeComponent();

            CuerpoCeleste3D = cuerpoEnEdicion;
            VelocidadArrow = velocidadArrow;

            this.Title = "Edicion: " + CuerpoCeleste3D.CuerpoCelesteProp.Nombre;
            this.DataContext = this;
        }

        private void ActualizarVectorVelocidad()
        {
            VelocidadArrow.Point1 = (CuerpoCeleste3D.CuerpoCelesteProp.Posicion + new Vector3D(0, 0, CuerpoCeleste3D.CuerpoCelesteProp.Radio)).ToPoint3D();
            VelocidadArrow.Point2 = (CuerpoCeleste3D.CuerpoCelesteProp.Posicion + (CuerpoCeleste3D.CuerpoCelesteProp.Velocidad * 10)).ToPoint3D();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CuerpoCeleste3D.RecargarTextura();
        }
    }
}
