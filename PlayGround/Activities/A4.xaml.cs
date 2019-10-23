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
        private CompositionEffectBrush brush;
        private Compositor compositor;


        Compositor _compositor = Window.Current.Compositor;
        SpringVector3NaturalMotionAnimation _springAnimation;

        public A4()
        {
            this.InitializeComponent();


            MainGrid.SizeChanged += OnMainGridSizeChanged;

            compositor = ElementCompositionPreview.GetElementVisual(MainGrid).Compositor;

            // we create the effect. 
            // Notice the Source parameter definition. Here we tell the effect that the source will come from another element/object
            var blurEffect = new GaussianBlurEffect
            {
                Name = "Blur",
                Source = new CompositionEffectSourceParameter("background"),
                BlurAmount = 100f,
                BorderMode = EffectBorderMode.Hard,
            };

            // we convert the effect to a brush that can be used to paint the visual layer
            var blurEffectFactory = compositor.CreateEffectFactory(blurEffect, new[] { "Blur.BlurAmount" });
            brush = blurEffectFactory.CreateBrush();

            // We create a special brush to get the image output of the previous layer.
            // we are basically chaining the layers (xaml grid definition -> rendered bitmap of the grid -> blur effect -> screen)
            var destinationBrush = compositor.CreateBackdropBrush();
            brush.SetSourceParameter("background", destinationBrush);

            // we create the visual sprite that will hold our generated bitmap (the blurred grid)
            // Visual Sprite are "raw" elements so there is no automatic layouting. You have to specify the size yourself
            var blurSprite = compositor.CreateSpriteVisual();
            blurSprite.Size = new Vector2((float)MainGrid.ActualWidth, (float)MainGrid.ActualHeight);
            blurSprite.Brush = brush;

            // we add our sprite to the rendering pipeline
            ElementCompositionPreview.SetElementChildVisual(MainGrid, blurSprite);

            //Animated

        }

        private void OnMainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SpriteVisual blurVisual = (SpriteVisual)ElementCompositionPreview.GetElementChildVisual(MainGrid);

            if (blurVisual != null)
            {
                blurVisual.Size = e.NewSize.ToVector2();
            }

            System.Diagnostics.Debug.WriteLine("Changed");
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // Scale up to 1.5
            CreateOrUpdateSpringAnimation(1.5f);

            (sender as UIElement).StartAnimation(_springAnimation);

            var blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            blurAnimation.InsertKeyFrame(0.0f, 100.0f);
            blurAnimation.InsertKeyFrame(0.5f, 0.0f);
            blurAnimation.Duration = TimeSpan.FromSeconds(3);
            //blurAnimation.IterationBehavior = AnimationIterationBehavior.Forever;
            brush.StartAnimation("Blur.BlurAmount", blurAnimation);
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Scale back down to 1.0
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }





        private void CreateOrUpdateSpringAnimation(float finalValue)
        {
            if (_springAnimation == null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }

            _springAnimation.FinalValue = new Vector3(finalValue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(A5));
        }
    }
}
