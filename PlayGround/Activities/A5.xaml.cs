﻿using PlayGround.Models;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
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

using EF = Microsoft.Toolkit.Uwp.UI.Animations.Expressions.ExpressionFunctions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PlayGround.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class A5 : Page
    {
        public A5()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<PhotoModel> list = new List<PhotoModel>();
            list.Add(new PhotoModel() { Name = "Hello", Info = "Description", Image = new Uri("ms-appx:///Assets/TestPics/Idea 21.png") });
            list.Add(new PhotoModel() { Name = "Hello", Info = "Description", Image = new Uri("ms-appx:///Assets/TestPics/Idea 1.png") });
            list.Add(new PhotoModel() { Name = "Hello", Info = "Description", Image = new Uri("ms-appx:///Assets/TestPics/Idea 10.png") });
            list.Add(new PhotoModel() { Name = "Hello", Info = "Description", Image = new Uri("ms-appx:///Assets/TestPics/Idea 25.png") });
            Items.ItemsSource = list;
            Items1.ItemsSource = list;


            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _container = _compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(root, _container);

            TiltUIElement();
        }

        private void A6_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(A6));
        }



        public static string StaticSampleName => "Pointer Rotate";
        public static string StaticSampleDescription => "Animate the Rotation Angle and Axis of an object based on the hover position of the pointer.";

        private void TiltUIElement()
        {

            // Grab the backing Visual for the UIElement image
            _tiltVisual = ElementCompositionPreview.GetElementVisual(tiltImage);

            // Set the CenterPoint of the backing Visual, so the rotation axis will defined from the middle
            _tiltVisual.CenterPoint = new Vector3((float)tiltImage.Width / 2, (float)tiltImage.Height / 2, 0f);

            // Grab the PropertySet containing the hover pointer data that will be used to drive the rotations
            // Note: We have a second UIElement we will grab the pointer data against and the other we will animate
            _hoverPositionPropertySet = ElementCompositionPreview.GetPointerPositionPropertySet(HitTestRect);

            // Calculate distance from corner of quadrant to Center
            var center = new Vector3((float)tiltImage.Width / 2, (float)tiltImage.Height / 2, 0);
            var xSquared = Math.Pow(tiltImage.Width / 2, 2);
            var ySquared = Math.Pow(tiltImage.Height / 2, 2);
            var distanceToCenter = (float)Math.Sqrt(xSquared + ySquared);

            // || DEFINE THE EXPRESSION FOR THE ROTATION ANGLE ||             
            // We calculate the Rotation Angle such that it increases from 0 to 35 as the cursor position moves away from the center.
            // Combined with animating the Rotation Axis, the image is "push down" on the point at which the cursor is located.
            // Note: We special case when the hover position is (0,0,0) as this is the starting hover position and and we want the image to be flat (rotationAngle = 0) at startup.             
            var hoverPosition = _hoverPositionPropertySet.GetSpecializedReference<PointerPositionPropertySetReferenceNode>().Position;
            var angleExpressionNode =
                EF.Conditional(
                    hoverPosition == new Vector3(0, 0, 0),
                    ExpressionValues.CurrentValue.CreateScalarCurrentValue(),
                    35 * ((EF.Clamp(EF.Distance(center, hoverPosition), 0, distanceToCenter) % distanceToCenter) / distanceToCenter));

            _tiltVisual.StartAnimation("RotationAngleInDegrees", angleExpressionNode);

            // || DEFINE THE EXPRESSION FOR THE ROTATION AXIS ||             
            // The RotationAxis will be defined as the axis perpendicular to vector position of the hover pointer (vector from center to hover position).
            // The axis is a vector calculated by first converting the pointer position into the coordinate space where the center point (0, 0) is in the middle.
            // The perpendicular axis is then calculated by transposing the cartesian x, y components and negating one (e.g. Vector3(-y,x,0) )
            var axisAngleExpressionNode = EF.Vector3(
                -(hoverPosition.Y - center.Y) * EF.Conditional(hoverPosition.Y == center.Y, 0, 1),
                (hoverPosition.X - center.X) * EF.Conditional(hoverPosition.X == center.X, 0, 1),
                0);

            _tiltVisual.StartAnimation("RotationAxis", axisAngleExpressionNode);
        }

        // Define a perspective for the image so a perceived z-distance will be shown when the image rotates
        private void UpdatePerspective()
        {
            var visual = ElementCompositionPreview.GetElementVisual(root);

            var size = new Vector2((float)root.ActualWidth, (float)root.ActualHeight);

            Matrix4x4 perspective = new Matrix4x4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, -1.0f / (size.X / 2),
                0.0f, 0.0f, 0.0f, 1.0f);

            // Matrix translations are to make sure the perspective is "centered"
            visual.TransformMatrix =
                Matrix4x4.CreateTranslation(-size.X / 2, -size.Y / 2, 0f) *
                perspective *
                Matrix4x4.CreateTranslation(size.X / 2, size.Y / 2, 0f);
        }

        private void Root_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            UpdatePerspective();
        }

        private ContainerVisual _container;
        private Compositor _compositor;
        private Visual _tiltVisual;
        private CompositionPropertySet _hoverPositionPropertySet;
    }
}
