using Kiosk.Abstractions.Enums;

namespace Kiosk.Abstractions.Models.LessonPlan;

public class CreateLessonPlanSimplified
{
    public required string Name { get; set; }
    
    public required int Year { get; set; }
    
    public required String Day { get; set; }
    
    public required int Start { get; set; }
    
    public required int Duration { get; set; }
    
    public List<string>? Groups { get; set; }
    
    public required List<string> Teachers { get; set; }
    
    public string? Class { get; set; }
    
    public required LessonPlanContentSimplified Details { get; set; }

    public Language SourceLanguage { get; set; } = Language.Pl;
}