using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

using NAudio;
using NAudio.Wave;
using NAudio.Dsp;

namespace Circo
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public enum EstadodeJuego { menu, gameplay, gameOver}

        public EstadodeJuego estadoActual = EstadodeJuego.menu;

        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;

        int aux = 0;

        public void iniciar()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
            if (aux == 0)
            {
                ThreadStart threadStart = new ThreadStart(actualizar);
                Thread threadPuntos = new Thread(threadStart);
                threadPuntos.Start();
                aux = 1;
            }
            estadoActual = EstadodeJuego.gameplay;
            pantallaPrincipal.Children.Clear();
            pantallaPrincipal.Children.Add(new Gameplay(perder));
            lblHertz.Visibility = Visibility.Visible;
        }

        public void perder()
        {
            stopwatch.Stop();
            estadoActual = EstadodeJuego.gameOver;
            pantallaPrincipal.Children.Clear();
            pantallaPrincipal.Children.Add(new GameOver(menu));
        }

        public void menu()
        {
            estadoActual = EstadodeJuego.menu;
            pantallaPrincipal.Children.Clear();
            pantallaPrincipal.Children.Add(new Menu(iniciar));
            lblHertz.Visibility = Visibility.Hidden;
            stopwatch.Restart();
        }
        void actualizar()
        {
            while (true)
            {
                if (estadoActual == EstadodeJuego.gameplay)
                {
                    Dispatcher.Invoke(
                   () =>
                       {
                           var tiempoActual = stopwatch.Elapsed;
                           var deltaTime = tiempoActual - tiempoAnterior;

                           tiempoAnterior = tiempoActual;

                           lblHertz.Text = tiempoAnterior.TotalSeconds.ToString("N") + " Puntos";
                       }
                   );
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            pantallaPrincipal.Children.Add(new Menu(iniciar));
        }

    }
    }
