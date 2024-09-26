using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using NoNameGame.Models;

namespace NoNameGame.CustomControls
{
    public class SelectAnimationDirectionModel
    {        
        public ObservableCollection<AnimationDirectionModel> AnimationDirections { get; set; }

        public AnimationDirectionModel SelectedAnimationDirection { get; set; }

        public SelectAnimationDirectionModel(ObservableCollection<AnimationDirectionModel> animationDirectionModels,
                                             AnimationDirectionModel selectedAnimationDirection)
        {
            AnimationDirections = animationDirectionModels;
            SelectedAnimationDirection = selectedAnimationDirection;          
        }
    }

    public partial class SelectAnimationDirectionControl : UserControl
    {
        private SelectAnimationDirectionModel _animationDirectionModel;

        public SelectAnimationDirectionModel AnimationDirectionModel
        {
            get { return _animationDirectionModel; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                DataContext = _animationDirectionModel = value;
            }
        }
        public event AnimationDirectionChangedEventHandler AnimationDirectionChanged;

        public SelectAnimationDirectionControl()
        {
            InitializeComponent();
            _animationDirectionModel = new SelectAnimationDirectionModel(new ObservableCollection<AnimationDirectionModel>(), null);         
        }
//        private void ListBoxThemes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (AnimationDirectionChanged!=null)
//                AnimationDirectionChanged.Invoke(this, AnimationDirectionModel.SelectedAnimationDirection);
//        }
        private bool sentinel = true;
        private void ListBoxThemes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!sentinel)
                return;
            if (AnimationDirectionChanged != null)
                AnimationDirectionChanged.Invoke(this, AnimationDirectionModel.SelectedAnimationDirection);
                      
        }
        public void ReselectSelectedItem()
        {
            sentinel = false;
                var temp = ListBoxThemes.SelectedItem;
                ListBoxThemes.SelectedItem = null;
                ListBoxThemes.SelectedItem = temp;
            sentinel = true;
        }
    }

    public delegate void AnimationDirectionChangedEventHandler(object sender, AnimationDirectionModel selectedAnimationDirection);
}
