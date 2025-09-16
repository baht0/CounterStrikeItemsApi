using Microsoft.AspNetCore.Components;
using WebAdminPanel.Components;
using WebAdminPanel.Components.References;
using WebAdminPanel.Contracts.Api.References;
using WebAdminPanel.Models.DTOs.Reference;
using WebAdminPanel.Models.DTOs.Reference.Collection;
using WebAdminPanel.Models.DTOs.Reference.ItemType;
using WebAdminPanel.Models.DTOs.Reference.Subtype;
using WebAdminPanel.Models.Metadata;

namespace WebAdminPanel.Pages
{
    public partial class Reference
    {
        private string? _selectedKey;
        private readonly Dictionary<string, RenderFragment> _views = [];

        protected override void OnInitialized()
        {
            InitUniqueItems();
            InitOrdinaryItems();
            InitColoredItems();

            _selectedKey = _views.Keys.FirstOrDefault(x => x == "Collections");
        }
        private void InitUniqueItems()
        {
            CreateDescriptor<CollectionDto, CollectionCreateDto, CollectionUpdateDto, CollectionRefFields<CollectionUpdateDto>>(
                title: "Collections",
                apiClient: RefApiFactory.GetClient<ICollectionApi>(),
                extraColumns: new ReferenceColumnDescriptor<CollectionDto>
                {
                    Title = "Collection Type",
                    ValueSelector = x => x.Type.Name
                });

            CreateDescriptor<SubtypeDto, SubtypeCreateDto, SubtypeUpdateDto, SubtypeFields<SubtypeUpdateDto>>(
                    title: "Subtypes",
                    apiClient: RefApiFactory.GetClient<ISubtypeApi>()
                );

            CreateDescriptor<ItemTypeDto, ItemTypeCreateDto, ItemTypeUpdateDto, ItemTypeFields<ItemTypeUpdateDto>>(
                    title: "Item Types",
                    apiClient: RefApiFactory.GetClient<IItemTypeApi>()
                );
        }
        private void InitOrdinaryItems()
        {
            var ordinaryItems = new Dictionary<string, IBaseReferenceApi<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto>>()
            {
                { "Collection Types", RefApiFactory.GetClient<ICollectionTypeApi>() },
                { "Exteriors", RefApiFactory.GetClient<IExteriorApi>() },
                { "Professional Players", RefApiFactory.GetClient<IProfessionalPlayerApi>() },
                { "Teams", RefApiFactory.GetClient<ITeamApi>() },
                { "Tournaments", RefApiFactory.GetClient<ITournamentApi>() }
            };
            foreach (var item in ordinaryItems)
            {
                CreateDescriptor<ReferenceDto, ReferenceCreateDto, ReferenceUpdateDto, ReferenceFields<ReferenceUpdateDto>>(
                    title: item.Key,
                    apiClient: item.Value
                );
            }
        }
        private void InitColoredItems()
        {
            var coloredItems = new Dictionary<string, IBaseReferenceApi<ReferenceColorDto, ReferenceColorCreateDto, ReferenceColorUpdateDto>>()
            {
                { "Categories", RefApiFactory.GetClient<ICategoryApi>() },
                { "Graffiti Colors", RefApiFactory.GetClient<IGraffitiColorApi>() },
                { "Qualities", RefApiFactory.GetClient<IQualityApi>() }
            };

            foreach (var item in coloredItems)
            {
                CreateDescriptor<ReferenceColorDto, ReferenceColorCreateDto, ReferenceColorUpdateDto, ReferenceColorFields<ReferenceColorUpdateDto>>(
                    title: item.Key,
                    apiClient: item.Value,
                    extraColumns: new ReferenceColumnDescriptor<ReferenceColorDto>
                    {
                        Title = "Color",
                        Template = x => builder =>
                        {
                            builder.OpenElement(0, "div");
                            builder.AddAttribute(1, "style", $"width:20px;height:20px;border-radius:4px;background:{x.HexColor}");
                            builder.CloseElement();
                        }
                    }
                );
            }
        }

        public void CreateDescriptor<TDto, TCreateDto, TUpdateDto, TComp>(
            string title,
            IBaseReferenceApi<TDto, TCreateDto, TUpdateDto> apiClient,
            params ReferenceColumnDescriptor<TDto>[] extraColumns) where TComp : IComponent
        {
            var descriptor = new ReferenceEntityDescriptor<TDto, TCreateDto, TUpdateDto>()
            {
                Title = title,
                GetAllAsync = apiClient.GetAll,
                CreateAsync = apiClient.Create,
                UpdateAsync = apiClient.Update,
                DeleteAsync = apiClient.Delete,
                MapToUpdateDto = d => Mapper.Map<TUpdateDto>(d),
                MapToCreateDto = u => Mapper.Map<TCreateDto>(u),
                EditDialog = model => builder =>
                {
                    builder.OpenComponent(0, typeof(TComp));
                    builder.AddAttribute(1, "Model", model); // компонент должен принимать параметр Model
                    builder.CloseComponent();
                },
                Columns = extraColumns?.ToList() ?? []
            };

            _views[title] = builder =>
            {
                builder.OpenComponent(0, typeof(ReferenceTable<TDto, TCreateDto, TUpdateDto>));
                builder.AddAttribute(1, "Descriptor", descriptor);
                builder.CloseComponent();
            };
        }
    }
}
