namespace TeisterMask.DataProcessor.ExportDto
{
    using System.Collections.Generic;

    public class EmploeeWithTasksExportDto
    {
        public string Username { get; set; }

        public ICollection<TaskExportDto> Tasks { get; set; }
    }
}
