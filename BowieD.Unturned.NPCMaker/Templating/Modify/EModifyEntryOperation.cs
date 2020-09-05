namespace BowieD.Unturned.NPCMaker.Templating.Modify
{
    public enum EModifyEntryOperation
    {
        /// <summary>
        /// Assigns value
        /// </summary>
        set,
        /// <summary>
        /// Sums (Original + Value)
        /// </summary>
        sum,
        /// <summary>
        /// Sums (Value + Original)
        /// </summary>
        sum2,
        /// <summary>
        /// Adds element to collection
        /// </summary>
        add,
        /// <summary>
        /// Concats strings (Original + Value)
        /// </summary>
        concat,
        /// <summary>
        /// Concats strings (Value + Original)
        /// </summary>
        concat2
    }
}
