using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace metodo_verlet.Model
{
    public class CuerpoCeleste
    {
        public string Nombre { get; set; }
        public double Masa { get; set; }
        public double Radio { get; set; }
        public Vector3D Posicion { get; set; }
        public Vector3D Velocidad { get; set; }
        public Vector3D Aceleracion { get; set; }
        public Brush Color { get; set; }


        public double PosicionX
        {
            get => Posicion.X;
        }
        public double PosicionY
        {
            get => Posicion.Y;
        }
        public double PosicionZ
        {
            get => Posicion.Z;
        }
    }
}
