using WebAdminPanel.Models.DTOs.Reference;

namespace WebAdminPanel.ViewModels
{
    public class ReferenceViewModel : ReferenceDto, IEditElement
    {
        public ItemStatus? Status { get; set; }
    }
}
