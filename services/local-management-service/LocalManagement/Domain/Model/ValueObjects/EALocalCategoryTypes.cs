using System.ComponentModel;

namespace LocalManagement.Domain.Model.ValueObjects;

public enum EALocalCategoryTypes
{
    [Description("Casa de playa")]
    BeachHouse,

    [Description("Casa de campo")]
    LandscapeHouse,

    [Description("Casa urbana")]
    CityHouse,

    [Description("Salón elegante")]
    ElegantRoom
}
