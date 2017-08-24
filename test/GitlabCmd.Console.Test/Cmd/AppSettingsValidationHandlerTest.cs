﻿using FluentAssertions;
using GitlabCmd.Console.App;
using GitlabCmd.Console.Configuration;
using Xunit;

namespace GitlabCmd.Console.Test.Cmd
{
    public class AppSettingsValidationHandlerTest
    {
        [Fact]
        public void SettingsWithoutHostUrlAreNotValid()
        {
            var appSettings = new AppSettings
            {
                DefaultGitLabProject = "test-project",
                GitLabAccessToken = "access-token",
                DefaultIssueLabel = "parsed-issue-label"
            };

            var sut = new AppSettingsValidationHandler(appSettings, new OutputPresenter());
            sut.Validate().Should().BeFalse();
        }
    }
}
