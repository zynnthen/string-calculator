namespace Calculator
{
    class Program
    {
        public string[] PrioOperand(string[] formulas)
        {
            int numberOfMultipleOperand = formulas.Count(x => x == "*");
            int numberOfDivideOperand = formulas.Count(x => x == "/");

            var list = formulas.ToList();


            int indexMultiple = 0;
            for (int i=0; i< numberOfMultipleOperand; i++)
            {
                indexMultiple = Array.IndexOf(formulas, "*", indexMultiple);
                list.Insert(indexMultiple - 1, "(");
                list.Insert(indexMultiple + 3, ")");
            }

            int indexDivide = 0;
            for (int i = 0; i < numberOfDivideOperand; i++)
            {
                indexDivide = Array.IndexOf(formulas, "*", indexDivide);
                list.Insert(indexDivide - 1, "(");
                list.Insert(indexDivide + 3, ")");
            }

            list.Insert(0, "(");
            list.Insert(list.Count - 1, ")");

            return formulas = list.ToArray();
        }

        public double CalculateTotal(string[] formulas)
        {
            double total = 0.0;
            for (var i = 0; i < formulas.Length; i++)
            {
                var isNumeric = double.TryParse(formulas[i], out double n);
                if (isNumeric)
                {
                    total += Convert.ToDouble(formulas[i]);
                }
                else
                {
                    switch (formulas[i])
                    {
                        case "+":
                            total += Convert.ToDouble(formulas[++i]);
                            break;
                        case "-":
                            total -= Convert.ToDouble(formulas[++i]);
                            break;
                        case "*":
                            total *= Convert.ToDouble(formulas[++i]);
                            break;
                        case "/":
                            total /= Convert.ToDouble(formulas[++i]);
                            break;
                        default:
                            break;
                    }
                }
            }

            return total;
        }

        public double Calculate(string sum)
        {
            string[] formulas = sum.Split(' ');
            int numberOfGroup = formulas.Count(x => x == "(");

            if (numberOfGroup == 0)
            {
                int numberOfFlatOperand = formulas.Distinct().Count(x => x == "+" || x == "-" || x == "*" || x == "/");
                if (numberOfFlatOperand > 1)
                {
                    formulas = PrioOperand(formulas);
                    numberOfGroup++;
                }
            }

            if (numberOfGroup > 0)
            {
                // get bracket grouping and replace the value
                var loopCounter = numberOfGroup;
                while (loopCounter > 0)
                {
                    int index = Array.LastIndexOf(formulas, "(");
                    int indexNext = Array.IndexOf(formulas, ")", index);
                    string[] innerBracket = formulas[(index+1)..indexNext];

                    int numberOfOperand = innerBracket.Distinct().Count(x => x == "+" || x == "-" || x == "*" || x == "/");
                    if (numberOfOperand > 1)
                    {
                        innerBracket = PrioOperand(innerBracket);

                        var updatedFormula = formulas.ToList();
                        updatedFormula.RemoveRange(index, indexNext - index + 1);
                        updatedFormula.InsertRange(index, innerBracket);
                        formulas = updatedFormula.ToArray();

                        loopCounter++;

                        continue;
                    }

                    double total = CalculateTotal(innerBracket);
                    formulas[index] = total.ToString();

                    var list = formulas.ToList();
                    list.RemoveRange(index + 1, indexNext - index);
                    formulas = list.ToArray();

                    loopCounter--;
                }
            }

            return CalculateTotal(formulas);
        }

        static void Main(string[] args)
        {
            Program program = new Program();

            string[] inputs = {
                "1 + 1",
                "2 * 2",
                "1 + 2 + 3",
                "6 / 2",
                "11 + 23",
                "11.1 + 23",
                "1 + 1 * 3",
                "( 11.5 + 15.4 ) + 10.1",
                "23 - ( 29.3 - 12.5 )",
                "( 1 / 2 ) - 1 + 1 ",
                "10 - ( 2 + 3 * ( 7 - 5 ) )",
                "( 11.5 + 15.4 ) + ( 11.5 + 15.4 )",
            };
            foreach(var input in inputs)
            {
                Console.WriteLine(
                    Math.Round(program.Calculate(input), 2)
                    );
            }
        }
    }
}