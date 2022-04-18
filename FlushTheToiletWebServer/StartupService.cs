using FlushTheToiletWebServer.Models;
using ioBroker.net;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FlushTheToiletWebServer
{
    public class StartupService : IHostedService
    {
        private IToiletFlusherStateMachineModel _stateMachine;
        private IIoBrokerDotNet _ioBrokerDotNet;

        public StartupService(IToiletFlusherStateMachineModel stateMachine, IIoBrokerDotNet ioBrokerDotNet)
        {
            _stateMachine = stateMachine;
            _ioBrokerDotNet = ioBrokerDotNet;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ioBrokerConnected = false;
            while (!ioBrokerConnected)
            {
                try
                {
                    await _ioBrokerDotNet.ConnectAsync(TimeSpan.FromSeconds(5));
                    var tempCountId = "javascript.0.toilet.flushes.count";
                    var tempCountResult = _ioBrokerDotNet.TryGetStateAsync<int>(tempCountId, TimeSpan.FromSeconds(5)).Result;
                    if (tempCountResult.Success)
                    {
                        ioBrokerConnected = true;
                    }
                }
                catch
                {
                    ioBrokerConnected = false;
                }
            }

            _stateMachine.StartToiletStateMachine();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
