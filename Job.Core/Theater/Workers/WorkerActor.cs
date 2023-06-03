﻿using Akka.Actor;
using Job.Core.Interfaces;
using Job.Core.Models;
using Job.Core.Theater.Workers.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace Job.Core.Theater.Workers;

public class WorkerActor : ReceiveActor
{
    private readonly Guid _actorId;
    private readonly string _groupId;
    
    private readonly IServiceScope _scope;
    
    private IJob _job;

    public WorkerActor(IServiceProvider serviceProvider, string groupId, Guid actorId)
    {
        _actorId = actorId;
        _groupId = groupId;

        _scope = serviceProvider.CreateScope();
        
        Receive<StartJobCommand>(StartJobCommandHandler);

        //Receive<Terminated>(DownloadActorSlaveTerminatedHandler);
    }

    private void StartJobCommandHandler(StartJobCommand obj)
    {
        _job.StartJob();
        Sender.Tell(new StartJobCommandResult(true, "Ok"));
    }
    
    protected override void PreStart()
    {
        _job = _scope.ServiceProvider.GetService<IJob>();
    }
    protected override void PostStop()
    {
        _scope.Dispose();
    }
}