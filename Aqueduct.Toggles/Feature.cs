﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Aqueduct.Toggles.Configuration;
using Aqueduct.Toggles.Configuration.Layouts;
using Aqueduct.Toggles.Configuration.Renderings;

namespace Aqueduct.Toggles
{
    public class Feature
    {
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string ShortDescription { get; set; }
        public string Requirements { get; set; }
        public string IssueTrackingReference { get; set; }
        public string ReleaseDate { get; set; }

        private string _languages;
        internal string Languages
        {
            get { return _languages; }
            set
            {
                _languages = value;
                UpdateLanguagesList(value);
            }
        }

        private void UpdateLanguagesList(string value)
        {
            var languages = !string.IsNullOrEmpty(value) ? value.Split(',') : Enumerable.Empty<string>();

            LanguagesList = languages.Select(x =>x.Trim().ToLower()).ToList();
        }

        internal List<string> LanguagesList { get; set; } 
        
        public bool EnabledForLanguage(string language)
        {
            if (string.IsNullOrWhiteSpace(language))
                return false;

            return LanguagesList.Count == 0 || LanguagesList.Contains(language.ToLower());
        }

        public IList<LayoutToggle> Templates { get; private set; }
        public IList<LayoutToggle> Items { get; private set; }

        public IList<RenderingToggle> Renderings { get; private set; }

        public Feature()
        {
            Templates = new List<LayoutToggle>();
            Items = new List<LayoutToggle>();
            Renderings = new List<RenderingToggle>();
            LanguagesList = new List<string>();
        }

        internal static Feature FromConfig(FeatureToggleConfigurationElement element)
        {
            Contract.Assert(element != null);

            var feature = new Feature
                          {
                              Enabled = element.Enabled,
                              Languages = element.Languages,
                              Name = element.Name
                          };
            if (element.Help != null)
            {
                feature.ShortDescription = element.Help.Description.Value;
                feature.Requirements = element.Help.Requirements.Value;
                feature.IssueTrackingReference = element.Help.IssueTrackingReference.Value;
                feature.ReleaseDate = element.Help.ReleaseDate.Value;
            }

            feature.Renderings =
                element.Renderings.Cast<FeatureRenderingConfigurationElement>()
                    .Select(RenderingToggle.FromConfig)
                    .ToList();
            feature.Templates =
                element.Templates.Cast<BaseFeatureToggleLayoutConfigurationElement>()
                    .Select(LayoutToggle.FromConfig)
                    .ToList();
            feature.Items =
                element.Items.Cast<BaseFeatureToggleLayoutConfigurationElement>()
                    .Select(LayoutToggle.FromConfig)
                    .ToList();

            return feature;
        }

    }
}
