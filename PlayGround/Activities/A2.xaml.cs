using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using PlayGround.Models;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PlayGround.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class A2 : Page
    {
        private ExpressionNode _parallaxExpression;
        private CompositionPropertySet _scrollProperties;
        Compositor _compositor = Window.Current.Compositor;
        SpringVector3NaturalMotionAnimation _springAnimation;
        private List<ListModelTest> L1;






        private CompositionEffectBrush brush;
        private Compositor compositor;

        public A2()
        {
            this.InitializeComponent();
            L1 = ItemManager.GetT();
            TestList.ItemsSource = L1;
            TestList1.ItemsSource = L1;


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
            var blurEffectFactory = compositor.CreateEffectFactory(blurEffect);
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




            MainGrid.SizeChanged += OnMainGridSizeChanged;
        }



        private void OnMainGridSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SpriteVisual blurVisual = (SpriteVisual)ElementCompositionPreview.GetElementChildVisual(MainGrid);

            if (blurVisual != null)
            {
                blurVisual.Size = e.NewSize.ToVector2();
            }
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


        private void TestButotn_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.5f);

            (sender as UIElement).StartAnimation(_springAnimation);

            var expressionAnim = _compositor.CreateExpressionAnimation();

            // The ellipse's scale is inversely proportional to the rectangle's scale
            expressionAnim.Expression = "Vector3(1/scaleElement.Scale.X, 1/scaleElement.Scale.Y, 1)";
            expressionAnim.Target = "Scale";

            // Use SetExpressionReferenceParameter to alias a UIElement into the expression string
            expressionAnim.SetExpressionReferenceParameter("scaleElement", TestButotn);

            // Start the animation on the ellipse
            EllipseTest.StartAnimation(expressionAnim);

        }

        private void TestButotn_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Compositor compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            ScrollViewer myScrollViewer = TestList1.GetFirstDescendantOfType<ScrollViewer>();
            _scrollProperties = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(myScrollViewer);

            var scrollPropSet = _scrollProperties.GetSpecializedReference<ManipulationPropertySetReferenceNode>();
            var startOffset = ExpressionValues.Constant.CreateConstantScalar("startOffset", 0.0f);
            var parallaxValue = 0.5f;
            var parallax = (scrollPropSet.Translation.Y + startOffset);
            _parallaxExpression = parallax * parallaxValue - parallax;
        }

        private void TestList1_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            ListModelTest t = args.Item as ListModelTest;
            Image image = args.ItemContainer.ContentTemplateRoot.GetFirstDescendantOfType<Image>();

            Visual visual = ElementCompositionPreview.GetElementVisual(image);
            visual.Size = new Vector2(960f, 960f);

            if (_parallaxExpression != null)
            {
                _parallaxExpression.SetScalarParameter("StartOffset", (float)args.ItemIndex * visual.Size.Y / 4.0f);
                visual.StartAnimation("Offset.Y", _parallaxExpression);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_parallaxExpression != null)
            {
                // (TODO) Re-add this in after Dispose() implemented on ExpressionNode
                //_parallaxExpression.Dispose();
                _parallaxExpression = null;
            }

            if (_scrollProperties != null)
            {
                _scrollProperties.Dispose();
                _scrollProperties = null;
            }
        }

        private void TestButotn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(A3));
        }
    }
}
