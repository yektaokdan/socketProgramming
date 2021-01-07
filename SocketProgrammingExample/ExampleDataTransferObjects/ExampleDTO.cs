using System;

namespace ExampleDataTransferObjects
{
    /// <summary>
    /// Serialize edebilmek için Serializable attributü ile işaretliyoruz.
    /// </summary>
    [Serializable]
    public class ExampleDTO
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public string ozelData { get; set; }
    }
}