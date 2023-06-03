using Akka.Actor;
using Job.Core.Interfaces;
using Job.Core.Models;
using Job.Core.Theater.Workers.Messages;

namespace Job.Core.Services;

internal class JobContext : IJobContext
{
    private readonly IActorRef _jobContext;
    
    public JobContext(IActorRef jobContext)
    {
        _jobContext = jobContext;
    }

    public Guid CreateJob()
    {
        var id = Guid.NewGuid();
        _jobContext.Tell(new MakeWorkCommand
        {
            JobId = id,
            GroupId = "test"
        });
        return id;
    }

    public async Task<MakeWorkCommandResult> MakeWorkAsync()
    {
        var id = Guid.NewGuid();
        return await _jobContext.Ask<MakeWorkCommandResult>(new MakeWorkCommand
        {
            JobId = id,
            GroupId = "test"
        });
    }

    public Task<StopJobCommandResult> StopJobAsync(Guid jobId)
    {
        throw new NotImplementedException();
        // return await _jobContext.Ask<StopJobCommandResult>(new StopJobCommand()
        // {
        //     JobId = jobId,
        // });
    }
}