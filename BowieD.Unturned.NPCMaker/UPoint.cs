namespace BowieD.Unturned.NPCMaker
{
    /// <summary>
    /// Universal point wrapper
    /// </summary>
    public struct UPoint
    {
        public UPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public static implicit operator System.Windows.Point(UPoint a)
        {
            return new System.Windows.Point(a.X, a.Y);
        }
        public static implicit operator UPoint(System.Windows.Point a)
        {
            return new UPoint(a.X, a.Y);
        }
    }
}
