using System.ComponentModel.DataAnnotations;
using Kiosk.Abstractions.Enums;
using Kiosk.Abstractions.Models.Major;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Kiosk.Abstractions.Models;

public class EctsSubjectResponse
{
    public required Degree Degree { get; set; }

    public required string Major { get; set; }
    
    public required int RecruitmentYear { get; set; }
    
    public required IEnumerable<SubjectsByYearAndTerm> SubjectsByYearAndTerm { get; set; }
}


public class SubjectsByYearAndTerm
{
    [Range(1, 6)] public required int Term { get; set; }

    [Range(1, 3)] public required Year Year { get; set; }
    
    public required IEnumerable<SubjectResponse> Subjects { get; set; }
}

public class SubjectResponse : EctsSubjectDetails
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? _id { get; set; }
    
    public required int LectureHours { get; set; }

    public required int RecitationHours { get; set; }

    public required int LabsHours { get; set; }

    public string? Pass { get; set; }

    public required int Ects { get; set; }
}