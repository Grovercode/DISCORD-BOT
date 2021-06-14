using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;

namespace Template.Services
{
    public class CommandHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        private readonly IConfiguration _config;

        public CommandHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service, IConfiguration config)
        {
            _provider = provider;
            _client = client;
            _service = service;
            _config = config;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            _client.MessageReceived += OnMessageReceived;
            _client.ChannelCreated += OnChannelCreated;
        
            _service.CommandExecuted += OnCommandExecuted;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), _provider);
        }

        private async Task OnChannelCreated(SocketChannel arg)
        {
            if (arg as ITextChannel == null) return;

            var channel = arg as ITextChannel;

            await channel.SendMessageAsync("I HATH ARRIVED!");

        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            if (!(arg is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            string mes = message.ToString();
            int size = mes.Length;

            for (int i = 0; i < size; i++)
            {
                if ((i + 1 < size) && ((mes[i] == 'R' && mes[i + 1] == 'L') || (mes[i]=='r' && mes[i+1] == 'l') || (mes[i] == 'r' && mes[i + 1] == 'L')
                    ) || (mes[i] == 'R' && mes[i + 1] == 'l'))
                {
                    await arg.Channel.SendMessageAsync("What a save!");
                }

                if (mes[i] == 't' && (i + 6) < size)
                {
                    if (mes[i + 1] == 'w' && mes[i + 2] == 'i'
                      && mes[i + 3] == 't' && mes[i + 4] == 't'
                       && mes[i + 5] == 'e' && mes[i + 6] == 'r')
                        await arg.Channel.SendMessageAsync("steve sucks");
                }



                if (((mes[i] == 'v')  && (i + 3 < size) && mes[i + 1] == 'a' && mes[i + 2] == 'l'
                      && mes[i + 3] == 'o'))
                {
                    
                        await arg.Channel.SendMessageAsync("come bros");
                }

                



            }


            var argPos = 0;
            if (!message.HasStringPrefix(_config["prefix"], ref argPos) && !message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;


            var context = new SocketCommandContext(_client, message);
            await _service.ExecuteAsync(context, argPos, _provider);
        }

        private async Task OnCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (command.IsSpecified && !result.IsSuccess) await context.Channel.SendMessageAsync($"Error: {result}");
        }
    }
}