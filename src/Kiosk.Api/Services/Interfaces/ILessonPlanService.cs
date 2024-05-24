using Kiosk.Abstractions.Enums;
using Kiosk.Abstractions.Models.LessonPlan;

namespace KioskAPI.Services.Interfaces;

public interface ILessonPlanService
{
    Task<IEnumerable<GetLessonPlanResponse>?> GetAllLecturesForMajorYear(GetLessonsPlanRequestLectures getLessonsPlanRequestLectures, Language language, CancellationToken cancellationToken);
    
    Task<IEnumerable<GetLessonPlanResponse>?> GetAllLessonsForMajorYearGroup(GetLessonPlanRequest getLessonPlanRequest, Language language, CancellationToken cancellationToken);

    Task<IEnumerable<string>?> GetMajors(CancellationToken cancellationToken);

    Task<GetMajorGroupsResponse> GetMajorGroups(string major, int year, CancellationToken cancellationToken);
    Task<IEnumerable<int>?> GetMajorYears(string major, CancellationToken cancellationToken);

    Task CreateLessons(IEnumerable<CreateLessonPlanRequest> createLessonPlanRequests, CancellationToken cancellationToken);
}