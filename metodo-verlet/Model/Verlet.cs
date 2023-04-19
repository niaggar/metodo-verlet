using metodo_verlet.Model3D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Media3D;

namespace metodo_verlet.Model
{
    class Verlet
    {
        private const double G = 200;
        private readonly List<CuerpoCeleste3D> cuerpos;
        private readonly double dt;

        public Verlet(List<CuerpoCeleste3D> cuerpos, double dt)
        {
            this.cuerpos = cuerpos;
            this.dt = dt;
        }

        public void Paso()
        {
            var au = new List<Vector3D>();
            foreach (var cuerpo in cuerpos)
            {
                au.Add(CalcularAceleracion(cuerpo, cuerpos));
            }

            foreach (var cuerpo in cuerpos)
            {
                cuerpo.CuerpoCelesteProp.Posicion = cuerpo.CuerpoCelesteProp.Posicion + cuerpo.CuerpoCelesteProp.Velocidad * dt + au[cuerpos.IndexOf(cuerpo)] * (Math.Pow(dt, 2) / 2);
            }

            var a = new List<Vector3D>();
            foreach (var cuerpo in cuerpos)
            {
                a.Add(CalcularAceleracion(cuerpo, cuerpos));
            }

            foreach (var cuerpo in cuerpos)
            {
                cuerpo.CuerpoCelesteProp.Velocidad = cuerpo.CuerpoCelesteProp.Velocidad + (a[cuerpos.IndexOf(cuerpo)] + au[cuerpos.IndexOf(cuerpo)]) * (dt / 2);
            }
        }

        private Vector3D CalcularAceleracion(CuerpoCeleste3D cuerpo, List<CuerpoCeleste3D> cuerpos)
        {
            var fuerzaResultante = new Vector3D();

            foreach (var otroCuerpo in cuerpos)
            {
                if (otroCuerpo == cuerpo) continue;

                var r = otroCuerpo.CuerpoCelesteProp.Posicion - cuerpo.CuerpoCelesteProp.Posicion;
                var r_hat = r / r.Length;
                var fuerza = G * cuerpo.CuerpoCelesteProp.Masa * otroCuerpo.CuerpoCelesteProp.Masa / Math.Pow(r.Length, 2);

                fuerzaResultante += fuerza * r_hat;
            }

            return fuerzaResultante / cuerpo.CuerpoCelesteProp.Masa;
        }
    }
}
