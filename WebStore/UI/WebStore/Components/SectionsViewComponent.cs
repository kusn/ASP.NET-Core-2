using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.ViewModels;

namespace WebStore.Components
{
    public class SectionsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public SectionsViewComponent(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IViewComponentResult Invoke(string SectionId)
        {
            var section_id = int.TryParse(SectionId, out var id) ? id : (int?)null;

            var sections = GetSections(section_id, out var parent_section_id);

            return View(new SelectableSectionsViewModel
            {
                Sections = sections,
                SectionId = section_id,
                ParentSectionId = parent_section_id,
            }); ;
        }

        private IEnumerable<SectionViewModel> GetSections(int? sectionId, out int? parentSectionId)
        {
            parentSectionId = null;

            var sections = _ProductData.GetSections();

            var parentSections = sections.Where(s => s.ParentId is null);

            var parentSectionsViews = parentSections
                .Select(s => new SectionViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Order = s.Order,
                }).ToList();

            foreach (var parentSection in parentSectionsViews)
            {
                var childs = sections.Where(s => s.ParentId == parentSection.Id);

                foreach (var childSections in childs)
                {
                    if (childSections.Id == sectionId)
                        parentSectionId = childSections.Id;

                    parentSection.ChildSections.Add(new SectionViewModel
                    {
                        Id = childSections.Id,
                        Name = childSections.Name,
                        Order = childSections.Order,
                        Parent = parentSection,
                    });
                }

                parentSection.ChildSections.Sort((a, b) =>
                    Comparer<int>.Default.Compare(a.Order, b.Order));
            }

            parentSectionsViews.Sort((a, b) =>
                    Comparer<int>.Default.Compare(a.Order, b.Order));

            return parentSectionsViews;
        }
    }
}
