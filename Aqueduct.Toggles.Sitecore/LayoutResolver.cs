﻿using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace Aqueduct.Toggles.Sitecore
{
    public class LayoutResolver : global::Sitecore.Pipelines.HttpRequest.LayoutResolver
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var item = Context.Item;
            if (item == null) return;

            var itemId = item.ID.Guid;
            var templateId = item.Template.ID.Guid;
            var currentLanguage = Context.Language.Name;
            if (SitecoreFeatureToggles.ShouldReplaceLayout(itemId, templateId, currentLanguage))
            {
                var replacement = SitecoreFeatureToggles.GetLayoutReplacement(itemId, templateId, currentLanguage);

                if (replacement.NewLayoutId.HasValue)
                {
                    LayoutItem pageEditLayout = Context.Database.GetItem(new ID(replacement.NewLayoutId.Value));
                    Context.Page.FilePath = pageEditLayout.FilePath;
                }
            }
        }
    }
}
