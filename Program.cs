using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Sinconizacion_EXactus
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new  Login());
            //  Application.Run(new CORECTX_APP.COMPRAS.Importador.odoo());
           //  Application.Run(new CORECTX_APP.BODEGA.PROMOCIONES.Promociones());

            // Application.Run(new FrmPromociones());

            //      Habilitar estas lineas si se desea compilar Ejectutable de Cargador FTP KC
            //      FtpAutoCargaKC KC = new FtpAutoCargaKC();
            //      KC.Ejecutar();

        }
    }
}
