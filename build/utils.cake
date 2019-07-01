// Output task status to TeamCity log
TaskSetup(setupContext =>
{
    var message = string.Format("Task: {0}", setupContext.Task.Name);
    if (BuildSystem.IsRunningOnTeamCity) BuildSystem.TeamCity.WriteStartBlock(message);
});

TaskTeardown(teardownContext =>
{
    var message = string.Format("Task: {0}", teardownContext.Task.Name);
    if (BuildSystem.IsRunningOnTeamCity) BuildSystem.TeamCity.WriteEndBlock(message);
});