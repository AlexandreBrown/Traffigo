using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using TraffiGo.VueModeles;
using System.Collections.ObjectModel;
using TraffiGo.Modeles;

namespace TraffiGo.Vues
{
    /// <summary>
    /// Interaction logic for EditionSimulationVueUserControl.xaml
    /// </summary>
    public partial class EditionSimulationVueUserControl : UserControl
    {
        // Objects
        private Image ImgBeingDragged { get; set; }

        public EditionSimulationVueUserControl()
        {
            InitializeComponent();
            ImgBeingDragged = null;
        }

        #region MouseCapture

        private void SetMouseCapture(UIElement elem)
        {
            Mouse.Capture(elem);
        }

        #endregion

        #region Animations
        private void LaunchDeleteImgBeingDraggedAnimation()
            {
                CnvContainer.IsHitTestVisible = false;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 1.0;
                animation.To = 0.0;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(animation);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Image.OpacityProperty));
                storyboard.Completed += Storyboard_Completed;
                storyboard.Begin(ImgBeingDragged);
            }

            private void Storyboard_Completed(object sender, EventArgs e)
            {
                CnvContainer.Children.Remove(ImgBeingDragged);
                ImgBeingDragged = null;
                Mouse.Capture(null);
                CnvContainer.IsHitTestVisible = true;
            }
        #endregion

        #region ValidationsUI

        private bool PositionEstDansHitbox(Point position, Point hitboxHorizontal, Point hitboxVertical)
        {
            // Retourne si oui ou non la position se trouve dans le hitbox
            return (position.X > hitboxHorizontal.X-1 && position.X < hitboxHorizontal.Y+1) && (position.Y > hitboxVertical.X-1 && position.Y < hitboxVertical.Y+1);
        }

        private Point GetHorizontalHitbox(int x, int width)
        {
            // On trouve le point le plus à gauche du rectangle
            double left = x;
            // On trouve le point le plus à droite du rectangle
            double right = left + width;

            return new Point(/*Valeur minimale :*/ left-1,  /*Valeur maximale :*/right+1);
        }

        private Point GetVerticalHitbox(int y, int height)
        {
            // On trouve le point le plus bas du rectangle
            double top = y;
            // On trouve le point le plus haut du rectangle
            double bottom = top + height;

            return new Point(/*Valeur minimale :*/top-1,/*Valeur maximale :*/bottom+1);
        }

        private Point GetPosInGrilleSimFromMouse(Point cursorPos)
        {
            foreach (ObservableCollection<Case> cases in GrilleSimulation.Items)
            {
                foreach (Case uneCase in cases)
                {
                    if (PositionEstDansHitbox(cursorPos, GetHorizontalHitbox(uneCase.MarginLeft, uneCase.Width), GetVerticalHitbox(uneCase.MarginTop, uneCase.Height)))
                    {
                        return uneCase.Position;
                    }
                }
            }
            throw new Exception("Le curseur n'est pas dans la grille de simulation");
        }

        #endregion

        #region UIEvents

        #region DraggableImage


        private void DraggableItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SetCurrentImageBeingDragged((Image)sender);
            }
        }


        private void SetCurrentImageBeingDragged(Image img)
        {
            ImgBeingDragged = new Image();
            ImgBeingDragged.Source = img.Source;
            ImgBeingDragged.Height = img.Height;
            ImgBeingDragged.Width = img.Width;
            ImgBeingDragged.Margin = img.Margin;
            ImgBeingDragged.MouseMove += ImgBeingDragged_MouseMove;
            ImgBeingDragged.MouseUp += ImgBeingDragged_MouseUp;

            CnvContainer.Children.Add(ImgBeingDragged);

            Mouse.Capture(ImgBeingDragged);
        }

        #endregion

        #region ImageBeingDragged

        private void ImgBeingDragged_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateImagePositionFromCursor(ImgBeingDragged);
        }

        public void UpdateImagePositionFromCursor(Image img)
        {
            if (img != null)
            {
                Point pos = Mouse.GetPosition(CnvContainer);
                img.Margin = new Thickness(pos.X - (img.Width / 2), pos.Y - (img.Height / 2), 0, 0);
            }
        }

        private void ImgBeingDragged_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HandleDrop();
        }

        private void SupprimerImgBeingDragged()
        {
            if (Mouse.Captured != null)
            {
                if (ImgBeingDragged != null)
                {
                    LaunchDeleteImgBeingDraggedAnimation();
                }
            }
        }

        private void HandleDrop()
        {

            Point cursorPosRelToGrille = Mouse.GetPosition(GrilleSimulation);
            if (PositionEstDansHitbox(cursorPosRelToGrille, new Point(0, EditionSimulationVueModele.GrilleSimulationWidth), new Point(0, EditionSimulationVueModele.GrilleSimulationHeight)))
            {
                Point positionInGrille = GetPosInGrilleSimFromMouse(cursorPosRelToGrille);
                AjouterImageDansCase(positionInGrille);
            }
            SupprimerImgBeingDragged();
            
        }

        private void AjouterImageDansCase(Point position)
        {
            ((EditionSimulationVueModele)DataContext).AjouterCaseSelonImg(position, ImgBeingDragged.Source.ToString());
        }

        #endregion

        #endregion

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2  && Mouse.LeftButton == MouseButtonState.Pressed && Mouse.RightButton != MouseButtonState.Pressed)
            {
                ((EditionSimulationVueModele)DataContext).AfficherEditionIntersection();
            }
        }
    }
}