namespace Scoreboard
{
    public class Step
    {
        public Step(Side side, int stepSize, int stepSizeOtherSide = 0)
        {
            Side = side;
            StepSize = stepSize;

            if (side == Side.both)
            {
                StepSizeOtherSide = stepSizeOtherSide;
            }
        }

        public Side Side { get; set; }

        public int StepSize { get; set; }

        public int StepSizeOtherSide { get; set; }
    }
}