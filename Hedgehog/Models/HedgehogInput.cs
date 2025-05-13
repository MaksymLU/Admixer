
namespace Hedgehog.Models
{
    public class HedgehogInput
    {
        private const long MaxTotal = (long)Int32.MaxValue;

        public Hedgehog? Population { get; private set; }
        public int TargetColor { get; private set; }

        public void ReadInput()
        {
            Console.WriteLine("Введiть кiлькiсть їжачкiв трьох кольорiв (через пробiл):");

            string populationInput = Console.ReadLine() ?? "";
            int[] counts = ParseColorArray(populationInput);

            if (counts.Length != 3)
                throw new ArgumentException("Потрiбно ввести рiвно три числа.");

            long total = (long)counts[0] + counts[1] + counts[2];
            if (total > MaxTotal)
                throw new OverflowException(
                    $"Загальна кiлькiсть їжачкiв ({total}) перевищує максимально допустиме значення {MaxTotal}.");

            Population = new Hedgehog(counts[0], counts[1], counts[2]);

            Console.WriteLine("Введiть бажаний колiр (0 - червоний, 1 - зелений, 2 - синiй):");
            string targetInput = Console.ReadLine() ?? "";
            if (!int.TryParse(targetInput, out int target) || target < 0 || target > 2)
                throw new ArgumentException("Невiрний колiр. Введiть число вiд 0 до 2.");

            TargetColor = target;
        }

        private int[] ParseColorArray(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Ввiд не може бути порожнiм.");

            var parts = input
                .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var result = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++)
            {
                string s = parts[i];

                if (!int.TryParse(s, out int num))
                    throw new FormatException(
                        $"“{s}” не є дiйсним цiлим числом або виходить за межi Int32.");

                if (num < 0)
                    throw new FormatException(
                        $"“{s}” не може бути вiд’ємним.");

                result[i] = num;
            }

            return result;
        }
    }
}
