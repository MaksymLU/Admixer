
namespace Hedgehog.Models
{
    public class Hedgehog
    {
        public int Red { get; }
        public int Green { get; }
        public int Blue { get; }

        public Hedgehog(int red, int green, int blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
        public int GetCount(int color) =>
            color switch
            {
                0 => Red,
                1 => Green,
                2 => Blue,
                _ => throw new ArgumentException("Invalid color index")
            };

        public int Total => Red + Green + Blue;

        public IEnumerable<Hedgehog> GenerateTransitions()
        {
            if (Red >= 1 && Green >= 1)
                yield return new Hedgehog(Red - 1, Green - 1, Blue + 2); 
            if (Green >= 1 && Blue >= 1)
                yield return new Hedgehog(Red + 2, Green - 1, Blue - 1);
            if (Blue >= 1 && Red >= 1)
                yield return new Hedgehog(Red - 1, Green + 2, Blue - 1);
        }
    }
}
