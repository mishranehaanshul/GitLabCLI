﻿using System.Threading.Tasks;
using CommandLine;

namespace GitLabCLI.Console.Parsing
{
    [Verb("merge", HelpText = "Commands: create, list. Run merge [command] --help to learn more.")]
    [SubVerbs(typeof(CreateMergeRequestOptions), typeof(ListMergesOptions))]
    public abstract class MergeOptions : ProjectOptions
    {
    }

    [Verb("create", HelpText = "Creates merge request.")]
    public sealed class CreateMergeRequestOptions : MergeOptions, IVisitableOption
    {
        [Option('t', "title", HelpText = "Title of merge request.", Required = true)]
        public string Title { get; set; }

        [Option('s', "source", HelpText = "Source branch.", Required = true)]
        public string Source { get; set; }

        [Option('d', "destination", HelpText = "Destination branch.", Required = true)]
        public string Destination { get; set; }

        [Option('a', "assignee", HelpText = "Assignee of issue.")]
        public string Assignee { get; set; }

        [Option("assign-myself", HelpText = "Assigns issue to current user.")]
        public bool AssignMyself { get; set; }

        public Task Accept(LaunchOptionsVisitor visitor) => visitor.Visit(this);
    }

    [Verb("list", HelpText = "Lists merge requests.")]
    public sealed class ListMergesOptions : MergeOptions, IVisitableOption
    {
        [Value(
            0,
            MetaName = "State",
            Default = "opened",
            HelpText = "Merge request state. Can be opened|merged|closed. Default is opened.")]
        public string State { get; set; }

        [Option('a', "assignee", HelpText = "Assignee of issue.")]
        public string Assignee { get; set; }

        [Option("assigned-to-me", HelpText = "Assigns issue to current user.")]
        public bool AssignedToMe { get; set; }

        public Task Accept(LaunchOptionsVisitor visitor) => visitor.Visit(this);
    }
}
