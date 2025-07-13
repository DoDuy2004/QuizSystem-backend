using Microsoft.AspNetCore.SignalR;
namespace QuizSystem_backend.Hubs;

public class QuizHub: Hub
{
    public Task JoinRoom(string roomId)
        => Groups.AddToGroupAsync(Context.ConnectionId, roomId);

    public Task LeaveRoom(string roomId)
        =>Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);    

}
