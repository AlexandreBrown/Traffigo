using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TraffiGo.Modeles.Data;

namespace TraffiGo.Modeles
{
    public class Case : Element
    {

        public int Height { get; set; }
        public int Width { get; set; }
        public int MarginLeft { get; set; }
        public int MarginTop { get; set; }
        public Brush Fill { get; set; }
        public Brush Stroke { get; set; }
        public static Brush DefaultStroke { get; set; } = Brushes.Black;
        public static int DefaultStrokeThickness { get; set; } = 1;
        public int StrokeThickness { get; set; }

        public Case(int height,int width,int x, int y) : base(x,y)
        {
            Height = height;
            Width = width;
            MarginLeft = x * width;
            MarginTop = y * height;
            Fill = null;
            Stroke = DefaultStroke;
            StrokeThickness = DefaultStrokeThickness;
        }

        public void SetFill(Element e)
        {
            if (e is Chemin || e is Intersection)
            {
                Fill = new ImageBrush(TrouverImageFillFrom(e));
            }
            else
            {
                Fill = Brushes.LightGray;
            }
        }

        private BitmapImage TrouverImageFillFrom(Element e)
        {
            BitmapImage imgSrc;
            string path = "";

            if (e is Chemin)
            {
                if (((Chemin)e).EstVirage())
                {
                    path = TrouverBonUriVirage((Chemin)e);
                }
                else
                {
                    path = TrouverBonUriDroit((Chemin)e);
                }
            }
            else if (e is Intersection)
            {
                path = TrouverBonUriIntersection((Intersection)e);
            }
            else
            {
                throw new Exception("No ImageFill could be found from object");
            }

            imgSrc = new BitmapImage(new Uri(path, UriKind.Absolute));
            return imgSrc;
        }

        public string TrouverBonUriDroit(Chemin c)
        {
            string path = PackUrl + DraggableItemsURL + "Chemin";
            switch (c.LstRoutes[0].Orientation)
            {
                case Orientation.NORD: path += "1.jpg"; break;
                case Orientation.SUD: path += "1.jpg"; break;
                case Orientation.EST: path += "0.jpg"; break;
                case Orientation.OUEST: path += "0.jpg"; break;
            }
            return path;
        }

        public string TrouverBonUriVirage(Chemin c)
        {
            string path = PackUrl + DraggableItemsURL + "Chemin";
            List<Orientation> lstPlacements = DefinirTypeVirage(c);
            switch (lstPlacements[0])
            {
                case Orientation.NORD:
                    if (lstPlacements[1] == lstPlacements[0] + 1)
                    {//NORD-EST
                        path += "5.jpg";
                    }
                    else
                    {//NORD-OUEST
                        path += "4.jpg";
                    }
                    break;
                case Orientation.SUD:
                    if (lstPlacements[1] == lstPlacements[0] + 1)
                    {//SUD-OUEST
                        path += "2.jpg";
                    }
                    else
                    {//SUD-EST
                        path += "3.jpg";
                    }
                    break;
                case Orientation.EST:
                    if (lstPlacements[1] == lstPlacements[0] + 1)
                    {//EST-SUD
                        path += "3.jpg";
                    }
                    else
                    {//EST-NORD
                        path += "5.jpg";
                    }
                    break;
                case Orientation.OUEST:
                    if (lstPlacements[1] == lstPlacements[0] + 1)
                    {//OUEST-NORD
                        path += "4.jpg";
                    }
                    else
                    {//OUEST-SUD
                        path += "2.jpg";
                    }
                    break;
            }
            return path;
        }

        public string TrouverBonUriIntersection(Intersection i)
        {
            string path = PackUrl + DraggableItemsURL + "intersection";

            if (i.LstChemins.Count == 4)
            {
                path += "1.jpg";
            }
            else
            {
                List<Orientation?> lst = definirTypeIntersection(i);

                if (lst.Contains(Orientation.EST) && lst.Contains(Orientation.SUD) && lst.Contains(Orientation.OUEST))
                    path += "2.jpg";

                if (lst.Contains(Orientation.NORD) && lst.Contains(Orientation.EST) && lst.Contains(Orientation.OUEST))
                    path += "3.jpg";

                if (lst.Contains(Orientation.NORD) && lst.Contains(Orientation.EST) && lst.Contains(Orientation.SUD))
                    path += "4.jpg";

                if (lst.Contains(Orientation.NORD) && lst.Contains(Orientation.SUD) && lst.Contains(Orientation.OUEST))
                    path += "5.jpg";

            }

            return path;
        }

        private List<Orientation> DefinirTypeVirage(Chemin c)
        {
            List<Orientation> lstPlacement = new List<Orientation>();

            foreach (Route r in c.LstRoutes)
            {
                if (!r.LstVoies[0].ContientDirection(Direction.TOUTDROIT))
                {
                    Direction d = r.LstVoies[0].LstDirections[0];
                    Orientation o = r.Orientation;

                    switch (o)
                    {
                        case Orientation.NORD:
                            o = Orientation.SUD;
                            lstPlacement.Add(o);
                            break;
                        case Orientation.SUD:
                            o = Orientation.NORD;
                            lstPlacement.Add(o);
                            break;
                        case Orientation.EST:
                            o = Orientation.OUEST;
                            lstPlacement.Add(o);
                            break;
                        case Orientation.OUEST:
                            o = Orientation.EST;
                            lstPlacement.Add(o);
                            break;
                    }
                    if (d == Direction.DROITE)
                    {
                        if (o == Orientation.NORD)
                            lstPlacement.Add(Orientation.OUEST);
                        else
                            lstPlacement.Add(o - 1);
                    }
                    else
                    {
                        if (o == Orientation.OUEST)
                            lstPlacement.Add(Orientation.NORD);
                        else
                            lstPlacement.Add(o + 1);
                    }

                    break;
                }
            }

            return lstPlacement;
        }

        private List<Orientation?> definirTypeIntersection(Intersection i)
        {
            List<Orientation?> lst = new List<Orientation?>();

            foreach (Chemin c in i.LstChemins)
                lst.Add(c.Emplacement);

            return lst;
        }
    }
}
