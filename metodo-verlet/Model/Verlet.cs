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
                for (int i = cuerpos.IndexOf(cuerpo) + 1; i < cuerpos.Count; i++)
                {
                    var otroCuerpo = cuerpos[i];
                    if (otroCuerpo == cuerpo) continue;

                    var r = otroCuerpo.CuerpoCelesteProp.Posicion - cuerpo.CuerpoCelesteProp.Posicion;
                    if (r.Length <= cuerpo.CuerpoCelesteProp.Radio + otroCuerpo.CuerpoCelesteProp.Radio)
                    {
                        var v1 = cuerpo.CuerpoCelesteProp.Velocidad;
                        var m1 = cuerpo.CuerpoCelesteProp.Masa;

                        var v2 = otroCuerpo.CuerpoCelesteProp.Velocidad;
                        var m2 = otroCuerpo.CuerpoCelesteProp.Masa;

                        var u1 = (v1 * (m1 - m2) + v2 * (2 * m2)) / (m1 + m2);
                        var u2 = (v2 * (m2 - m1) + v1 * (2 * m1)) / (m1 + m2);

                        cuerpo.CuerpoCelesteProp.Velocidad = u1;
                        otroCuerpo.CuerpoCelesteProp.Velocidad = u2;
                    }
                    else
                    {
                        cuerpo.CuerpoCelesteProp.Aceleracion = (a[cuerpos.IndexOf(cuerpo)] + au[cuerpos.IndexOf(cuerpo)]) / 2;
                        cuerpo.CuerpoCelesteProp.Velocidad = cuerpo.CuerpoCelesteProp.Velocidad + (cuerpo.CuerpoCelesteProp.Aceleracion) * (dt);

                        otroCuerpo.CuerpoCelesteProp.Aceleracion = (a[cuerpos.IndexOf(otroCuerpo)] + au[cuerpos.IndexOf(otroCuerpo)]) / 2;
                        otroCuerpo.CuerpoCelesteProp.Velocidad = otroCuerpo.CuerpoCelesteProp.Velocidad + (otroCuerpo.CuerpoCelesteProp.Aceleracion) * (dt);
                    }
                }
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
