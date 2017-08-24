﻿using System.Threading.Tasks;
using GitLabCmd.Console.Configuration;
using GitLabCmd.Console.Parsing;

namespace GitLabCmd.Console
{
    public sealed class LaunchOptionsVisitor
    {
        private readonly GitLabIssueHandler _issuesHandler;
        private readonly GitLabMergeRequestsHandler _mergesHandler;
        private readonly ConfigurationHandler _configurationHandler;
        private readonly ParametersHandler _parametersHandler;

        public LaunchOptionsVisitor(
            GitLabIssueHandler issuesHandler,
            GitLabMergeRequestsHandler mergesHandler,
            ConfigurationHandler configurationHandler,
            ParametersHandler parametersHandler)
        {
            _issuesHandler = issuesHandler;
            _mergesHandler = mergesHandler;
            _configurationHandler = configurationHandler;
            _parametersHandler = parametersHandler;
        }

        public async Task Visit(CreateIssueOptions options)
        {
            if (!ValidateConfiguration())
                return;

            var parameters = _parametersHandler.NegotiateAddIssueParameters(options);
            if (parameters.IsSuccess)
                await _issuesHandler.CreateIssue(parameters.Value);
        }

        public async Task Visit(CreateMergeRequestOptions options)
        {
            if (!ValidateConfiguration())
                return;

            var parameters = _parametersHandler.NegotiateCreateMergeRequestParameters(options);
            if (parameters.IsSuccess)
                await _mergesHandler.CreateMergeRequest(parameters.Value);
        }

        public async Task Visit(ListIssuesOptions options)
        {
            if (!ValidateConfiguration())
                return;

            var parameters = _parametersHandler.NegotiateListIssuesParameters(options);
            if (parameters.IsSuccess)
                await _issuesHandler.ListIssues(parameters.Value);
        }

        public async Task Visit(ListMergesOptions options)
        {
            if (!ValidateConfiguration())
                return;

            var parameters = _parametersHandler.NegotiateListMergesParameters(options);
            if (parameters.IsSuccess)
                await _mergesHandler.ListMerges(parameters.Value);
        }

        public Task Visit(ConfigurationOptions options)
        {
            var parameters = _parametersHandler.NegotiateConfigurationParameters(options);
            _configurationHandler.StoreConfiguration(parameters);
            return Task.CompletedTask;
        }

        private bool ValidateConfiguration() => _configurationHandler.Validate();
    }
}
