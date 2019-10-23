using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PlayGround.Elements
{
    public sealed partial class MyUserControl4 : UserControl
    {
        public MyUserControl4()
        {
            this.InitializeComponent();
        }

        private void Toggler_Checked(object sender, RoutedEventArgs e)
        {
            Toggler.Rotation = 45;
        }

        private void Toggler_Unchecked(object sender, RoutedEventArgs e)
        {
            Toggler.Rotation = 0;
        }

        private void Toggler_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Toggler.Scale = new System.Numerics.Vector3(3.0f, 3.0f, 1);
        }

        private void Toggler_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Toggler.Scale = new System.Numerics.Vector3(1, 1, 1);
        }
    }
}
