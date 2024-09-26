namespace NoNameGame.BoardPresentation.Animations.CircularTransaltion
{
    public struct DistanceFromCircleCenter
    {
        public int Dx { get; private set; }
        public int Dy { get; private set; }
        public DistanceFromCircleCenter(int dx, int dy) : this()
        {
            this.Dx = dx;
            this.Dy = dy;
        }
    }
}