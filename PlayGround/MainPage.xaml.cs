using PlayGround.Activities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PlayGround
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private void LoginPressed(object sender, RoutedEventArgs e)
        {
            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", Cortana);
            Frame.Navigate(typeof(A1), null, new DrillInNavigationTransitionInfo());
        }

        private void RegisterPressed(object sender, RoutedEventArgs e)
        {

        }

        private void Login_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //Login.Scale = new Vector3(2.0f, 2.0f, 2.0f);
        }

        private void Login_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Login.Scale = new Vector3(0f, 0f, 0f);
        }
    }
}
