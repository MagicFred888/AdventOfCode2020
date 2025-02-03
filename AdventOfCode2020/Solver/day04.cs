namespace AdventOfCode2020.Solver;

internal partial class Day04 : BaseSolver
{
    public override string PuzzleTitle { get; } = "Passport Processing";

    public readonly List<Dictionary<string, string>> _allPassports = [];

    public override string GetSolution1(bool isChallenge)
    {
        ExtractData();

        // Check and count
        List<string> mandatoryFields = ["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
        return _allPassports.Count(passport => mandatoryFields.All(f => passport.ContainsKey(f))).ToString();
    }

    public override string GetSolution2(bool isChallenge)
    {
        ExtractData();

        // Check and count
        return _allPassports.Count(passport => IsValid(passport)).ToString();
    }

    private static bool IsValid(Dictionary<string, string> passport)
    {
        int nbrOfValidFields = 0;
        string[] validEyesColor = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];
        foreach (var (field, value) in passport)
        {
            switch (field)
            {
                // All valid cases
                case "byr" when int.TryParse(value, out int byr) && byr is >= 1920 and <= 2002:
                case "iyr" when int.TryParse(value, out int iyr) && iyr is >= 2010 and <= 2020:
                case "eyr" when int.TryParse(value, out int eyr) && eyr is >= 2020 and <= 2030:
                case "hgt" when value.EndsWith("cm") && int.TryParse(value[..^2], out int heightCm) && heightCm is >= 150 and <= 193:
                case "hgt" when value.EndsWith("in") && int.TryParse(value[..^2], out int heightIn) && heightIn is >= 59 and <= 76:
                case "hcl" when value.Length == 7 && value[0] == '#' && value[1..].All(c => char.IsDigit(c) || (c >= 'a' && c <= 'f')):
                case "ecl" when validEyesColor.Contains(value):
                case "pid" when value.Length == 9 && value.All(char.IsDigit):
                    nbrOfValidFields++;
                    continue;

                // Ignored cases
                case "cid":
                    break;

                // Invalid cases
                default:
                    return false;
            }
        }
        return nbrOfValidFields == 7;
    }

    private void ExtractData()
    {
        _allPassports.Clear();
        Dictionary<string, string> passport = [];
        foreach (string line in _puzzleInput)
        {
            // Save passport and reset if empty line
            if (line == "" && passport.Count > 0)
            {
                _allPassports.Add(passport);
                passport = [];
                continue;
            }
            foreach (string field in line.Split(' '))
            {
                string[] keyValue = field.Split(':');
                passport[keyValue[0]] = keyValue[1];
            }
        }

        // Save last passport
        if (passport.Count > 0)
        {
            _allPassports.Add(passport);
        }
    }
}