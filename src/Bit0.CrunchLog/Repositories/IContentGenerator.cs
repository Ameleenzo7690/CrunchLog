﻿using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Microsoft.Extensions.Logging;

namespace Bit0.CrunchLog.Repositories
{
    public interface IContentGenerator
    {
        void PublishAll();
        void CleanOutput();
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContentGenerator : IContentGenerator
    {
        private readonly IContentProvider _contentProvider;
        private readonly CrunchConfig _config;
        private readonly ILogger<ContentGenerator> _logger;

        public ContentGenerator(IContentProvider contentProvider, CrunchConfig config, ILogger<ContentGenerator> logger)
        {
            _logger = logger;
            _config = config;
            _contentProvider = contentProvider;
        }

        public void CleanOutput()
        {
            if (_config.OutputPath.Exists)
            {
                _config.OutputPath.Delete(true);
            }

            _logger.LogInformation($"Cleaned output folder {_config.OutputPath.FullName}");

        }

        public void PublishAll()
        {
            var all = _contentProvider.GetAll().ToList();
            var published = all.Where(c => c.Value.Published).ToList();

            // get posts
            // create archive, tag and category pages
            // create main index

            // get parent for pages
            // create a tree
            // generate permalink from tree

            foreach (var content in published)
            {
                content.Value.WriteFile(_config.OutputPath);
            }

            _logger.LogInformation($"Published: {published.Count}/{all.Count}");
        }
    }
}
