using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CardReality.Data;
using CardReality.Data.Data;
using CardReality.Data.Models;

namespace CardReality.Services
{
    public class CardsService : Service
    {
        public const int DefenseStandardMinimumDeviation = 3;
        public const int DefenseStandardMaximumDeviation = 15;
        public const int DefenseStandardDeviationModifier = 10;
        public const int SpecialCardDropChance = 20;


        public CardsService(IApplicationData data) : base(data)
        {
        }

        public int CalculateAttackPoints(string cardName)
        {
            double sum = 0;
            string[] letters = cardName.ToCharArray().Select(c => c.ToString()).ToArray();

            foreach (string letter in letters)
            {
                var letterData = this.Data.Letters.All().FirstOrDefault(l => l.Char.ToLower() == letter);
                if (letterData == null) continue;

                sum += letterData.Weight;
            }

            return (int)sum;
        }

        public int CalculateDefensePoints(string cardName)
        {
            Random rnd = new Random();
            double sum = 0;
            string[] letters = cardName.ToCharArray().Select(c => c.ToString()).ToArray();

            foreach (string letter in letters)
            {
                var letterData = this.Data.Letters.All().FirstOrDefault(l => l.Char.ToLower() == letter);
                if (letterData == null) continue;

                sum += letterData.Weight * (
                    (double)rnd.Next(
                        CardsService.DefenseStandardMinimumDeviation,
                        CardsService.DefenseStandardMaximumDeviation + 1
                    ) / CardsService.DefenseStandardDeviationModifier);
            }

            return (int)sum;
        }

        public bool IsSpecial()
        {
            Random rnd = new Random();
            int result = rnd.Next(1, 101);

            return result <= CardsService.SpecialCardDropChance;
        }

        public SpecialEffect GetSpecialEffect()
        {
            Random rnd = new Random();
            List<SpecialEffect> effects = Enum.GetValues(typeof(SpecialEffect)).Cast<SpecialEffect>().ToList();

            return effects[rnd.Next(0, effects.Count)];
        }

    }
}