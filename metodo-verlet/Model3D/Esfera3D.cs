using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;
using HelixToolkit.Wpf;

namespace metodo_verlet.Model3D
{
    public class Esfera3D : ModelVisual3D
    {
        public ImageBrush Texture { get; private set; }
        public SphereVisual3D Sphere { get; private set; }
        
        
        public Esfera3D(string Nombre, double Radio)
        {
            Sphere = new SphereVisual3D() { ThetaDiv = 60, PhiDiv = 30, Radius = Radio };
            Children.Add(Sphere);

            ActualizarTextura(Nombre);
        }

        private BitmapImage ObtenerTextura(string Nombre)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("D:\\projects\\metodo-verlet\\metodo-verlet\\Textures\\" + Nombre + ".jpg");
            img.EndInit();
            return img;
        }

        void ActualizarTextura(string Nombre)
        {
            var img = ObtenerTextura(Nombre);
            Texture = new ImageBrush(img);
            
            Sphere.Material = MaterialHelper.CreateMaterial(Texture);
        }
    }
}
