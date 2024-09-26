//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls.Primitives;
//using System.Windows.Interactivity;
//using Microsoft.Phone.Controls;
//using NoNameGame.CustomControls.Levels;
//
//namespace NoNameGame.Helpers.LongList
//{
//
//    public class LLSIncrementalLoadingBehavior : Behavior<LongListSelector>
//    {
//        private ScrollBar llsScrollBar;
//
//        #region Dependency Properties
//
////        public static readonly DependencyProperty RequestMoreDataProperty = DependencyProperty.Register(
////          "RequestMoreData", typeof(ICommand), typeof(LLSIncrementalLoadingBehavior), new PropertyMetadata(null, OnRequestMoreDataChanged));
////
////        /// <summary>
////        /// The action to invoke to initiate loading of more data
////        /// </summary>
////        public ICommand RequestMoreData
////        {
////            get { return (ICommand)this.GetValue(RequestMoreDataProperty); }
////            set { this.SetValue(RequestMoreDataProperty, value); }
////        }
////
////        private static void OnRequestMoreDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
////        {
////            ((LLSIncrementalLoadingBehavior)d).RequestMoreData = (ICommand)e.NewValue;
////        }
//        public static readonly DependencyProperty RequestMoreDataProperty = DependencyProperty.Register(
//            "RequestMoreData", typeof (ICommand), typeof (LLSIncrementalLoadingBehavior), new PropertyMetadata(null));
//        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
//        {
//            ((LLSIncrementalLoadingBehavior)dependencyObject).RequestMoreData = (ICommand)e.NewValue;
//        }
//
//        public ICommand RequestMoreData
//        {
//            get { return (ICommand) GetValue(RequestMoreDataProperty); }
//            set { SetValue(RequestMoreDataProperty, value); }
//        }
//
//        public static readonly DependencyProperty ThresholdProperty = DependencyProperty.Register(
//          "Threshold", typeof(double), typeof(LLSIncrementalLoadingBehavior), new PropertyMetadata(0.8, OnThresholdChanged));
//
//        /// <summary>
//        /// A value between 0 and 1 that controls how early more data is requested. Use 1 to only trigger it at the very end
//        /// </summary>
//        public double Threshold
//        {
//            get { return (double)this.GetValue(ThresholdProperty); }
//            set { this.SetValue(ThresholdProperty, value); }
//        }
//
//        private static void OnThresholdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//        {
//            ((LLSIncrementalLoadingBehavior)d).Threshold = (double)e.NewValue;
//        }
//
//        #endregion
//
//        protected override void OnAttached()
//        {
//            base.OnAttached();
//            AssociatedObject.Loaded += OnLoaded;
//        }
//
//        private void OnLoaded(object sender, RoutedEventArgs e)
//        {
//            llsScrollBar = VisualTreeHelperExtensions.FindFirstElementInVisualTree<ScrollBar>(AssociatedObject);
//
//            llsScrollBar.ValueChanged += OnLlsScrollBarValueChanged;
//        }
//
//        private void OnLlsScrollBarValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
//        {
//            var bottomEdge = (float)(e.NewValue + AssociatedObject.ActualHeight);
//            var bottom = llsScrollBar.Maximum + AssociatedObject.ActualHeight;
//            var threshold = bottom * Threshold;
//
//            if (bottomEdge >= threshold)
//                RequestMoreData.Execute();
//        }
//
//        protected override void OnDetaching()
//        {
//            base.OnDetaching();
//
//            if (llsScrollBar != null)
//            {
//                llsScrollBar.ValueChanged -= OnLlsScrollBarValueChanged;
//            }
//        }
//    }
//}
