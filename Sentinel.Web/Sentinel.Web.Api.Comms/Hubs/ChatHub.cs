    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using System.Linq;
    using Microsoft.AspNetCore.SignalR;

    namespace Sentinel.Web.Api.Comms.Hubs
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class ChatHub : Hub
        {
            public Task Send(string message)
            {
                var user = Context.User;
                return Clients.All.SendAsync("Send", message);
            }

            public override async Task OnConnectedAsync()
            {
            // await Clients.Client(Context.ConnectionId).InvokeAsync("SetUsersOnline", await GetUsersOnline());
            // var iden = this.Context.User.Identity;
            await base.OnConnectedAsync();
            }
            public override Task OnDisconnectedAsync(Exception exception)
            {
            return base.OnDisconnectedAsync(exception);
            }
        }

        public static class extesions
        {
            public static void UseSignalRJwtAuthentication(this IApplicationBuilder app)
            {
                app.Use(async (context, next) =>
                {
                    if (string.IsNullOrWhiteSpace(context.Request.Headers["Authorization"]))
                    {
                        if (context.Request.QueryString.HasValue)
                        {
                            var token = context.Request.QueryString.Value.Split('&').SingleOrDefault(x => x.Contains("authorization"))?.Split('=')[1];
                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                context.Request.Headers.Add("Authorization", new[] { $"Bearer {token}" });
                            }
                        }
                    }
                    await next.Invoke();
                });
            }
        }
    }
