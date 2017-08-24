﻿namespace GitlabCmd.Console.App
{
    public static class ExitCode
    {
        public static int Success { get; } = 1;

        public static int InvalidArguments { get; } = 1;

        public static int InvalidConfiguration { get; } = 2;
    }
}
