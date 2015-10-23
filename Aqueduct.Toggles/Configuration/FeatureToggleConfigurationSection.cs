﻿using System.Configuration;

namespace Aqueduct.Toggles.Configuration
{
    internal class FeatureToggleConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(FeatureToggleConfigurationCollection), AddItemName = "feature")]
        internal FeatureToggleConfigurationCollection Features => base[""] as FeatureToggleConfigurationCollection;
    }
}
