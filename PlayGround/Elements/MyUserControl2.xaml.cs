using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PlayGround.Elements
{
    public sealed partial class MyUserControl2 : UserControl
    {
        public MyUserControl2()
        {
            this.InitializeComponent();
        }
        private async void Border_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Title.Scale = new System.Numerics.Vector3(0.3f, 0.3f, 0);
            Description.Translation = new System.Numerics.Vector3(0, -3, 0);
            await CoverImage.Blur(value: 10, duration: 1500, delay: 0).StartAsync();
        }
        private async void Border_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Title.Scale = new System.Numerics.Vector3(1, 1, 0);
            Description.Translation = new System.Numerics.Vector3(0, 150, 0);
            await CoverImage.Blur(value: 0, duration: 700, delay: 0).StartAsync();
        }
    }
}
