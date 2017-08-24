﻿using System.Collections.Generic;
using System.Linq;
using CommandLine;
using FluentAssertions;
using GitlabCmd.Console.Cmd;
using Xunit;

namespace GitlabCmd.Console.Test.Cmd
{
    public class LaunchHandlerParsingTest
    {
        private readonly ParserSpy _sut = new ParserSpy(Parser.Default);

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
        public void CreateIssueWithShortArgsParsedAsCreateIssueOptions(params string[] args)
        {
            //act
            _sut.Parse(args);

            //assert
            _sut.Options.Should().Match<CreateIssueOptions>(
                s => s.Title == "test title" && 
                s.Description == "test description" && 
                s.Assignee == "testUser" &&
                s.AssignMyself &&
                s.Labels.SequenceEqual(new[] { "testlabel1", "testlabel2"}) &&
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
        public void AddMergeCallsCreateIsuesWithDescriptionHavingSpaces(params string[] args)
        {
            //act
            _sut.Parse(args);

            //assert
            _sut.Options.Should().Match<CreateMergeRequestOptions>(
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
            "-l", "testlabel1")]
        [InlineData(
            "issue", "ls",
            "--assigned-to-me",
            "--labels", "testlabel1")]
        public void ListIssuesParsedAsListIssueOptions(params string[] args)
        {
            //act
            _sut.Parse(args);

            //assert
            _sut.Options.Should().Match<ListIssuesOptions>(
                s => s.AssignedToMe &&
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
            "-l", "testdefaultissuelabel")]
        [InlineData(
            "config",
            "--token", "testtoken",
            "--host", "testhost",
            "--username", "testusername",
            "--password", "testpassword",
            "--default-project", "testproject",
            "--default-issues-project", "testdefaultissuesproject",
            "--default-merges-project", "testdefaultmergesproject",
            "--default-issues-label", "testdefaultissuelabel")]
        public void ConfigCommandParsedConfigurationOptions(params string[] args)
        {
            //act
            _sut.Parse(args);

            //assert
            _sut.Options.Should().Match<ConfigurationOptions>(s =>
                s.Token == "testtoken" &&
                s.Host == "testhost" &&
                s.Username == "testusername" &&
                s.Password == "testpassword" &&
                s.DefaultProject == "testproject" &&
                s.DefaultIssuesProject == "testdefaultissuesproject" &&
                s.DefaultMergesProject == "testdefaultmergesproject" &&
                s.DefaulIssuesLabel == "testdefaultissuelabel");
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
        public void MergeListCommandParsedConfigurationOptions(params string[] args)
        {
            //act
            _sut.Parse(args);

            //assert
            _sut.Options.Should().Match<ListMergesOptions>(
                s => s.State == "opened" &&
                     s.AssignedToMe &&
                     s.Assignee == "testuser" &&
                     s.Project == "testproject");
        }

        private class ParserSpy
        {
            private readonly Parser _parser;

            public ParserSpy(Parser parser) => _parser = parser;

            public void Parse(string[] args) => _parser.
                ParseVerbs<
                    IssueOptions,
                    MergeOptions,
                    ConfigurationOptions>(args).
                MapResult(
                    (IssueOptions o) => Options = o,
                    (MergeOptions o) => Options = o,
                    (ConfigurationOptions o) => Options = o,
                    errors => Errors = errors);

            public object Options { get; set; }

            public IEnumerable<Error> Errors { get; set; }
        }
    }
}
