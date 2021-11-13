﻿namespace Test262Harness;

public sealed class Summary
{
    public List<Test262File> AllowedFailure { get; } = new();
    public List<Test262File> AllowedFalsePositive { get; } = new();
    public List<Test262File> AllowedFalseNegative { get; } = new();
    public List<Test262File> AllowedSuccess { get; } = new();

    public List<Test262File> DisallowedFailure { get; } = new();
    public List<Test262File> DisallowedFalsePositive { get; } = new();
    public List<Test262File> DisallowedFalseNegative { get; } = new();
    public List<Test262File> DisallowedSuccess { get; } = new();

    public List<string> Unrecognized { get; } = new();

    public bool HasProblems => Problems.Any();

    public IEnumerable<Test262File> Problems =>
        DisallowedFailure.Concat(DisallowedFalsePositive).Concat(DisallowedFalseNegative).Concat(DisallowedSuccess);

    public void UpdateAllowList(string targetFile, HashSet<string> currentAllowedFailing)
    {
        var toRemove = new HashSet<string>(
            DisallowedSuccess.Select(x => x.FormatFileLine())
                .Concat(DisallowedFailure.Select(x => x.FormatFileLine()))
                .Concat(Unrecognized));

        var toAdd = DisallowedFalsePositive.Select(x => x.FormatFileLine())
            .Concat(DisallowedFalseNegative.Select(x => x.FormatFileLine()))
            .ToArray();

        var finalLines = new List<string>(toAdd.Length);
        finalLines.AddRange(currentAllowedFailing.Where(line => !toRemove.Contains(line)));
        finalLines.AddRange(toAdd);
        finalLines.Sort();

        File.WriteAllLines(targetFile, finalLines);
    }

}