﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitlabCmd.Console.App;
using GitlabCmd.Console.Utilities;
using NGitLab.Models;

namespace GitlabCmd.Console.GitLab
{
    public class GitLabIssueHandler
    {
        private readonly GitLabFacade _gitLabFacade;
        private readonly OutputPresenter _presenter;

        public GitLabIssueHandler(
            GitLabFacade gitLabFacade,
            OutputPresenter presenter)
        {
            _gitLabFacade = gitLabFacade;
            _presenter = presenter;
        }

        public async Task AddIssue(CreateIssueParameters parameters)
        {
            var issueResult = await InnerAddIssue(parameters);
            if (issueResult.IsFailure)
            {
                _presenter.FailureResult("Failed to create issue", issueResult.Error);
                return;
            }

            _presenter.SuccessResult($"Successfully created issue #{issueResult.Value}");
        }

        public async Task ListIssues(ListIssuesParameters parameters)
        {
            var issueResult = await InnerListIssues(parameters);
            if (issueResult.IsFailure)
            {
                _presenter.FailureResult("Failed to retrieve issues", issueResult.Error);
                return;
            }

            var issues = issueResult.Value;

            _presenter.GridResult(
                $"Issues ({issues.Count})", 
                new[] { "Issue Id", "Title", "Description"},
                issues.Select(s => new object[] { s.IssueId, s.Title, s.Description }));
        }

        private async Task<Result<IReadOnlyList<Issue>>> InnerListIssues(ListIssuesParameters parameters)
        {
            if (parameters.AssignedToCurrentUser)
            {
                return await _gitLabFacade.ListIssuesForCurrentUser(
                    parameters.ProjectName, 
                    parameters.Labels);
            }

            return await _gitLabFacade.ListIssues(
                parameters.ProjectName,
                parameters.Assignee,
                parameters.Labels);
        }

        private async Task<Result<int>> InnerAddIssue(CreateIssueParameters parameters)
        {
            if (parameters.AssignToCurrentUser)
            {
                return await _gitLabFacade.CreateIssueForCurrentUser(
                    parameters.Title,
                    parameters.Description,
                    parameters.ProjectName,
                    parameters.Labels);
            }

            return await _gitLabFacade.CreateIssue(
                parameters.Title,
                parameters.Description,
                parameters.ProjectName,
                parameters.AssigneeName,
                parameters.Labels);
        }
    }
}
