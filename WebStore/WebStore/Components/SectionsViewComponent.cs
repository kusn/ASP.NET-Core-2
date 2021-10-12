using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
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

        public IViewComponentResult Invoke()
        {
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

            return View(parentSectionsViews);
        }
    }
}
