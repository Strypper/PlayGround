using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PlayGround.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class A4 : Page
    {
        private Compositor compositor = Window.Current.Compositor;
        private SpriteVisual effectVisual;
        private CompositionEffectBrush effectBrush, effectBrushStraighten;
        private CompositionEffectFactory effectFactory, effectFactoryStraighten;
        private SpringScalarNaturalMotionAnimation _springAnimation;
        private LoadedImageSurface _LoadImageSurface;
        public A4()
        {
            this.InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Create Visual to contain effect
            effectVisual = compositor.CreateSpriteVisual();
            var destinationBrush = compositor.CreateBackdropBrush();
            //Create the Effect you want
            var graphicsEffect = new GaussianBlurEffect
            {
                Name = "Blur",
                BlurAmount = 0f,
                BorderMode = EffectBorderMode.Hard,
                Source = new CompositionEffectSourceParameter("Background")
            };



            effectFactory = compositor.CreateEffectFactory(graphicsEffect, new[] { "Blur.BlurAmount" });
            effectBrush = effectFactory.CreateBrush();
            effectBrush.SetSourceParameter("Background", destinationBrush);

            effectVisual.Brush = effectBrush;
            //By the default the Visual have the size width and height (defined in Vector 2) 0 and 0
            ResizeVisual();

            ElementCompositionPreview.SetElementChildVisual(Back, effectVisual);

            //Create Spring Animation for nature increase and decrease value
            _springAnimation = compositor.CreateSpringScalarAnimation();
            _springAnimation.Period = TimeSpan.FromSeconds(0.5);
            _springAnimation.DampingRatio = 0.75f;

        }

        private void ResizeVisual()
        {
            if (effectVisual == null) return;
            effectVisual.Size = new Vector2((float)Back.ActualWidth, (float)Back.ActualHeight);
        }
        private void BlurAmount_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var blur_amount = (float)e.NewValue;
            effectBrush.Properties.InsertScalar("Blur.BlurAmount", blur_amount);
            effectVisual.Brush = effectBrush;
        }


        private void BlurTheImage_Click(object sender, RoutedEventArgs e)
        {
            //ScalarKeyFrameAnimation blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            //blurAnimation.InsertKeyFrame(0.0f, 0.0f);
            //blurAnimation.InsertKeyFrame(1.0f, 100.0f);
            //blurAnimation.Duration = TimeSpan.FromSeconds(4);
            ////blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;



            //effectBrush.StartAnimation("Blur.BlurAmount", blurAnimation);
        }

        private void ToA5_Click(object sender, RoutedEventArgs e)
        {
            //ScalarKeyFrameAnimation blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            //blurAnimation.InsertKeyFrame(0.0f, 100.0f);
            //blurAnimation.InsertKeyFrame(1.0f, 0.0f);
            //blurAnimation.Duration = TimeSpan.FromSeconds(4);
            ////blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            //effectBrush.StartAnimation("Blur.BlurAmount", blurAnimation);
            Frame.Navigate(typeof(A5));

        }

        private void BlurTheImage_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _springAnimation.FinalValue = 100f;
            effectBrush.StartAnimation("Blur.BlurAmount", _springAnimation);
        }

        private void BlurTheImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _springAnimation.FinalValue = 0f;
            effectBrush.StartAnimation("Blur.BlurAmount", _springAnimation);
        }

        public void ApplyCompositor()
        {
            var container = compositor.CreateContainerVisual();
            container.RotationAngle = 45;

            ElementCompositionPreview.SetElementChildVisual(theGrid, container);


            _LoadImageSurface = LoadedImageSurface.StartLoadFromUri(new Uri("ms-appx:///Assets/21.png"));
            _LoadImageSurface.LoadCompleted += (sender, args) =>
            {
                if (args.Status == LoadedImageSourceLoadStatus.Success)
                {
                    var brush = compositor.CreateSurfaceBrush(_LoadImageSurface);
                    brush.Stretch = CompositionStretch.Uniform;
                    var imageVisual = compositor.CreateSpriteVisual();
                    imageVisual.Size = new Vector2(300, 300);
                    imageVisual.Brush = brush;
                    //imageVisual.RotationAngle = 45;
                    container.Children.InsertAtTop(imageVisual);
                };
            };
            //container.Compositor.CreateAmbientLight();
        }
    }
}
