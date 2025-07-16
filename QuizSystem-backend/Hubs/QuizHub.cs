using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QuizSystem_backend.Models;
namespace QuizSystem_backend.Hubs;

[Authorize]
public class QuizHub: Hub
{
    public Task JoinCourseClassGroup(Guid courseClassId)
        => Groups.AddToGroupAsync(Context.ConnectionId, $"CourseClass_{courseClassId}");

    public Task LeaveCourseClassGroup(Guid courseClassId)
        =>Groups.RemoveFromGroupAsync(Context.ConnectionId, $"CourseClass_{courseClassId}");    

}
