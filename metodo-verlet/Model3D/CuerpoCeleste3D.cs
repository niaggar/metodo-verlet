using HelixToolkit.Wpf;
using metodo_verlet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace metodo_verlet.Model3D
{
    public class CuerpoCeleste3D : Esfera3D
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public CuerpoCeleste CuerpoCelesteProp { get; set; }

        public delegate void CuerpoCeleste3DHandler(Guid guid);
        public event CuerpoCeleste3DHandler CuerpoCeleste3DClick;

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

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var gm = Visual3DModel as GeometryModel3D;
                gm.Material = gm.Material == Materials.Yellow ? MaterialHelper.CreateMaterial(Texture) : Materials.Yellow;

                CuerpoCeleste3DClick?.Invoke(this.Guid);

                e.Handled = true;
            }
        }
    }
}
