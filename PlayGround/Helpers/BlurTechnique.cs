using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Composition;

namespace PlayGround.Helpers
{


    public abstract class EffectTechniques
    {
        protected Compositor _compositor;
        protected CompositionEffectFactory _effectFactory;
        protected LoadTimeEffectHandler _loadEffectHandler;

        public enum EffectTypes
        {
            Exposure = 0,
            Desaturation,
            Blur,
            SpotLight,
            PointLightFollow,
        }

        public EffectTechniques(Compositor compositor)
        {
            _compositor = compositor;
        }

        public abstract Task<CompositionDrawingSurface> LoadResources();
        public abstract void ReleaseResources();
        public abstract void OnPointerEnter(Vector2 pointerPosition, CompositionImage image);
        public abstract void OnPointerExit(Vector2 pointerPosition, CompositionImage image);
        public virtual void OnPointerMoved(Vector2 pointerPosition, CompositionImage image)
        {

        }

        public virtual CompositionEffectBrush CreateBrush()
        {
            return _effectFactory.CreateBrush();
        }

        public LoadTimeEffectHandler LoadTimeEffectHandler
        {
            get { return _loadEffectHandler; }
        }
    }




    public class BlurTechnique : EffectTechniques
    {



        ScalarKeyFrameAnimation _animationIncreasing;
        ScalarKeyFrameAnimation _animationDecreasing;

        public BlurTechnique(Compositor compositor) : base(compositor)
        {
        }

#pragma warning disable 1998
        public override async Task<CompositionDrawingSurface> LoadResources()
        {
            // Create the effect template
            var graphicsEffect = new ArithmeticCompositeEffect
            {
                Name = "Arithmetic",
                Source1 = new CompositionEffectSourceParameter("ImageSource"),
                Source1Amount = 1,
                Source2 = new CompositionEffectSourceParameter("EffectSource"),
                Source2Amount = 0,
                MultiplyAmount = 0
            };

            _effectFactory = _compositor.CreateEffectFactory(graphicsEffect, new[] { "Arithmetic.Source1Amount", "Arithmetic.Source2Amount" });


            // Create the animations
            _animationDecreasing = _compositor.CreateScalarKeyFrameAnimation();
            _animationDecreasing.InsertKeyFrame(1f, 0f);
            _animationDecreasing.Duration = TimeSpan.FromMilliseconds(1000);
            _animationDecreasing.IterationBehavior = AnimationIterationBehavior.Count;
            _animationDecreasing.IterationCount = 1;

            _animationIncreasing = _compositor.CreateScalarKeyFrameAnimation();
            _animationIncreasing.InsertKeyFrame(1f, 1f);
            _animationIncreasing.Duration = TimeSpan.FromMilliseconds(1000);
            _animationIncreasing.IterationBehavior = AnimationIterationBehavior.Count;
            _animationIncreasing.IterationCount = 1;

            _loadEffectHandler = ApplyBlurEffect;

            return null;
        }

        private void ApplyBlurEffect(CompositionDrawingSurface surface, CanvasBitmap bitmap, CompositionGraphicsDevice device)
        {
            GaussianBlurEffect blurEffect = new GaussianBlurEffect()
            {
                Source = bitmap,
                BlurAmount = 40.0f,
                BorderMode = EffectBorderMode.Hard,
            };

            using (var ds = CanvasComposition.CreateDrawingSession(surface))
            {
                ds.DrawImage(blurEffect);
                ds.FillRectangle(new Rect(0, 0, surface.Size.Width, surface.Size.Height), Windows.UI.Color.FromArgb(60, 0, 0, 0));
            }
        }

        public override void ReleaseResources()
        {
            if (_effectFactory != null)
            {
                _effectFactory.Dispose();
                _effectFactory = null;
            }

            if (_animationIncreasing != null)
            {
                _animationIncreasing.Dispose();
                _animationIncreasing = null;
            }

            if (_animationDecreasing != null)
            {
                _animationDecreasing.Dispose();
                _animationDecreasing = null;
            }
        }

        public override void OnPointerEnter(Vector2 pointerPosition, CompositionImage image)
        {
            image.Brush.StartAnimation("Arithmetic.Source1Amount", _animationDecreasing);
            image.Brush.StartAnimation("Arithmetic.Source2Amount", _animationIncreasing);
        }

        public override void OnPointerExit(Vector2 pointerPosition, CompositionImage image)
        {
            image.Brush.StartAnimation("Arithmetic.Source1Amount", _animationIncreasing);
            image.Brush.StartAnimation("Arithmetic.Source2Amount", _animationDecreasing);
        }
    }
}
