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
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PlayGround.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class A1 : Page
    {
        public A1()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("ForwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(Cortana, new[] { Back, Test });
            }
            CreateImplicitAnimation();
            Panel.Visibility = Visibility.Visible;
            Canvas.Opacity = 1;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            rectangle.Rotation = Int32.Parse(Value.Text);
            System.Diagnostics.Debug.WriteLine(rectangle.CenterPoint.X.ToString());
            System.Diagnostics.Debug.WriteLine(rectangle.CenterPoint.Y.ToString());
            rectangle1.Rotation = Int32.Parse(Value.Text);
            myStoryboard.Begin();
        }

        private void Reverse_Click(object sender, RoutedEventArgs e)
        {
            myStoryboard.AutoReverse = true;
        }



        private void HoverEffect_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            rectangle2.Translation = new Vector3(100, 0, 0);
            TestingGrid.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(233, 220, 150, 100));
            
        }

        private void HoverEffect_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            rectangle2.Translation = new Vector3(0, 0, 0);
            TestingGrid.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(153, 20, 100, 0));
        }









        Windows.UI.Composition.Compositor _composition = null;
        void FetchCompositor()
        {
            if (_composition == null)
                _composition = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }
        void CreateImplicitAnimation()
        {
            FetchCompositor();
            Panel.Visibility = Visibility.Collapsed;

            var ScaleHeaderAnimation = _composition.CreateScalarKeyFrameAnimation();

            ScaleHeaderAnimation.InsertKeyFrame(0, 0);
            ScaleHeaderAnimation.InsertKeyFrame(1, 1f);
            ScaleHeaderAnimation.Duration = TimeSpan.FromSeconds(0.5f);
            ScaleHeaderAnimation.Target = "Scale.Y";
            ElementCompositionPreview.SetImplicitShowAnimation(Panel, ScaleHeaderAnimation);

            var FadeHeaderAnimation = _composition.CreateScalarKeyFrameAnimation();

            FadeHeaderAnimation.InsertKeyFrame(0, 0);
            FadeHeaderAnimation.InsertKeyFrame(1, 1f);
            FadeHeaderAnimation.Duration = TimeSpan.FromSeconds(1.5f);
            FadeHeaderAnimation.Target = "Opacity";
            ElementCompositionPreview.SetImplicitShowAnimation(Canvas, FadeHeaderAnimation);



        }

        private void HoverEffect_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(A2), null, new DrillInNavigationTransitionInfo());
        }



        //private void Page_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CreateImplicitAnimation();
        //    Panel.Visibility = Visibility.Visible;
        //    TestingGrid.Opacity = 1.0;
        //}
    }
    
}
