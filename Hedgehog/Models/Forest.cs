namespace Hedgehog.Models
{
    public class Forest
    {
        private readonly Hedgehog initial;
        private readonly int targetColor;

        public Forest(Hedgehog startState, int target)
        {
            initial = startState;
            targetColor = target;
        }
        public bool IsSolvable(int c1Index,int c2Index) 
        {
            int diff = Math.Abs(initial.GetCount(c1Index) - initial.GetCount(c2Index));
            if (initial == null) return false;
            else if (diff % 2 == 0 && diff > 0 ) {return false; }
            else if (diff % 3 != 0) { return false; }
                return true;
        }
        private int[] ReducePairs(int[] colors, int targetColor, out int convertSteps)
        {
            int c1 = (targetColor + 1) % 3;
            int c2 = (targetColor + 2) % 3;

            convertSteps = Math.Min(colors[c1], colors[c2]);

            colors[targetColor] += 2 * convertSteps;
            colors[c1] -= convertSteps;
            colors[c2] -= convertSteps;

            return colors;
        }

        private int OptimizeSingleColor(int[] colors, int targetColor)
        {
            int c1 = (targetColor + 1) % 3;
            int c2 = (targetColor + 2) % 3;
            int stepsTotal = 0;

            while (true)
            {
                int main = -1, zero = -1;

                if (colors[c1] > 0 && colors[c2] == 0)
                {
                    main = c1;
                    zero = c2;
                }
                else if (colors[c2] > 0 && colors[c1] == 0)
                {
                    main = c2;
                    zero = c1;
                }
                else
                {
                    break; 
                }

                int third = Math.Min(colors[main] / 3, colors[targetColor]);
                if (third == 0) break;

                colors[main] -= third;
                colors[targetColor] -= third;
                colors[zero] += 2 * third;
                stepsTotal += third;

                int pairs = Math.Min(colors[main], colors[zero]);
                if (pairs == 0) break;

                colors[main] -= pairs;
                colors[zero] -= pairs;
                colors[targetColor] += 2 * pairs;
                stepsTotal += pairs;
            }

            return stepsTotal;
        }


        private int BfsSolve(int[] colors, int targetColor, int baseSteps)
        {
            var start = new Hedgehog(colors[0], colors[1], colors[2]);
            if (start.GetCount(targetColor) == start.Total)
                return baseSteps;

            var visited = new HashSet<Hedgehog>();
            var queue = new Queue<(Hedgehog state, int steps)>();
            visited.Add(start);
            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (current, steps) = queue.Dequeue();
                foreach (var next in current.GenerateTransitions())
                {
                    if (!visited.Contains(next))
                    {
                        if (next.GetCount(targetColor) == next.Total)
                            return baseSteps + steps + 1;

                        visited.Add(next);
                        queue.Enqueue((next, steps + 1));
                    }
                }
            }

            return -1;
        }

        public int MinMeetingsToUnify()
        {
            if (initial.Total == initial.GetCount(targetColor))
                return 0;

            int c1Index = (targetColor + 1) % 3;
            int c2Index = (targetColor + 2) % 3;

            if (!IsSolvable(c1Index, c2Index))
                return -1;

            if (initial.GetCount(c1Index) == initial.GetCount(c2Index))
                return initial.GetCount(c1Index);

            int[] colors = new int[3];
            colors[0] = initial.Red;
            colors[1] = initial.Green;
            colors[2] = initial.Blue;

            colors = ReducePairs(colors, targetColor, out int convertSteps);

            int extraSteps = OptimizeSingleColor(colors, targetColor);

            return BfsSolve(colors, targetColor, convertSteps + extraSteps);
        }

    }
}
