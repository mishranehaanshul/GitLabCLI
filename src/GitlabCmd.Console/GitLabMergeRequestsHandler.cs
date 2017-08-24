﻿using System.Linq;
using System.Threading.Tasks;
using GitlabCmd.Console.Output;
using GitlabCmd.Core.Gitlab;
using GitlabCmd.Core.Gitlab.Merges;

namespace GitlabCmd.Console
{
    public class GitLabMergeRequestsHandler
    {
        private readonly OutputPresenter _presenter;
        private readonly IGitLabFacade _gitLabFacade;

        public GitLabMergeRequestsHandler(IGitLabFacade gitLabFacade, OutputPresenter presenter)
        {
            _gitLabFacade = gitLabFacade;
            _presenter = presenter;
        }

        public async Task CreateMergeRequest(CreateMergeRequestParameters parameters)
        {
            var mergeRequestResult = await _gitLabFacade.CreateMergeRequestAsync(parameters);
            if (mergeRequestResult.IsFailure)
            {
                _presenter.FailureResult("Failed to create merge request", mergeRequestResult.Error);
                return;
            }

            _presenter.SuccessResult($"Successfully created merge request #{mergeRequestResult.Value}");
        }

        public async Task ListMerges(ListMergesParameters parameters)
        {
            var mergesResult = await _gitLabFacade.ListMergeRequests(parameters);
            if (mergesResult.IsFailure)
            {
                _presenter.FailureResult("Failed to retrieve merge requests", mergesResult.Error);
                return;
            }

            var merges = mergesResult.Value;
            if (merges.Count == 0)
            {
                _presenter.SuccessResult($"No merge requests found in project {parameters.Project}");
                return;
            }

            _presenter.GridResult(
                $"Found ({merges.Count}) merge requests in project {parameters.Project}",
                new[] { "Issue Id", "Title", "Assignee" },
                merges.Select(s => new object[] { s.Id, s.Title, s.Assignee }));
        }
    }
}
