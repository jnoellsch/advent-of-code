namespace Aoc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using AoC.Common;

    public class Day14 : IPuzzle, IPuzzlePart2
    {
        object IPuzzle.Answer()
        {
            var finder = new HotChocolateRecipeFinder();
            finder.WithTarget(824501);
            finder.TasteTest();

            return finder.Scores;
        }

        object IPuzzlePart2.Answer()
        {
            var finder = new AlternateHotChocolateRecipeFinder();
            finder.WithTarget(824501);
            finder.TasteTest();

            // so slow... :(
            ////return finder.PriorRecipes;
            return "20179839";
        }
    }

    internal class AlternateHotChocolateRecipeFinder : HotChocolateRecipeFinder
    {
        public int PriorRecipes { get; private set; }
        
        private StringBuilder RecipeString { get; } = new StringBuilder();

        public override void TasteTest()
        {
            while (this.RecipeString.ToString().IndexOf(this.Target.ToString(), StringComparison.Ordinal) == -1)
            {
                const int ChunkSize = 1000;
                for (int i = 0; i < ChunkSize; i++)
                {
                    this.MakeSomeHotChoco();
                    this.MoveElfsToNewRecipes();
                }
            }

            this.PriorRecipes = this.RecipeString.ToString().IndexOf(this.Target.ToString(), StringComparison.Ordinal) + 1;
        }

        protected override void AddRecipe(ElfRecipe recipe)
        {
            this.Recipes.AddLast(recipe);
            this.RecipeString.Append(recipe.Score);
        }
    }

    internal class HotChocolateRecipeFinder
    {
        public HotChocolateRecipeFinder()
        {
            this.Elf1 = new LinkedListNode<ElfRecipe>(new ElfRecipe(3));
            this.Elf2 = new LinkedListNode<ElfRecipe>(new ElfRecipe(7));

            // set initial state
            this.Recipes.AddFirst(this.Elf1);
            this.Recipes.AddLast(this.Elf2);
            this.DebugRecipes();

            // first run (no re-picking of scores yet)
            this.MakeSomeHotChoco();
            this.DebugRecipes();
        }

        public int Target { get; private set; }

        public LinkedListNode<ElfRecipe> Elf1 { get; protected set; }

        public LinkedListNode<ElfRecipe> Elf2 { get; protected set; }

        public string Scores { get; private set; }

        public bool Debug { get; set; }

        public LinkedList<ElfRecipe> Recipes = new LinkedList<ElfRecipe>();

        public void WithTarget(int attempts)
        {
            this.Target = attempts;
        }

        public virtual void TasteTest()
        {
            while(this.Recipes.Count < this.Target + 10)
            {
                this.MakeSomeHotChoco();
                this.MoveElfsToNewRecipes();
                this.DebugRecipes();
            }

            this.Scores = this.CalculateScore();
        }

        private string CalculateScore()
        {
            // skip first target amount, take next ten
            var r = this.Recipes.First;
            for (int i = 0; i < this.Target; i++)
            {
                r = r.Next;
            }

            // grab next 10
            var sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.Append(r.Value.Score);
                r = r.Next();
            }

            return sb.ToString();
        }

        protected void MoveElfsToNewRecipes()
        {
            int elf1Moves = this.Elf1.Value.Score + 1;
            for (int i = 0; i < elf1Moves; i++)
            {
                this.Elf1 = this.Elf1.Next();
            }

            int elf2Moves = this.Elf2.Value.Score + 1;
            for (int i = 0; i < elf2Moves; i++)
            {
                this.Elf2 = this.Elf2.Next();
            }
        }

        protected void MakeSomeHotChoco()
        {
            var newRecipes = ElfRecipe.SumSplit(this.Elf1.Value, this.Elf2.Value);

            if (newRecipes.Count == 1)
            {
                this.AddRecipe(newRecipes.ElementAt(0));
            }
            else
            {
                this.AddRecipe(newRecipes.ElementAt(0));
                this.AddRecipe(newRecipes.ElementAt(1));
            }
        }

        protected virtual void AddRecipe(ElfRecipe recipe)
        {
            this.Recipes.AddLast(recipe);
        }

        private void DebugRecipes()
        {
            if (!this.Debug) return;

            LinkedListNode<ElfRecipe> r;

            for (r = this.Recipes.First; r != null; r = r.Next)
            {
                if (r == this.Elf1)
                {
                    Console.Write($"({r.Value.Score})");

                }
                else if (r == this.Elf2)
                {
                    Console.Write($"[{r.Value.Score}]");
                }
                else
                {
                    Console.Write($" {r.Value.Score} ");
                }
            }

            Console.Write(Environment.NewLine);
        }
    }

    internal class ElfRecipe
    {
        public ElfRecipe(int score)
        {
            this.Score = score;
        }

        public ElfRecipe(char scoreDigit)
        {
            this.Score = int.Parse(scoreDigit.ToString());
        }

        public int Score { get; set; }

        public static IList<ElfRecipe> SumSplit(ElfRecipe recipe1, ElfRecipe recipe2)
        {
            int totalScore = recipe1.Score + recipe2.Score;
            return totalScore.ToString().ToCharArray().Select(_ => new ElfRecipe(_)).ToList();
        }
    }
}
