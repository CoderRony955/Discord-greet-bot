using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace WelcomeBot
{
    class Program
    {
        private DiscordSocketClient _client;
        private ulong _welcomeChannelId = 1234567890123456; // Replace with your actual welcome channel ID

        static void Main(string[] args)
        {
            try
            {
                new Program().RunBotAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public async Task RunBotAsync()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMembers | GatewayIntents.GuildMessages
            };

            _client = new DiscordSocketClient(config);
            _client.Log += Log;
            _client.UserJoined += UserJoined;

            string token = "BOT_TOKEN"; // Replace with your bot token

            try
            {
                Console.WriteLine("Logging in...");
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
                Console.WriteLine("Bot started.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging in: {ex.Message}");
                return;
            }

            // Keep the bot running indefinitely
            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task UserJoined(SocketGuildUser user)
        {
            Console.WriteLine($"New user joined: {user.Username}#{user.Discriminator}");

            try
            {
                var welcomeChannel = user.Guild.GetTextChannel(_welcomeChannelId);
                if (welcomeChannel != null)
                {
                    Console.WriteLine($"Welcome channel found: {welcomeChannel.Name}");
                    var embed = new EmbedBuilder()
                        .WithTitle("Welcome to the Server!")
                        .WithDescription($"Hello {user.Mention}! Welcome to our Discord server, we're thrilled to have you join our community! Whether you're here to chat, share, or simply hang out, you've found the perfect spot. Dive into the conversations, make new friends, and most importantly, have fun! If you have any questions or need help, our awesome team is here to assist. Enjoy your stay, and let's make some great memories together! 🚀✨\r\n\r\n#WelcomeToTheFamily 💬🎉")
                        .WithColor(Color.Green)
                        .WithThumbnailUrl(user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl())
                        .WithCurrentTimestamp()
                        .Build();

                    await welcomeChannel.SendMessageAsync(embed: embed);
                    Console.WriteLine("Welcome message sent.");
                }
                else
                {
                    Console.WriteLine($"Welcome channel not found! ID: {_welcomeChannelId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the welcome message: {ex.Message}");
            }
        }
    }
}
