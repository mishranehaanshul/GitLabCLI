﻿using System.Threading.Tasks;
using GitlabCmd.Utilities;

namespace GitlabCmd.Console.GitLab
{
    public sealed class GitLabClientExFactory
    {
        private readonly GitLabSettings _settings;
        private GitLabClientEx _client;

        public GitLabClientExFactory(GitLabSettings settings) => _settings = settings;

        public async Task<GitLabClientEx> Create()
        {
            if (_client != null)
                return _client;

            if (_settings.GitLabAccessToken.IsNotEmpty())
            {
                _client = new GitLabClientEx(_settings.GitLabHostUrl, _settings.GitLabAccessToken);
                return _client;
            }

            _client = new GitLabClientEx(_settings.GitLabHostUrl);
            await _client.LoginAsync(_settings.GitLabUserName, _settings.GitLabPassword);
            return _client;
        }
    }
}
