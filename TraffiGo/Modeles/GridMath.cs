namespace TraffiGo.Modeles
{
    public static class GridMath
    {
        public static int TrouverSizeOptimaleCaseSelonDim(int nbCol, int nbRow, int cnvHeight, int cnvWidth)
        {
            int optimalHeight = cnvHeight / nbRow;
            int optimalWidth = cnvWidth / nbCol;
            if (optimalHeight >= optimalWidth)
            {
                return optimalWidth;
            }
            return optimalHeight;
        }
    }
}
