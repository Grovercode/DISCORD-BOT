using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Template.Utilities;

namespace Template.Modules
{
    public class ExampleModule : ModuleBase<SocketCommandContext>
    {
        private readonly images _images;

        private readonly ILogger<ExampleModule> _logger;

        public ExampleModule(ILogger<ExampleModule> logger, images images1)
        {
            _logger = logger;
            _images = images1;
        }
           

        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong!");
           
        }

        [Command("echo")]
        public async Task EchoAsync([Remainder] string text)
        {
            await ReplyAsync(text);
          
        }

        [Command("math")]
        public async Task MathAsync([Remainder] string math)
        {
            var dt = new DataTable();
            var result = dt.Compute(math, null);
            
            await ReplyAsync($"Result: {result}");
           
        }

        [Command("ping")]
        public async Task Ping()
        {
            await Context.Channel.SendMessageAsync("Pong!");
        }

        [Command("info")]
        public async Task Info(SocketGuildUser user = null)
        {

            if (user == null)
            {
                var builder = new EmbedBuilder()
                    .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                    .WithDescription("Information about yourself!")
                    .WithColor(new Color(33, 176, 252))
                    .AddField("User ID", Context.User.Id, true)
                    .AddField("Discriminator", Context.User.Discriminator, true)
                    .AddField("Created account at ", Context.User.CreatedAt.ToString("dd/MM/yyyy"), true)
                    .AddField("Joined this server at", (Context.User as SocketGuildUser).JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                    .AddField("Roles", string.Join(" ", (Context.User as SocketGuildUser).Roles.Select(x => x.Mention)))
                    .WithCurrentTimestamp();
                var embed = builder.Build();

                await Context.Channel.SendMessageAsync(null, false, embed);
            }

            else
            {
                var builder = new EmbedBuilder()
                                   .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                                   .WithDescription($"Information about {user.Username} !")
                                   .WithColor(new Color(33, 176, 252))
                                   .AddField("User ID", user.Id, true)
                                   .AddField("Discriminator", user.Discriminator, true)
                                   .AddField("Created account at ", user.CreatedAt.ToString("dd/MM/yyyy"), true)
                                   .AddField("Joined this server at", user.JoinedAt.Value.ToString("dd/MM/yyyy"), true)
                                   .AddField("Roles", string.Join(" ", user.Roles.Select(x => x.Mention)))
                                   .WithCurrentTimestamp();

                var embed = builder.Build();

                await Context.Channel.SendMessageAsync(null, false, embed);
            
            }

        }


        [Command("slogan")]
        [Alias("motto", "quote", "saying")]
        public async Task Slogan(SocketGuildUser user = null)
        {

            if (user == null)
            {
                string[] slogans;
                slogans = new string[15] {"Sometimes when I close my eyes, I can't see. ",
                "I put my phone in airplane mode, but it's not flying!",
                    "I know that I am stupid but when I look around me I feel a lot better.",
                    "I don't mean to brag, but I put together a puzzle in 1 day and the box said 2-4 years.",
                    "Fart when someone hugs you, it makes them feel strong.  ",
                    "	I have no clue where I am going.",
                    "I quit before I even try. ",
                "I wasted the whole week.",
                "I'm not funny",
                "I cant get over my ex",
                "I'm fat as hell",
                "I can rage over anything ",
                "Im scared of everything",
                "Being an incel is hard",
                "I'm always hungry",};



                Random rnd = new Random();
                int value = StaticRandom.Instance.Next(0,14);
              
                var builder = new EmbedBuilder()
                    .WithThumbnailUrl(Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl())
                    .WithDescription("Information about yourself!")
                    .WithColor(new Color(33, 176, 252))
                    .AddField("Life motto: ", slogans[value], true);

                var embed = builder.Build();

                await Context.Channel.SendMessageAsync(null, false, embed);
            }

            else
            {
                string[] slogans;
                slogans = new string[15] {"Sometimes when I close my eyes, I can't see. ",
                "I put my phone in airplane mode, but it's not flying!",
                    "I know that I am stupid but when I look around me I feel a lot better.",
                    "I don't mean to brag, but I put together a puzzle in 1 day and the box said 2-4 years.",
                    "Fart when someone hugs you, it makes them feel strong.  ",
                    "	I have no clue where I am going.",
                    "I quit before I even try. ",
                "I wasted the whole week.",
                "I'm not funny",
                "I cant get over my ex",
                "I'm fat as hell",
                "I can rage over anything ",
                "Im scared of everything",
                "Being an incel is hard",
                "I'm always hungry",};


                Random rnd = new Random();
                int value = StaticRandom.Instance.Next(0, 14);

                var builder = new EmbedBuilder()
                                   .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                                   .WithDescription($"Information about {user.Username} !")
                                   .WithColor(new Color(33, 176, 252))
                                   .AddField("Life motto: ", slogans[value], true);

                var embed = builder.Build();

                await Context.Channel.SendMessageAsync(null, false, embed);
            }
        }

        [Command("meme")]
        [Alias("reddit")]
        public async Task Meme(string subreddit = null)
        {
            var client = new HttpClient();
            var result = await client.GetStringAsync($"https://reddit.com/r/{subreddit ?? "memes"}/random.json?limit=1");
            if (!result.StartsWith("["))
            {
                await Context.Channel.SendMessageAsync("This subreddit doesn't exist!");
                return;
            }
            JArray arr = JArray.Parse(result);
            JObject post = JObject.Parse(arr[0]["data"]["children"][0]["data"].ToString());

            var builder = new EmbedBuilder()
                .WithImageUrl(post["url"].ToString())
                .WithColor(new Color(33, 176, 252))
                .WithTitle(post["title"].ToString())
                .WithUrl("https://reddit.com" + post["permalink"].ToString())
                .WithFooter($"🗨️ {post["num_comments"]} ⬆️ {post["ups"]}");
            var embed = builder.Build();
            await Context.Channel.SendMessageAsync(null, false, embed);
        }


        [Command("roast", RunMode = RunMode.Async)]
        [Alias("roasted", "ohh" , "boom", "damn")]
        public async Task Image(SocketGuildUser user)
        {
            var path = await _images.CreateImageAsync(user);
            await Context.Channel.SendFileAsync(path);

            File.Delete(path);


        }

    }
}