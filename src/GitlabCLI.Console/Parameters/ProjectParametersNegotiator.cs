﻿using GitLabCLI.Console.Parsing;
using GitLabCLI.Core;
using GitLabCLI.Utilities;
using static GitLabCLI.Core.Result;

namespace GitLabCLI.Console.Parameters
{
    public abstract class ProjectParametersNegotiator
    {
        protected Result<string> GetProject(ProjectOptions options, string defaultProject)
        {
            string projectName = !options.Project.IsNullOrEmpty() ?
                options.Project : defaultProject;

            return projectName.IsNullOrEmpty() ?
                Fail<string>(
                    "Project name is not provided and default is not set. " +
                    "You can set default project by running 'gitlab config --default-project {project}'") :
                Ok(projectName);
        }
    }
}
