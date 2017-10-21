﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace GitLabCLI.Console.Parsing
{
    [Verb("issue", HelpText = "Commands: create, list. Run issue [command] --help to learn more.")]
    [SubVerbs(typeof(CreateIssueOptions), typeof(ListIssuesOptions))]
    public abstract class IssueOptions : ProjectOptions
    {
    }

    [Verb("create", HelpText = "Creates new issue.")]
    public sealed class CreateIssueOptions : IssueOptions, IVisitableOption
    {
        [Option('t', "title", HelpText = "Title of issue.", Required = true)]
        public string Title { get; set; }

        [Option('d', "description", HelpText = "Description of issue.")]
        public string Description { get; set; }

        [Option('l', "labels", Separator = ',', HelpText = "Labels of issue. Separated by ','.")]
        public IEnumerable<string> Labels { get; set; }

        [Option('a', "assignee", HelpText = "Assignee of issue.")]
        public string Assignee { get; set; }

        [Option("assign-myself", HelpText = "Assigns issue to current user.")]
        public bool AssignMyself { get; set; }

        public Task Accept(LaunchOptionsVisitor visitor) => visitor.Visit(this);
    }

    [Verb("list", HelpText = "Lists issues.")]
    public sealed class ListIssuesOptions : IssueOptions, IVisitableOption
    {
        [Option('l', "labels", Separator = ',', HelpText = "Labels of issue. Separated by ','.")]
        public IEnumerable<string> Labels { get; set; }

        [Option("assigned-to-me", HelpText = "Assigns issue to current user.")]
        public bool AssignedToMe { get; set; }

        [Option('f', "format", HelpText = "Output format. Can be rows|grid. Default is rows.")]
        public string Format { get; set; }

        [Option('a', "assignee", HelpText = "Assignee of issue.")]
        public string Assignee { get; set; }

        [Option('s', "state", HelpText = "Issue state. Can be open|closed|all. Default is open.")]
        public string State { get; set; }

        public Task Accept(LaunchOptionsVisitor visitor) => visitor.Visit(this);
    }
}
