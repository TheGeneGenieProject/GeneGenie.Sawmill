namespace GeneGenie.Sawmill.Models
{
    /// <summary>
    /// The type of event such as Birth, Death etc.
    /// </summary>
    public enum PersonEventType
    {
        /// <summary>An event should never be set to this value, it is left here to catch errors as it is the default for an int.</summary>
        NotSet = 0,

        /// <summary>Marks an event as the birth of a person.</summary>
        Birth = 1,

        /// <summary>Marks an event as the death of a person.</summary>
        Death = 2,
    }
}
