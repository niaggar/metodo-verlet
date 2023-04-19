using metodo_verlet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace metodo_verlet.Model3D
{
    public class CuerpoCeleste3D : Esfera3D
    {
        public CuerpoCeleste CuerpoCelesteProp { get; set; }

        public CuerpoCeleste3D(CuerpoCeleste CuerpoCelesteProp) : base(CuerpoCelesteProp.Nombre, CuerpoCelesteProp.Radio)
        {
            this.CuerpoCelesteProp = CuerpoCelesteProp;

            ActualizarMovimiento();
        }

        public void ActualizarMovimiento()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var tg = new Transform3DGroup();
                tg.Children.Add(new TranslateTransform3D(CuerpoCelesteProp.Posicion));
                Transform = tg;
            });
        }
    }
}
