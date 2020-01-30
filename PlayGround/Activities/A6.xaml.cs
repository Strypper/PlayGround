using LottieUWP.Animation.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PlayGround.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class A6 : Page
    {
        private IOutputStream x;

        public A6()
        {
            this.InitializeComponent();
        }

        private async void Ink_Loaded(object sender, RoutedEventArgs e)
        {
            var s = sender as InkCanvas;
            //InkStroke st;
            //var list = st.GetInkPoints();
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => {
            //    System.Diagnostics.Debug.WriteLine(stroke);
            //});
        }
    }
}
