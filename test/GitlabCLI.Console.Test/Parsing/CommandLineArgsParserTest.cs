﻿using System.Linq;
using CommandLine;
using FluentAssertions;
using GitLabCLI.Console.Parsing;
using Xunit;

namespace GitLabCLI.Console.Test.Parsing
{
    public sealed class CommandLineArgsParserTest
    {
        private readonly CommandLineArgsParser _sut = new CommandLineArgsParser(Parser.Default);

        [Theory]
        [InlineData(
            "issue", "create",
            "-t", "test title",
            "-d", "test description",
            "-a", "testUser",
            "-l", "testlabel1,testlabel2",
            "-p", "testProject", 
            "--assign-myself")]
        [InlineData(
            "issue", "create",
            "--title", "test title",
            "--description", "test description",
            "--assignee", "testUser",
            "--labels", "testlabel1,testlabel2",
            "--project", "testProject",
            "--assign-myself")]
        public void CommandIssueCreateParsedAsCreateIssueOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<CreateIssueOptions>(
                s => s.Title == "test title" && 
                s.Description == "test description" && 
                s.Assignee == "testUser" &&
                s.AssignMyself &&
                s.Labels.SequenceEqual(new[] { "testlabel1", "testlabel2"}) &&
                s.Project == "testProject");
        }

        [Theory]
        [InlineData(
            "issue", "close",
            "-i", "1",
            "-p", "testProject")]
        [InlineData(
            "issue", "close",
            "--id", "1",
            "--project", "testProject")]
        public void CommandIssueCloseParsedAsCloseIssueOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<CloseIssueOptions>(
                s => s.Id == 1 &&
                     s.Project == "testProject");
        }

        [Theory]
        [InlineData(
            "issue", "browse",
            "-i", "1",
            "-p", "testProject")]
        [InlineData(
            "issue", "browse",
            "--id", "1",
            "--project", "testProject")]
        public void CommandIssueBrowseParsedAsBrowseOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<BrowseOptions>(
                s => s.Id == 1 &&
                     s.Project == "testProject");
        }

        [Theory]
        [InlineData(
            "merge", "create",
            "-a", "testUser",
            "-t", "testtitle",
            "-s", "testSourceBranch",
            "-d", "testDestinationBranch",
            "-p", "testProject")]
        [InlineData(
            "merge", "create",
            "--assignee", "testUser",
            "--title", "testtitle",
            "--source", "testSourceBranch",
            "--destination", "testDestinationBranch",
            "--project", "testProject")]
        public void CommandMergeCreateParsedAsCreateMergeRequestOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<CreateMergeRequestOptions>(
                s => s
                .Title == "testtitle" && 
                s.Destination == "testDestinationBranch" &&
                s.Source == "testSourceBranch" && 
                s.Assignee == "testUser" &&
                s.Project == "testProject");
        }

        [Theory]
        [InlineData(
            "issue", "list",
            "--assigned-to-me",
            "-o", "grid",
            "-f", "filter",
            "-s", "open",
            "-l", "testlabel1",
            "-i", "1, 2, 3")]
        [InlineData(
            "issue", "list",
            "--assigned-to-me",
            "--output", "grid",
            "--filter", "filter",
            "--state", "open",
            "--labels", "testlabel1",
            "--id", "1, 2, 3")]
        public void CommandIssueListParsedAsListIssueOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<ListIssuesOptions>(
                s => s.AssignedToMe &&
                     s.Output == "grid" &&
                     s.Filter == "filter" &&
                     s.State == "open" &&
                     s.Ids.SequenceEqual(new[] { 1, 2, 3 }) &&
                     s.Labels.SequenceEqual(new[] { "testlabel1" }));
        }

        [Theory]
        [InlineData(
            "config",
            "-t", "testtoken",
            "-h", "testhost",
            "-u", "testusername",
            "-p", "testpassword",
            "-d", "testproject",
            "-i", "testdefaultissuesproject",
            "-m", "testdefaultmergesproject",
            "-l", "testdefaultissuelabel",
            "-s")]
        [InlineData(
            "config",
            "--token", "testtoken",
            "--host", "testhost",
            "--username", "testusername",
            "--password", "testpassword",
            "--default-project", "testproject",
            "--default-issues-project", "testdefaultissuesproject",
            "--default-merges-project", "testdefaultmergesproject",
            "--default-issues-label", "testdefaultissuelabel",
            "--show")]
        public void CommandConfigParsedAsConfigurationOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<ConfigurationOptions>(s =>
                s.Token == "testtoken" &&
                s.Host == "testhost" &&
                s.Username == "testusername" &&
                s.Password == "testpassword" &&
                s.DefaultProject == "testproject" &&
                s.DefaultIssuesProject == "testdefaultissuesproject" &&
                s.DefaultMergesProject == "testdefaultmergesproject" &&
                s.DefaulIssuesLabel == "testdefaultissuelabel" &&
                s.Show);
        }

        [Theory]
        [InlineData(
            "merge", "list",
            "opened",
            "--assigned-to-me",
            "-a", "testuser",
            "-p", "testproject")]
        [InlineData(
            "merge", "list",
            "opened",
            "--assigned-to-me",
            "--assignee", "testuser",
            "--project", "testproject")]
        public void MergeListCommandParsedAsListMergesOptions(params string[] args)
        {
            _sut.Parse(args).Should().Match<ListMergesOptions>(
                s => s.State == "opened" &&
                     s.AssignedToMe &&
                     s.Assignee == "testuser" &&
                     s.Project == "testproject");
        }
    }
}
